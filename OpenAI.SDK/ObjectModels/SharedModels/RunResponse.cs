using System.Text.Json.Serialization;
using OpenAI.ObjectModels.RequestModels;
using OpenAI.ObjectModels.ResponseModels;

namespace OpenAI.ObjectModels.SharedModels;

public record RunListResponse : DataWithPagingBaseResponse<List<RunResponse>>
{

}

public record RunResponse : BaseResponse, IOpenAiModels.IId, IOpenAiModels.IModel, IOpenAiModels.ICreatedAt, IOpenAiModels.IFileIds, IOpenAiModels.IMetaData
{

    /// <summary>
    ///     The ID of the thread that was executed on as a part of this run.
    /// </summary>
    [JsonPropertyName("thread_id")]
    public string ThreadId { get; set; }

    /// <summary>
    ///     The ID of the assistant used for execution of this run.
    /// </summary>
    [JsonPropertyName("assistant_id")]
    public string AssistantId { get; set; }

    /// <summary>
    ///     The status of the run, which can be either queued, in_progress, requires_action, cancelling, cancelled, failed,
    ///     completed, or expired.
    /// </summary>
    [JsonPropertyName("status")]
    public string Status { get; set; }

    /// <summary>
    ///     Details on the action required to continue the run.
    ///     Will be null if no action is required.
    /// </summary>
    [JsonPropertyName("required_action")]
    public RequiredAction? RequiredAction { get; set; }

    /// <summary>
    ///     The last error associated with this run. Will be null if there are no errors.
    /// </summary>
    [JsonPropertyName("last_error")]
    public Error? LastError { get; set; }
    

    /// <summary>
    ///     Details on why the run is incomplete. Will be null if the run is not incomplete.
    /// </summary>
    [JsonPropertyName("incomplete_details")]
    public IncompleteDetails? IncompleteDetails { get; set; }

    /// <summary>
    ///     The Unix timestamp (in seconds) for when the run will expire.
    /// </summary>
    [JsonPropertyName("expires_at")]
    public int? ExpiresAt { get; set; }

    /// <summary>
    ///     The Unix timestamp (in seconds) for when the run was started.
    /// </summary>
    [JsonPropertyName("started_at")]
    public int? StartedAt { get; set; }

    /// <summary>
    ///     The Unix timestamp (in seconds) for when the run was cancelled.
    /// </summary>
    [JsonPropertyName("cancelled_at")]
    public int? CancelledAt { get; set; }

    /// <summary>
    ///     The Unix timestamp (in seconds) for when the run failed.
    /// </summary>
    [JsonPropertyName("failed_at")]
    public int? FailedAt { get; set; }

    /// <summary>
    ///     The Unix timestamp (in seconds) for when the run was completed.
    /// </summary>
    [JsonPropertyName("completed_at")]
    public int? CompletedAt { get; set; }

    /// <summary>
    ///     The instructions that the assistant used for this run.
    /// </summary>
    [JsonPropertyName("instructions")]
    public string Instructions { get; set; }

    /// <summary>
    ///     The list of tools that the assistant used for this run.
    /// </summary>
    [JsonPropertyName("tools")]
    public List<ToolDefinition>? Tools { get; set; }

    /// <summary>
    ///     Usage statistics related to the run. This value will be null if the run is not in a terminal state (i.e. in_progress, queued, etc.).
    /// </summary>
    [JsonPropertyName("usage")]
    public UsageResponse? Usage { get; set; }

    /// <summary>
    ///     The sampling temperature used for this run. If not set, defaults to 1.
    /// </summary>
    [JsonPropertyName("temperature")]
    public double? Temperature { get; set; }

    /// <summary>
    ///     The maximum number of prompt tokens specified to have been used over the course of the run.
    /// </summary>
    [JsonPropertyName("max_prompt_tokens")]
    public int? MaxPromptTokens { get; set; }

    /// <summary>
    ///     The maximum number of completion tokens specified to have been used over the course of the run.
    /// </summary>
    [JsonPropertyName("max_completion_tokens")]
    public int? MaxCompletionTokens { get; set; }

    /// <summary>
    ///     The truncation strategy to use for the thread. The default is auto.
    /// </summary>
    [JsonPropertyName("truncation_strategy")]
    public TruncationStrategy? TruncationStrategy { get; set; }

    /// <summary>
    ///     Controls which (if any) tool is called by the model. none means the model will not call any tools and instead generates a message.
    ///     auto is the default value and means the model can pick between generating a message or calling a tool.
    ///     Specifying a particular tool like {"type": "TOOL_TYPE"} or {"type": "function", "function": {"name": "my_function"}} forces the model to call that tool.
    /// </summary>
    [JsonPropertyName("tool_choice")]
    public object? ToolChoice { get; set; }

    /// <summary>
    ///     Specifies the format that the model must output. Compatible with GPT-4 Turbo and all GPT-3.5 Turbo models newer than gpt-3.5-turbo-1106.
    /// </summary>
    [JsonPropertyName("response_format")]
    public object? ResponseFormat { get; set; }

    /// <summary>
    ///     The Unix timestamp (in seconds) for when the run was created.
    /// </summary>
    [JsonPropertyName("created_at")]
    public long CreatedAt { get; set; }

    /// <summary>
    ///     The list of File IDs the assistant used for this run.
    /// </summary>
    [JsonPropertyName("file_ids")]
    public List<string>? FileIds { get; set; }

    /// <summary>
    ///     The identifier, which can be referenced in API endpoints.
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; set; }

    /// <summary>
    ///     Set of 16 key-value pairs that can be attached to an object.
    ///     This can be useful for storing additional information about the object in a structured format.
    ///     Keys can be a maximum of 64 characters long and values can be a maxium of 512 characters long.
    /// </summary>
    [JsonPropertyName("metadata")]
    public Dictionary<string, string>? Metadata { get; set; }

    /// <summary>
    ///     The model that the assistant used for this run.
    /// </summary>
    [JsonPropertyName("model")]
    public string Model { get; set; }
}

public class IncompleteDetails
{
    /// <summary>
    ///     The reason why the run is incomplete.
    ///     This will point to which specific token limit was reached over the course of the run.
    /// </summary>
    [JsonPropertyName("reason")]
    public string Reason { get; set; }
}