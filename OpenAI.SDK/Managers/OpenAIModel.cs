using OpenAI.GPT3.Extensions;
using OpenAI.GPT3.Interfaces;
using OpenAI.GPT3.ObjectModels.ResponseModels.ModelResponseModels;

namespace OpenAI.GPT3.Managers;

public partial class OpenAIService : IModelService
{
    public async Task<ModelListResponse> ModelList()
    {
        return await _httpClient.GetFromJsonAsync<ModelListResponse>(_endpointProvider.ListModels());
    }

    public async Task<ModelRetrieveResponse> ModelRetrieve(string model)
    {
        return await _httpClient.GetFromJsonAsync<ModelRetrieveResponse>(_endpointProvider.RetrieveModel(ProcessEngineId(model)));
    }
}