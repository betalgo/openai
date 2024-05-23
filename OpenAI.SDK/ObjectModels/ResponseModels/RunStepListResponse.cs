using System.Text.Json.Serialization;
using OpenAI.ObjectModels.SharedModels;

namespace OpenAI.ObjectModels.ResponseModels;

public record RunStepListResponse : DataWithPagingBaseResponse<List<RunStepResponse>>
{
}

public record RunStepResponse : BaseResponse, IOpenAiModels.IId, IOpenAiModels.ICreatedAt
{
    /// <summary>
    ///     The ID of the [assistant](/docs/api-reference/assistants) associated with the run step.
    /// </summary>
    [JsonPropertyName("assistant_id")]
    public string AssistantId { get; set; }

    /// <summary>
    ///     The ID of the [thread](/docs/api-reference/threads) that was run.
    /// </summary>
    [JsonPropertyName("thread_id")]
    public string ThreadId { get; set; }

    /// <summary>
    ///     The ID of the [run](/docs/api-reference/runs) that this run step is a part of.
    /// </summary>
    [JsonPropertyName("run_id")]
    public string RunId { get; set; }

    /// <summary>
    ///     The type of run step, which can be either `message_creation` or `tool_calls`.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; }

    /// <summary>
    ///     The status of the run step, which can be either `in_progress`, `cancelled`, `failed`, `completed`, `expired`, or
    ///     'incomplete'.
    /// </summary>
    [JsonPropertyName("status")]
    public string Status { get; set; }

    [JsonPropertyName("step_details")]
    public RunStepDetails? StepDetails { get; set; }

    /// <summary>
    ///     The last error associated with this run step. Will be `null` if there are no errors.
    /// </summary>
    [JsonPropertyName("last_error")]
    public Error? LastError { get; set; }

    /// <summary>
    ///     The Unix timestamp (in seconds) for when the run step expired. A step is considered expired if the parent run is
    ///     expired.
    /// </summary>
    [JsonPropertyName("expired_at")]
    public int? ExpiredAt { get; set; }

    /// <summary>
    ///     The Unix timestamp (in seconds) for when the run step was cancelled.
    /// </summary>
    [JsonPropertyName("cancelled_at")]
    public int? CancelledAt { get; set; }

    /// <summary>
    ///     The Unix timestamp (in seconds) for when the run step failed.
    /// </summary>
    [JsonPropertyName("failed_at")]
    public int? FailedAt { get; set; }

    /// <summary>
    ///     The Unix timestamp (in seconds) for when the run step completed.
    /// </summary>
    [JsonPropertyName("completed_at")]
    public int? CompletedAt { get; set; }

    /// <summary>
    ///     Set of 16 key-value pairs that can be attached to an object. This can be useful for storing additional information
    ///     about the object in a structured format. Keys can be a maximum of 64 characters long and values can be a maxium of
    ///     512 characters long.
    /// </summary>
    [JsonPropertyName("metadata")]
    public Dictionary<string, string>? Metadata { get; set; }

    /// <summary>
    ///     Usage statistics related to the run step. This value will be `null` while the run step&apos;s status is
    ///     `in_progress`.
    /// </summary>
    [JsonPropertyName("usage")]
    public UsageResponse? Usage { get; set; }

    /// <summary>
    ///     The Unix timestamp (in seconds) for when the run step was created.
    /// </summary>
    [JsonPropertyName("created_at")]
    public long CreatedAt { get; set; }

    /// <summary>
    ///     The identifier of the run step, which can be referenced in API endpoints.
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; set; }
}

public record RunStepDetails
{
    /// <summary>
    ///     Always message_creation.
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