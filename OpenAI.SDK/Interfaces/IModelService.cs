using OpenAI.GPT3.ObjectModels.ResponseModels.ModelResponseModels;

namespace OpenAI.GPT3.Interfaces;

/// <summary>
///     List and describe the various models available in the API. You can refer to the
///     <a href="https://platform.openai.com/docs/models">Models</a> documentation to understand what models are available
///     and
///     the differences between them.
/// </summary>
public interface IModelService
{
    /// <summary>
    ///     Lists the currently available models, and provides basic information about each one such as the owner and
    ///     availability.
    /// </summary>
    /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
    /// <returns></returns>
    Task<ModelListResponse> ListModel(CancellationToken cancellationToken = default);

    /// <summary>
    ///     Retrieves a model instance, providing basic information about the model such as the owner and permissioning.
    /// </summary>
    /// <param name="model">The ID of the model to use for this request</param>
    /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
    /// <returns></returns>
    Task<ModelRetrieveResponse> RetrieveModel(string model, CancellationToken cancellationToken = default);
}