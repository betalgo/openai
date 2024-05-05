using OpenAI.Extensions;
using OpenAI.Interfaces;
using OpenAI.ObjectModels.RequestModels;
using OpenAI.ObjectModels.SharedModels;

namespace OpenAI.Managers;

public partial class OpenAIService : IThreadService
{
    /// <summary>
    ///     Create a thread.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task<ThreadResponse> ThreadCreate(ThreadCreateRequest? request = null, CancellationToken cancellationToken = default)
    {
        return await _httpClient.PostAndReadAsAsync<ThreadResponse>(_endpointProvider.ThreadCreate(), request, cancellationToken);
    }

    /// <summary>
    ///     Retrieves a thread.
    /// </summary>
    /// <param name="threadId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public async Task<ThreadResponse> ThreadRetrieve(string threadId, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(threadId))
        {
            throw new ArgumentNullException(nameof(threadId));
        }

        return await _httpClient.GetReadAsAsync<ThreadResponse>(_endpointProvider.ThreadRetrieve(threadId), cancellationToken);
    }

    /// <summary>
    ///     Delete a thread.
    /// </summary>
    /// <param name="threadId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public async Task<DeletionStatusResponse> ThreadDelete(string threadId, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(threadId))
        {
            throw new ArgumentNullException(nameof(threadId));
        }

        return await _httpClient.DeleteAndReadAsAsync<DeletionStatusResponse>(_endpointProvider.ThreadDelete(threadId), cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<ThreadResponse> ModifyThread(string threadId, ModifyThreadRequest requestBody, CancellationToken cancellationToken = default)
    {
        return await _httpClient.PostAndReadAsAsync<ThreadResponse>(_endpointProvider.ThreadModify(threadId), requestBody, cancellationToken);
    }
}