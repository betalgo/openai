using OpenAI.GPT3.Extensions;
using OpenAI.GPT3.Interfaces;
using OpenAI.GPT3.ObjectModels.RequestModels;
using OpenAI.GPT3.ObjectModels.ResponseModels;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Net.Http.Json;

namespace OpenAI.GPT3.Managers;

public partial class OpenAIService : ICompletionService
{
    public async Task<CompletionCreateResponse> CreateCompletion(CompletionCreateRequest createCompletionRequest, string? modelId = null)
    {
        createCompletionRequest.ProcessModelId(modelId, _defaultModelId);
        return await _httpClient.PostAndReadAsAsync<CompletionCreateResponse>(_endpointProvider.CompletionCreate(), createCompletionRequest);
    }
    
    public async IAsyncEnumerable<CompletionCreateResponse> CreateCompletionAsStream(CompletionCreateRequest createCompletionRequest, string? modelId = null)
    {
        // Mark the request as streaming and include a logit bias
        createCompletionRequest.Stream = true;

        // Send the request to the CompletionCreate endpoint
        createCompletionRequest.ProcessModelId(modelId, _defaultModelId);

        var request = await _httpClient.PostAsJsonAsync(_endpointProvider.CompletionCreate(), createCompletionRequest, new JsonSerializerOptions()
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault
        });

        // Read the response as a stream
        await using var stream = await request.Content.ReadAsStreamAsync();
        using var reader = new StreamReader(stream);
        // Continuously read the stream until the end of it
        while (!reader.EndOfStream)
        {
            var line = await reader.ReadLineAsync();
            // Skip empty lines
            if (string.IsNullOrEmpty(line)) continue;

            line = line.Replace("data: ", string.Empty);

            // Exit the loop if the stream is done
            if (line.Contains("[DONE]")) break;

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
            if (null != block) yield return block;
        }
    }
}
        