using System.Text.Json.Serialization;

namespace OpenAI.SDK.Models.ResponseModels
{
    public record CreateCompletionResponse : BaseResponse
    {
        [JsonPropertyName("id")] public string Id { get; set; }

        [JsonPropertyName("created")] public int Created { get; set; }

        [JsonPropertyName("model")] public string Model { get; set; }

        [JsonPropertyName("choices")] public List<Choice> Choices { get; set; }
    }

    public record Choice
    {
        [JsonPropertyName("text")] public string Text { get; set; }

        [JsonPropertyName("index")] public int Index { get; set; }

        [JsonPropertyName("logprobs")] public object Logprobs { get; set; }

        [JsonPropertyName("finish_reason")] public string FinishReason { get; set; }
    }
}