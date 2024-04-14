using System.Text.Json.Serialization;

namespace OpenAI.ObjectModels.RequestModels;

public class AssistantFileCreateRequest
{
    /// <summary>
    ///     A File ID (with purpose="assistants") that the assistant should use. Useful for tools like retrieval and
    ///     code_interpreter that can access files.
    /// </summary>
    [JsonPropertyName("file_id")]
    public string FileId { get; set; }
}