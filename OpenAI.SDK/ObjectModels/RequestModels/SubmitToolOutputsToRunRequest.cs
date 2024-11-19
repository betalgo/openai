using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Betalgo.Ranul.OpenAI.ObjectModels.RequestModels;

public class SubmitToolOutputsToRunRequest
{
    /// <summary>
    ///     A list of tools for which the outputs are being submitted.
    /// </summary>
    [Required]
    [JsonPropertyName("tool_outputs")]
    public List<ToolOutput> ToolOutputs { get; set; }

    /// <summary>
    ///     If true, returns a stream of events that happen during the Run as server-sent events, terminating when the Run
    ///     enters a terminal state with a data: [DONE] message.
    /// </summary>
    [JsonPropertyName("stream")]
    public bool? Stream { get; set; }
}

/// <summary>
///     A list of tools for which the outputs are being submitted.
/// </summary>
public class ToolOutput
{
    /// <summary>
    ///     The ID of the tool call in the required_action object
    ///     within the run object the output is being submitted for.
    /// </summary>
    [JsonPropertyName("tool_call_id")]
    public string? ToolCallId { get; set; }

    /// <summary>
    ///     The output of the tool call to be submitted to continue the run.
    /// </summary>
    [JsonPropertyName("output")]
    public string? Output { get; set; }
}