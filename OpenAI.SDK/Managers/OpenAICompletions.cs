using OpenAI.SDK.Extensions;
using OpenAI.SDK.Interfaces;
using OpenAI.SDK.Models.RequestModels;
using OpenAI.SDK.Models.ResponseModels;

namespace OpenAI.SDK.Managers;

public partial class OpenAISdk : ICompletion
{
    public async Task<CompletionCreateResponse> Create(CompletionCreateRequest createCompletionRequest, string? engineId = null)
    {
        return await _httpClient.PostAndReadAsAsync<CompletionCreateResponse>(_endpointProvider.CreateCompletion(ProcessEngineId(engineId)), createCompletionRequest);
    }
}