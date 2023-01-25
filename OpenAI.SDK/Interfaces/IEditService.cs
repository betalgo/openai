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
    /// <returns></returns>
    Task<EditCreateResponse> CreateEdit(EditCreateRequest editCreate, string? engineId = null);

}

public static class IEditServiceExtension
{
    /// <summary>
    ///     Creates a new edit for the provided input, instruction, and parameters
    /// </summary>
    /// <param name="service"></param>
    /// <param name="editCreate"></param>
    /// <param name="engineId">The ID of the engine to use for this request</param>
    /// <returns></returns>
    public static Task<EditCreateResponse> Edit(this IEditService service, EditCreateRequest editCreate, Models.Model engineId)
    {
        return service.CreateEdit(editCreate, engineId.EnumToString());
    }
}