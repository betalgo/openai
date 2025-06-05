using System.Text.Json.Serialization;

namespace Betalgo.Ranul.OpenAI.ObjectModels.RequestModels;

public class CreateVectorStoreFileRequest
{
    /// <summary>
    ///     A [File](/docs/api-reference/files) ID that the vector store should use. Useful for tools like `file_search` that
    ///     can access files.
    /// </summary>
    [JsonPropertyName("file_id")]
    public string FileId { get; set; }

    /// <summary>
    /// Set of 16 key-value pairs that can be attached to an object. 
    /// This can be useful for storing additional information about the object in a structured format, 
    /// and querying for objects via API or the dashboard. Keys are strings with a maximum length of 64 characters. 
    /// Values are strings with a maximum length of 512 characters, booleans, or numbers.
    /// </summary>
    [JsonPropertyName("attributes")]
    public Dictionary<string, object>? Attributes { get; set; }
}