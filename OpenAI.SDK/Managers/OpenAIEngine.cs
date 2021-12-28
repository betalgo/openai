using OpenAI.SDK.Extensions;
using OpenAI.SDK.Interfaces;
using OpenAI.SDK.Models.ResponseModels.EngineResponseModels;

namespace OpenAI.SDK.Managers;

public partial class OpenAISdk : IEngine
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