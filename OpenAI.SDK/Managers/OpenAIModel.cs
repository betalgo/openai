using System.Net.Http.Json;
using OpenAI.Interfaces;
using OpenAI.ObjectModels.ResponseModels.ModelResponseModels;

namespace OpenAI.Managers;

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