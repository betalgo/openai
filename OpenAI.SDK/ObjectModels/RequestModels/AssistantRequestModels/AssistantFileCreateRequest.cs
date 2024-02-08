using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace OpenAI.ObjectModels.RequestModels.AssistantRequestModels;

public record AssistantFileCreateRequest
{
    /// <summary>
    /// A File ID that the assistant should use.
    /// </summary>
    [JsonPropertyName("file_id")]
    [Required]
    public string FileId { get; set; }
}