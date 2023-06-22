using OpenAI.ObjectModels;
using OpenAI.ObjectModels.RequestModels;
using OpenAI.ObjectModels.ResponseModels;

namespace OpenAI.Interfaces;

/// <summary>
///     Given a prompt and an instruction, the model will return an edited version of the prompt.
/// </summary>
public interface IEditService
{
    /// <summary>
    ///     Creates a new edit for the provided input, instruction, and parameters
    /// </summary>
    /// <param name="editCreate"></param>
    /// <param name="modelId">The ID of the model to use for this request</param>
    /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
    /// <returns></returns>
    Task<EditCreateResponse> CreateEdit(EditCreateRequest editCreate, string? modelId = null, CancellationToken cancellationToken = default);
}

public static class IEditServiceExtension
{
    /// <summary>
    ///     Creates a new edit for the provided input, instruction, and parameters
    /// </summary>
    /// <param name="service"></param>
    /// <param name="editCreate"></param>
    /// <param name="modelId">The ID of the model to use for this request</param>
    /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
    /// <returns></returns>
    public static Task<EditCreateResponse> Edit(this IEditService service, EditCreateRequest editCreate, Models.Model modelId, CancellationToken cancellationToken = default)
    {
        return service.CreateEdit(editCreate, modelId.EnumToString(), cancellationToken);
    }
}