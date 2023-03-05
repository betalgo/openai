using System.Net.Http.Json;
using OpenAI.GPT3.Interfaces;
using OpenAI.GPT3.ObjectModels.ResponseModels.ModelResponseModels;

namespace OpenAI.GPT3.Managers;

public partial class OpenAIService : IModelService
{
    public async Task<ModelListResponse> ListModel(CancellationToken cancellationToken = default)
    {
        return (await _httpClient.GetFromJsonAsync<ModelListResponse>(_endpointProvider.ModelsList(), cancellationToken))!;
    }

    public async Task<ModelRetrieveResponse> RetrieveModel(string model, CancellationToken cancellationToken = default)
    {
        return (await _httpClient.GetFromJsonAsync<ModelRetrieveResponse>(_endpointProvider.ModelRetrieve(model), cancellationToken))!;
    }
}