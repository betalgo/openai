using System.Text.Json.Serialization;

namespace OpenAI.ObjectModels.RequestModels;

/// <summary>
///     Definition of a valid tool.
/// </summary>
public class ToolDefinition
{
    /// <summary>
    ///     Required. The type of the tool. Currently, only function is supported.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = StaticValues.CompletionStatics.ToolType.Function;

    /// <summary>
    ///     Required. The description of what the function does.
    /// </summary>
    [JsonPropertyName("function")]
    public FunctionDefinition? Function { get; set; }
}