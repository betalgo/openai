using OpenAI.GPT3.Extensions;
using OpenAI.GPT3.Interfaces;
using OpenAI.GPT3.Models.ResponseModels.EngineResponseModels;

namespace OpenAI.GPT3.Managers;

public partial class OpenAIService : IEngine
{
    public async Task<EngineListResponse> EngineList()
    {
        return await _httpClient.GetFromJsonAsync<EngineListResponse>(_endpointProvider.ListEngines());
    }

    public async Task<EngineRetrieveResponse> EngineRetrieve(string? engineId = null)
    {
        return await _httpClient.GetFromJsonAsync<EngineRetrieveResponse>(_endpointProvider.RetrieveEngine(ProcessEngineId(engineId)));
    }
}