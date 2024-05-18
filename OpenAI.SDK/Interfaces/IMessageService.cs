using OpenAI.ObjectModels.RequestModels;
using OpenAI.ObjectModels.ResponseModels;
using OpenAI.ObjectModels.SharedModels;

namespace OpenAI.Interfaces;

public interface IMessageService
{
    /// <summary>
    ///     Create a message.
    /// </summary>
    /// <param name="threadId"></param>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<MessageResponse> CreateMessage(string threadId, MessageCreateRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Returns a list of messages for a given thread.
    /// </summary>
    /// <param name="threadId"></param>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<MessageListResponse> ListMessages(string threadId, PaginationRequest? request = null, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Retrieve a message.
    /// </summary>
    Task<MessageResponse> RetrieveMessage(string threadId, string messageId, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Modifies a message.
    /// </summary>
    Task<MessageResponse> ModifyMessage(string threadId, string messageId, ModifyMessageRequest requestBody, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Deletes a message.
    /// </summary>
    /// <param name="threadId">The ID of the thread to which this message belongs.</param>
    /// <param name="messageId">The ID of the message to delete.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<DeletionStatusResponse> DeleteMessage(string threadId, string messageId, CancellationToken cancellationToken = default);
}