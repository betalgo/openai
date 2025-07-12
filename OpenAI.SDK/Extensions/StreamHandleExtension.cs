using System.Runtime.CompilerServices;
using System.Text.Json;
using Betalgo.Ranul.OpenAI.Contracts.Enums;
using Betalgo.Ranul.OpenAI.ObjectModels;
using Betalgo.Ranul.OpenAI.ObjectModels.RequestModels;
using Betalgo.Ranul.OpenAI.ObjectModels.ResponseModels;

namespace Betalgo.Ranul.OpenAI.Extensions;

public static class StreamHandleExtension
{
    public static async IAsyncEnumerable<BaseResponse> AsStream(this HttpResponseMessage response, bool justDataMode = true, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        await foreach (var baseResponse in AsStream<BaseResponse>(response, justDataMode, cancellationToken)) yield return baseResponse;
    }
    public static async IAsyncEnumerable<TResponse> AsStream<TResponse>(this HttpResponseMessage response, bool justDataMode = true, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        where TResponse : BaseResponse, new()
    {
        // Helper data in case we need to reassemble a multi-packet response
        ReassemblyContext ctx = new();

        // Ensure that we parse headers only once to improve performance a little bit.
        var httpStatusCode = response.StatusCode;
        var headerValues = response.ParseHeaders();

        await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
        using var reader = new StreamReader(stream);
        string? tempStreamEvent = null;
        bool isEventDelta;
        // Continuously read the stream until the end of it
        while (true)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var line = await reader.ReadLineAsync();
         //   Console.WriteLine("---" + line);
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

            if (line.StartsWith("event: "))
            {
                line = line.RemoveIfStartWith("event: ");
                tempStreamEvent = line;
                isEventDelta = true;
            }
            else
            {
                isEventDelta = false;
            }

            if (justDataMode && !line.StartsWith("data: "))
            {
                continue;
            }

            if (!justDataMode && isEventDelta )
            {
                yield return new(){ObjectTypeName = "base.stream.event",StreamEvent = tempStreamEvent};
                continue;
            }

            line = line.RemoveIfStartWith("data: ");

            // Exit the loop if the stream is done
            if (line.StartsWith("[DONE]"))
            {
                break;
            }

            TResponse? block;
            try
            {
                // When the response is good, each line is a serializable CompletionCreateRequest
                if (typeof(TResponse) == typeof(BaseResponse))
                {
                   block =JsonSerializer.Deserialize(line, JsonToObjectRouterExtension.Route(line), new JsonSerializerOptions()) as TResponse;
                }
                else
                {
                    block = JsonSerializer.Deserialize<TResponse>(line);
                }
            }
            catch (Exception)
            {
                // When the API returns an error, it does not come back as a block, it returns a single character of text ("{").
                // In this instance, read through the rest of the response, which should be a complete object to parse.
                line += await reader.ReadToEndAsync();
                block = JsonSerializer.Deserialize<TResponse>(line);
            }


            if (null != block)
            {
                if (typeof(TResponse) == typeof(ChatCompletionCreateResponse))
                {
                    ctx.Process(block as ChatCompletionCreateResponse ?? throw new InvalidOperationException());
                }

                if (!ctx.IsFnAssemblyActive)
                {
                    block.HttpStatusCode = httpStatusCode;
                    block.HeaderValues = headerValues;
                    block.StreamEvent = tempStreamEvent;
                    tempStreamEvent = null;
                    yield return block;
                }
            }
        }
    }

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
                    if (t.FunctionCall != null && t.Type == ToolCallTypeEnum.Function)
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
                    using var argumentsList = ExtractArgsSoFar().GetEnumerator();
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
                firstChoice.Message.Role ??= ChatMessageRoleEnum.Assistant;
                firstChoice.Message.ToolCalls = new List<ToolCall>(_deltaFnCallList);
                _deltaFnCallList.Clear();
            }

            // Returns true if we're actively streaming, and also have a partial tool call main item ( id != (null | "")) of type "function" in the response
            bool IsStreamingFunctionCall()
            {
                return firstChoice.FinishReason == null && // actively streaming, is a tool call main item, and have a function call
                       firstChoice.Message?.ToolCalls?.Count > 0 && (firstChoice.Message?.ToolCalls.Any(t => t.FunctionCall != null && !string.IsNullOrEmpty(t.Id) && t.Type == ToolCallTypeEnum.Function) ?? false);
            }

            (int index, string? id, ToolCallTypeEnum? type) GetToolCallMetadata()
            {
                var tc = block.Choices?.FirstOrDefault()?.Message?.ToolCalls?.Where(t => t.FunctionCall != null).Select(t => t).FirstOrDefault();

                return tc switch
                {
                    not null => (tc.Index, tc.Id, tc.Type),
                    _ => (-1, default, default)
                };
            }

            IEnumerable<string> ExtractArgsSoFar()
            {
                var toolCalls = block.Choices?.FirstOrDefault()?.Message?.ToolCalls;

                if (toolCalls != null)
                {
                    var functionCallList = toolCalls.Where(t => t.FunctionCall != null).Select(t => t.FunctionCall);

                    foreach (var functionCall in functionCallList)
                    {
                        yield return functionCall!.Arguments ?? "";
                    }
                }
            }
        }
    }
}