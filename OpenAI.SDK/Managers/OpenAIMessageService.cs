using OpenAI.Extensions;
using OpenAI.Interfaces;
using OpenAI.ObjectModels.RequestModels;
using OpenAI.ObjectModels.ResponseModels;
using OpenAI.ObjectModels.SharedModels;

namespace OpenAI.Managers;

public partial class OpenAIService : IMessageService
{
    /// <summary>
    ///     Create a message.
    /// </summary>
    /// <param name="threadId"></param>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public async Task<MessageResponse> CreateMessage(string threadId, MessageCreateRequest request, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(threadId))
        {
            throw new ArgumentNullException(nameof(threadId));
        }

        return await _httpClient.PostAndReadAsAsync<MessageResponse>(_endpointProvider.MessageCreate(threadId), request, cancellationToken);
    }

    /// <summary>
    ///     Returns a list of messages for a given thread.
    /// </summary>
    /// <param name="threadId"></param>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task<MessageListResponse> ListMessages(string threadId, PaginationRequest? request = null, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(threadId))
        {
            throw new ArgumentNullException(nameof(threadId));
        }

        return await _httpClient.GetReadAsAsync<MessageListResponse>(_endpointProvider.MessageList(threadId, request), cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<MessageResponse> RetrieveMessage(string threadId, string messageId, CancellationToken cancellationToken = default)
    {
        return await _httpClient.GetReadAsAsync<MessageResponse>(_endpointProvider.MessageRetrieve(threadId, messageId), cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<MessageResponse> ModifyMessage(string threadId, string messageId, ModifyMessageRequest requestBody, CancellationToken cancellationToken = default)
    {
        return await _httpClient.PostAndReadAsAsync<MessageResponse>(_endpointProvider.MessageModify(threadId, messageId), requestBody, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<DeletionStatusResponse> DeleteMessage(string threadId, string messageId, CancellationToken cancellationToken = default)
    {
        return await _httpClient.DeleteAndReadAsAsync<DeletionStatusResponse>(_endpointProvider.MessageDelete(threadId, messageId), cancellationToken);
    }
}