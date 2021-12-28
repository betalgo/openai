using OpenAI.GPT3.Extensions;
using OpenAI.GPT3.Interfaces;
using OpenAI.GPT3.Models.RequestModels;
using OpenAI.GPT3.Models.ResponseModels;

namespace OpenAI.GPT3.Managers;

public partial class OpenAISdk : ICompletion
{
    public async Task<CompletionCreateResponse> Create(CompletionCreateRequest createCompletionRequest, string? engineId = null)
    {
        return await _httpClient.PostAndReadAsAsync<CompletionCreateResponse>(_endpointProvider.CreateCompletion(ProcessEngineId(engineId)), createCompletionRequest);
    }
}