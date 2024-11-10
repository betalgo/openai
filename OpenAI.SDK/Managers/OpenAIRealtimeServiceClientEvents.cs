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

public interface IOpenAIRealtimeServiceClientEvents
{
    ISession Session { get; }
    IInputAudioBuffer InputAudioBuffer { get; }
    IConversation Conversation { get; }
    IResponse Response { get; }

    public interface ISession
    {
        Task Update(SessionUpdateRequest request, CancellationToken cancellationToken = default);
    }

    public interface IInputAudioBuffer
    {
        Task Append(ReadOnlyMemory<byte> audioData, CancellationToken cancellationToken = default);
        Task Commit(CancellationToken cancellationToken = default);
        Task Clear(CancellationToken cancellationToken = default);
    }

    public interface IConversation
    {
        IItem Item { get; }

        public interface IItem
        {
            Task Create(ConversationItemCreateRequest request, CancellationToken cancellationToken = default);
            Task Truncate(string itemId, int contentIndex, int audioEndMs, CancellationToken cancellationToken = default);
            Task Delete(string itemId, CancellationToken cancellationToken = default);
        }
    }

    public interface IResponse
    {
        Task Create(ResponseCreateRequest request, CancellationToken cancellationToken = default);
        Task Create(CancellationToken cancellationToken = default);
        Task Cancel(CancellationToken cancellationToken = default);
    }
}