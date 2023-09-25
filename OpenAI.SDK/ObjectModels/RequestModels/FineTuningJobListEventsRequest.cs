using System.Text.Json.Serialization;

namespace OpenAI.ObjectModels.RequestModels
{
    public class FineTuningJobListEventsRequest:FineTuningJobListRequest
    {
        /// <summary>
        /// The ID of the fine-tuning job to get events for.
        /// </summary>
        [JsonIgnore]
        public string FineTuningJobId { get; set; }
    }

    public class FineTuningJobListRequest
    {
        /// <summary>
        /// Identifier for the last job from the previous pagination request.
        /// </summary>
        [JsonPropertyName("after")]
        public string? After { get; set; }

        /// <summary>
        /// Number of fine-tuning jobs to retrieve.
        /// Defaults to 20.
        /// </summary>
        [JsonPropertyName("limit")]
        public int? Limit { get; set; }
    }
}
