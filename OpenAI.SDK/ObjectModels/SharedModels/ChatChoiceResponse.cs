using System.Text.Json.Serialization;
using OpenAI.GPT3.ObjectModels.RequestModels;

namespace OpenAI.GPT3.ObjectModels.SharedModels;

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
}