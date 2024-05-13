using System.Text.Json.Serialization;
using OpenAI.ObjectModels.SharedModels;

namespace OpenAI.ObjectModels.RequestModels;

public class RunCreateRequest : IOpenAiModels.IModel, IOpenAiModels.IMetaData, IOpenAiModels.ITemperature
{
    /// <summary>
    ///     The ID of the assistant to use to execute this run.
    /// </summary>
    [JsonPropertyName("assistant_id")]
    public string AssistantId { get; set; }

    /// <summary>
    ///     Overrides the instructions of the assistant.
    ///     This is useful for modifying the behavior on a per-run basis.
    /// </summary>
    [JsonPropertyName("instructions")]
    public string? Instructions { get; set; }

    /// <summary>
    ///     Appends additional instructions at the end of the instructions for the run.
    ///     This is useful for modifying the behavior on a per-run basis without overriding other instructions.
    /// </summary>
    [JsonPropertyName("additional_instructions")]
    public string? AdditionalInstructions { get; set; }

    /// <summary>
    ///     Adds additional messages to the thread before creating the run.
    /// </summary>
    [JsonPropertyName("additional_messages")]
    public List<MessageCreateRequest>? AdditionalMessages { get; set; }

    /// <summary>
    ///     Override the tools the assistant can use for this run.
    ///     This is useful for modifying the behavior on a per-run basis.
    /// </summary>
    [JsonPropertyName("tools")]
    public List<ToolDefinition>? Tools { get; set; }

    /// <summary>
    ///     If true, returns a stream of events that happen during the Run as server-sent events,
    ///     terminating when the Run enters a terminal state with a data: [DONE] message.
    /// </summary>
    [JsonPropertyName("stream")]
    public bool? Stream { get; set; }

    /// <summary>
    ///     The maximum number of prompt tokens that may be used over the course of the run.
    ///     The run will make a best effort to use only the number of prompt tokens specified, across multiple turns of the
    ///     run.
    ///     If the run exceeds the number of prompt tokens specified, the run will end with status complete.
    ///     See incomplete_details for more info.
    /// </summary>
    [JsonPropertyName("max_prompt_tokens")]
    public int? MaxPromptTokens { get; set; }

    /// <summary>
    ///     The maximum number of completion tokens that may be used over the course of the run.
    ///     The run will make a best effort to use only the number of completion tokens specified, across multiple turns of the
    ///     run.
    ///     If the run exceeds the number of completion tokens specified, the run will end with status complete.
    ///     See incomplete_details for more info.
    /// </summary>
    [JsonPropertyName("max_completion_tokens")]
    public int? MaxCompletionTokens { get; set; }

    /// <summary>
    ///     The truncation strategy to use for the thread.
    /// </summary>
    [JsonPropertyName("truncation_strategy")]
    public TruncationStrategy? TruncationStrategy { get; set; }

    /// <summary>
    ///     Controls which (if any) tool is called by the model.
    /// </summary>
    [JsonPropertyName("tool_choice")]
    public ToolChoice? ToolChoice { get; set; }

    /// <summary>
    ///     Specifies the format that the model must output.
    /// </summary>
    [JsonPropertyName("response_format")]
    public ResponseFormat? ResponseFormat { get; set; }

    /// <summary>
    ///     Set of 16 key-value pairs that can be attached to an object.
    ///     This can be useful for storing additional information about the object in a structured format.
    ///     Keys can be a maximum of 64 characters long and values can be a maxium of 512 characters long.
    /// </summary>
    [JsonPropertyName("metadata")]
    public Dictionary<string, string>? Metadata { get; set; }

    /// <summary>
    ///     The ID of the Model to be used to execute this run.
    ///     If a value is provided here, it will override the model associated with the assistant.
    ///     If not, the model associated with the assistant will be used.
    /// </summary>
    [JsonPropertyName("model")]
    public string? Model { get; set; }

    /// <summary>
    ///     What sampling temperature to use, between 0 and 2. Higher values like 0.8 will make the output more random,
    ///     while lower values like 0.2 will make it more focused and deterministic. Defaults to 1.
    /// </summary>
    [JsonPropertyName("temperature")]
    public float? Temperature { get; set; }
}