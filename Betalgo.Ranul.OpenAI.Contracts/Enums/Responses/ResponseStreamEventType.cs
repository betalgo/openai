using System.Text.Json;
using System.Text.Json.Serialization;

namespace Betalgo.Ranul.OpenAI.Contracts.Enums.Responses;

/// <summary>
///     The type of a streaming event in the Responses API.
///     <see href="https://platform.openai.com/docs/api-reference/responses-streaming">OpenAI API documentation</see>.
/// </summary>
[JsonConverter(typeof(Converter))]
public readonly struct ResponseStreamEventType(string value) : IEquatable<ResponseStreamEventType>
{
    #region Lifecycle Events (7)

    /// <summary>
    ///     Emitted when a response is created.
    /// </summary>
    public static ResponseStreamEventType ResponseCreated { get; } = new("response.created");

    /// <summary>
    ///     Emitted when a response is queued.
    /// </summary>
    public static ResponseStreamEventType ResponseQueued { get; } = new("response.queued");

    /// <summary>
    ///     Emitted when the response is in progress.
    /// </summary>
    public static ResponseStreamEventType ResponseInProgress { get; } = new("response.in_progress");

    /// <summary>
    ///     Emitted when the response is completed.
    /// </summary>
    public static ResponseStreamEventType ResponseCompleted { get; } = new("response.completed");

    /// <summary>
    ///     Emitted when the response fails.
    /// </summary>
    public static ResponseStreamEventType ResponseFailed { get; } = new("response.failed");

    /// <summary>
    ///     Emitted when the response is incomplete.
    /// </summary>
    public static ResponseStreamEventType ResponseIncomplete { get; } = new("response.incomplete");

    /// <summary>
    ///     Emitted when a streaming error occurs.
    /// </summary>
    public static ResponseStreamEventType Error { get; } = new("error");

    #endregion

    #region Output Structure Events (4)

    /// <summary>
    ///     Emitted when a new output item is added.
    /// </summary>
    public static ResponseStreamEventType OutputItemAdded { get; } = new("response.output_item.added");

    /// <summary>
    ///     Emitted when an output item is done.
    /// </summary>
    public static ResponseStreamEventType OutputItemDone { get; } = new("response.output_item.done");

    /// <summary>
    ///     Emitted when a content part is added.
    /// </summary>
    public static ResponseStreamEventType ContentPartAdded { get; } = new("response.content_part.added");

    /// <summary>
    ///     Emitted when a content part is done.
    /// </summary>
    public static ResponseStreamEventType ContentPartDone { get; } = new("response.content_part.done");

    #endregion

    #region Text Output Events (3)

    /// <summary>
    ///     Emitted when there is a text delta.
    /// </summary>
    public static ResponseStreamEventType OutputTextDelta { get; } = new("response.output_text.delta");

    /// <summary>
    ///     Emitted when text output is done.
    /// </summary>
    public static ResponseStreamEventType OutputTextDone { get; } = new("response.output_text.done");

    /// <summary>
    ///     Emitted when an annotation is added to output text.
    /// </summary>
    public static ResponseStreamEventType OutputTextAnnotationAdded { get; } = new("response.output_text.annotation.added");

    #endregion

    #region Refusal Events (2)

    /// <summary>
    ///     Emitted when there is a refusal delta.
    /// </summary>
    public static ResponseStreamEventType RefusalDelta { get; } = new("response.refusal.delta");

    /// <summary>
    ///     Emitted when refusal is done.
    /// </summary>
    public static ResponseStreamEventType RefusalDone { get; } = new("response.refusal.done");

    #endregion

    #region Reasoning Events (6)

    /// <summary>
    ///     Emitted when there is a reasoning text delta.
    /// </summary>
    public static ResponseStreamEventType ReasoningTextDelta { get; } = new("response.reasoning_text.delta");

    /// <summary>
    ///     Emitted when reasoning text is done.
    /// </summary>
    public static ResponseStreamEventType ReasoningTextDone { get; } = new("response.reasoning_text.done");

    /// <summary>
    ///     Emitted when there is a reasoning summary text delta.
    /// </summary>
    public static ResponseStreamEventType ReasoningSummaryTextDelta { get; } = new("response.reasoning_summary_text.delta");

    /// <summary>
    ///     Emitted when reasoning summary text is done.
    /// </summary>
    public static ResponseStreamEventType ReasoningSummaryTextDone { get; } = new("response.reasoning_summary_text.done");

    /// <summary>
    ///     Emitted when a reasoning summary part is added.
    /// </summary>
    public static ResponseStreamEventType ReasoningSummaryPartAdded { get; } = new("response.reasoning_summary_part.added");

    /// <summary>
    ///     Emitted when a reasoning summary part is done.
    /// </summary>
    public static ResponseStreamEventType ReasoningSummaryPartDone { get; } = new("response.reasoning_summary_part.done");

    #endregion

    #region Function Call Events (2)

    /// <summary>
    ///     Emitted when there is a function call arguments delta.
    /// </summary>
    public static ResponseStreamEventType FunctionCallArgumentsDelta { get; } = new("response.function_call_arguments.delta");

    /// <summary>
    ///     Emitted when function call arguments are done.
    /// </summary>
    public static ResponseStreamEventType FunctionCallArgumentsDone { get; } = new("response.function_call_arguments.done");

    #endregion

    #region Custom Tool Events (2)

    /// <summary>
    ///     Emitted when there is a custom tool call input delta.
    /// </summary>
    public static ResponseStreamEventType CustomToolCallInputDelta { get; } = new("response.custom_tool_call_input.delta");

    /// <summary>
    ///     Emitted when custom tool call input is done.
    /// </summary>
    public static ResponseStreamEventType CustomToolCallInputDone { get; } = new("response.custom_tool_call_input.done");

    #endregion

    #region Code Interpreter Events (5)

    /// <summary>
    ///     Emitted when a code interpreter call is in progress.
    /// </summary>
    public static ResponseStreamEventType CodeInterpreterCallInProgress { get; } = new("response.code_interpreter_call.in_progress");

    /// <summary>
    ///     Emitted when a code interpreter call is interpreting.
    /// </summary>
    public static ResponseStreamEventType CodeInterpreterCallInterpreting { get; } = new("response.code_interpreter_call.interpreting");

    /// <summary>
    ///     Emitted when there is a code interpreter code delta.
    /// </summary>
    public static ResponseStreamEventType CodeInterpreterCallCodeDelta { get; } = new("response.code_interpreter_call_code.delta");

    /// <summary>
    ///     Emitted when code interpreter code is done.
    /// </summary>
    public static ResponseStreamEventType CodeInterpreterCallCodeDone { get; } = new("response.code_interpreter_call_code.done");

    /// <summary>
    ///     Emitted when a code interpreter call is completed.
    /// </summary>
    public static ResponseStreamEventType CodeInterpreterCallCompleted { get; } = new("response.code_interpreter_call.completed");

    #endregion

    #region File Search Events (3)

    /// <summary>
    ///     Emitted when a file search call is in progress.
    /// </summary>
    public static ResponseStreamEventType FileSearchCallInProgress { get; } = new("response.file_search_call.in_progress");

    /// <summary>
    ///     Emitted when a file search call is searching.
    /// </summary>
    public static ResponseStreamEventType FileSearchCallSearching { get; } = new("response.file_search_call.searching");

    /// <summary>
    ///     Emitted when a file search call is completed.
    /// </summary>
    public static ResponseStreamEventType FileSearchCallCompleted { get; } = new("response.file_search_call.completed");

    #endregion

    #region Web Search Events (3)

    /// <summary>
    ///     Emitted when a web search call is in progress.
    /// </summary>
    public static ResponseStreamEventType WebSearchCallInProgress { get; } = new("response.web_search_call.in_progress");

    /// <summary>
    ///     Emitted when a web search call is searching.
    /// </summary>
    public static ResponseStreamEventType WebSearchCallSearching { get; } = new("response.web_search_call.searching");

    /// <summary>
    ///     Emitted when a web search call is completed.
    /// </summary>
    public static ResponseStreamEventType WebSearchCallCompleted { get; } = new("response.web_search_call.completed");

    #endregion

    #region Image Generation Events (4)

    /// <summary>
    ///     Emitted when an image generation call is in progress.
    /// </summary>
    public static ResponseStreamEventType ImageGenCallInProgress { get; } = new("response.image_generation_call.in_progress");

    /// <summary>
    ///     Emitted when an image generation call is generating.
    /// </summary>
    public static ResponseStreamEventType ImageGenCallGenerating { get; } = new("response.image_generation_call.generating");

    /// <summary>
    ///     Emitted when a partial image is available.
    /// </summary>
    public static ResponseStreamEventType ImageGenCallPartialImage { get; } = new("response.image_generation_call.partial_image");

    /// <summary>
    ///     Emitted when an image generation call is completed.
    /// </summary>
    public static ResponseStreamEventType ImageGenCallCompleted { get; } = new("response.image_generation_call.completed");

    #endregion

    #region MCP Events (8)

    /// <summary>
    ///     Emitted when there is an MCP call arguments delta.
    /// </summary>
    public static ResponseStreamEventType MCPCallArgumentsDelta { get; } = new("response.mcp_call_arguments.delta");

    /// <summary>
    ///     Emitted when MCP call arguments are done.
    /// </summary>
    public static ResponseStreamEventType MCPCallArgumentsDone { get; } = new("response.mcp_call_arguments.done");

    /// <summary>
    ///     Emitted when an MCP call is in progress.
    /// </summary>
    public static ResponseStreamEventType MCPCallInProgress { get; } = new("response.mcp_call.in_progress");

    /// <summary>
    ///     Emitted when an MCP call is completed.
    /// </summary>
    public static ResponseStreamEventType MCPCallCompleted { get; } = new("response.mcp_call.completed");

    /// <summary>
    ///     Emitted when an MCP call fails.
    /// </summary>
    public static ResponseStreamEventType MCPCallFailed { get; } = new("response.mcp_call.failed");

    /// <summary>
    ///     Emitted when MCP list tools is in progress.
    /// </summary>
    public static ResponseStreamEventType MCPListToolsInProgress { get; } = new("response.mcp_list_tools.in_progress");

    /// <summary>
    ///     Emitted when MCP list tools is completed.
    /// </summary>
    public static ResponseStreamEventType MCPListToolsCompleted { get; } = new("response.mcp_list_tools.completed");

    /// <summary>
    ///     Emitted when MCP list tools fails.
    /// </summary>
    public static ResponseStreamEventType MCPListToolsFailed { get; } = new("response.mcp_list_tools.failed");

    #endregion

    #region Audio Events (4)

    /// <summary>
    ///     Emitted when there is an audio delta.
    /// </summary>
    public static ResponseStreamEventType AudioDelta { get; } = new("response.audio.delta");

    /// <summary>
    ///     Emitted when audio is done.
    /// </summary>
    public static ResponseStreamEventType AudioDone { get; } = new("response.audio.done");

    /// <summary>
    ///     Emitted when there is an audio transcript delta.
    /// </summary>
    public static ResponseStreamEventType AudioTranscriptDelta { get; } = new("response.audio.transcript.delta");

    /// <summary>
    ///     Emitted when audio transcript is done.
    /// </summary>
    public static ResponseStreamEventType AudioTranscriptDone { get; } = new("response.audio.transcript.done");

    #endregion

    /// <summary>
    ///     The underlying string value of the event type.
    /// </summary>
    public string Value { get; } = value;

    /// <inheritdoc />
    public override string ToString() => Value;

    /// <inheritdoc />
    public bool Equals(ResponseStreamEventType other) => string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);

    /// <inheritdoc />
    public override bool Equals(object? obj) => obj is ResponseStreamEventType other && Equals(other);

    /// <inheritdoc />
    public override int GetHashCode() => StringComparer.OrdinalIgnoreCase.GetHashCode(Value ?? string.Empty);

    /// <summary>
    ///     Determines whether two <see cref="ResponseStreamEventType" /> values are equal.
    /// </summary>
    public static bool operator ==(ResponseStreamEventType left, ResponseStreamEventType right) => left.Equals(right);

    /// <summary>
    ///     Determines whether two <see cref="ResponseStreamEventType" /> values are not equal.
    /// </summary>
    public static bool operator !=(ResponseStreamEventType left, ResponseStreamEventType right) => !(left == right);

    /// <summary>
    ///     Implicitly converts a <see cref="ResponseStreamEventType" /> to a string.
    /// </summary>
    public static implicit operator string(ResponseStreamEventType eventType) => eventType.Value;

    /// <summary>
    ///     Implicitly converts a string to a <see cref="ResponseStreamEventType" />.
    /// </summary>
    public static implicit operator ResponseStreamEventType(string value) => new(value);

    /// <summary>
    ///     JSON converter for <see cref="ResponseStreamEventType" />.
    /// </summary>
    public sealed class Converter : JsonConverter<ResponseStreamEventType>
    {
        /// <inheritdoc />
        public override ResponseStreamEventType Read(ref Utf8JsonReader reader, Type t, JsonSerializerOptions o) => new(reader.GetString()!);

        /// <inheritdoc />
        public override void Write(Utf8JsonWriter writer, ResponseStreamEventType value, JsonSerializerOptions o) => writer.WriteStringValue(value.Value);
    }
}
