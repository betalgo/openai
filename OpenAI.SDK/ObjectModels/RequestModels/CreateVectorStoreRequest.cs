using System.Text.Json.Serialization;
using OpenAI.ObjectModels.ResponseModels.VectorStoreResponseModels;

namespace OpenAI.ObjectModels.RequestModels;

public class CreateVectorStoreRequest
{
    /// <summary>
    ///     A list of [File](/docs/api-reference/files) IDs that the vector store should use. Useful for tools like
    ///     `file_search` that can access files.
    /// </summary>
    [JsonPropertyName("file_ids")]
    public List<string>? FileIds { get; set; }

    /// <summary>
    ///     The name of the vector store.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    /// <summary>
    ///     The expiration policy for a vector store.
    /// </summary>
    [JsonPropertyName("expires_after")]
    public ExpiresAfter? ExpiresAfter { get; set; }

    /// <summary>
    ///     Set of 16 key-value pairs that can be attached to an object. This can be useful for storing additional information
    ///     about the object in a structured format. Keys can be a maximum of 64 characters long and values can be a maxium of
    ///     512 characters long.
    /// </summary>
    [JsonPropertyName("metadata")]
    public Dictionary<string, string>? Metadata { get; set; }

    /// <summary>
    ///     The chunking strategy used to chunk the file(s). If not set, will use the auto strategy.
    ///     Only applicable if file_ids is non-empty.
    /// </summary>
    [JsonPropertyName("chunking_strategy")]
    public ChunkingStrategy? ChunkingStrategy { get; set; }
}

public class ChunkingStrategy
{
    /// <summary>
    ///     The type of chunking strategy. Must be either "auto" or "static".
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; }

    /// <summary>
    ///     The static chunking parameters. Required if type is "static".
    /// </summary>
    [JsonPropertyName("static")]
    public StaticChunkingParameters? StaticParameters { get; set; }
}

public class StaticChunkingParameters
{
    /// <summary>
    ///     The maximum number of tokens in each chunk. The default value is 800.
    ///     The minimum value is 100 and the maximum value is 4096.
    /// </summary>
    [JsonPropertyName("max_chunk_size_tokens")]
    public int MaxChunkSizeTokens { get; set; }

    /// <summary>
    ///     The number of tokens that overlap between chunks. The default value is 400.
    ///     Note that the overlap must not exceed half of max_chunk_size_tokens.
    /// </summary>
    [JsonPropertyName("chunk_overlap_tokens")]
    public int ChunkOverlapTokens { get; set; }
}