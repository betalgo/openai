using System.Text.Json.Serialization;
using OpenAI.ObjectModels.ResponseModels.VectorStoreResponseModels;

namespace OpenAI.ObjectModels.RequestModels;

public class CreateVectorStoreRequest
{
    /// <summary>
    /// A list of [File](/docs/api-reference/files) IDs that the vector store should use. Useful for tools like `file_search` that can access files.
    /// </summary>
    [JsonPropertyName("file_ids")]
    public List<string>? FileIds { get; set; }

    /// <summary>
    /// The name of the vector store.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    /// <summary>
    /// The expiration policy for a vector store.
    /// </summary>
    [JsonPropertyName("expires_after")]
    public ExpiresAfter? ExpiresAfter { get; set; }

    /// <summary>
    /// Set of 16 key-value pairs that can be attached to an object. This can be useful for storing additional information about the object in a structured format. Keys can be a maximum of 64 characters long and values can be a maxium of 512 characters long.
    /// </summary>
    [JsonPropertyName("metadata")]
    public Dictionary<string, string>? Metadata { get; set; }
}