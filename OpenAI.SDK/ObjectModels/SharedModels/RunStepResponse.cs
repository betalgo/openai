using OpenAI.ObjectModels.ResponseModels;
using System.Text.Json.Serialization;

namespace OpenAI.ObjectModels.SharedModels;

/// <summary>
///     Represents a step within a run.
/// </summary>
public record RunStepResponse : BaseResponse, IOpenAiModels.IId, IOpenAiModels.ICreatedAt
{
    /// <summary>
    ///     The identifier, which can be referenced in API endpoints.
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; set; }

    /// <summary>
    ///     The ID of the thread that was run.
    /// </summary>
    [JsonPropertyName("thread_id")]
    public string ThreadId { get; set; }

    /// <summary>
    ///     The ID of the assistant associated with the run step.
    /// </summary>
    [JsonPropertyName("assistant_id")]
    public string AssistantId { get; set; }

    /// <summary>
    ///     The ID of the run that this run step is a part of.
    /// </summary>
    [JsonPropertyName("run_id")]
    public string RunId { get; set; }

    /// <summary>
    ///     The Unix timestamp (in seconds) for when the run step was created.
    /// </summary>
    [JsonPropertyName("created_at")]
    public int CreatedAt { get; set; }

    /// <summary>
    ///     Unix timestamp (in seconds)
    /// </summary>
    [JsonPropertyName("cancelled_at")]
    public int? CancelledAt { get; set; }

    /// <summary>
    ///     Unix timestamp (in seconds)
    /// </summary>
    [JsonPropertyName("completed_at")]
    public int? CompletedAt { get; set; }

    /// <summary>
    ///     Unix timestamp (in seconds)
    /// </summary>
    [JsonPropertyName("expired_at")]
    public int? ExpiredAt { get; set; }

    /// <summary>
    ///     Unix timestamp (in seconds)
    /// </summary>
    [JsonPropertyName("failed_at")]
    public int? FailedAt { get; set; }

    /// <summary>
    /// The type of run step, which can be either message_creation or tool_calls.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; }

    /// <summary>
    /// The status of the run step, which can be either in_progress, cancelled, failed, completed, or expired.
    /// </summary>
    [JsonPropertyName("status")]
    public string Status { get; set; }

    [JsonPropertyName("last_error")]
    public string? LastError { get; set; }

    [JsonPropertyName("step_details")]
    public RunStepDetails? StepDetails { get; set; }

    [JsonPropertyName("usage")]
    public UsageResponse Usage { get; set; }

    public class RunStepDetails
    {
        /// <summary>
        /// Always message_creation.
        /// </summary>
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("message_creation")]
        public RunStepMessageCreation MessageCreation { get; set; }

        public class RunStepMessageCreation
        {
            [JsonPropertyName("message_id")]
            public string MessageId { get; set; }
        }
    }
}