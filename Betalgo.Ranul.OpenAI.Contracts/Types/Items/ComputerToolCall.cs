using System.Text.Json.Serialization;
using Betalgo.Ranul.OpenAI.Contracts.Enums.Responses;

namespace Betalgo.Ranul.OpenAI.Contracts.Types.Items;

/// <summary>
///     A tool call to a computer use tool. See the
///     <see href="https://platform.openai.com/docs/guides/tools-computer-use">computer use guide</see> for more information.
///     <see href="https://github.com/betalgo/openai/blob/master/Docs/openapi-split/components/schemas/computertoolcall.yml">
///         Source Definition
///     </see>
/// </summary>
public class ComputerToolCall : IOutputItem
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="ComputerToolCall" /> class.
    /// </summary>
    public ComputerToolCall()
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="ComputerToolCall" /> class with required parameters.
    /// </summary>
    /// <param name="id">The unique ID of the computer call.</param>
    /// <param name="callId">An identifier used when responding to the tool call with output.</param>
    /// <param name="action">The action to perform.</param>
    /// <param name="pendingSafetyChecks">The pending safety checks for the computer call.</param>
    /// <param name="status">The status of the item.</param>
    public ComputerToolCall(string id, string callId, object action, List<ComputerCallSafetyCheck> pendingSafetyChecks, ItemStatus status)
    {
        Id = id;
        CallId = callId;
        Action = action;
        PendingSafetyChecks = pendingSafetyChecks;
        Status = status;
    }

    /// <summary>
    ///     The type of the computer call. Always <c>computer_call</c>.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type => "computer_call";

    /// <summary>
    ///     The unique ID of the computer call.
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; set; } = null!;

    /// <summary>
    ///     An identifier used when responding to the tool call with output.
    /// </summary>
    [JsonPropertyName("call_id")]
    public string CallId { get; set; } = null!;

    /// <summary>
    ///     The action to perform. Can be click, double_click, drag, key_press, move, screenshot, scroll, type, or wait.
    /// </summary>
    [JsonPropertyName("action")]
    public object Action { get; set; } = null!;

    /// <summary>
    ///     The pending safety checks for the computer call.
    /// </summary>
    [JsonPropertyName("pending_safety_checks")]
    public List<ComputerCallSafetyCheck> PendingSafetyChecks { get; set; } = null!;

    /// <summary>
    ///     The status of the item. One of <c>in_progress</c>, <c>completed</c>, or <c>incomplete</c>.
    /// </summary>
    [JsonPropertyName("status")]
    public ItemStatus Status { get; set; }
}

