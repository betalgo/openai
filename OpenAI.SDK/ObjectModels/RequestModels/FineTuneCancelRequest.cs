using System.Text.Json.Serialization;

namespace OpenAI.ObjectModels.RequestModels;

public record FineTuneCancelRequest
{
    [JsonPropertyName("fine_tune_id")] public string FineTuneId { get; set; }
}