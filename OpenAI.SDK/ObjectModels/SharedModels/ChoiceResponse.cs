using System.Text.Json.Serialization;

namespace OpenAI.GPT3.ObjectModels.SharedModels;

public record ChoiceResponse : IOpenAiModels.ILogProbs
{
    [JsonPropertyName("text")] public string Text { get; set; }

    [JsonPropertyName("index")] public int? Index { get; set; }

    [JsonPropertyName("finish_reason")] public string FinishReason { get; set; }

    [JsonPropertyName("logprobs")] public int? LogProbs { get; set; }
}