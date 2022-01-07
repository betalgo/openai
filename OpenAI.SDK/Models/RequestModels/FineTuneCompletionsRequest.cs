using System.Text.Json.Serialization;
using OpenAI.GPT3.Models.SharedModels;

namespace OpenAI.GPT3.Models.RequestModels
{
    public record FineTuneCompletionsRequest : IOpenAiModels.ILogprobs
    {
        [JsonPropertyName("prompt")] public string Prompt { get; set; }
        [JsonPropertyName("max_tokens")] public int? MaxTokens { get; set; }
        [JsonPropertyName("model")] public string Model { get; set; }
        [JsonPropertyName("logprobs")] public int? Logprobs { get; set; }
    }
}