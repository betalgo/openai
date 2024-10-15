using Betalgo.OpenAI.Extensions;
using Betalgo.OpenAI.Interfaces;
using Betalgo.OpenAI.ObjectModels.RequestModels;
using Betalgo.OpenAI.ObjectModels.ResponseModels;

namespace Betalgo.OpenAI.Managers;

public partial class OpenAIService : IEmbeddingService
{
    public async Task<EmbeddingCreateResponse> CreateEmbedding(EmbeddingCreateRequest createEmbeddingRequest, CancellationToken cancellationToken = default)
    {
        return await _httpClient.PostAndReadAsAsync<EmbeddingCreateResponse>(_endpointProvider.EmbeddingCreate(), createEmbeddingRequest, cancellationToken);
    }
}