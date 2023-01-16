using OpenAI.GPT3.Extensions;
using OpenAI.GPT3.Interfaces;
using OpenAI.GPT3.ObjectModels.RequestModels;
using OpenAI.GPT3.ObjectModels.ResponseModels;
using System.Text;

namespace OpenAI.GPT3.Managers;

public partial class OpenAIService : ICompletionService
{
    public async Task<CompletionCreateResponse> CreateCompletion(CompletionCreateRequest createCompletionRequest, string? modelId = null)
    {
        createCompletionRequest.ProcessModelId(modelId, _defaultModelId);

        return await _httpClient.PostAndReadAsAsync<CompletionCreateResponse>(_endpointProvider.CompletionCreate(), createCompletionRequest);
    }



    public async IAsyncEnumerable<CompletionCreateResponse> CreateCompletionStream(CompletionCreateRequest createCompletionRequest, string? engineId = null)
    {
        // Mark the request as streaming and include a logit bias
        createCompletionRequest.Stream = true;

        // Serialize the request and create a StringContent
        var content = new StringContent(System.Text.Json.JsonSerializer.Serialize(createCompletionRequest), Encoding.UTF8, "application/json");

        // Send the request to the CompletionCreate endpoint
        var request = await _httpClient.PostAsync(_endpointProvider.CompletionCreate(ProcessEngineId(engineId)), content);

        // Read the response as a stream
        using (var stream = await request.Content.ReadAsStreamAsync())
        using (var reader = new StreamReader(stream))
        {
            // Continuously read the stream until the end of it
            while (!reader.EndOfStream)
            {
                var line = await reader.ReadLineAsync();
                // Skip empty lines
                if (string.IsNullOrEmpty(line)) continue;

                line = line.Replace("data: ", string.Empty);

                // Exit the loop if the stream is done
                if (line.Contains("[DONE]")) break;

                CompletionCreateResponse block = null;
                try
                {
                    // When the response is good, each line is a serializable CompletionCreateRequest
                    block = System.Text.Json.JsonSerializer.Deserialize<CompletionCreateResponse>(line);
                }
                catch (Exception)
                {
                    // When the API returns an error, it does not come back as a block, it returns a single character of text ("{").
                    // In this instance, read through the rest of the response, which should be a complete object to parse.
                    line += await reader.ReadToEndAsync();
                    block = System.Text.Json.JsonSerializer.Deserialize<CompletionCreateResponse>(line);
                }
                if (null != block) yield return block;
            }
        }
    }
}
        