using System.Text.Json.Serialization;
using OpenAI.ObjectModels.SharedModels;

namespace OpenAI.ObjectModels.RequestModels;

/// <summary>
///     Definition of a valid function call.
/// </summary>
public class FunctionDefinition
{
    /// <summary>
    ///     The name of the function to be called. Must be a-z, A-Z, 0-9,
    ///     or contain underscores and dashes, with a maximum length of 64.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; }

    /// <summary>
    ///     A description of what the function does, used by the model to choose when and how to call the function.
    /// </summary>
    [JsonPropertyName("description")]
    public string? Description { get; set; }

    /// <summary>
    ///     Optional. The parameters the functions accepts, described as a JSON Schema object.
    ///     See the <a href="https://platform.openai.com/docs/guides/gpt/function-calling">guide</a> for examples,
    ///     and the <a href="https://json-schema.org/understanding-json-schema/">JSON Schema reference</a> for
    ///     documentation about the format.
    /// </summary>
    [JsonPropertyName("parameters")]
    public PropertyDefinition Parameters { get; set; }


    /// <summary>
    ///     Whether to enable strict schema adherence when generating the function call. If set to true, the model will follow
    ///     the exact schema defined in the parameters field. Only a subset of JSON Schema is supported when strict is true.
    ///     Learn more about Structured Outputs in the <a href="https://platform.openai.com/docs/api-reference/chat/docs/guides/function-calling">function calling guide</a>.
    /// </summary>
    [JsonPropertyName("strict")]
    public bool? Strict { get; set; }
}