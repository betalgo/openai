using System.Text.Json.Serialization;
using Betalgo.OpenAI.ObjectModels.RequestModels;
using Betalgo.OpenAI.ObjectModels.ResponseModels;

namespace Betalgo.OpenAI.ObjectModels.SharedModels;

public record AssistantResponse : BaseResponse, IOpenAIModels.IId, IOpenAIModels.ICreatedAt, IOpenAIModels.IModel, IOpenAIModels.IMetaData, IOpenAIModels.ITools
{
    /// <summary>
    ///     The name of the assistant. The maximum length is 256 characters.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    /// <summary>
    ///     The description of the assistant. The maximum length is 512 characters.
    /// </summary>
    [JsonPropertyName("description")]
    public string? Description { get; set; }

    /// <summary>
    ///     The system instructions that the assistant uses.
    ///     The maximum length is 32768 characters.
    /// </summary>
    [JsonPropertyName("instructions")]
    public string? Instructions { get; set; }

    /// <summary>
    ///     A list of file IDs attached to this assistant.
    ///     There can be a maximum of 20 files attached to the assistant.
    ///     Files are ordered by their creation date in ascending order.
    /// </summary>
    [JsonPropertyName("file_ids")]
    public List<string> FileIds { get; set; }

    /// <summary>
    ///     A set of resources that are used by the assistant's tools. The resources are specific to the type of tool. For
    ///     example, the code_interpreter tool requires a list of file IDs, while the file_search tool requires a list of
    ///     vector store IDs.
    /// </summary>
    [JsonPropertyName("tool_resources")]
    public ToolResources? ToolResources { get; set; }

    /// <summary>
    ///     Specifies the format that the model must output. Compatible with GPT-4o, GPT-4 Turbo, and all GPT-3.5 Turbo models
    ///     since gpt-3.5-turbo-1106.
    ///     Setting to { "type": "json_schema", "json_schema": { ...} }
    ///     enables Structured Outputs which ensures the model will match your supplied JSON schema.Learn more in the
    ///     Structured Outputs guide.
    ///     Setting to { "type": "json_object" }
    ///     enables JSON mode, which ensures the message the model generates is valid JSON.
    ///     Important: when using JSON mode, you must also instruct the model to produce JSON yourself via a system or user
    ///     message.Without this, the model may generate an unending stream of whitespace until the generation reaches the
    ///     token limit, resulting in a long-running and seemingly "stuck" request.Also note that the message content may be
    ///     partially cut off if finish_reason= "length", which indicates the generation exceeded max_tokens or the
    ///     conversation exceeded the max context length.
    /// </summary>
    [JsonPropertyName("response_format")]
    public ResponseFormatOneOfType ResponseFormatOneOfType { get; set; }

    /// <summary>
    ///     The Unix timestamp (in seconds) for when the assistant was created.
    /// </summary>
    [JsonPropertyName("created_at")]
    public int CreatedAt { get; set; }

    /// <summary>
    ///     The identifier, which can be referenced in API endpoints.
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; set; }


    /// <summary>
    ///     Set of 16 key-value pairs that can be attached to an object.
    /// </summary>
    [JsonPropertyName("metadata")]
    public Dictionary<string, string> Metadata { get; set; }

    /// <summary>
    ///     ID of the model to use
    /// </summary>
    [JsonPropertyName("model")]
    public string Model { get; set; }

    /// <summary>
    ///     A list of tools enabled on the assistant.
    /// </summary>
    [JsonPropertyName("tools")]
    public List<ToolDefinition> Tools { get; set; }
}