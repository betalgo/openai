namespace OpenAI.SDK
{
    public class OpenAiSettings
    {
        public static string SettingKey = "OpenAISdkSettings";
        public string? Organization { get; set; }
        public string ApiKey { get; set; } = null!;
    }
}