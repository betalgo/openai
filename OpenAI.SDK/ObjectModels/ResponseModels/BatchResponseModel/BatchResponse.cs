using System.Text.Json.Serialization;
using OpenAI.ObjectModels.SharedModels;

namespace OpenAI.ObjectModels.ResponseModels.BatchResponseModel;

public record BatchResponse : BaseResponse,IOpenAiModels.IMetaData
{
    /// <summary>
    ///  
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; set; }

    /// <summary>
    ///     The OpenAI API endpoint used by the batch.
    /// </summary>
    [JsonPropertyName("endpoint")]
    public string Endpoint { get; set; }

    [JsonPropertyName("errors")]
    public ErrorList? Errors { get; set; }

    /// <summary>
    ///     The ID of the input file for the batch.
    /// </summary>
    [JsonPropertyName("input_file_id")]
    public string InputFileId { get; set; }

    /// <summary>
    ///     The time frame within which the batch should be processed.
    /// </summary>
    [JsonPropertyName("completion_window")]
    public string CompletionWindow { get; set; }

    /// <summary>
    ///     The current status of the batch.
    /// </summary>
    [JsonPropertyName("status")]
    public string Status { get; set; }

    /// <summary>
    ///     The ID of the file containing the outputs of successfully executed requests.
    /// </summary>
    [JsonPropertyName("output_file_id")]
    public string? OutputFileId { get; set; }

    /// <summary>
    ///     The ID of the file containing the outputs of requests with errors.
    /// </summary>
    [JsonPropertyName("error_file_id")]
    public string? ErrorFileId { get; set; }

    /// <summary>
    ///     The Unix timestamp (in seconds) for when the batch was created.
    /// </summary>
    [JsonPropertyName("created_at")]
    public long CreatedAt { get; set; }

    /// <summary>
    ///     The Unix timestamp (in seconds) for when the batch started processing.
    /// </summary>
    [JsonPropertyName("in_progress_at")]
    public int? InProgressAt { get; set; }

    /// <summary>
    ///     The Unix timestamp (in seconds) for when the batch will expire.
    /// </summary>
    [JsonPropertyName("expires_at")]
    public int? ExpiresAt { get; set; }

    /// <summary>
    ///     The Unix timestamp (in seconds) for when the batch started finalizing.
    /// </summary>
    [JsonPropertyName("finalizing_at")]
    public int? FinalizingAt { get; set; }

    /// <summary>
    ///     The Unix timestamp (in seconds) for when the batch was completed.
    /// </summary>
    [JsonPropertyName("completed_at")]
    public int? CompletedAt { get; set; }

    /// <summary>
    ///     The Unix timestamp (in seconds) for when the batch failed.
    /// </summary>
    [JsonPropertyName("failed_at")]
    public int? FailedAt { get; set; }

    /// <summary>
    ///     The Unix timestamp (in seconds) for when the batch expired.
    /// </summary>
    [JsonPropertyName("expired_at")]
    public int? ExpiredAt { get; set; }

    /// <summary>
    ///     The Unix timestamp (in seconds) for when the batch started cancelling.
    /// </summary>
    [JsonPropertyName("cancelling_at")]
    public int? CancellingAt { get; set; }

    /// <summary>
    ///     The Unix timestamp (in seconds) for when the batch was cancelled.
    /// </summary>
    [JsonPropertyName("cancelled_at")]
    public int? CancelledAt { get; set; }

    /// <summary>
    ///     The request counts for different statuses within the batch.
    /// </summary>
    [JsonPropertyName("request_counts")]
    public RequestCountsResponse RequestCounts { get; set; }

    /// <summary>
    ///     Set of 16 key-value pairs that can be attached to an object.
    ///     This can be useful for storing additional information about the object in a structured format.
    ///     Keys can be a maximum of 64 characters long and values can be a maximum of 512 characters long.
    /// </summary>
    [JsonPropertyName("metadata")]
    public Dictionary<string, string>? Metadata { get; set; }
}