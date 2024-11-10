namespace Betalgo.Ranul.OpenAI.ObjectModels.RealtimeModels;

public static class RealtimeEventTypes
{
    public static class Client
    {
        public static class Session
        {
            public const string Update = "session.update";
        }

        public static class InputAudioBuffer
        {
            public const string Append = "input_audio_buffer.append";
            public const string Commit = "input_audio_buffer.commit";
            public const string Clear = "input_audio_buffer.clear";
        }

        public static class Conversation
        {
            public static class Item
            {
                public const string Create = "conversation.item.create";
                public const string Truncate = "conversation.item.truncate";
                public const string Delete = "conversation.item.delete";
            }
        }

        public static class Response
        {
            public const string Create = "response.create";
            public const string Cancel = "response.cancel";
        }
    }

    public static class Server
    {
        public const string Error = "error";

        public static class Session
        {
            public const string Created = "session.created";
            public const string Updated = "session.updated";
        }

        public static class Conversation
        {
            public const string Created = "conversation.created";

            public static class Item
            {
                public const string Created = "conversation.item.created";
                public const string Truncated = "conversation.item.truncated";
                public const string Deleted = "conversation.item.deleted";

                public static class InputAudioTranscription
                {
                    public const string Completed = "conversation.item.input_audio_transcription.completed";
                    public const string Failed = "conversation.item.input_audio_transcription.failed";
                }
            }
        }

        public static class InputAudioBuffer
        {
            public const string Committed = "input_audio_buffer.committed";
            public const string Cleared = "input_audio_buffer.cleared";
            public const string SpeechStarted = "input_audio_buffer.speech_started";
            public const string SpeechStopped = "input_audio_buffer.speech_stopped";
        }

        public static class Response
        {
            public const string Created = "response.created";
            public const string Done = "response.done";

            public static class OutputItem
            {
                public const string Added = "response.output_item.added";
                public const string Done = "response.output_item.done";
            }

            public static class ContentPart
            {
                public const string Added = "response.content_part.added";
                public const string Done = "response.content_part.done";
            }

            public static class Text
            {
                public const string Delta = "response.text.delta";
                public const string Done = "response.text.done";
            }

            public static class AudioTranscript
            {
                public const string Delta = "response.audio_transcript.delta";
                public const string Done = "response.audio_transcript.done";
            }

            public static class Audio
            {
                public const string Delta = "response.audio.delta";
                public const string Done = "response.audio.done";
            }

            public static class FunctionCallArguments
            {
                public const string Delta = "response.function_call_arguments.delta";
                public const string Done = "response.function_call_arguments.done";
            }
        }

        public static class RateLimits
        {
            public const string Updated = "rate_limits.updated";
        }
    }
}