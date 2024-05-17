using System.Text.Json.Serialization;
using OpenAI.ObjectModels.SharedModels;

namespace OpenAI.ObjectModels.RequestModels;

public class RunCreateRequest : IOpenAiModels.IModel, IOpenAiModels.IMetaData, IOpenAiModels.ITemperature, IOpenAiModels.IAssistantId
{
    /// <summary>
    ///     Override the default system message of the assistant. This is useful for modifying the behavior on a per-run basis.
    /// </summary>
    [JsonPropertyName("instructions")]
    public string? Instructions { get; set; }

    /// <summary>
    ///     Appends additional instructions at the end of the instructions for the run. This is useful for modifying the
    ///     behavior on a per-run basis without overriding other instructions.
    /// </summary>
    [JsonPropertyName("additional_instructions")]
    public string? AdditionalInstructions { get; set; }

    /// <summary>
    ///     Adds additional messages to the thread before creating the run.
    /// </summary>
    [JsonPropertyName("additional_messages")]
    public List<MessageCreateRequest>? AdditionalMessages { get; set; }

    /// <summary>
    ///     Override the tools the assistant can use for this run. This is useful for modifying the behavior on a per-run
    ///     basis.
    /// </summary>
    [JsonPropertyName("tools")]
    public List<ToolDefinition>? Tools { get; set; }

    /// <summary>
    ///     An alternative to sampling with temperature, called nucleus sampling, where the model considers the results of the
    ///     tokens with top_p probability mass. So 0.1 means only the tokens comprising the top 10% probability mass are
    ///     considered.
    ///     We generally recommend altering this or temperature but not both.
    /// </summary>
    [JsonPropertyName("top_p")]
    public float? TopP { get; set; }

    /// <summary>
    ///     If `true`, returns a stream of events that happen during the Run as server-sent events, terminating when the Run
    ///     enters a terminal state with a `data: [DONE]` message.
    /// </summary>
    [JsonPropertyName("stream")]
    public bool? Stream { get; set; }

    /// <summary>
    ///     The maximum number of prompt tokens that may be used over the course of the run. The run will make a best effort to
    ///     use only the number of prompt tokens specified, across multiple turns of the run. If the run exceeds the number of
    ///     prompt tokens specified, the run will end with status `incomplete`. See `incomplete_details` for more info.
    /// </summary>
    [JsonPropertyName("max_prompt_tokens")]
    public int? MaxPromptTokens { get; set; }

    /// <summary>
    ///     The maximum number of completion tokens that may be used over the course of the run. The run will make a best
    ///     effort to use only the number of completion tokens specified, across multiple turns of the run. If the run exceeds
    ///     the number of completion tokens specified, the run will end with status `incomplete`. See `incomplete_details` for
    ///     more info.
    /// </summary>
    [JsonPropertyName("max_completion_tokens")]
    public int? MaxCompletionTokens { get; set; }

    /// <summary>
    ///     Controls how a thread will be truncated prior to the run. Use this to control the initial context window of the
    ///     run.
    /// </summary>
    [JsonPropertyName("truncation_strategy")]
    public TruncationStrategy? TruncationStrategy { get; set; }

    /// <summary>
    ///     Controls which (if any) tool is called by the model.
    /// </summary>
    [JsonPropertyName("tool_choice")]
    public ToolChoiceOneOfType? ToolChoice { get; set; }

    /// <summary>
    ///     Specifies the format that the model must output. Compatible with
    ///     <see href="https://platform.openai.com/docs/models/gpt-4o">GPT-4o</see>,
    ///     <see href="https://platform.openai.com/docs/models/gpt-4-turbo-and-gpt-4">GPT-4 Turbo</see>, and all GPT-3.5 Turbo
    ///     models since `gpt-3.5-turbo-1106`.
    /// </summary>
    [JsonPropertyName("response_format")]
    public ResponseFormatOneOfType? ResponseFormat { get; set; }


    /// <summary>
    ///     The ID of the <see href="https://platform.openai.com/docs/api-reference/assistants">assistant</see> to use to
    ///     execute this run.
    /// </summary>
    [JsonPropertyName("assistant_id")]
    public string AssistantId { get; set; }


    /// <summary>
    ///     Set of 16 key-value pairs that can be attached to an object. This can be useful for storing additional information
    ///     about the object in a structured format. Keys can be a maximum of 64 characters long and values can be a maxium of
    ///     512 characters long.
    /// </summary>
    [JsonPropertyName("metadata")]
    public Dictionary<string, string>? Metadata { get; set; }

    /// <summary>
    ///     The ID of the <see href="https://platform.openai.com/docs/api-reference/models">Model</see> to be used to execute
    ///     this run. If a value is provided here, it will override the model associated with the assistant. If not, the model
    ///     associated with the assistant will be used.
    /// </summary>
    [JsonPropertyName("model")]
    public string? Model { get; set; }

    /// <summary>
    ///     What sampling temperature to use, between 0 and 2. Higher values like 0.8 will make the output more random, while
    ///     lower values like 0.2 will make it more focused and deterministic.
    /// </summary>
    [JsonPropertyName("temperature")]
    public float? Temperature { get; set; }
}