using System.Text.Json.Serialization;

namespace OpenAI.GPT3.ObjectModels.RequestModels;

public record FineTuneCancelRequest
{
    [JsonPropertyName("fine_tune_id")] public string FineTuneId { get; set; }
}