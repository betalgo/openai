using System.Text.Json.Serialization;

namespace OpenAI.GPT3.Models.RequestModels
{
    public record FineTuneCancelRequest
    {
        [JsonPropertyName("fine_tune_id")] public string FineTuneId { get; set; }
    }
}