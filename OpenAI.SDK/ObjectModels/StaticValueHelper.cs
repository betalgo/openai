namespace OpenAI.ObjectModels;

public class StaticValues
{
    public static class CompletionStatics
    {
        public static class ResponseFormat
        {
            public static string Json => "json_object";
            public static string Text => "text";
        }
    }
    public static class ImageStatics
    {
        public static class Size
        {
            public static string Size256 => "256x256";
            public static string Size512 => "512x512";
            public static string Size1024 => "1024x1024";
            /// <summary>
            ///     Only dall-e-3 model
            /// </summary>
            // ReSharper disable once InconsistentNaming
            public static string Size1792x1024 => "1792x1024";
            /// <summary>
            ///     Only dall-e-3 model
            /// </summary>
            // ReSharper disable once InconsistentNaming
            public static string Size1024x1792 => "1024x1792";
        }

        public static class ResponseFormat
        {
            public static string Url => "url";
            public static string Base64 => "b64_json";
        }

        public static class Style
        {
            public static string Vivid => "vivid";
            public static string Natural => "natural";
        }

        public static class ImageFileTypes
        {
            public static string Jpeg => "JPEG";
            public static string Png => "PNG";
            public static string Webp => "WEBP";
            public static string Gif => "GIF";
        }

        public static class ImageDetailTypes
        {
            public static string High => "high";
            public static string Low => "low";
            public static string Auto => "auto";

        }
    }

    public static class AudioStatics
    {
        public static class ResponseFormat
        {
            public static string Json => "json";
            public static string Text => "text";
            public static string Srt => "srt";
            public static string VerboseJson => "verbose_json";
            public static string Vtt => "vtt";
        }
        public static class CreateSpeechResponseFormat
        {
            public static string Mp3 => "mp3";
            public static string Opus => "opus";
            public static string Aac => "aac";
            public static string Flac => "flac";
        }

        public static class Voice
        {
            public static string Alloy => "alloy";
            public static string Echo => "echo";
            public static string Fable => "fable";
            public static string Nova => "nova";
            public static string Onyx => "onyx";
            public static string Shimmer => "shimmer";
        }
    }

    public static class ChatMessageRoles
    {
        public static string System => "system";
        public static string User => "user";
        public static string Assistant => "assistant";
        public static string Function => "function";
    }
}