using System.Text.Json.Serialization;

namespace OpenAI.ObjectModels.RequestModels;

public record FineTuningJobCancelRequest
{
    [JsonPropertyName("fine_tuning_job_id")]
    public string FineTuningJobId { get; set; }
}