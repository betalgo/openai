using System.Text.Json.Serialization;
using OpenAI.ObjectModels.RequestModels;

namespace OpenAI.ObjectModels.SharedModels;

public record ChatChoiceResponse
{
    [JsonPropertyName("delta")]
    public ChatMessage Delta
    {
        get => Message;
        set => Message = value;
    }

    [JsonPropertyName("message")] public ChatMessage Message { get; set; }

    [JsonPropertyName("index")] public int? Index { get; set; }

    [JsonPropertyName("finish_reason")] public string FinishReason { get; set; }

    [JsonPropertyName("finish_details")] public FinishDetailsResponse? FinishDetails { get; set; }

    [JsonPropertyName("logprobs")] public ChatLogProbsResponse LogProbs { get; set; }
    public class FinishDetailsResponse
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }
        [JsonPropertyName("stop")]
        public string Stop { get; set; }
    }

    public record ChatLogProbsResponse
    {
        [JsonPropertyName("content")] public List<ContentItem> Content { get; set; }
    }

    public record ContentItemBase
    {
        [JsonPropertyName("token")] public string Token { get; set; }

        [JsonPropertyName("logprob")] public double LogProb { get; set; }

        [JsonPropertyName("bytes")] public List<int> Bytes { get; set; }
    }
    public record ContentItem : ContentItemBase
    {
        [JsonPropertyName("top_logprobs")] public List<ContentItemBase> TopLogProbs { get; set; }
    }
}