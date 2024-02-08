using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using OpenAI.ObjectModels.SharedModels.BetaSharedModels;

namespace OpenAI.ObjectModels.RequestModels.AssistantRequestModels;

public record AssistantCreateRequest 
{
    /// <summary>
    ///  ID of the model to use
    /// </summary>
    [Required]
    [JsonPropertyName("model")]
    public string Model { get; set; }

    /// <summary>
    /// The name of the assistant. The maximum length is 256
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; }

    /// <summary>
    /// The description of the assistant.
    /// </summary>
    [JsonPropertyName("description")]
    public string Description { get; set; }

    /// <summary>
    /// The system instructions that the assistant uses.
    /// </summary>
    [JsonPropertyName("instructions")]
    public string Instructions { get; set; }

    /// <summary>
    /// A list of tools enabled on the assistant.
    /// </summary>
    [JsonPropertyName("tools")]
    public List<Tool> Tools { get; set; }

    /// <summary>
    /// A list of file IDs attached to this assistant.
    /// </summary>
    [JsonPropertyName("file_ids")]
    public List<string> FileIds { get; set; }

    /// <summary>
    /// Set of 16 key-value pairs that can be attached to an object.
    /// </summary>
    [JsonPropertyName("metadata")]
    public Dictionary<string,string> Metadata { get; set; }
}