using System.Net.Http.Json;
using Betalgo.Ranul.OpenAI.Extensions;
using Betalgo.Ranul.OpenAI.Interfaces;
using Betalgo.Ranul.OpenAI.ObjectModels.RequestModels;
using Betalgo.Ranul.OpenAI.ObjectModels.ResponseModels.BatchResponseModel;

namespace Betalgo.Ranul.OpenAI.Managers;

public partial class OpenAIService : IBatchService
{
    /// <inheritdoc />
    public async Task<BatchResponse> BatchCreate(BatchCreateRequest request, CancellationToken cancellationToken = default)
    {
        return await _httpClient.PostAndReadAsAsync<BatchResponse>(_endpointProvider.BatchCreate(), request, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<BatchResponse?> BatchRetrieve(string batchId, CancellationToken cancellationToken = default)
    {
        return await _httpClient.GetFromJsonAsync<BatchResponse>(_endpointProvider.BatchRetrieve(batchId), cancellationToken);
    }

    /// <inheritdoc />
    public async Task<BatchResponse> BatchCancel(string batchId, CancellationToken cancellationToken = default)
    {
        return await _httpClient.PostAndReadAsAsync<BatchResponse>(_endpointProvider.BatchCancel(batchId), null, cancellationToken);
    }
}