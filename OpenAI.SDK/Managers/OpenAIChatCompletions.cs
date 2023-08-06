using System.Runtime.CompilerServices;
using System.Text.Json;
using OpenAI.Extensions;
using OpenAI.Interfaces;
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
    public async IAsyncEnumerable<ChatCompletionCreateResponse> CreateCompletionAsStream(ChatCompletionCreateRequest chatCompletionCreateRequest, string? modelId = null,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        // Helper data in case we need to reassemble a multi-packet response
        ReassemblyContext ctx = new();

        // Mark the request as streaming
        chatCompletionCreateRequest.Stream = true;

        // Send the request to the CompletionCreate endpoint
        chatCompletionCreateRequest.ProcessModelId(modelId, _defaultModelId);

        using var response = _httpClient.PostAsStreamAsync(_endpointProvider.ChatCompletionCreate(), chatCompletionCreateRequest, cancellationToken);
        await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
        using var reader = new StreamReader(stream);

        // Continuously read the stream until the end of it
        while (!reader.EndOfStream)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var line = await reader.ReadLineAsync();
            // Skip empty lines
            if (string.IsNullOrEmpty(line))
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
                    yield return block;
                }
            }
        }
    }

    /// <summary>
    ///     This helper class attempts to reassemble a function call response
    ///     that was split up across several streamed chunks.
    ///     Note that this only works for the first message in each response,
    ///     and ignores the others; if OpenAI ever changes their response format
    ///     this will need to be adjusted.
    /// </summary>
    private class ReassemblyContext
    {
        private FunctionCall? FnCall;

        public bool IsFnAssemblyActive => FnCall != null;


        /// <summary>
        ///     Detects if a response block is a part of a multi-chunk
        ///     streamed function call response. As long as that's true,
        ///     it keeps accumulating block contents, and once function call
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
            var justStarted = false;

            // If we're not yet assembling, and we just got a streaming block that has a function_call segment,
            // this is the beginning of a function call assembly.
            // We're going to steal the partial message and squirrel it away for the time being.
            if (!IsFnAssemblyActive && isStreamingFnCall)
            {
                FnCall = firstChoice.Message.FunctionCall;
                firstChoice.Message.FunctionCall = null;
                justStarted = true;
            }

            // As long as we're assembling, keep on appending those args
            // (Skip the first one, because it was already processed in the block above)
            if (IsFnAssemblyActive && !justStarted)
            {
                FnCall.Arguments += ExtractArgsSoFar();
            }

            // If we were assembling and it just finished, fill this block with the info we've assembled, and we're done.
            if (IsFnAssemblyActive && !isStreamingFnCall)
            {
                firstChoice.Message ??= ChatMessage.FromAssistant(""); // just in case? not sure it's needed
                firstChoice.Message.FunctionCall = FnCall;
                FnCall = null;
            }

            // Returns true if we're actively streaming, and also have a partial function call in the response
            bool IsStreamingFunctionCall()
            {
                return firstChoice.FinishReason == null && // actively streaming, and
                       firstChoice.Message?.FunctionCall != null;
            } // have a function call

            string ExtractArgsSoFar()
            {
                return block.Choices?.FirstOrDefault()?.Message?.FunctionCall?.Arguments ?? "";
            }
        }
    }
}