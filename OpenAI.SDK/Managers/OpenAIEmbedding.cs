using OpenAI.Extensions;
using OpenAI.Interfaces;
using OpenAI.ObjectModels.RequestModels;
using OpenAI.ObjectModels.ResponseModels;

namespace OpenAI.Managers;

public partial class OpenAIService : IEmbeddingService
{
    public async Task<EmbeddingCreateResponse> CreateEmbedding(EmbeddingCreateRequest createEmbeddingRequest, CancellationToken cancellationToken = default)
    {
        return await _httpClient.PostAndReadAsAsync<EmbeddingCreateResponse>(_endpointProvider.EmbeddingCreate(), createEmbeddingRequest, cancellationToken);
    }
}