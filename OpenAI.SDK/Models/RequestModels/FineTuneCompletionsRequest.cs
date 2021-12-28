using System.Text.Json.Serialization;

namespace OpenAI.SDK.Models.RequestModels
{
    public record FineTuneCompletionsRequest
    {
        [JsonPropertyName("prompt")] public string Prompt { get; set; }

        [JsonPropertyName("max_tokens")] public int? MaxTokens { get; set; }

        [JsonPropertyName("model")] public string Model { get; set; }
    }
}