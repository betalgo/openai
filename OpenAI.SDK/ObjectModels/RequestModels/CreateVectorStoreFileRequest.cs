using System.Text.Json.Serialization;

namespace OpenAI.ObjectModels.RequestModels;

public class CreateVectorStoreFileRequest
{
    /// <summary>
    /// A [File](/docs/api-reference/files) ID that the vector store should use. Useful for tools like `file_search` that can access files.
    /// </summary>
    [JsonPropertyName("file_id")]
    public string FileId { get; set; }
}