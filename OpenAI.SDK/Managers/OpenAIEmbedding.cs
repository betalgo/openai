using Betalgo.Ranul.OpenAI.Extensions;
using Betalgo.Ranul.OpenAI.Interfaces;
using Betalgo.Ranul.OpenAI.ObjectModels.RequestModels;
using Betalgo.Ranul.OpenAI.ObjectModels.ResponseModels;

namespace Betalgo.Ranul.OpenAI.Managers;

public partial class OpenAIService : IEmbeddingService
{
    public async Task<EmbeddingCreateResponse> CreateEmbedding(EmbeddingCreateRequest createEmbeddingRequest, CancellationToken cancellationToken = default)
    {
        return await _httpClient.PostAndReadAsAsync<EmbeddingCreateResponse>(_endpointProvider.EmbeddingCreate(), createEmbeddingRequest, cancellationToken);
    }
}