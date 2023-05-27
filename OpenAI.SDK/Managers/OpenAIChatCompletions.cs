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
                yield return block;
            }
        }
    }
}