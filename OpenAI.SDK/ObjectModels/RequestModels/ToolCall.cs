using System.Text.Json.Serialization;

namespace OpenAI.ObjectModels.RequestModels;

public class ToolCall
{
    /// <summary>
    ///     The Index of the tool call.
    /// </summary>
    [JsonPropertyName("index")]
    public int Index { get; set; }

    /// <summary>
    ///     The ID of the tool call.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    /// <summary>
    ///     The type of the tool. Currently, only function is supported.
    /// </summary>
    [JsonPropertyName("type")]
    public string? Type { get; set; }

    /// <summary>
    ///     The function that the model called.
    /// </summary>
    [JsonPropertyName("function")]
    public FunctionCall? FunctionCall { get; set; }
}