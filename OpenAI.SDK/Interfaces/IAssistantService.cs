using OpenAI.ObjectModels.RequestModels;
using OpenAI.ObjectModels.ResponseModels;
using OpenAI.ObjectModels.SharedModels;

namespace OpenAI.Interfaces;

public interface IAssistantService
{
    /// <summary>
    ///     Create an assistant with a model and instructions.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="modelId">ID of the model to use. You can use the List models API to see all of your available models, or see our Model overview for descriptions of them.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<AssistantResponse> AssistantCreate(AssistantCreateRequest request, string? modelId = null, CancellationToken cancellationToken = default);
    
    /// <summary>
    ///     Returns a list of assistants.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<AssistantListResponse> AssistantList(PaginationRequest? request = null, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Retrieves an assistant.
    /// </summary>
    /// <param name="assistantId">The ID of the assistant to retrieve.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<AssistantResponse> AssistantRetrieve(string assistantId, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Modifies an assistant.
    /// </summary>
    /// <param name="assistantId">The ID of the assistant to modify.</param>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<AssistantResponse> AssistantModify(string assistantId, AssistantModifyRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Delete an assistant.
    /// </summary>
    /// <param name="assistantId">The ID of the assistant to delete.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<DeletionStatusResponse> AssistantDelete(string assistantId, CancellationToken cancellationToken = default);
}