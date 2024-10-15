using System.Text.Json.Serialization;

namespace Betalgo.OpenAI.ObjectModels.RequestModels;

public record FineTuneCancelRequest
{
    [JsonPropertyName("fine_tune_id")]
    public string FineTuneId { get; set; }
}