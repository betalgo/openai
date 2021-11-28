using OpenAI.SDK.Models.RequestModels;
using OpenAI.SDK.Models.ResponseModels;

namespace OpenAI.SDK.Interfaces;

/// <summary>
///     Given a prompt, the model will return one or more predicted completions, and can also return the probabilities of
///     alternative tokens at each position.
/// </summary>
public interface ICompletions
{
    /// <summary>
    ///     Creates a new completion for the provided prompt and parameters
    /// </summary>
    /// <param name="engineId">The ID of the engine to use for this request</param>
    /// <param name="createCompletionModel"></param>
    /// <returns></returns>
    Task<CreateCompletionResponse?> CreateCompletion(string engineId, CreateCompletionRequest createCompletionModel);
}