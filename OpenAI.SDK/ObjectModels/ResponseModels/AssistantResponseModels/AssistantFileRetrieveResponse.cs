using System.Text.Json.Serialization;
using OpenAI.ObjectModels.SharedModels;

namespace OpenAI.ObjectModels.ResponseModels.AssistantResponseModels;

public record AssistantFileRetrieveResponse : BaseResponse, IOpenAiModels.ICreatedAt, IOpenAiModels.IId
{
    /// <summary>
    /// The identifier of the Assistant File
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; set; }
    
    /// <summary>
    /// The Unix timestamp (in seconds) for when the assistant file was created.
    /// </summary>
    [JsonPropertyName("created_at")]
    public int CreatedAt { get; set; }
    
    /// <summary>
    ///  The assistant ID that the file is attached to
    /// </summary>
    [JsonPropertyName("assistant_id")]
    public string AssistantId { get; set; }
}