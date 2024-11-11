namespace Betalgo.Ranul.OpenAI.ObjectModels.RealtimeModels;

/// <summary>
/// Contains constant string values for all event types in the OpenAI Realtime WebSocket API.
/// </summary>
public static class RealtimeEventTypes
{
    /// <summary>
    /// Events that the OpenAI Realtime WebSocket server will accept from the client.
    /// </summary>
    public static class Client
    {
        /// <summary>
        /// Session-related client events.
        /// </summary>
        public static class Session
        {
            /// <summary>
            /// Event to update the session's default configuration.
            /// </summary>
            public const string Update = "session.update";
        }

        /// <summary>
        /// Input audio buffer-related client events.
        /// </summary>
        public static class InputAudioBuffer
        {
            /// <summary>
            /// Event to append audio bytes to the input audio buffer.
            /// </summary>
            public const string Append = "input_audio_buffer.append";

            /// <summary>
            /// Event to commit audio bytes to a user message.
            /// </summary>
            public const string Commit = "input_audio_buffer.commit";

            /// <summary>
            /// Event to clear the audio bytes in the buffer.
            /// </summary>
            public const string Clear = "input_audio_buffer.clear";
        }

        /// <summary>
        /// Conversation-related client events.
        /// </summary>
        public static class Conversation
        {
            /// <summary>
            /// Conversation item-related client events.
            /// </summary>
            public static class Item
            {
                /// <summary>
                /// Event for adding an item to the conversation.
                /// </summary>
                public const string Create = "conversation.item.create";

                /// <summary>
                /// Event when you want to truncate a previous assistant message's audio.
                /// </summary>
                public const string Truncate = "conversation.item.truncate";

                /// <summary>
                /// Event when you want to remove any item from the conversation history.
                /// </summary>
                public const string Delete = "conversation.item.delete";
            }
        }

        /// <summary>
        /// Response-related client events.
        /// </summary>
        public static class Response
        {
            /// <summary>
            /// Event to trigger a response generation.
            /// </summary>
            public const string Create = "response.create";

            /// <summary>
            /// Event to cancel an in-progress response.
            /// </summary>
            public const string Cancel = "response.cancel";
        }
    }

    /// <summary>
    /// Events emitted from the OpenAI Realtime WebSocket server to the client.
    /// </summary>
    public static class Server
    {
        /// <summary>
        /// Returned when an error occurs.
        /// </summary>
        public const string Error = "error";

        /// <summary>
        /// Session-related server events.
        /// </summary>
        public static class Session
        {
            /// <summary>
            /// Returned when a session is created. Emitted automatically when a new connection is established.
            /// </summary>
            public const string Created = "session.created";

            /// <summary>
            /// Returned when a session is updated.
            /// </summary>
            public const string Updated = "session.updated";
        }

        /// <summary>
        /// Conversation-related server events.
        /// </summary>
        public static class Conversation
        {
            /// <summary>
            /// Returned when a conversation is created. Emitted right after session creation.
            /// </summary>
            public const string Created = "conversation.created";

            /// <summary>
            /// Conversation item-related server events.
            /// </summary>
            public static class Item
            {
                /// <summary>
                /// Returned when a conversation item is created.
                /// </summary>
                public const string Created = "conversation.item.created";

                /// <summary>
                /// Returned when an earlier assistant audio message item is truncated by the client.
                /// </summary>
                public const string Truncated = "conversation.item.truncated";

                /// <summary>
                /// Returned when an item in the conversation is deleted.
                /// </summary>
                public const string Deleted = "conversation.item.deleted";

                /// <summary>
                /// Input audio transcription-related server events.
                /// </summary>
                public static class InputAudioTranscription
                {
                    /// <summary>
                    /// Returned when input audio transcription is enabled and a transcription succeeds.
                    /// </summary>
                    public const string Completed = "conversation.item.input_audio_transcription.completed";

                    /// <summary>
                    /// Returned when input audio transcription is configured, and a transcription request for a user message failed.
                    /// </summary>
                    public const string Failed = "conversation.item.input_audio_transcription.failed";
                }
            }
        }

        /// <summary>
        /// Input audio buffer-related server events.
        /// </summary>
        public static class InputAudioBuffer
        {
            /// <summary>
            /// Returned when an input audio buffer is committed, either by the client or automatically in server VAD mode.
            /// </summary>
            public const string Committed = "input_audio_buffer.committed";

            /// <summary>
            /// Returned when the input audio buffer is cleared by the client.
            /// </summary>
            public const string Cleared = "input_audio_buffer.cleared";

            /// <summary>
            /// Returned in server turn detection mode when speech is detected.
            /// </summary>
            public const string SpeechStarted = "input_audio_buffer.speech_started";

            /// <summary>
            /// Returned in server turn detection mode when speech stops.
            /// </summary>
            public const string SpeechStopped = "input_audio_buffer.speech_stopped";
        }

        /// <summary>
        /// Response-related server events.
        /// </summary>
        public static class Response
        {
            /// <summary>
            /// Returned when a new Response is created. The first event of response creation, where the response is in an initial state of "in_progress".
            /// </summary>
            public const string Created = "response.created";

            /// <summary>
            /// Returned when a Response is done streaming. Always emitted, no matter the final state.
            /// </summary>
            public const string Done = "response.done";

            /// <summary>
            /// Output item-related server events.
            /// </summary>
            public static class OutputItem
            {
                /// <summary>
                /// Returned when a new Item is created during response generation.
                /// </summary>
                public const string Added = "response.output_item.added";

                /// <summary>
                /// Returned when an Item is done streaming. Also emitted when a Response is interrupted, incomplete, or cancelled.
                /// </summary>
                public const string Done = "response.output_item.done";
            }

            /// <summary>
            /// Content part-related server events.
            /// </summary>
            public static class ContentPart
            {
                /// <summary>
                /// Returned when a new content part is added to an assistant message item during response generation.
                /// </summary>
                public const string Added = "response.content_part.added";

                /// <summary>
                /// Returned when a content part is done streaming in an assistant message item. Also emitted when a Response is interrupted, incomplete, or cancelled.
                /// </summary>
                public const string Done = "response.content_part.done";
            }

            /// <summary>
            /// Text-related server events.
            /// </summary>
            public static class Text
            {
                /// <summary>
                /// Returned when the text value of a "text" content part is updated.
                /// </summary>
                public const string Delta = "response.text.delta";

                /// <summary>
                /// Returned when the text value of a "text" content part is done streaming. Also emitted when a Response is interrupted, incomplete, or cancelled.
                /// </summary>
                public const string Done = "response.text.done";
            }

            /// <summary>
            /// Audio transcript-related server events.
            /// </summary>
            public static class AudioTranscript
            {
                /// <summary>
                /// Returned when the model-generated transcription of audio output is updated.
                /// </summary>
                public const string Delta = "response.audio_transcript.delta";

                /// <summary>
                /// Returned when the model-generated transcription of audio output is done streaming. Also emitted when a Response is interrupted, incomplete, or cancelled.
                /// </summary>
                public const string Done = "response.audio_transcript.done";
            }

            /// <summary>
            /// Audio-related server events.
            /// </summary>
            public static class Audio
            {
                /// <summary>
                /// Returned when the model-generated audio is updated.
                /// </summary>
                public const string Delta = "response.audio.delta";

                /// <summary>
                /// Returned when the model-generated audio is done. Also emitted when a Response is interrupted, incomplete, or cancelled.
                /// </summary>
                public const string Done = "response.audio.done";
            }

            /// <summary>
            /// Function call arguments-related server events.
            /// </summary>
            public static class FunctionCallArguments
            {
                /// <summary>
                /// Returned when the model-generated function call arguments are updated.
                /// </summary>
                public const string Delta = "response.function_call_arguments.delta";

                /// <summary>
                /// Returned when the model-generated function call arguments are done streaming. Also emitted when a Response is interrupted, incomplete, or cancelled.
                /// </summary>
                public const string Done = "response.function_call_arguments.done";
            }
        }

        /// <summary>
        /// Rate limits-related server events.
        /// </summary>
        public static class RateLimits
        {
            /// <summary>
            /// Emitted after every "response.done" event to indicate the updated rate limits.
            /// </summary>
            public const string Updated = "rate_limits.updated";
        }
    }
}