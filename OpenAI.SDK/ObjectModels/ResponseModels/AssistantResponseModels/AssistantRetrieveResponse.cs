using System.Text.Json.Serialization;
using OpenAI.ObjectModels.SharedModels;
using OpenAI.ObjectModels.SharedModels.BetaSharedModels;

namespace OpenAI.ObjectModels.ResponseModels.AssistantResponseModels;

public record AssistantRetrieveResponse : BaseResponse, IOpenAiModels.IId, IOpenAiModels.ICreatedAt
{
    /// <summary>
    /// The identifier, which can be referenced in API endpoints.
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; set; }
    
    /// <summary>
    /// The Unix timestamp (in seconds) for when the assistant was created.
    /// </summary>
    [JsonPropertyName("created_at")]
    public int CreatedAt { get; set; }
    
    /// <summary>
    /// The name of the assistant.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; }
    
    /// <summary>
    /// The description of the assistant.
    /// </summary>
    [JsonPropertyName("description")]
    public string Description { get; set; } 
    
    /// <summary>
    /// ID of the model to use
    /// </summary>
    [JsonPropertyName("model")]
    public string Model { get; set; }
    
    /// <summary>
    /// The system instructions that the assistant uses.
    /// </summary>
    [JsonPropertyName("instructions")]
    public string Insturctions { get; set; }
    
    /// <summary>
    /// A list of tools enabled on the assistant.
    /// </summary>
    [JsonPropertyName("tools")]
    public List<Tool> Tools { get; set;}
    
    /// <summary>
    /// A list of file IDs attached to this assistant.
    /// </summary>
    [JsonPropertyName("file_ids")]
    public List<string> File_IDs { get; set; }
    
    /// <summary>
    /// Set of 16 key-value pairs that can be attached to an object.
    /// </summary>
    [JsonPropertyName("metadata")]
    public Dictionary<string,string> Metadata { get; set;}
    
}