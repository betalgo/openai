using Betalgo.Ranul.OpenAI.ObjectModels.RealtimeModels;

namespace Betalgo.Ranul.OpenAI.Managers;

public partial class OpenAIRealtimeService
{
    public IOpenAIRealtimeServiceClientEvents ClientEvents => _clientEvents;

    private class ClientEventsImplementation(OpenAIRealtimeService client) : IOpenAIRealtimeServiceClientEvents
    {
        private readonly ConversationImplementation _conversation = new(client);
        private readonly InputAudioBufferImplementation _inputAudioBuffer = new(client);
        private readonly ResponseImplementation _response = new(client);
        private readonly SessionImplementation _session = new(client);

        public IOpenAIRealtimeServiceClientEvents.ISession Session => _session;
        public IOpenAIRealtimeServiceClientEvents.IInputAudioBuffer InputAudioBuffer => _inputAudioBuffer;
        public IOpenAIRealtimeServiceClientEvents.IConversation Conversation => _conversation;
        public IOpenAIRealtimeServiceClientEvents.IResponse Response => _response;
    }
}

public partial class OpenAIRealtimeService
{
    public IOpenAIRealtimeServiceServerEvents ServerEvents => _serverEvents;

    private class SessionImplementation(OpenAIRealtimeService client) : IOpenAIRealtimeServiceClientEvents.ISession
    {
        public Task Update(SessionUpdateRequest request, CancellationToken cancellationToken = default)
        {
            if (!client.IsConnected)
                throw new InvalidOperationException("Not connected to Realtime API.");

            return client.SendEvent(request, cancellationToken);
        }
    }

    private class InputAudioBufferImplementation(OpenAIRealtimeService client) : IOpenAIRealtimeServiceClientEvents.IInputAudioBuffer
    {
        public Task Append(ReadOnlyMemory<byte> audioData, CancellationToken cancellationToken = default)
        {
            if (!client.IsConnected)
                throw new InvalidOperationException("Not connected to Realtime API.");

            var request = new AudioBufferAppendRequest
            {
                Audio = Convert.ToBase64String(audioData.ToArray())
            };
            return client.SendEvent(request, cancellationToken);
        }

        public Task Commit(CancellationToken cancellationToken = default)
        {
            if (!client.IsConnected)
                throw new InvalidOperationException("Not connected to Realtime API.");

            return client.SendEvent(new AudioBufferCommitRequest(), cancellationToken);
        }

        public Task Clear(CancellationToken cancellationToken = default)
        {
            if (!client.IsConnected)
                throw new InvalidOperationException("Not connected to Realtime API.");

            return client.SendEvent(new AudioBufferClearRequest(), cancellationToken);
        }
    }

    private class ConversationImplementation(OpenAIRealtimeService client) : IOpenAIRealtimeServiceClientEvents.IConversation
    {
        private readonly ConversationItemImplementation _item = new(client);

        public IOpenAIRealtimeServiceClientEvents.IConversation.IItem Item => _item;

        private class ConversationItemImplementation(OpenAIRealtimeService client) : IOpenAIRealtimeServiceClientEvents.IConversation.IItem
        {
            public Task Create(ConversationItemCreateRequest request, CancellationToken cancellationToken = default)
            {
                if (!client.IsConnected)
                    throw new InvalidOperationException("Not connected to Realtime API.");

                return client.SendEvent(request, cancellationToken);
            }

            public Task Truncate(string itemId, int contentIndex, int audioEndMs, CancellationToken cancellationToken = default)
            {
                if (!client.IsConnected)
                    throw new InvalidOperationException("Not connected to Realtime API.");

                var request = new ConversationItemTruncateRequest
                {
                    ItemId = itemId,
                    ContentIndex = contentIndex,
                    AudioEndMs = audioEndMs
                };
                return client.SendEvent(request, cancellationToken);
            }

            public Task Delete(string itemId, CancellationToken cancellationToken = default)
            {
                if (!client.IsConnected)
                    throw new InvalidOperationException("Not connected to Realtime API.");

                var request = new ConversationItemDeleteRequest
                {
                    ItemId = itemId
                };
                return client.SendEvent(request, cancellationToken);
            }
        }
    }

    private class ResponseImplementation(OpenAIRealtimeService client) : IOpenAIRealtimeServiceClientEvents.IResponse
    {
        public Task Create(CancellationToken cancellationToken = default)
        {
            return Create(new(), cancellationToken);
        }

        public Task Create(ResponseCreateRequest request, CancellationToken cancellationToken = default)
        {
            if (!client.IsConnected)
                throw new InvalidOperationException("Not connected to Realtime API.");

            return client.SendEvent(request, cancellationToken);
        }

        public Task Cancel(CancellationToken cancellationToken = default)
        {
            if (!client.IsConnected)
                throw new InvalidOperationException("Not connected to Realtime API.");

            return client.SendEvent(new ResponseCancelRequest(), cancellationToken);
        }
    }
}


/// <summary>
/// Interface for interacting with the OpenAI Realtime WebSocket server client events.
/// </summary>
public interface IOpenAIRealtimeServiceClientEvents
{
    /// <summary>
    /// Provides access to session-related operations.
    /// </summary>
    ISession Session { get; }

    /// <summary>
    /// Provides access to input audio buffer operations.
    /// </summary>
    IInputAudioBuffer InputAudioBuffer { get; }

    /// <summary>
    /// Provides access to conversation-related operations.
    /// </summary>
    IConversation Conversation { get; }

    /// <summary>
    /// Provides access to response-related operations.
    /// </summary>
    IResponse Response { get; }

    /// <summary>
    /// Interface for managing session configuration.
    /// </summary>
    public interface ISession
    {
        /// <summary>
        /// Updates the session's default configuration.
        /// </summary>
        /// <param name="request">Configuration settings including modalities, instructions, voice, audio formats, tools, and more.</param>
        /// <param name="cancellationToken">Optional cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task Update(SessionUpdateRequest request, CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// Interface for managing input audio buffer operations.
    /// </summary>
    public interface IInputAudioBuffer
    {
        /// <summary>
        /// Appends audio bytes to the input audio buffer.
        /// </summary>
        /// <param name="audioData">The audio data to append.</param>
        /// <param name="cancellationToken">Optional cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task Append(ReadOnlyMemory<byte> audioData, CancellationToken cancellationToken = default);

        /// <summary>
        /// Commits audio bytes to a user message.
        /// </summary>
        /// <param name="cancellationToken">Optional cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task Commit(CancellationToken cancellationToken = default);

        /// <summary>
        /// Clears the audio bytes in the buffer.
        /// </summary>
        /// <param name="cancellationToken">Optional cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task Clear(CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// Interface for managing conversation operations.
    /// </summary>
    public interface IConversation
    {
        /// <summary>
        /// Provides access to conversation item operations.
        /// </summary>
        IItem Item { get; }

        /// <summary>
        /// Interface for managing conversation items.
        /// </summary>
        public interface IItem
        {
            /// <summary>
            /// Creates a new item in the conversation.
            /// </summary>
            /// <param name="request">Request containing item details such as type, status, role, and content.</param>
            /// <param name="cancellationToken">Optional cancellation token.</param>
            /// <returns>A task representing the asynchronous operation.</returns>
            Task Create(ConversationItemCreateRequest request, CancellationToken cancellationToken = default);

            /// <summary>
            /// Truncates a previous assistant message's audio.
            /// </summary>
            /// <param name="itemId">The ID of the assistant message item to truncate.</param>
            /// <param name="contentIndex">The index of the content part to truncate.</param>
            /// <param name="audioEndMs">Inclusive duration up to which audio is truncated, in milliseconds.</param>
            /// <param name="cancellationToken">Optional cancellation token.</param>
            /// <returns>A task representing the asynchronous operation.</returns>
            Task Truncate(string itemId, int contentIndex, int audioEndMs, CancellationToken cancellationToken = default);

            /// <summary>
            /// Removes any item from the conversation history.
            /// </summary>
            /// <param name="itemId">The ID of the item to delete.</param>
            /// <param name="cancellationToken">Optional cancellation token.</param>
            /// <returns>A task representing the asynchronous operation.</returns>
            Task Delete(string itemId, CancellationToken cancellationToken = default);
        }
    }

    /// <summary>
    /// Interface for managing response operations.
    /// </summary>
    public interface IResponse
    {
        /// <summary>
        /// Triggers a response generation with specific configuration.
        /// </summary>
        /// <param name="request">Configuration for the response including modalities, instructions, voice, tools, and other settings.</param>
        /// <param name="cancellationToken">Optional cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task Create(ResponseCreateRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        /// Triggers a response generation with default configuration.
        /// </summary>
        /// <param name="cancellationToken">Optional cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task Create(CancellationToken cancellationToken = default);

        /// <summary>
        /// Cancels an in-progress response.
        /// </summary>
        /// <param name="cancellationToken">Optional cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task Cancel(CancellationToken cancellationToken = default);
    }
}