using Betalgo.Ranul.OpenAI.ObjectModels.RealtimeModels;

namespace Betalgo.Ranul.OpenAI.Managers;


public class OpenAIRealtimeServiceServerEvents : IOpenAIRealtimeServiceServerEvents
{
    internal readonly ConversationImplementation ConversationImpl = new();
    internal readonly InputAudioBufferImplementation InputAudioBufferImpl = new();
    internal readonly RateLimitsImplementation RateLimitsImpl = new();
    internal readonly ResponseImplementation ResponseImpl = new();
    internal readonly SessionImplementation SessionImpl = new();

    public event EventHandler<ErrorEvent>? OnError;
    public event EventHandler<string>? OnAll;

    public IOpenAIRealtimeServiceServerEvents.ISession Session => SessionImpl;
    public IOpenAIRealtimeServiceServerEvents.IConversation Conversation => ConversationImpl;
    public IOpenAIRealtimeServiceServerEvents.IInputAudioBuffer InputAudioBuffer => InputAudioBufferImpl;
    public IOpenAIRealtimeServiceServerEvents.IResponse Response => ResponseImpl;
    public IOpenAIRealtimeServiceServerEvents.IRateLimits RateLimits => RateLimitsImpl;

    internal void RaiseOnError(object sender, ErrorEvent e)
    {
        OnError?.Invoke(sender, e);
    }

    internal void RaiseOnAll(object sender, string message)
    {
        OnAll?.Invoke(sender, message);
    }

    internal class SessionImplementation : IOpenAIRealtimeServiceServerEvents.ISession
    {
        public event EventHandler<SessionEvent>? OnCreated;
        public event EventHandler<SessionEvent>? OnUpdated;

        internal void RaiseOnCreated(object sender, SessionEvent e)
        {
            OnCreated?.Invoke(sender, e);
        }

        internal void RaiseOnUpdated(object sender, SessionEvent e)
        {
            OnUpdated?.Invoke(sender, e);
        }
    }

    internal class ConversationImplementation : IOpenAIRealtimeServiceServerEvents.IConversation
    {
        internal readonly ItemImplementation ItemImpl = new();
        public event EventHandler<ConversationCreatedEvent>? OnCreated;
        public IOpenAIRealtimeServiceServerEvents.IConversation.IItem Item => ItemImpl;

        internal void RaiseOnCreated(object sender, ConversationCreatedEvent e)
        {
            OnCreated?.Invoke(sender, e);
        }

        internal class ItemImplementation : IOpenAIRealtimeServiceServerEvents.IConversation.IItem
        {
            internal readonly InputAudioTranscriptionImplementation InputAudioTranscriptionImpl = new();
            public event EventHandler<ConversationItemCreatedEvent>? OnCreated;
            public event EventHandler<ConversationItemTruncatedEvent>? OnTruncated;
            public event EventHandler<ConversationItemDeletedEvent>? OnDeleted;
            public IOpenAIRealtimeServiceServerEvents.IConversation.IItem.IInputAudioTranscription InputAudioTranscription => InputAudioTranscriptionImpl;

            internal void RaiseOnCreated(object sender, ConversationItemCreatedEvent e)
            {
                OnCreated?.Invoke(sender, e);
            }

            internal void RaiseOnTruncated(object sender, ConversationItemTruncatedEvent e)
            {
                OnTruncated?.Invoke(sender, e);
            }

            internal void RaiseOnDeleted(object sender, ConversationItemDeletedEvent e)
            {
                OnDeleted?.Invoke(sender, e);
            }

            internal class InputAudioTranscriptionImplementation : IOpenAIRealtimeServiceServerEvents.IConversation.IItem.IInputAudioTranscription
            {
                public event EventHandler<InputAudioTranscriptionCompletedEvent>? OnCompleted;
                public event EventHandler<InputAudioTranscriptionFailedEvent>? OnFailed;

                internal void RaiseOnCompleted(object sender, InputAudioTranscriptionCompletedEvent e)
                {
                    OnCompleted?.Invoke(sender, e);
                }

                internal void RaiseOnFailed(object sender, InputAudioTranscriptionFailedEvent e)
                {
                    OnFailed?.Invoke(sender, e);
                }
            }
        }
    }

    internal class InputAudioBufferImplementation : IOpenAIRealtimeServiceServerEvents.IInputAudioBuffer
    {
        public event EventHandler<AudioBufferCommittedEvent>? OnCommitted;
        public event EventHandler<AudioBufferClearedEvent>? OnCleared;
        public event EventHandler<AudioBufferSpeechStartedEvent>? OnSpeechStarted;
        public event EventHandler<AudioBufferSpeechStoppedEvent>? OnSpeechStopped;

        internal void RaiseOnCommitted(object sender, AudioBufferCommittedEvent e)
        {
            OnCommitted?.Invoke(sender, e);
        }

        internal void RaiseOnCleared(object sender, AudioBufferClearedEvent e)
        {
            OnCleared?.Invoke(sender, e);
        }

        internal void RaiseOnSpeechStarted(object sender, AudioBufferSpeechStartedEvent e)
        {
            OnSpeechStarted?.Invoke(sender, e);
        }

        internal void RaiseOnSpeechStopped(object sender, AudioBufferSpeechStoppedEvent e)
        {
            OnSpeechStopped?.Invoke(sender, e);
        }
    }

    internal class ResponseImplementation : IOpenAIRealtimeServiceServerEvents.IResponse
    {
        internal readonly AudioImplementation AudioImpl = new();
        internal readonly AudioTranscriptImplementation AudioTranscriptImpl = new();
        internal readonly ContentPartImplementation ContentPartImpl = new();
        internal readonly FunctionCallArgumentsImplementation FunctionCallArgumentsImpl = new();

        internal readonly OutputItemImplementation OutputItemImpl = new();
        internal readonly TextImplementation TextImpl = new();
        public event EventHandler<ResponseEvent>? OnCreated;
        public event EventHandler<ResponseEvent>? OnDone;

        public IOpenAIRealtimeServiceServerEvents.IResponse.IOutputItem OutputItem => OutputItemImpl;
        public IOpenAIRealtimeServiceServerEvents.IResponse.IContentPart ContentPart => ContentPartImpl;
        public IOpenAIRealtimeServiceServerEvents.IResponse.IText Text => TextImpl;
        public IOpenAIRealtimeServiceServerEvents.IResponse.IAudioTranscript AudioTranscript => AudioTranscriptImpl;
        public IOpenAIRealtimeServiceServerEvents.IResponse.IAudio Audio => AudioImpl;
        public IOpenAIRealtimeServiceServerEvents.IResponse.IFunctionCallArguments FunctionCallArguments => FunctionCallArgumentsImpl;

        internal void RaiseOnCreated(object sender, ResponseEvent e)
        {
            OnCreated?.Invoke(sender, e);
        }

        internal void RaiseOnDone(object sender, ResponseEvent e)
        {
            OnDone?.Invoke(sender, e);
        }

        internal class OutputItemImplementation : IOpenAIRealtimeServiceServerEvents.IResponse.IOutputItem
        {
            public event EventHandler<ResponseOutputItemAddedEvent>? OnAdded;
            public event EventHandler<ResponseOutputItemDoneEvent>? OnDone;

            internal void RaiseOnAdded(object sender, ResponseOutputItemAddedEvent e)
            {
                OnAdded?.Invoke(sender, e);
            }

            internal void RaiseOnDone(object sender, ResponseOutputItemDoneEvent e)
            {
                OnDone?.Invoke(sender, e);
            }
        }

        internal class ContentPartImplementation : IOpenAIRealtimeServiceServerEvents.IResponse.IContentPart
        {
            public event EventHandler<ResponseContentPartEvent>? OnAdded;
            public event EventHandler<ResponseContentPartEvent>? OnDone;

            internal void RaiseOnAdded(object sender, ResponseContentPartEvent e)
            {
                OnAdded?.Invoke(sender, e);
            }

            internal void RaiseOnDone(object sender, ResponseContentPartEvent e)
            {
                OnDone?.Invoke(sender, e);
            }
        }

        internal class TextImplementation : IOpenAIRealtimeServiceServerEvents.IResponse.IText
        {
            public event EventHandler<TextStreamEvent>? OnDelta;
            public event EventHandler<TextStreamEvent>? OnDone;

            internal void RaiseOnDelta(object sender, TextStreamEvent e)
            {
                OnDelta?.Invoke(sender, e);
            }

            internal void RaiseOnDone(object sender, TextStreamEvent e)
            {
                OnDone?.Invoke(sender, e);
            }
        }

        internal class AudioTranscriptImplementation : IOpenAIRealtimeServiceServerEvents.IResponse.IAudioTranscript
        {
            public event EventHandler<AudioTranscriptStreamEvent>? OnDelta;
            public event EventHandler<AudioTranscriptStreamEvent>? OnDone;

            internal void RaiseOnDelta(object sender, AudioTranscriptStreamEvent e)
            {
                OnDelta?.Invoke(sender, e);
            }

            internal void RaiseOnDone(object sender, AudioTranscriptStreamEvent e)
            {
                OnDone?.Invoke(sender, e);
            }
        }

        internal class AudioImplementation : IOpenAIRealtimeServiceServerEvents.IResponse.IAudio
        {
            public event EventHandler<AudioStreamEvent>? OnDelta;
            public event EventHandler<AudioStreamEvent>? OnDone;

            internal void RaiseOnDelta(object sender, AudioStreamEvent e)
            {
                OnDelta?.Invoke(sender, e);
            }

            internal void RaiseOnDone(object sender, AudioStreamEvent e)
            {
                OnDone?.Invoke(sender, e);
            }
        }

        internal class FunctionCallArgumentsImplementation : IOpenAIRealtimeServiceServerEvents.IResponse.IFunctionCallArguments
        {
            public event EventHandler<FunctionCallStreamEvent>? OnDelta;
            public event EventHandler<FunctionCallStreamEvent>? OnDone;

            internal void RaiseOnDelta(object sender, FunctionCallStreamEvent e)
            {
                OnDelta?.Invoke(sender, e);
            }

            internal void RaiseOnDone(object sender, FunctionCallStreamEvent e)
            {
                OnDone?.Invoke(sender, e);
            }
        }
    }

    internal class RateLimitsImplementation : IOpenAIRealtimeServiceServerEvents.IRateLimits
    {
        public event EventHandler<RateLimitsEvent>? OnUpdated;

        internal void RaiseOnUpdated(object sender, RateLimitsEvent e)
        {
            OnUpdated?.Invoke(sender, e);
        }
    }
}




/// <summary>
/// Interface for handling OpenAI Realtime WebSocket server events.
/// Provides access to various components of the realtime communication system.
/// </summary>
public interface IOpenAIRealtimeServiceServerEvents
{
    /// <summary>
    /// Provides access to session-related events and functionality.
    /// </summary>
    ISession Session { get; }

    /// <summary>
    /// Provides access to conversation-related events and functionality.
    /// </summary>
    IConversation Conversation { get; }

    /// <summary>
    /// Provides access to input audio buffer events and functionality.
    /// </summary>
    IInputAudioBuffer InputAudioBuffer { get; }

    /// <summary>
    /// Provides access to response-related events and functionality.
    /// </summary>
    IResponse Response { get; }

    /// <summary>
    /// Provides access to rate limits events and functionality.
    /// </summary>
    IRateLimits RateLimits { get; }

    /// <summary>
    /// Event raised when an error occurs during realtime communication.
    /// </summary>
    event EventHandler<ErrorEvent>? OnError;

    /// <summary>
    /// Event raised for all server events, providing raw event data.
    /// </summary>
    event EventHandler<string>? OnAll;

    /// <summary>
    /// Interface for handling session-related events.
    /// </summary>
    public interface ISession
    {
        /// <summary>
        /// Event raised when a new session is created. Emitted automatically when a new connection is established.
        /// </summary>
        event EventHandler<SessionEvent>? OnCreated;

        /// <summary>
        /// Event raised when a session is updated with new configuration.
        /// </summary>
        event EventHandler<SessionEvent>? OnUpdated;
    }

    /// <summary>
    /// Interface for handling conversation-related events.
    /// </summary>
    public interface IConversation
    {
        /// <summary>
        /// Provides access to conversation item-related events.
        /// </summary>
        IItem Item { get; }

        /// <summary>
        /// Event raised when a conversation is created. Emitted right after session creation.
        /// </summary>
        event EventHandler<ConversationCreatedEvent>? OnCreated;

        /// <summary>
        /// Interface for handling conversation item events.
        /// </summary>
        public interface IItem
        {
            /// <summary>
            /// Provides access to input audio transcription events.
            /// </summary>
            IInputAudioTranscription InputAudioTranscription { get; }

            /// <summary>
            /// Event raised when a conversation item is created.
            /// </summary>
            event EventHandler<ConversationItemCreatedEvent>? OnCreated;

            /// <summary>
            /// Event raised when an earlier assistant audio message item is truncated by the client.
            /// </summary>
            event EventHandler<ConversationItemTruncatedEvent>? OnTruncated;

            /// <summary>
            /// Event raised when an item in the conversation is deleted.
            /// </summary>
            event EventHandler<ConversationItemDeletedEvent>? OnDeleted;

            /// <summary>
            /// Interface for handling input audio transcription events.
            /// </summary>
            public interface IInputAudioTranscription
            {
                /// <summary>
                /// Event raised when input audio transcription is enabled and a transcription succeeds.
                /// </summary>
                event EventHandler<InputAudioTranscriptionCompletedEvent>? OnCompleted;

                /// <summary>
                /// Event raised when input audio transcription is configured, and a transcription request for a user message failed.
                /// </summary>
                event EventHandler<InputAudioTranscriptionFailedEvent>? OnFailed;
            }
        }
    }

    /// <summary>
    /// Interface for handling input audio buffer events.
    /// </summary>
    public interface IInputAudioBuffer
    {
        /// <summary>
        /// Event raised when an input audio buffer is committed, either by the client or automatically in server VAD mode.
        /// </summary>
        event EventHandler<AudioBufferCommittedEvent>? OnCommitted;

        /// <summary>
        /// Event raised when the input audio buffer is cleared by the client.
        /// </summary>
        event EventHandler<AudioBufferClearedEvent>? OnCleared;

        /// <summary>
        /// Event raised in server turn detection mode when speech is detected.
        /// </summary>
        event EventHandler<AudioBufferSpeechStartedEvent>? OnSpeechStarted;

        /// <summary>
        /// Event raised in server turn detection mode when speech stops.
        /// </summary>
        event EventHandler<AudioBufferSpeechStoppedEvent>? OnSpeechStopped;
    }

    /// <summary>
    /// Interface for handling response-related events.
    /// </summary>
    public interface IResponse
    {
        /// <summary>
        /// Provides access to response output item events.
        /// </summary>
        IOutputItem OutputItem { get; }

        /// <summary>
        /// Provides access to content part events.
        /// </summary>
        IContentPart ContentPart { get; }

        /// <summary>
        /// Provides access to text stream events.
        /// </summary>
        IText Text { get; }

        /// <summary>
        /// Provides access to audio transcript stream events.
        /// </summary>
        IAudioTranscript AudioTranscript { get; }

        /// <summary>
        /// Provides access to audio stream events.
        /// </summary>
        IAudio Audio { get; }

        /// <summary>
        /// Provides access to function call arguments stream events.
        /// </summary>
        IFunctionCallArguments FunctionCallArguments { get; }

        /// <summary>
        /// Event raised when a new Response is created. The first event of response creation, where the response is in an initial state of "in_progress".
        /// </summary>
        event EventHandler<ResponseEvent>? OnCreated;

        /// <summary>
        /// Event raised when a Response is done streaming. Always emitted, no matter the final state.
        /// </summary>
        event EventHandler<ResponseEvent>? OnDone;

        /// <summary>
        /// Interface for handling response output item events.
        /// </summary>
        public interface IOutputItem
        {
            /// <summary>
            /// Event raised when a new Item is created during response generation.
            /// </summary>
            event EventHandler<ResponseOutputItemAddedEvent>? OnAdded;

            /// <summary>
            /// Event raised when an Item is done streaming. Also emitted when a Response is interrupted, incomplete, or cancelled.
            /// </summary>
            event EventHandler<ResponseOutputItemDoneEvent>? OnDone;
        }

        /// <summary>
        /// Interface for handling content part events.
        /// </summary>
        public interface IContentPart
        {
            /// <summary>
            /// Event raised when a new content part is added to an assistant message item during response generation.
            /// </summary>
            event EventHandler<ResponseContentPartEvent>? OnAdded;

            /// <summary>
            /// Event raised when a content part is done streaming in an assistant message item.
            /// Also emitted when a Response is interrupted, incomplete, or cancelled.
            /// </summary>
            event EventHandler<ResponseContentPartEvent>? OnDone;
        }

        /// <summary>
        /// Interface for handling text stream events.
        /// </summary>
        public interface IText
        {
            /// <summary>
            /// Event raised when the text value of a "text" content part is updated.
            /// </summary>
            event EventHandler<TextStreamEvent>? OnDelta;

            /// <summary>
            /// Event raised when the text value of a "text" content part is done streaming.
            /// Also emitted when a Response is interrupted, incomplete, or cancelled.
            /// </summary>
            event EventHandler<TextStreamEvent>? OnDone;
        }

        /// <summary>
        /// Interface for handling audio transcript stream events.
        /// </summary>
        public interface IAudioTranscript
        {
            /// <summary>
            /// Event raised when the model-generated transcription of audio output is updated.
            /// </summary>
            event EventHandler<AudioTranscriptStreamEvent>? OnDelta;

            /// <summary>
            /// Event raised when the model-generated transcription of audio output is done streaming.
            /// Also emitted when a Response is interrupted, incomplete, or cancelled.
            /// </summary>
            event EventHandler<AudioTranscriptStreamEvent>? OnDone;
        }

        /// <summary>
        /// Interface for handling audio stream events.
        /// </summary>
        public interface IAudio
        {
            /// <summary>
            /// Event raised when the model-generated audio is updated.
            /// </summary>
            event EventHandler<AudioStreamEvent>? OnDelta;

            /// <summary>
            /// Event raised when the model-generated audio is done.
            /// Also emitted when a Response is interrupted, incomplete, or cancelled.
            /// </summary>
            event EventHandler<AudioStreamEvent>? OnDone;
        }

        /// <summary>
        /// Interface for handling function call arguments stream events.
        /// </summary>
        public interface IFunctionCallArguments
        {
            /// <summary>
            /// Event raised when the model-generated function call arguments are updated.
            /// </summary>
            event EventHandler<FunctionCallStreamEvent>? OnDelta;

            /// <summary>
            /// Event raised when the model-generated function call arguments are done streaming.
            /// Also emitted when a Response is interrupted, incomplete, or cancelled.
            /// </summary>
            event EventHandler<FunctionCallStreamEvent>? OnDone;
        }
    }

    /// <summary>
    /// Interface for handling rate limits events.
    /// </summary>
    public interface IRateLimits
    {
        /// <summary>
        /// Event raised after every "response.done" event to indicate the updated rate limits.
        /// </summary>
        event EventHandler<RateLimitsEvent>? OnUpdated;
    }
}