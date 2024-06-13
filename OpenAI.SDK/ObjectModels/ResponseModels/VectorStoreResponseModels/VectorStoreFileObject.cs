using System.Text.Json.Serialization;

namespace OpenAI.ObjectModels.ResponseModels.VectorStoreResponseModels;
public record VectorStoreFileListObject : DataWithPagingBaseResponse<List<VectorStoreFileObject>>
{
}
public record VectorStoreFileObject : BaseResponse
{
    /// <summary>
    /// The identifier, which can be referenced in API endpoints.
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; set; }


    /// <summary>
    /// The total vector store usage in bytes. Note that this may be different from the original file size.
    /// </summary>
    [JsonPropertyName("usage_bytes")]
    public int UsageBytes { get; set; }

    /// <summary>
    /// The Unix timestamp (in seconds) for when the vector store file was created.
    /// </summary>
    [JsonPropertyName("created_at")]
    public long CreatedAt { get; set; }

    /// <summary>
    /// The ID of the [vector store](/docs/api-reference/vector-stores/object) that the [File](/docs/api-reference/files) is attached to.
    /// </summary>
    [JsonPropertyName("vector_store_id")]
    public string VectorStoreId { get; set; }

    /// <summary>
    /// The status of the vector store file, which can be either `in_progress`, `completed`, `cancelled`, or `failed`. The status `completed` indicates that the vector store file is ready for use.
    /// </summary>
    [JsonPropertyName("status")]
    public string Status { get; set; }

    /// <summary>
    /// The last error associated with this vector store file. Will be `null` if there are no errors.
    /// </summary>
    [JsonPropertyName("last_error")]
    public Error? LastError { get; set; }
}