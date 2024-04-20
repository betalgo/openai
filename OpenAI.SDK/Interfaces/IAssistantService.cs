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
    /// <param name="modelId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<AssistantResponse> AssistantCreate(AssistantCreateRequest request, string? modelId = null, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Create an assistant file by attaching a File to an assistant.
    /// </summary>
    /// <param name="assistantId"></param>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<AssistantFileResponse> AssistantFileCreate(string assistantId, AssistantFileCreateRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Returns a list of assistants.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<AssistantListResponse> AssistantList(AssistantListRequest? request = null, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Returns a list of assistant files.
    /// </summary>
    /// <param name="assistantId"></param>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<AssistantFileListResponse> AssistantFileList(string assistantId, AssistantFileListRequest? request = null, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Retrieves an assistant.
    /// </summary>
    /// <param name="assistantId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<AssistantResponse> AssistantRetrieve(string assistantId, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Retrieves an AssistantFile.
    /// </summary>
    /// <param name="assistantId"></param>
    /// <param name="fileId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<AssistantFileResponse> AssistantFileRetrieve(string assistantId, string fileId, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Modifies an assistant.
    /// </summary>
    /// <param name="assistantId"></param>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<AssistantResponse> AssistantModify(string assistantId, AssistantModifyRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Delete an assistant.
    /// </summary>
    /// <param name="assistantId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<DeletionStatusResponse> AssistantDelete(string assistantId, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Delete an assistant file.
    /// </summary>
    /// <param name="assistantId"></param>
    /// <param name="fileId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<DeletionStatusResponse> AssistantFileDelete(string assistantId, string fileId, CancellationToken cancellationToken = default);
}