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
    Task<MessageResponse> MessageCreate(string threadId, MessageCreateRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Returns a list of messages for a given thread.
    /// </summary>
    /// <param name="threadId"></param>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<MessageListResponse> MessageList(string threadId, MessageListRequest? request = null, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Retrieve a message.
    /// </summary>
    /// <param name="threadId"></param>
    /// <param name="messageId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<MessageResponse> MessageRetrieve(string threadId, string messageId, CancellationToken cancellationToken = default);
}