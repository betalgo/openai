namespace OpenAI.GPT3.ObjectModels;

public class StaticValues
{
    public static class ImageStatics
    {
        public static class Size
        {
            public static string Size256 => "256x256";
            public static string Size512 => "512x512";
            public static string Size1024 => "1024x1024";
        }

        public static class ResponseFormat
        {
            public static string Url => "url";
            public static string Base64 => "b64_json";
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
    }

    public static class ChatMessageRoles
    {
        public static string System => "system";
        public static string User => "user";
        public static string Assistant => "assistant";
    }
}