using OpenAI.GPT3.ObjectModels;
using OpenAI.GPT3.ObjectModels.RequestModels;
using OpenAI.GPT3.ObjectModels.ResponseModels;

namespace OpenAI.GPT3.Interfaces;

/// <summary>
///     Given a prompt and an instruction, the model will return an edited version of the prompt.
/// </summary>
public interface IEditService
{
    /// <summary>
    ///     Creates a new edit for the provided input, instruction, and parameters
    /// </summary>
    /// <param name="editCreate"></param>
    /// <param name="engineId">The ID of the engine to use for this request</param>
    /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
    /// <returns></returns>
    Task<EditCreateResponse> CreateEdit(EditCreateRequest editCreate, string? engineId = null, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Creates a new edit for the provided input, instruction, and parameters
    /// </summary>
    /// <param name="editCreate"></param>
    /// <param name="engineId">The ID of the engine to use for this request</param>
    /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
    /// <returns></returns>
    Task<EditCreateResponse> Edit(EditCreateRequest editCreate, Models.Model engineId, CancellationToken cancellationToken = default)
    {
        return CreateEdit(editCreate, engineId.EnumToString(), cancellationToken);
    }
}