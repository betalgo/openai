﻿using System.Text.Json.Serialization;
using Betalgo.Ranul.OpenAI.Contracts.Enums;
using Betalgo.Ranul.OpenAI.ObjectModels.RequestModels;

namespace Betalgo.Ranul.OpenAI.ObjectModels.SharedModels;

/// <summary>
///     Details on the action required to continue the run.
/// </summary>
public class RequiredAction
{
    /// <summary>
    ///     For now, this is always submit_tool_outputs.
    /// </summary>
    [JsonPropertyName("type")]
    public RequiredActionType Type { get; set; }

    /// <summary>
    ///     Details on the tool outputs needed for this run to continue.
    /// </summary>
    [JsonPropertyName("submit_tool_outputs")]
    public SubmitToolOutputs SubmitToolOutputs { get; set; }
}

public class SubmitToolOutputs
{
    /// <summary>
    ///     A list of the relevant tool calls.
    /// </summary>
    [JsonPropertyName("tool_calls")]
    public List<ToolCall> ToolCalls { get; set; }
}