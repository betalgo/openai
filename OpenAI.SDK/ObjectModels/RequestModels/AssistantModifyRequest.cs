using System.Text.Json.Serialization;
using OpenAI.ObjectModels.SharedModels;

namespace OpenAI.ObjectModels.RequestModels;

public class AssistantModifyRequest : IOpenAiModels.IModel, IOpenAiModels.IFileIds, IOpenAiModels.IMetaData
{
    /// <summary>
    ///     The name of the assistant. The maximum length is 256
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    /// <summary>
    ///     The description of the assistant. The maximum length is 512 characters.
    /// </summary>
    [JsonPropertyName("description")]
    public string? Description { get; set; }

    /// <summary>
    ///     The system instructions that the assistant uses. The maximum length is 32768 characters.
    /// </summary>
    [JsonPropertyName("instructions")]
    public string? Instructions { get; set; }

    /// <summary>
    ///     A list of tools enabled on the assistant.
    /// </summary>
    [JsonPropertyName("tools")]
    public List<ToolDefinition>? Tools { get; set; }

    /// <summary>
    ///     A list of File IDs attached to this assistant.
    /// </summary>
    [JsonPropertyName("file_ids")]
    public List<string>? FileIds { get; set; }

    /// <summary>
    ///     Set of 16 key-value pairs that can be attached to an object.
    /// </summary>
    [JsonPropertyName("metadata")]
    public Dictionary<string, string>? Metadata { get; set; }

    /// <summary>
    ///     ID of the model to use
    /// </summary>
    [JsonPropertyName("model")]
    public string Model { get; set; }
}