using System.Net.Http.Json;
using Betalgo.Ranul.OpenAI.Interfaces;
using Betalgo.Ranul.OpenAI.ObjectModels.ResponseModels.ModelResponseModels;

namespace Betalgo.Ranul.OpenAI.Managers;

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

    public async Task<ModelDeleteResponse?> DeleteModel(string modelId, CancellationToken cancellationToken = default)
    {
        return await (await _httpClient.DeleteAsync(_endpointProvider.ModelsDelete(modelId), cancellationToken)).Content.ReadFromJsonAsync<ModelDeleteResponse>(cancellationToken: cancellationToken);
    }
}