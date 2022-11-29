namespace OpenAI.GPT3.ObjectModels
{
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
    }
}