using System.Net.Http.Json;
using Betalgo.OpenAI.Extensions;
using Betalgo.OpenAI.Interfaces;
using Betalgo.OpenAI.ObjectModels.RequestModels;
using Betalgo.OpenAI.ObjectModels.ResponseModels.BatchResponseModel;

namespace Betalgo.OpenAI.Managers;

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