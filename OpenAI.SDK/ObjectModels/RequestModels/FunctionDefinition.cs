using System.Text.Json.Serialization;
using OpenAI.ObjectModels.SharedModels;

namespace OpenAI.ObjectModels.RequestModels;

/// <summary>
///     Definition of a valid function call.
/// </summary>
public class FunctionDefinition
{
    /// <summary>
    ///     Required. The name of the function to be called. Must be a-z, A-Z, 0-9,
    ///     or contain underscores and dashes, with a maximum length of 64.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; }

    /// <summary>
    ///     Optional. The description of what the function does.
    /// </summary>
    [JsonPropertyName("description")]
    public string? Description { get; set; }

    /// <summary>
    ///     Optional. The parameters the functions accepts, described as a JSON Schema object.
    ///     See the guide (https://platform.openai.com/docs/guides/gpt/function-calling) for examples,
    ///     and the JSON Schema reference (https://json-schema.org/understanding-json-schema/)
    ///     for documentation about the format.
    /// </summary>
    [JsonPropertyName("parameters")]
    public PropertyDefinition? Parameters { get; set; }
}