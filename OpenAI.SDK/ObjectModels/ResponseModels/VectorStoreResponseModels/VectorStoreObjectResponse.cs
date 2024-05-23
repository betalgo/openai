using System.Text.Json.Serialization;

namespace OpenAI.ObjectModels.ResponseModels.VectorStoreResponseModels;

public record VectorStoreObjectResponse:BaseResponse
{
    /// <summary>
    /// The identifier, which can be referenced in API endpoints.
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; set; }

    /// <summary>
    /// The Unix timestamp (in seconds) for when the vector store was created.
    /// </summary>
    [JsonPropertyName("created_at")]
    public long CreatedAt { get; set; }

    /// <summary>
    /// The name of the vector store.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; }

    /// <summary>
    /// The total number of bytes used by the files in the vector store.
    /// </summary>
    [JsonPropertyName("usage_bytes")]
    public int UsageBytes { get; set; }

    [JsonPropertyName("file_counts")]
    public FileCounts FileCounts { get; set; }

    /// <summary>
    /// The status of the vector store, which can be either `expired`, `in_progress`, or `completed`. A status of `completed` indicates that the vector store is ready for use.
    /// </summary>
    [JsonPropertyName("status")]
    public string Status { get; set; }

    /// <summary>
    /// The expiration policy for a vector store.
    /// </summary>
    [JsonPropertyName("expires_after")]
    public ExpiresAfter ExpiresAfter { get; set; }

    /// <summary>
    /// The Unix timestamp (in seconds) for when the vector store will expire.
    /// </summary>
    [JsonPropertyName("expires_at")]
    public int? ExpiresAt { get; set; }

    /// <summary>
    /// The Unix timestamp (in seconds) for when the vector store was last active.
    /// </summary>
    [JsonPropertyName("last_active_at")]
    public int? LastActiveAt { get; set; }

    /// <summary>
    /// Set of 16 key-value pairs that can be attached to an object. This can be useful for storing additional information about the object in a structured format. Keys can be a maximum of 64 characters long and values can be a maxium of 512 characters long.
    /// </summary>
    [JsonPropertyName("metadata")]
    public Dictionary<string, string>? Metadata { get; set; }


}