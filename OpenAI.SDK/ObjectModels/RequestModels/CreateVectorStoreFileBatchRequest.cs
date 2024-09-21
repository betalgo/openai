using System.Text.Json.Serialization;

namespace OpenAI.ObjectModels.RequestModels;

public class CreateVectorStoreFileBatchRequest
{
    /// <summary>
    ///     A list of [File](/docs/api-reference/files) IDs that the vector store should use. Useful for tools like
    ///     `file_search` that can access files.
    /// </summary>
    [JsonPropertyName("file_ids")]
    public List<string> FileIds { get; set; }

    /// <summary>
    ///     The chunking strategy used to chunk the file(s). If not set, will use the auto strategy.
    ///     Only applicable if file_ids is non-empty.
    /// </summary>
    [JsonPropertyName("chunking_strategy")]
    public ChunkingStrategy? ChunkingStrategy { get; set; }
}