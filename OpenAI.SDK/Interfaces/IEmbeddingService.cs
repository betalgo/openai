using OpenAI.GPT3.ObjectModels.RequestModels;
using OpenAI.GPT3.ObjectModels.ResponseModels;

namespace OpenAI.GPT3.Interfaces;

/// <summary>
///     Creates an embedding vector representing the input text.
/// </summary>
public interface IEmbeddingService
{
    /// <summary>
    ///     Creates a new embedding for the provided input and parameters.
    /// </summary>
    /// <param name="createEmbeddingModel"></param>
    /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
    /// <returns></returns>
    Task<EmbeddingCreateResponse> CreateEmbedding(EmbeddingCreateRequest createEmbeddingModel, CancellationToken cancellationToken = default);
}