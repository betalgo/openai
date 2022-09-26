using OpenAI.GPT3.ObjectModels;
using OpenAI.GPT3.ObjectModels.RequestModels;
using OpenAI.GPT3.ObjectModels.ResponseModels;

namespace OpenAI.GPT3.Interfaces;

/// <summary>
///     Given a prompt, the model will return one or more predicted completions, and can also return the probabilities of
///     alternative tokens at each position.
/// </summary>
public interface ICompletionService
{
    /// <summary>
    ///     Creates a new completion for the provided prompt and parameters
    /// </summary>
    /// <param name="engineId">The ID of the engine to use for this request</param>
    /// <param name="createCompletionModel"></param>
    /// <returns></returns>
    Task<CompletionCreateResponse> CreateCompletion(CompletionCreateRequest createCompletionModel, string? engineId = null);

    /// <summary>
    ///     Creates a new completion for the provided prompt and parameters
    /// </summary>
    /// <param name="createCompletionModel"></param>
    /// <param name="engineId">The ID of the engine to use for this request</param>
    /// <returns></returns>
    Task<CompletionCreateResponse> Create(CompletionCreateRequest createCompletionModel, Models.Model engineId)
    {
        return CreateCompletion(createCompletionModel, engineId.EnumToString());
    }
}