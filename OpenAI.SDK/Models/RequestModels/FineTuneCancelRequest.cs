using System.Text.Json.Serialization;

namespace OpenAI.SDK.Models.RequestModels
{
    public record FineTuneCancelRequest
    {
        [JsonPropertyName("fine_tune_id")] public string FineTuneId { get; set; }
    }
}