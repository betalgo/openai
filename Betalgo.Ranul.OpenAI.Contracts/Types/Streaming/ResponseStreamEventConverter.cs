using System.Text.Json;
using System.Text.Json.Serialization;
using Betalgo.Ranul.OpenAI.Contracts.Types.Streaming.Events;
using Betalgo.Ranul.OpenAI.Contracts.Types.Streaming.Events.Audio;
using Betalgo.Ranul.OpenAI.Contracts.Types.Streaming.Events.FunctionCalls;
using Betalgo.Ranul.OpenAI.Contracts.Types.Streaming.Events.Lifecycle;
using Betalgo.Ranul.OpenAI.Contracts.Types.Streaming.Events.Output;
using Betalgo.Ranul.OpenAI.Contracts.Types.Streaming.Events.Text;
using Betalgo.Ranul.OpenAI.Contracts.Types.Streaming.Events.Tools;

namespace Betalgo.Ranul.OpenAI.Contracts.Types.Streaming;

/// <summary>
///     JSON converter for <see cref="IResponseStreamEvent" /> that handles polymorphic deserialization
///     based on the type discriminator.
///     <see href="https://platform.openai.com/docs/api-reference/responses-streaming">OpenAI API documentation</see>.
/// </summary>
public class ResponseStreamEventConverter : JsonConverter<IResponseStreamEvent>
{
    /// <inheritdoc />
    public override IResponseStreamEvent? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
            throw new JsonException("Expected StartObject token for ResponseStreamEvent");

        using var jsonDoc = JsonDocument.ParseValue(ref reader);
        if (!jsonDoc.RootElement.TryGetProperty("type", out var typeProp))
            throw new JsonException("Missing 'type' property in ResponseStreamEvent");

        var type = typeProp.GetString();
        var rawText = jsonDoc.RootElement.GetRawText();

        return type switch
        {
            // Lifecycle Events (7)
            "response.created" => JsonSerializer.Deserialize<ResponseCreatedEvent>(rawText, options),
            "response.queued" => JsonSerializer.Deserialize<ResponseQueuedEvent>(rawText, options),
            "response.in_progress" => JsonSerializer.Deserialize<ResponseInProgressEvent>(rawText, options),
            "response.completed" => JsonSerializer.Deserialize<ResponseCompletedEvent>(rawText, options),
            "response.failed" => JsonSerializer.Deserialize<ResponseFailedEvent>(rawText, options),
            "response.incomplete" => JsonSerializer.Deserialize<ResponseIncompleteEvent>(rawText, options),
            "error" => JsonSerializer.Deserialize<ResponseErrorEvent>(rawText, options),

            // Output Structure Events (4)
            "response.output_item.added" => JsonSerializer.Deserialize<ResponseOutputItemAddedEvent>(rawText, options),
            "response.output_item.done" => JsonSerializer.Deserialize<ResponseOutputItemDoneEvent>(rawText, options),
            "response.content_part.added" => JsonSerializer.Deserialize<ResponseContentPartAddedEvent>(rawText, options),
            "response.content_part.done" => JsonSerializer.Deserialize<ResponseContentPartDoneEvent>(rawText, options),

            // Text Output Events (3)
            "response.output_text.delta" => JsonSerializer.Deserialize<ResponseTextDeltaEvent>(rawText, options),
            "response.output_text.done" => JsonSerializer.Deserialize<ResponseTextDoneEvent>(rawText, options),
            "response.output_text.annotation.added" => JsonSerializer.Deserialize<ResponseOutputTextAnnotationAddedEvent>(rawText, options),

            // Refusal Events (2)
            "response.refusal.delta" => JsonSerializer.Deserialize<ResponseRefusalDeltaEvent>(rawText, options),
            "response.refusal.done" => JsonSerializer.Deserialize<ResponseRefusalDoneEvent>(rawText, options),

            // Reasoning Events (6)
            "response.reasoning_text.delta" => JsonSerializer.Deserialize<ResponseReasoningTextDeltaEvent>(rawText, options),
            "response.reasoning_text.done" => JsonSerializer.Deserialize<ResponseReasoningTextDoneEvent>(rawText, options),
            "response.reasoning_summary_text.delta" => JsonSerializer.Deserialize<ResponseReasoningSummaryTextDeltaEvent>(rawText, options),
            "response.reasoning_summary_text.done" => JsonSerializer.Deserialize<ResponseReasoningSummaryTextDoneEvent>(rawText, options),
            "response.reasoning_summary_part.added" => JsonSerializer.Deserialize<ResponseReasoningSummaryPartAddedEvent>(rawText, options),
            "response.reasoning_summary_part.done" => JsonSerializer.Deserialize<ResponseReasoningSummaryPartDoneEvent>(rawText, options),

            // Function Call Events (2)
            "response.function_call_arguments.delta" => JsonSerializer.Deserialize<ResponseFunctionCallArgumentsDeltaEvent>(rawText, options),
            "response.function_call_arguments.done" => JsonSerializer.Deserialize<ResponseFunctionCallArgumentsDoneEvent>(rawText, options),

            // Custom Tool Events (2)
            "response.custom_tool_call_input.delta" => JsonSerializer.Deserialize<ResponseCustomToolCallInputDeltaEvent>(rawText, options),
            "response.custom_tool_call_input.done" => JsonSerializer.Deserialize<ResponseCustomToolCallInputDoneEvent>(rawText, options),

            // Code Interpreter Events (5)
            "response.code_interpreter_call.in_progress" => JsonSerializer.Deserialize<ResponseCodeInterpreterCallInProgressEvent>(rawText, options),
            "response.code_interpreter_call.interpreting" => JsonSerializer.Deserialize<ResponseCodeInterpreterCallInterpretingEvent>(rawText, options),
            "response.code_interpreter_call_code.delta" => JsonSerializer.Deserialize<ResponseCodeInterpreterCallCodeDeltaEvent>(rawText, options),
            "response.code_interpreter_call_code.done" => JsonSerializer.Deserialize<ResponseCodeInterpreterCallCodeDoneEvent>(rawText, options),
            "response.code_interpreter_call.completed" => JsonSerializer.Deserialize<ResponseCodeInterpreterCallCompletedEvent>(rawText, options),

            // File Search Events (3)
            "response.file_search_call.in_progress" => JsonSerializer.Deserialize<ResponseFileSearchCallInProgressEvent>(rawText, options),
            "response.file_search_call.searching" => JsonSerializer.Deserialize<ResponseFileSearchCallSearchingEvent>(rawText, options),
            "response.file_search_call.completed" => JsonSerializer.Deserialize<ResponseFileSearchCallCompletedEvent>(rawText, options),

            // Web Search Events (3)
            "response.web_search_call.in_progress" => JsonSerializer.Deserialize<ResponseWebSearchCallInProgressEvent>(rawText, options),
            "response.web_search_call.searching" => JsonSerializer.Deserialize<ResponseWebSearchCallSearchingEvent>(rawText, options),
            "response.web_search_call.completed" => JsonSerializer.Deserialize<ResponseWebSearchCallCompletedEvent>(rawText, options),

            // Image Generation Events (4)
            "response.image_generation_call.in_progress" => JsonSerializer.Deserialize<ResponseImageGenCallInProgressEvent>(rawText, options),
            "response.image_generation_call.generating" => JsonSerializer.Deserialize<ResponseImageGenCallGeneratingEvent>(rawText, options),
            "response.image_generation_call.partial_image" => JsonSerializer.Deserialize<ResponseImageGenCallPartialImageEvent>(rawText, options),
            "response.image_generation_call.completed" => JsonSerializer.Deserialize<ResponseImageGenCallCompletedEvent>(rawText, options),

            // MCP Events (8)
            "response.mcp_call_arguments.delta" => JsonSerializer.Deserialize<ResponseMCPCallArgumentsDeltaEvent>(rawText, options),
            "response.mcp_call_arguments.done" => JsonSerializer.Deserialize<ResponseMCPCallArgumentsDoneEvent>(rawText, options),
            "response.mcp_call.in_progress" => JsonSerializer.Deserialize<ResponseMCPCallInProgressEvent>(rawText, options),
            "response.mcp_call.completed" => JsonSerializer.Deserialize<ResponseMCPCallCompletedEvent>(rawText, options),
            "response.mcp_call.failed" => JsonSerializer.Deserialize<ResponseMCPCallFailedEvent>(rawText, options),
            "response.mcp_list_tools.in_progress" => JsonSerializer.Deserialize<ResponseMCPListToolsInProgressEvent>(rawText, options),
            "response.mcp_list_tools.completed" => JsonSerializer.Deserialize<ResponseMCPListToolsCompletedEvent>(rawText, options),
            "response.mcp_list_tools.failed" => JsonSerializer.Deserialize<ResponseMCPListToolsFailedEvent>(rawText, options),

            // Audio Events (4)
            "response.audio.delta" => JsonSerializer.Deserialize<ResponseAudioDeltaEvent>(rawText, options),
            "response.audio.done" => JsonSerializer.Deserialize<ResponseAudioDoneEvent>(rawText, options),
            "response.audio.transcript.delta" => JsonSerializer.Deserialize<ResponseAudioTranscriptDeltaEvent>(rawText, options),
            "response.audio.transcript.done" => JsonSerializer.Deserialize<ResponseAudioTranscriptDoneEvent>(rawText, options),

            // Unknown/future events - graceful forward compatibility
            _ => CreateUnknownEvent(type, jsonDoc.RootElement, rawText)
        };
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, IResponseStreamEvent value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value, value.GetType(), options);
    }

    /// <summary>
    ///     Creates an <see cref="UnknownResponseStreamEvent" /> for forward compatibility with new event types.
    /// </summary>
    private static UnknownResponseStreamEvent CreateUnknownEvent(string type, JsonElement root, string rawText)
    {
        var sequenceNumber = root.TryGetProperty("sequence_number", out var seqProp) && seqProp.TryGetInt32(out var seq)
            ? seq
            : 0;

        return new UnknownResponseStreamEvent(type, sequenceNumber, rawText);
    }
}
