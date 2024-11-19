using Betalgo.Ranul.OpenAI.ObjectModels.RequestModels;
using Betalgo.Ranul.OpenAI.ObjectModels.SharedModels;

namespace Betalgo.Ranul.OpenAI.Interfaces;

public interface IThreadService
{
    /// <summary>
    ///     Create a thread.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<ThreadResponse> ThreadCreate(ThreadCreateRequest? request = null, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Retrieves a thread.
    /// </summary>
    /// <param name="threadId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<ThreadResponse> ThreadRetrieve(string threadId, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Delete a thread.
    /// </summary>
    /// <param name="threadId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<DeletionStatusResponse> ThreadDelete(string threadId, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Modifies a thread.
    /// </summary>
    Task<ThreadResponse> ModifyThread(string threadId, ModifyThreadRequest requestBody, CancellationToken cancellationToken = default);
}