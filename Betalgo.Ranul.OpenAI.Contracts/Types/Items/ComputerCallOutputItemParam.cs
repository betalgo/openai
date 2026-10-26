using System.Text.Json.Serialization;
using Betalgo.Ranul.OpenAI.Contracts.Enums.Responses;

namespace Betalgo.Ranul.OpenAI.Contracts.Types.Items;

/// <summary>
///     The output of a computer tool call.
///     <see href="https://platform.openai.com/docs/guides/tools-computer-use">Computer Use Guide</see>.
///     <see href="https://github.com/betalgo/openai/blob/master/Docs/openapi-split/components/schemas/computercalloutputitemparam.yml">
///         Source Definition
///     </see>
/// </summary>
public class ComputerCallOutputItemParam : IInputItem
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="ComputerCallOutputItemParam" /> class.
    /// </summary>
    public ComputerCallOutputItemParam()
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="ComputerCallOutputItemParam" /> class with required parameters.
    /// </summary>
    /// <param name="callId">The ID of the computer tool call that produced the output.</param>
    /// <param name="output">The screenshot image output.</param>
    public ComputerCallOutputItemParam(string callId, ComputerScreenshotImage output)
    {
        CallId = callId;
        Output = output;
    }

    /// <summary>
    ///     The type of the computer tool call output. Always <c>computer_call_output</c>.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type => "computer_call_output";

    /// <summary>
    ///     The ID of the computer tool call output.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    /// <summary>
    ///     The ID of the computer tool call that produced the output.
    /// </summary>
    [JsonPropertyName("call_id")]
    public string CallId { get; set; } = null!;

    /// <summary>
    ///     The screenshot image output.
    /// </summary>
    [JsonPropertyName("output")]
    public ComputerScreenshotImage Output { get; set; } = null!;

    /// <summary>
    ///     The safety checks reported by the API that have been acknowledged by the developer.
    /// </summary>
    [JsonPropertyName("acknowledged_safety_checks")]
    public List<ComputerCallSafetyCheck>? AcknowledgedSafetyChecks { get; set; }

    /// <summary>
    ///     The status of the item.
    /// </summary>
    [JsonPropertyName("status")]
    public ItemStatus? Status { get; set; }
}

