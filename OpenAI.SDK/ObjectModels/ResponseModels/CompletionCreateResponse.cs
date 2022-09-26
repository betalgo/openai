using System.Text.Json.Serialization;
using OpenAI.GPT3.ObjectModels.SharedModels;

namespace OpenAI.GPT3.ObjectModels.ResponseModels
{
    public record CompletionCreateResponse : BaseResponse, IOpenAiModels.IId, IOpenAiModels.ICreatedAt
    {
        [JsonPropertyName("model")] public string Model { get; set; }

        [JsonPropertyName("choices")] public List<Choice> Choices { get; set; }

        [JsonPropertyName("usage")] public UsageResponse Usage { get; set; }

        [JsonPropertyName("created")] public int CreatedAt { get; set; }

        [JsonPropertyName("id")] public string Id { get; set; }
    }

    public record Choice : IOpenAiModels.ILogProbs
    {
        [JsonPropertyName("text")] public string Text { get; set; }

        [JsonPropertyName("index")] public int? Index { get; set; }

        [JsonPropertyName("finish_reason")] public string FinishReason { get; set; }

        [JsonPropertyName("logprobs")] public int? LogProbs { get; set; }
    }
}