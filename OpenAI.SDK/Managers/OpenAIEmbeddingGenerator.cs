using Microsoft.Extensions.AI;

namespace OpenAI.Managers;

public partial class OpenAIService : IEmbeddingGenerator<string, Embedding<float>>
{
    private EmbeddingGeneratorMetadata? _embeddingMetadata;

    EmbeddingGeneratorMetadata IEmbeddingGenerator<string, Embedding<float>>.Metadata =>
        _embeddingMetadata ??= new(nameof(OpenAIService), _httpClient.BaseAddress, _defaultModelId);

    TService? IEmbeddingGenerator<string, Embedding<float>>.GetService<TService>(object? key) where TService : class =>
        this as TService;

    async Task<GeneratedEmbeddings<Embedding<float>>> IEmbeddingGenerator<string, Embedding<float>>.GenerateAsync(IEnumerable<string> values, EmbeddingGenerationOptions? options, CancellationToken cancellationToken)
    {
        var response = await this.Embeddings.CreateEmbedding(new()
        {
            Model = options?.ModelId ?? _defaultModelId,
            Dimensions = options?.Dimensions,
            InputAsList = values.ToList(),
        }, cancellationToken);

        if (!response.Successful)
        {
            throw new InvalidOperationException(response.Error is { } error ?
                $"{response.Error.Code}: {response.Error.Message}" :
                "Unknown error");
        }

        return new(response.Data.Select(e => new Embedding<float>(e.Embedding.Select(d => (float)d).ToArray()) { ModelId = response.Model }))
        {
            Usage = response.Usage is { } usage ? new()
            {
                InputTokenCount = usage.PromptTokens,
                OutputTokenCount = usage.CompletionTokens,
                TotalTokenCount = usage.TotalTokens,
            } : null,
        };
    }
}