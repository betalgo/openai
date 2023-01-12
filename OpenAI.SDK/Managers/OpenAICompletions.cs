using OpenAI.GPT3.Extensions;
using OpenAI.GPT3.Interfaces;
using OpenAI.GPT3.ObjectModels.RequestModels;
using OpenAI.GPT3.ObjectModels.ResponseModels;

namespace OpenAI.GPT3.Managers;

public partial class OpenAIService : ICompletionService
{
    public async Task<CompletionCreateResponse> CreateCompletion(CompletionCreateRequest createCompletionRequest, string? engineId = null)
    {
        // if I don't have an engine I use the new endpoint
        CompletionCreateResponse response =
            !string.IsNullOrEmpty(createCompletionRequest.Model)
                        ? await _httpClient.PostAndReadAsAsync<CompletionCreateResponse>(_endpointProvider.CompletionCreate(), createCompletionRequest)
                        : await _httpClient.PostAndReadAsAsync<CompletionCreateResponse>(_endpointProvider.CompletionCreate(ProcessEngineId(engineId)), createCompletionRequest);

        return response;
    }
}