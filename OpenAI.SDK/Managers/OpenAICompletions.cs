using System.Runtime.CompilerServices;
using System.Text.Json;
using OpenAI.Extensions;
using OpenAI.Interfaces;
using OpenAI.ObjectModels.RequestModels;
using OpenAI.ObjectModels.ResponseModels;

namespace OpenAI.Managers;

public partial class OpenAIService : ICompletionService
{
    /// <inheritdoc />
    public async Task<CompletionCreateResponse> CreateCompletion(CompletionCreateRequest createCompletionRequest, string? modelId = null, CancellationToken cancellationToken = default)
    {
        createCompletionRequest.ProcessModelId(modelId, _defaultModelId);
        return await _httpClient.PostAndReadAsAsync<CompletionCreateResponse>(_endpointProvider.CompletionCreate(), createCompletionRequest, cancellationToken);
    }

    /// <inheritdoc />
    public async IAsyncEnumerable<CompletionCreateResponse> CreateCompletionAsStream(CompletionCreateRequest createCompletionRequest, string? modelId = null, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        // Mark the request as streaming
        createCompletionRequest.Stream = true;

        // Send the request to the CompletionCreate endpoint
        createCompletionRequest.ProcessModelId(modelId, _defaultModelId);

        using var response = _httpClient.PostAsStreamAsync(_endpointProvider.CompletionCreate(), createCompletionRequest, cancellationToken);
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

            CompletionCreateResponse? block;
            try
            {
                // When the response is good, each line is a serializable CompletionCreateRequest
                block = JsonSerializer.Deserialize<CompletionCreateResponse>(line);
            }
            catch (Exception)
            {
                // When the API returns an error, it does not come back as a block, it returns a single character of text ("{").
                // In this instance, read through the rest of the response, which should be a complete object to parse.
                line += await reader.ReadToEndAsync();
                block = JsonSerializer.Deserialize<CompletionCreateResponse>(line);
            }


            if (null != block)
            {
                yield return block;
            }
        }
    }
}