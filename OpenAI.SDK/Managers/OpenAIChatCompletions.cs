using System.Runtime.CompilerServices;
using System.Text.Json;
using OpenAI.Extensions;
using OpenAI.Interfaces;
using OpenAI.ObjectModels;
using OpenAI.ObjectModels.RequestModels;
using OpenAI.ObjectModels.ResponseModels;

namespace OpenAI.Managers;

public partial class OpenAIService : IChatCompletionService
{
    /// <inheritdoc />
    public async Task<ChatCompletionCreateResponse> CreateCompletion(ChatCompletionCreateRequest chatCompletionCreateRequest, string? modelId = null, CancellationToken cancellationToken = default)
    {
        chatCompletionCreateRequest.ProcessModelId(modelId, _defaultModelId);
        return await _httpClient.PostAndReadAsAsync<ChatCompletionCreateResponse>(_endpointProvider.ChatCompletionCreate(), chatCompletionCreateRequest, cancellationToken);
    }

    /// <inheritdoc />
    public async IAsyncEnumerable<ChatCompletionCreateResponse> CreateCompletionAsStream(ChatCompletionCreateRequest chatCompletionCreateRequest, string? modelId = null, bool justDataMode = true,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        // Helper data in case we need to reassemble a multi-packet response
        ReassemblyContext ctx = new();

        // Mark the request as streaming
        chatCompletionCreateRequest.Stream = true;

        // Send the request to the CompletionCreate endpoint
        chatCompletionCreateRequest.ProcessModelId(modelId, _defaultModelId);

        using var response = _httpClient.PostAsStreamAsync(_endpointProvider.ChatCompletionCreate(), chatCompletionCreateRequest, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            yield return await response.HandleResponseContent<ChatCompletionCreateResponse>(cancellationToken);
            yield break;
        }

        // Ensure that we parse headers only once to improve performance a little bit.
        var httpStatusCode = response.StatusCode;
        var headerValues = response.ParseHeaders();

        await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
        using var reader = new StreamReader(stream);

        // Continuously read the stream until the end of it
        while (true)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var line = await reader.ReadLineAsync();
            // Break the loop if we have reached the end of the stream
            if (line == null)
            {
                break;
            }

            // Skip empty lines
            if (string.IsNullOrEmpty(line))
            {
                continue;
            }

            if (justDataMode && !line.StartsWith("data: "))
            {
                continue;
            }

            line = line.RemoveIfStartWith("data: ");

            // Exit the loop if the stream is done
            if (line.StartsWith("[DONE]"))
            {
                break;
            }

            ChatCompletionCreateResponse? block;
            try
            {
                // When the response is good, each line is a serializable CompletionCreateRequest
                block = JsonSerializer.Deserialize<ChatCompletionCreateResponse>(line);
            }
            catch (Exception)
            {
                // When the API returns an error, it does not come back as a block, it returns a single character of text ("{").
                // In this instance, read through the rest of the response, which should be a complete object to parse.
                line += await reader.ReadToEndAsync();
                block = JsonSerializer.Deserialize<ChatCompletionCreateResponse>(line);
            }


            if (null != block)
            {
                ctx.Process(block);

                if (!ctx.IsFnAssemblyActive)
                {
                    block.HttpStatusCode = httpStatusCode;
                    block.HeaderValues = headerValues;
                    yield return block;
                }
            }
        }
    }

    /// <summary>
    ///     This helper class attempts to reassemble a tool call with type == "function" response
    ///     that was split up across several streamed chunks.
    ///     Note that this only works for the first message in each response,
    ///     and ignores the others; if OpenAI ever changes their response format
    ///     this will need to be adjusted.
    /// </summary>
    private class ReassemblyContext
    {
        private IList<ToolCall> _deltaFnCallList = new List<ToolCall>();
        public bool IsFnAssemblyActive => _deltaFnCallList.Count > 0;


        /// <summary>
        ///     Detects if a response block is a part of a multi-chunk
        ///     streamed tool call response of type == "function". As long as that's true,
        ///     it keeps accumulating block contents even handling multiple parallel tool calls, and once all the function call
        ///     streaming is done, it produces the assembled results in the final block.
        /// </summary>
        /// <param name="block"></param>
        public void Process(ChatCompletionCreateResponse block)
        {
            var firstChoice = block.Choices?.FirstOrDefault();
            if (firstChoice == null)
            {
                return;
            } // not a valid state? nothing to do

            var isStreamingFnCall = IsStreamingFunctionCall();
            var isStreamingFnCallEnd = firstChoice.FinishReason != null;

            var justStarted = false;

            // Check if the streaming block has a tool_call segment of "function" type, according to the value returned by IsStreamingFunctionCall() above.
            // If so, this is the beginning entry point of a function call assembly for each tool_call main item, even in case of multiple parallel tool calls.
            // We're going to steal the partial message and squirrel it away for the time being.
            if (isStreamingFnCall)
            {
                foreach (var t in firstChoice.Message.ToolCalls!)
                {
                    //Handles just ToolCall type == "function" as according to the value returned by IsStreamingFunctionCall() above
                    if (t.FunctionCall != null && t.Type == StaticValues.CompletionStatics.ToolType.Function)
                        _deltaFnCallList.Add(t);
                }

                justStarted = true;
            }

            // As long as we're assembling, keep on appending those args,
            // respecting the stream arguments sequence aligned with the last tool call main item which the arguments belong to.
            if (IsFnAssemblyActive && !justStarted)
            {
                //Get current toolcall metadata in order to search by index reference which to bind arguments to.
                var tcMetadata = GetToolCallMetadata();

                if (tcMetadata.index > -1)
                {
                    //Handles just ToolCall type == "function"
                    using var argumentsList = ExtractArgsSoFar()
                        .GetEnumerator();
                    var existItems = argumentsList.MoveNext();

                    if (existItems)
                    {
                        //toolcall item must exists as added in previous steps, otherwise First() will raise an InvalidOperationException
                        var tc = _deltaFnCallList!.First(t => t.Index == tcMetadata.index);
                        tc.FunctionCall!.Arguments += argumentsList.Current;
                        argumentsList.MoveNext();
                    }
                }
            }

            // If we were assembling and it just finished, fill this block with the info we've assembled, and we're done.
            if (IsFnAssemblyActive && isStreamingFnCallEnd)
            {
                firstChoice.Message ??= ChatMessage.FromAssistant(""); // just in case? not sure it's needed
                // TODO When more than one function call is in a single index, OpenAI only returns the role delta at the beginning, which causes an issue.
                // TODO The current solution addresses this problem, but we need to fix it by using the role of the index.
                firstChoice.Message.Role ??= "assistant";
                firstChoice.Message.ToolCalls = new List<ToolCall>(_deltaFnCallList);
                _deltaFnCallList.Clear();
            }

            // Returns true if we're actively streaming, and also have a partial tool call main item ( id != (null | "")) of type "function" in the response
            bool IsStreamingFunctionCall()
            {
                return firstChoice.FinishReason == null && // actively streaming, is a tool call main item, and have a function call
                       firstChoice.Message?.ToolCalls?.Count > 0 && (firstChoice.Message?.ToolCalls.Any(t => t.FunctionCall != null && !string.IsNullOrEmpty(t.Id) && t.Type == StaticValues.CompletionStatics.ToolType.Function) ?? false);
            }

            (int index, string? id, string? type) GetToolCallMetadata()
            {
                var tc = block.Choices
                    ?.FirstOrDefault()
                    ?.Message
                    ?.ToolCalls
                    ?.Where(t => t.FunctionCall != null)
                    .Select(t => t)
                    .FirstOrDefault();

                return tc switch
                {
                    not null => (tc.Index, tc.Id, tc.Type),
                    _ => (-1, default, default)
                };
            }

            IEnumerable<string> ExtractArgsSoFar()
            {
                var toolCalls = block.Choices?.FirstOrDefault()
                    ?.Message?.ToolCalls;

                if (toolCalls != null)
                {
                    var functionCallList = toolCalls.Where(t => t.FunctionCall != null)
                        .Select(t => t.FunctionCall);

                    foreach (var functionCall in functionCallList)
                    {
                        yield return functionCall!.Arguments ?? "";
                    }
                }
            }
        }
    }
}