using OpenAI.ObjectModels;
using OpenAI.ObjectModels.RequestModels;
using OpenAI.ObjectModels.ResponseModels;

namespace OpenAI.Interfaces;

/// <summary>
///     Given a prompt, the model will return one or more predicted completions, and can also return the probabilities of
///     alternative tokens at each position.
/// </summary>
public interface ICompletionService
{
    /// <summary>
    ///     Creates a new completion for the provided prompt and parameters
    /// </summary>
    /// <param name="modelId">The ID of the model to use for this request</param>
    /// <param name="createCompletionModel"></param>
    /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
    /// <returns></returns>
    Task<CompletionCreateResponse> CreateCompletion(CompletionCreateRequest createCompletionModel, string? modelId = null, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Creates a new completion for the provided prompt and parameters and returns a stream of CompletionCreateRequests
    /// </summary>
    /// <param name="modelId">The ID of the model to use for this request</param>
    /// <param name="createCompletionModel"></param>
    /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
    /// <returns></returns>
    IAsyncEnumerable<CompletionCreateResponse> CreateCompletionAsStream(CompletionCreateRequest createCompletionModel, string? modelId = null, CancellationToken cancellationToken = default);
}

public static class ICompletionServiceExtension
{
    /// <summary>
    ///     Creates a new completion for the provided prompt and parameters
    /// </summary>
    /// <param name="service"></param>
    /// <param name="createCompletionModel"></param>
    /// <param name="modelId">The ID of the model to use for this request</param>
    /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
    /// <returns></returns>
    public static Task<CompletionCreateResponse> Create(this ICompletionService service, CompletionCreateRequest createCompletionModel, Models.Model modelId, CancellationToken cancellationToken = default)
    {
        return service.CreateCompletion(createCompletionModel, modelId.EnumToString(), cancellationToken);
    }
}