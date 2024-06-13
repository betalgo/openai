using System.Text.Json.Serialization;

namespace OpenAI.ObjectModels.ResponseModels.VectorStoreResponseModels;

public record VectorStoreFileBatchObject:BaseResponse
{
    /// <summary>
    /// The identifier, which can be referenced in API endpoints.
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; set; }


    /// <summary>
    /// The Unix timestamp (in seconds) for when the vector store files batch was created.
    /// </summary>
    [JsonPropertyName("created_at")]
    public long CreatedAt { get; set; }

    /// <summary>
    /// The ID of the [vector store](/docs/api-reference/vector-stores/object) that the [File](/docs/api-reference/files) is attached to.
    /// </summary>
    [JsonPropertyName("vector_store_id")]
    public string VectorStoreId { get; set; }

    /// <summary>
    /// The status of the vector store files batch, which can be either `in_progress`, `completed`, `cancelled` or `failed`.
    /// </summary>
    [JsonPropertyName("status")]
    public string Status { get; set; }

    [JsonPropertyName("file_counts")]
    public FileCounts FileCounts { get; set; }
}