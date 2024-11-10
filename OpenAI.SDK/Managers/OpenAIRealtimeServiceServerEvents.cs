using Betalgo.Ranul.OpenAI.ObjectModels.RealtimeModels;

namespace Betalgo.Ranul.OpenAI.Managers;

public interface IOpenAIRealtimeServiceServerEvents
{
    ISession Session { get; }
    IConversation Conversation { get; }
    IInputAudioBuffer InputAudioBuffer { get; }
    IResponse Response { get; }
    IRateLimits RateLimits { get; }
    event EventHandler<ErrorEvent>? OnError;
    event EventHandler<string>? OnAll;

    public interface ISession
    {
        event EventHandler<SessionEvent>? OnCreated;
        event EventHandler<SessionEvent>? OnUpdated;
    }

    public interface IConversation
    {
        IItem Item { get; }
        event EventHandler<ConversationCreatedEvent>? OnCreated;

        public interface IItem
        {
            IInputAudioTranscription InputAudioTranscription { get; }
            event EventHandler<ConversationItemCreatedEvent>? OnCreated;
            event EventHandler<ConversationItemTruncatedEvent>? OnTruncated;
            event EventHandler<ConversationItemDeletedEvent>? OnDeleted;

            public interface IInputAudioTranscription
            {
                event EventHandler<InputAudioTranscriptionCompletedEvent>? OnCompleted;
                event EventHandler<InputAudioTranscriptionFailedEvent>? OnFailed;
            }
        }
    }

    public interface IInputAudioBuffer
    {
        event EventHandler<AudioBufferCommittedEvent>? OnCommitted;
        event EventHandler<AudioBufferClearedEvent>? OnCleared;
        event EventHandler<AudioBufferSpeechStartedEvent>? OnSpeechStarted;
        event EventHandler<AudioBufferSpeechStoppedEvent>? OnSpeechStopped;
    }

    public interface IResponse
    {
        IOutputItem OutputItem { get; }
        IContentPart ContentPart { get; }
        IText Text { get; }
        IAudioTranscript AudioTranscript { get; }
        IAudio Audio { get; }
        IFunctionCallArguments FunctionCallArguments { get; }
        event EventHandler<ResponseEvent>? OnCreated;
        event EventHandler<ResponseEvent>? OnDone;

        public interface IOutputItem
        {
            event EventHandler<ResponseOutputItemAddedEvent>? OnAdded;
            event EventHandler<ResponseOutputItemDoneEvent>? OnDone;
        }

        public interface IContentPart
        {
            event EventHandler<ResponseContentPartEvent>? OnAdded;
            event EventHandler<ResponseContentPartEvent>? OnDone;
        }

        public interface IText
        {
            event EventHandler<TextStreamEvent>? OnDelta;
            event EventHandler<TextStreamEvent>? OnDone;
        }

        public interface IAudioTranscript
        {
            event EventHandler<AudioTranscriptStreamEvent>? OnDelta;
            event EventHandler<AudioTranscriptStreamEvent>? OnDone;
        }

        public interface IAudio
        {
            event EventHandler<AudioStreamEvent>? OnDelta;
            event EventHandler<AudioStreamEvent>? OnDone;
        }

        public interface IFunctionCallArguments
        {
            event EventHandler<FunctionCallStreamEvent>? OnDelta;
            event EventHandler<FunctionCallStreamEvent>? OnDone;
        }
    }

    public interface IRateLimits
    {
        event EventHandler<RateLimitsEvent>? OnUpdated;
    }
}

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