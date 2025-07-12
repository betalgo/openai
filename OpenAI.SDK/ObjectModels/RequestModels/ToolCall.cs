using System.Text.Json.Serialization;
using Betalgo.Ranul.OpenAI.Contracts.Enums;

namespace Betalgo.Ranul.OpenAI.ObjectModels.RequestModels;

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
    public ToolCallTypeEnum? Type { get; set; }

    /// <summary>
    ///     The function that the model called.
    /// </summary>
    [JsonPropertyName("function")]
    public FunctionCall? FunctionCall { get; set; }
}