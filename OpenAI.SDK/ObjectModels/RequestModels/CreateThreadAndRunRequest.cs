using System.Text.Json.Serialization;
using OpenAI.ObjectModels.SharedModels;

namespace OpenAI.ObjectModels.RequestModels;

public class CreateThreadAndRunRequest : IOpenAiModels.IAssistantId
{
    /// <summary>
    ///     The ID of the [assistant](/docs/api-reference/assistants) to use to execute this run.
    /// </summary>
    [JsonPropertyName("assistant_id")]
    public string AssistantId { get; set; }

    [JsonPropertyName("thread")]
    public ThreadCreateRequest Thread { get; set; }

    /// <summary>
    ///     The ID of the [Model](/docs/api-reference/models) to be used to execute this run. If a value is provided here, it
    ///     will override the model associated with the assistant. If not, the model associated with the assistant will be
    ///     used.
    /// </summary>
    [JsonPropertyName("model")]
    public string Model { get; set; }

    /// <summary>
    ///     Override the default system message of the assistant. This is useful for modifying the behavior on a per-run basis.
    /// </summary>
    [JsonPropertyName("instructions")]
    public string? Instructions { get; set; }

    /// <summary>
    ///     Override the tools the assistant can use for this run. This is useful for modifying the behavior on a per-run
    ///     basis.
    /// </summary>
    [JsonPropertyName("tools")]
    public List<ToolsItem>? Tools { get; set; }

    /// <summary>
    ///     A set of resources that are used by the assistant&apos;s tools. The resources are specific to the type of tool. For
    ///     example, the `code_interpreter` tool requires a list of file IDs, while the `file_search` tool requires a list of
    ///     vector store IDs.
    /// </summary>
    [JsonPropertyName("tool_resources")]
    public ToolResources? ToolResources { get; set; }

    /// <summary>
    ///     Set of 16 key-value pairs that can be attached to an object. This can be useful for storing additional information
    ///     about the object in a structured format. Keys can be a maximum of 64 characters long and values can be a maxium of
    ///     512 characters long.
    /// </summary>
    [JsonPropertyName("metadata")]
    public Dictionary<string, string>? Metadata { get; set; }

    /// <summary>
    ///     What sampling temperature to use, between 0 and 2. Higher values like 0.8 will make the output more random, while
    ///     lower values like 0.2 will make it more focused and deterministic.
    /// </summary>
    [JsonPropertyName("temperature")]
    public double? Temperature { get; set; }

    /// <summary>
    ///     An alternative to sampling with temperature, called nucleus sampling, where the model considers the results of the
    ///     tokens with top_p probability mass. So 0.1 means only the tokens comprising the top 10% probability mass are
    ///     considered.
    ///     We generally recommend altering this or temperature but not both.
    /// </summary>
    [JsonPropertyName("top_p")]
    public double? TopP { get; set; }

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
    ///     Controls for how a thread will be truncated prior to the run. Use this to control the intial context window of the
    ///     run.
    /// </summary>
    [JsonPropertyName("truncation_strategy")]
    public TruncationStrategy TruncationStrategy { get; set; }

    /// <summary>
    ///     Controls which (if any) tool is called by the model.
    ///     `none` means the model will not call any tools and instead generates a message.
    ///     `auto` is the default value and means the model can pick between generating a message or calling a tool.
    ///     Specifying a particular tool like `{&quot;type&quot;: &quot;file_search&quot;}` or `{&quot;type&quot;: &quot;
    ///     function&quot;, &quot;function&quot;: {&quot;name&quot;: &quot;my_function&quot;}}` forces the model to call that
    ///     tool.
    /// </summary>
    
    [JsonPropertyName("tool_choice")]
    public AssistantsApiToolChoiceOneOfType ToolChoice { get; set; }

    /// <summary>
    ///     Specifies the format that the model must output. Compatible with [GPT-4 Turbo](/docs/models/gpt-4-and-gpt-4-turbo)
    ///     and all GPT-3.5 Turbo models since `gpt-3.5-turbo-1106`.
    ///     Setting to `{ &quot;type&quot;: &quot;json_object&quot; }` enables JSON mode, which guarantees the message the
    ///     model generates is valid JSON.
    ///     **Important:** when using JSON mode, you **must** also instruct the model to produce JSON yourself via a system or
    ///     user message. Without this, the model may generate an unending stream of whitespace until the generation reaches
    ///     the token limit, resulting in a long-running and seemingly &quot;stuck&quot; request. Also note that the message
    ///     content may be partially cut off if `finish_reason=&quot;length&quot;`, which indicates the generation exceeded
    ///     `max_tokens` or the conversation exceeded the max context length.
    /// </summary>
    [JsonPropertyName("response_format")]
    public ResponseFormatOneOfType ResponseFormat { get; set; }

    
}
[JsonConverter(typeof(AssistantsApiToolChoiceConverter))]
public class AssistantsApiToolChoiceOneOfType
{
    [JsonIgnore]
    public string? AsString { get; set; }

    [JsonIgnore]
    public ToolChoice? AsObject{ get; set; }
}

public class ToolResources
{
    [JsonPropertyName("code_interpreter")]
    public CodeInterpreter CodeInterpreter { get; set; }

    [JsonPropertyName("file_search")]
    public FileSearch FileSearch { get; set; }

}
public class FileSearch
{
    /// <summary>
    /// The vector store attached to this assistant. There can be a maximum of 1 vector store attached to the assistant.
    /// </summary>
    [JsonPropertyName("vector_store_ids")]
    public List<string>? VectorStoreIds { get; set; }
    /// <summary>
    /// A helper to create a vector store with file_ids and attach it to this assistant. There can be a maximum of 1 vector store attached to the assistant.
    /// </summary>
    [JsonPropertyName("vector_stores")]
    public List<VectorStores>? VectorStores { get; set; }
}

public class VectorStores : IOpenAiModels.IFileIds, IOpenAiModels.IMetaData
{
    /// <summary>
    /// A list of file IDs to add to the vector store. There can be a maximum of 10000 files in a vector store.
    /// </summary>
    [JsonPropertyName("file_ids")]
    public List<string>? FileIds { get; set; }
    /// <summary>
    /// Set of 16 key-value pairs that can be attached to a vector store. This can be useful for storing additional information about the vector store in a structured format. Keys can be a maximum of 64 characters long and values can be a maxium of 512 characters long.
    /// </summary>
    [JsonPropertyName("metadata")]
    public Dictionary<string, string>? Metadata { get; set; }
}
public class CodeInterpreter : IOpenAiModels.IFileIds
{
    /// <summary>
    /// A list of file IDs made available to the code_interpreter tool. There can be a maximum of 20 files associated with the tool.
    /// </summary>
    [JsonPropertyName("file_ids")]
    public List<string>? FileIds { get; set; }
}
public class ToolsItem
{
    /// <summary>
    /// The type of the tool. Currently, only `function` is supported.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("function")]
    public Function Function { get; set; }
}

public class Function
{
    /// <summary>
    /// The name of the function.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; }

    /// <summary>
    /// The arguments passed to the function.
    /// </summary>
    [JsonPropertyName("arguments")]
    public string Arguments { get; set; }

    /// <summary>
    /// The output of the function. This will be `null` if the outputs have not been [submitted](/docs/api-reference/runs/submitToolOutputs) yet.
    /// </summary>
    [JsonPropertyName("output")]
    public string? Output { get; set; }
}