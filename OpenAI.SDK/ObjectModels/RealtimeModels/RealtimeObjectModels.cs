using System.Text.Json;
using System.Text.Json.Serialization;
using Betalgo.Ranul.OpenAI.ObjectModels.RequestModels;
using Betalgo.Ranul.OpenAI.ObjectModels.ResponseModels;
using Betalgo.Ranul.OpenAI.ObjectModels.SharedModels;

// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedType.Global

namespace Betalgo.Ranul.OpenAI.ObjectModels.RealtimeModels;

#region Base Classes

/// <summary>
///     Base class for all events in the Realtime API.
/// </summary>
public abstract class EventBase
{
    protected EventBase()
    {
    }

    protected EventBase(string clientEventType)
    {
        Type = clientEventType;
    }

    /// <summary>
    ///     Optional client-generated ID used to identify this event
    /// </summary>
    [JsonPropertyName("event_id")]
    public string? EventId { get; set; }

    /// <summary>
    ///     The event type that identifies the kind of event
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; }
}

/// <summary>
///     Base class for error events returned when an error occurs.
/// </summary>
public abstract class ErrorEventBase : EventBase
{
    /// <summary>
    ///     Details of the error.
    /// </summary>
    [JsonPropertyName("error")]
    public Error Error { get; set; }
}

#endregion

#region Session Models

/// <summary>
///     Configuration for a session.
/// </summary>
public class SessionConfig
{
    /// <summary>
    ///     The set of modalities the model can respond with. To disable audio, set this to ["text"].
    /// </summary>
    [JsonPropertyName("modalities")]
    public List<string>? Modalities { get; set; }

    /// <summary>
    ///     The default system instructions prepended to model calls.
    /// </summary>
    [JsonPropertyName("instructions")]
    public string? Instructions { get; set; }

    /// <summary>
    ///     The voice the model uses to respond.
    ///     Cannot be changed once the model has responded with audio at least once.
    /// </summary>
    [JsonPropertyName("voice")]
    public string? Voice { get; set; }

    /// <summary>
    ///     The format of input audio.
    /// </summary>
    [JsonPropertyName("input_audio_format")]
    public AudioFormat? InputAudioFormat { get; set; }

    /// <summary>
    ///     The format of output audio.
    /// </summary>
    [JsonPropertyName("output_audio_format")]
    public AudioFormat? OutputAudioFormat { get; set; }

    /// <summary>
    ///     Configuration for input audio transcription. Can be set to null to turn off.
    /// </summary>
    [JsonPropertyName("input_audio_transcription")]
    public AudioTranscriptionConfig? InputAudioTranscription { get; set; }

    /// <summary>
    ///     Configuration for turn detection. Can be set to null to turn off.
    /// </summary>
    [JsonPropertyName("turn_detection")]
    public TurnDetectionConfig? TurnDetection { get; set; }

    /// <summary>
    ///     Tools (functions) available to the model.
    /// </summary>
    [JsonPropertyName("tools")]
    public List<RealtimeToolDefinition>? Tools { get; set; }

    /// <summary>
    ///     How the model chooses tools.
    ///     <see cref="StaticValues.CompletionStatics.ToolChoiceType" />
    /// </summary>
    [JsonPropertyName("tool_choice")]
    public string? ToolChoice { get; set; }

    /// <summary>
    ///     Sampling temperature for the model.
    /// </summary>
    [JsonPropertyName("temperature")]
    public double? Temperature { get; set; }


    /// <summary>
    ///     Maximum number of output tokens for a single assistant response.
    ///     Can be an integer between 1 and 4096, or "inf" for maximum available tokens.
    /// </summary>
    [JsonPropertyName("max_output_tokens")]
    public MaxOutputTokens? MaxOutputTokens { get; set; }

    // Example usage method
    public void SetMaxTokens(int tokens)
    {
        MaxOutputTokens = MaxOutputTokens.FromInt(tokens);
    }

    public void SetInfiniteTokens()
    {
        MaxOutputTokens = MaxOutputTokens.Infinite();
    }
}

public class RealtimeToolDefinition
{
    /// <summary>
    ///     The type of the tool, i.e. function.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; }

    /// <summary>
    ///     The name of the function to be called. Must be a-z, A-Z, 0-9,
    ///     or contain underscores and dashes, with a maximum length of 64.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; }

    /// <summary>
    ///     A description of what the function does, used by the model to choose when and how to call the function.
    /// </summary>
    [JsonPropertyName("description")]
    public string? Description { get; set; }

    /// <summary>
    ///     Optional. The parameters the functions accepts, described as a JSON Schema object.
    ///     See the <a href="https://platform.openai.com/docs/guides/gpt/function-calling">guide</a> for examples,
    ///     and the <a href="https://json-schema.org/understanding-json-schema/">JSON Schema reference</a> for
    ///     documentation about the format.
    /// </summary>
    [JsonPropertyName("parameters")]
    public PropertyDefinition Parameters { get; set; }
}

/// <summary>
///     Configuration for audio transcription.
/// </summary>
public class AudioTranscriptionConfig
{
    /// <summary>
    ///     The model to use for transcription (e.g., "whisper-1").
    /// </summary>
    [JsonPropertyName("model")]
    public string Model { get; set; }
}

/// <summary>
///     Configuration for turn detection.
/// </summary>
public class TurnDetectionConfig
{
    /// <summary>
    ///     Type of turn detection, only "server_vad" is currently supported.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; }

    /// <summary>
    ///     Activation threshold for VAD (0.0 to 1.0).
    /// </summary>
    [JsonPropertyName("threshold")]
    public double Threshold { get; set; }

    /// <summary>
    ///     Amount of audio to include before speech starts (in milliseconds).
    /// </summary>
    [JsonPropertyName("prefix_padding_ms")]
    public int PrefixPaddingMs { get; set; }

    /// <summary>
    ///     Duration of silence to detect speech stop (in milliseconds).
    /// </summary>
    [JsonPropertyName("silence_duration_ms")]
    public int SilenceDurationMs { get; set; }
}

/// <summary>
///     JSON Schema for function parameters.
/// </summary>
public class JsonSchema
{
    /// <summary>
    ///     The type of the schema (e.g., "object").
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; }

    /// <summary>
    ///     Properties of the schema.
    /// </summary>
    [JsonPropertyName("properties")]
    public Dictionary<string, JsonSchemaProperty> Properties { get; set; }

    /// <summary>
    ///     Required properties.
    /// </summary>
    [JsonPropertyName("required")]
    public List<string>? Required { get; set; }
}

/// <summary>
///     JSON Schema property definition.
/// </summary>
public class JsonSchemaProperty
{
    /// <summary>
    ///     The type of the property.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; }

    /// <summary>
    ///     Description of the property.
    /// </summary>
    [JsonPropertyName("description")]
    public string? Description { get; set; }
}

#endregion

#region Conversation Models

/// <summary>
///     Represents an item in the conversation.
/// </summary>
public class ConversationItem
{
    /// <summary>
    ///     The unique ID of the item.
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; set; }

    /// <summary>
    ///     The type of the item.
    /// </summary>
    [JsonPropertyName("type")]
    public ItemType Type { get; set; }

    ///// <summary>
    /////     The status of the item.
    ///// </summary>
    [JsonPropertyName("status")]
    public Status? Status { get; set; }

    /// <summary>
    ///     The role of the message sender.
    /// </summary>
    [JsonPropertyName("role")]
    public Role? Role { get; set; }

    /// <summary>
    ///     The content of the message.
    /// </summary>
    [JsonPropertyName("content")]
    public List<ContentPart>? Content { get; set; }

    /// <summary>
    ///     The ID of the function call (for "function_call" items).
    /// </summary>
    [JsonPropertyName("call_id")]
    public string? CallId { get; set; }

    /// <summary>
    ///     The name of the function being called (for "function_call" items).
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    /// <summary>
    ///     The arguments of the function call (for "function_call" items).
    /// </summary>
    [JsonPropertyName("arguments")]
    public string? Arguments { get; set; }

    /// <summary>
    ///     The output of the function call (for "function_call_output" items).
    /// </summary>
    [JsonPropertyName("output")]
    public string? Output { get; set; }
}

/// <summary>
///     Represents a content part within a message.
/// </summary>
public class ContentPart
{
    /// <summary>
    ///     The content type.
    /// </summary>
    [JsonPropertyName("type")]
    public ContentType Type { get; set; }

    /// <summary>
    ///     The text content.
    /// </summary>
    [JsonPropertyName("text")]
    public string? Text { get; set; }

    /// <summary>
    ///     Base64-encoded audio data.
    /// </summary>
    [JsonPropertyName("audio")]
    public string? Audio { get; set; }

    /// <summary>
    ///     The transcript of the audio.
    /// </summary>
    [JsonPropertyName("transcript")]
    public string? Transcript { get; set; }
}

#endregion

#region Request Events

/// <summary>
///     Request to update the session's default configuration
/// </summary>
public class SessionUpdateRequest() : EventBase(RealtimeEventTypes.Client.Session.Update)
{
    /// <summary>
    ///     Session configuration to update
    /// </summary>
    [JsonPropertyName("session")]
    public SessionConfig Session { get; set; }
}

/// <summary>
///     Request to append audio bytes to the input audio buffer
/// </summary>
public class AudioBufferAppendRequest() : EventBase(RealtimeEventTypes.Client.InputAudioBuffer.Append)
{
    /// <summary>
    ///     Base64-encoded audio bytes
    /// </summary>
    [JsonPropertyName("audio")]
    public string Audio { get; set; }
}

/// <summary>
///     Request to commit audio bytes to a user message
/// </summary>
public class AudioBufferCommitRequest() : EventBase(RealtimeEventTypes.Client.InputAudioBuffer.Commit);

/// <summary>
///     Request to clear the audio bytes in the buffer
/// </summary>
public class AudioBufferClearRequest() : EventBase(RealtimeEventTypes.Client.InputAudioBuffer.Clear);

/// <summary>
///     Request to create a conversation item
/// </summary>
public class ConversationItemCreateRequest() : EventBase(RealtimeEventTypes.Client.Conversation.Item.Create)
{
    /// <summary>
    ///     The ID of the preceding item after which the new item will be inserted
    /// </summary>
    [JsonPropertyName("previous_item_id")]
    public string? PreviousItemId { get; set; }

    /// <summary>
    ///     The item to add to the conversation
    /// </summary>
    [JsonPropertyName("item")]
    public ConversationItem Item { get; set; }
}

/// <summary>
///     Request to truncate a previous assistant message's audio
/// </summary>
public class ConversationItemTruncateRequest() : EventBase(RealtimeEventTypes.Client.Conversation.Item.Truncate)
{
    /// <summary>
    ///     The ID of the assistant message item to truncate
    /// </summary>
    [JsonPropertyName("item_id")]
    public string ItemId { get; set; }

    /// <summary>
    ///     The index of the content part to truncate
    /// </summary>
    [JsonPropertyName("content_index")]
    public int ContentIndex { get; set; }

    /// <summary>
    ///     Inclusive duration up to which audio is truncated, in milliseconds
    /// </summary>
    [JsonPropertyName("audio_end_ms")]
    public int AudioEndMs { get; set; }
}

/// <summary>
///     Request to delete a conversation item
/// </summary>
public class ConversationItemDeleteRequest() : EventBase(RealtimeEventTypes.Client.Conversation.Item.Delete)
{
    /// <summary>
    ///     The ID of the item to delete
    /// </summary>
    [JsonPropertyName("item_id")]
    public string ItemId { get; set; }
}

/// <summary>
///     Request to trigger a response generation
/// </summary>
public class ResponseCreateRequest() : EventBase(RealtimeEventTypes.Client.Response.Create)
{
    /// <summary>
    ///     Configuration for the response
    /// </summary>
    [JsonPropertyName("response")]
    public ResponseConfig Response { get; set; }
}

/// <summary>
///     Configuration for generating a response.
/// </summary>
public class ResponseConfig
{
    /// <summary>
    ///     The modalities for the response.
    /// </summary>
    [JsonPropertyName("modalities")]
    public List<string>? Modalities { get; set; }

    /// <summary>
    ///     Instructions for the model.
    /// </summary>
    [JsonPropertyName("instructions")]
    public string? Instructions { get; set; }

    /// <summary>
    ///     The voice the model uses to respond.
    /// </summary>
    [JsonPropertyName("voice")]
    public string? Voice { get; set; }

    /// <summary>
    ///     The format of output audio.
    /// </summary>
    [JsonPropertyName("output_audio_format")]
    public AudioFormat? OutputAudioFormat { get; set; }

    /// <summary>
    ///     Tools (functions) available to the model.
    /// </summary>
    [JsonPropertyName("tools")]
    public List<FunctionDefinition>? Tools { get; set; }

    /// <summary>
    ///     How the model chooses tools.
    ///     <see cref="StaticValues.CompletionStatics.ToolChoiceType" />
    /// </summary>
    [JsonPropertyName("tool_choice")]
    public string? ToolChoice { get; set; }

    /// <summary>
    ///     Sampling temperature.
    /// </summary>
    [JsonPropertyName("temperature")]
    public double? Temperature { get; set; }

    /// <summary>
    ///     Maximum number of output tokens for a single assistant response.
    ///     Can be an integer between 1 and 4096, or "inf" for maximum available tokens.
    /// </summary>
    [JsonPropertyName("max_output_tokens")]
    public MaxOutputTokens? MaxOutputTokens { get; set; }
}

/// <summary>
///     Request to cancel an in-progress response
/// </summary>
public class ResponseCancelRequest() : EventBase(RealtimeEventTypes.Client.Response.Cancel);

#endregion

#region Server Events

/// <summary>
///     Error event returned when an error occurs.
/// </summary>
public class ErrorEvent : ErrorEventBase;

/// <summary>
///     Event returned when a session is created or updated.
/// </summary>
public class SessionEvent : EventBase
{
    /// <summary>
    ///     The session resource.
    /// </summary>
    [JsonPropertyName("session")]
    public SessionResource Session { get; set; }
}

/// <summary>
///     The session resource.
/// </summary>
public class SessionResource
{
    /// <summary>
    ///     The unique ID of the session.
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; set; }

    /// <summary>
    ///     The object type, must be "realtime.session".
    /// </summary>
    [JsonPropertyName("object")]
    public string Object { get; set; } = "realtime.session";

    /// <summary>
    ///     The default model used for this session.
    /// </summary>
    [JsonPropertyName("model")]
    public string Model { get; set; }

    /// <summary>
    ///     The set of modalities the model can respond with.
    /// </summary>
    [JsonPropertyName("modalities")]
    public List<string> Modalities { get; set; }

    /// <summary>
    ///     The default system instructions.
    /// </summary>
    [JsonPropertyName("instructions")]
    public string? Instructions { get; set; }

    /// <summary>
    ///     The voice the model uses to respond.
    /// </summary>
    [JsonPropertyName("voice")]
    public string Voice { get; set; }

    /// <summary>
    ///     The format of input audio.
    /// </summary>
    [JsonPropertyName("input_audio_format")]
    public AudioFormat InputAudioFormat { get; set; }

    /// <summary>
    ///     The format of output audio.
    /// </summary>
    [JsonPropertyName("output_audio_format")]
    public AudioFormat OutputAudioFormat { get; set; }

    /// <summary>
    ///     Configuration for input audio transcription.
    /// </summary>
    [JsonPropertyName("input_audio_transcription")]
    public AudioTranscriptionConfig? InputAudioTranscription { get; set; }

    /// <summary>
    ///     Configuration for turn detection.
    /// </summary>
    [JsonPropertyName("turn_detection")]
    public TurnDetectionConfig? TurnDetection { get; set; }

    /// <summary>
    ///     Tools (functions) available to the model.
    /// </summary>
    [JsonPropertyName("tools")]
    public List<FunctionDefinition> Tools { get; set; }

    /// <summary>
    ///     How the model chooses tools.
    ///     <see cref="StaticValues.CompletionStatics.ToolChoiceType" />
    /// </summary>
    [JsonPropertyName("tool_choice")]
    public string ToolChoice { get; set; }

    /// <summary>
    ///     Sampling temperature.
    /// </summary>
    [JsonPropertyName("temperature")]
    public double Temperature { get; set; }

    /// <summary>
    ///     Maximum number of output tokens.
    /// </summary>
    [JsonPropertyName("max_output_tokens")]
    public MaxOutputTokens? MaxOutputTokens { get; set; }
}

/// <summary>
///     Event returned when a conversation is created.
/// </summary>
public class ConversationCreatedEvent : EventBase
{
    /// <summary>
    ///     The conversation resource.
    /// </summary>
    [JsonPropertyName("conversation")]
    public ConversationResource Conversation { get; set; }
}

/// <summary>
///     The conversation resource.
/// </summary>
public class ConversationResource
{
    /// <summary>
    ///     The unique ID of the conversation.
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; set; }

    /// <summary>
    ///     The object type, must be "realtime.conversation".
    /// </summary>
    [JsonPropertyName("object")]
    public string Object { get; set; } = "realtime.conversation";
}

/// <summary>
///     Event returned when a conversation item is created.
/// </summary>
public class ConversationItemCreatedEvent : EventBase
{
    /// <summary>
    ///     The ID of the preceding item.
    /// </summary>
    [JsonPropertyName("previous_item_id")]
    public string? PreviousItemId { get; set; }

    /// <summary>
    ///     The item that was created.
    /// </summary>
    [JsonPropertyName("item")]
    public ConversationItem Item { get; set; }
}

/// <summary>
///     Event returned when input audio transcription is enabled and transcription succeeds.
/// </summary>
public class InputAudioTranscriptionCompletedEvent : EventBase
{
    /// <summary>
    ///     The ID of the user message item.
    /// </summary>
    [JsonPropertyName("item_id")]
    public string ItemId { get; set; }

    /// <summary>
    ///     The index of the content part containing the audio.
    /// </summary>
    [JsonPropertyName("content_index")]
    public int ContentIndex { get; set; }

    /// <summary>
    ///     The transcribed text.
    /// </summary>
    [JsonPropertyName("transcript")]
    public string Transcript { get; set; }
}

/// <summary>
///     Event returned when input audio transcription fails.
/// </summary>
public class InputAudioTranscriptionFailedEvent : EventBase
{
    /// <summary>
    ///     The ID of the user message item.
    /// </summary>
    [JsonPropertyName("item_id")]
    public string ItemId { get; set; }

    /// <summary>
    ///     The index of the content part containing the audio.
    /// </summary>
    [JsonPropertyName("content_index")]
    public int ContentIndex { get; set; }

    /// <summary>
    ///     Details of the transcription error.
    /// </summary>
    [JsonPropertyName("error")]
    public Error Error { get; set; }
}

/// <summary>
///     Event returned when an earlier assistant audio message item is truncated.
/// </summary>
public class ConversationItemTruncatedEvent : EventBase
{
    /// <summary>
    ///     The ID of the assistant message item that was truncated.
    /// </summary>
    [JsonPropertyName("item_id")]
    public string ItemId { get; set; }

    /// <summary>
    ///     The index of the content part that was truncated.
    /// </summary>
    [JsonPropertyName("content_index")]
    public int ContentIndex { get; set; }

    /// <summary>
    ///     The duration up to which the audio was truncated, in milliseconds.
    /// </summary>
    [JsonPropertyName("audio_end_ms")]
    public int AudioEndMs { get; set; }
}

/// <summary>
///     Event returned when an item in the conversation is deleted.
/// </summary>
public class ConversationItemDeletedEvent : EventBase
{
    /// <summary>
    ///     The ID of the item that was deleted.
    /// </summary>
    [JsonPropertyName("item_id")]
    public string ItemId { get; set; }
}

/// <summary>
///     Event returned when an input audio buffer is committed.
/// </summary>
public class AudioBufferCommittedEvent : EventBase
{
    /// <summary>
    ///     The ID of the preceding item after which the new item will be inserted.
    /// </summary>
    [JsonPropertyName("previous_item_id")]
    public string PreviousItemId { get; set; }

    /// <summary>
    ///     The ID of the user message item that will be created.
    /// </summary>
    [JsonPropertyName("item_id")]
    public string ItemId { get; set; }
}

/// <summary>
///     Event returned when the input audio buffer is cleared.
/// </summary>
public class AudioBufferClearedEvent : EventBase;

/// <summary>
///     Event returned in server turn detection mode when speech is detected.
/// </summary>
public class AudioBufferSpeechStartedEvent : EventBase
{
    /// <summary>
    ///     Milliseconds since the session started when speech was detected.
    /// </summary>
    [JsonPropertyName("audio_start_ms")]
    public int AudioStartMs { get; set; }

    /// <summary>
    ///     The ID of the user message item that will be created when speech stops.
    /// </summary>
    [JsonPropertyName("item_id")]
    public string ItemId { get; set; }
}

/// <summary>
///     Event returned in server turn detection mode when speech stops.
/// </summary>
public class AudioBufferSpeechStoppedEvent : EventBase
{
    /// <summary>
    ///     Milliseconds since the session started when speech stopped.
    /// </summary>
    [JsonPropertyName("audio_end_ms")]
    public int AudioEndMs { get; set; }

    /// <summary>
    ///     The ID of the user message item that will be created.
    /// </summary>
    [JsonPropertyName("item_id")]
    public string ItemId { get; set; }
}

/// <summary>
///     Event returned when a Response is created or done.
/// </summary>
public class ResponseEvent : EventBase
{
    /// <summary>
    ///     The response resource.
    /// </summary>
    [JsonPropertyName("response")]
    public ResponseResource Response { get; set; }
}

/// <summary>
///     The response resource.
/// </summary>
public class ResponseResource
{
    /// <summary>
    ///     The unique ID of the response.
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; set; }

    /// <summary>
    ///     The object type, must be "realtime.response".
    /// </summary>
    [JsonPropertyName("object")]
    public string Object { get; set; } = "realtime.response";

    /// <summary>
    ///     The status of the response.
    /// </summary>
    [JsonPropertyName("status")]
    public Status Status { get; set; }

    /// <summary>
    ///     Additional details about the status.
    /// </summary>
    [JsonPropertyName("status_details")]
    public Dictionary<string, string>? StatusDetails { get; set; }

    /// <summary>
    ///     The list of output items generated by the response.
    /// </summary>
    [JsonPropertyName("output")]
    public List<ConversationItem> Output { get; set; }

    /// <summary>
    ///     Usage statistics for the response.
    /// </summary>
    [JsonPropertyName("usage")]
    public UsageResponse? Usage { get; set; }
}

/// <summary>
///     Event returned when a new Item is created during response generation.
/// </summary>
public class ResponseOutputItemAddedEvent : EventBase
{
    /// <summary>
    ///     The ID of the response to which the item belongs.
    /// </summary>
    [JsonPropertyName("response_id")]
    public string ResponseId { get; set; }

    /// <summary>
    ///     The index of the output item in the response.
    /// </summary>
    [JsonPropertyName("output_index")]
    public int OutputIndex { get; set; }

    /// <summary>
    ///     The item that was added.
    /// </summary>
    [JsonPropertyName("item")]
    public ConversationItem Item { get; set; }
}

/// <summary>
///     Event returned when an Item is done streaming.
/// </summary>
public class ResponseOutputItemDoneEvent : EventBase
{
    /// <summary>
    ///     The ID of the response to which the item belongs.
    /// </summary>
    [JsonPropertyName("response_id")]
    public string ResponseId { get; set; }

    /// <summary>
    ///     The index of the output item in the response.
    /// </summary>
    [JsonPropertyName("output_index")]
    public int OutputIndex { get; set; }

    /// <summary>
    ///     The completed item.
    /// </summary>
    [JsonPropertyName("item")]
    public ConversationItem Item { get; set; }
}

/// <summary>
///     Event returned when new content part is added.
/// </summary>
public class ResponseContentPartEvent : EventBase
{
    /// <summary>
    ///     The ID of the response.
    /// </summary>
    [JsonPropertyName("response_id")]
    public string ResponseId { get; set; }

    /// <summary>
    ///     The ID of the item.
    /// </summary>
    [JsonPropertyName("item_id")]
    public string ItemId { get; set; }

    /// <summary>
    ///     The index of the output item in the response.
    /// </summary>
    [JsonPropertyName("output_index")]
    public int OutputIndex { get; set; }

    /// <summary>
    ///     The index of the content part in the item's content array.
    /// </summary>
    [JsonPropertyName("content_index")]
    public int ContentIndex { get; set; }

    /// <summary>
    ///     The content part.
    /// </summary>
    [JsonPropertyName("part")]
    public ContentPart Part { get; set; }
}

/// <summary>
///     Base class for streaming events with content information.
/// </summary>
public abstract class ContentStreamEventBase : EventBase
{
    /// <summary>
    ///     The ID of the response.
    /// </summary>
    [JsonPropertyName("response_id")]
    public string ResponseId { get; set; }

    /// <summary>
    ///     The ID of the item.
    /// </summary>
    [JsonPropertyName("item_id")]
    public string ItemId { get; set; }

    /// <summary>
    ///     The index of the output item in the response.
    /// </summary>
    [JsonPropertyName("output_index")]
    public int OutputIndex { get; set; }

    /// <summary>
    ///     The index of the content part in the item's content array.
    /// </summary>
    [JsonPropertyName("content_index")]
    public int ContentIndex { get; set; }
}

/// <summary>
///     Event for streaming text content.
/// </summary>
public class TextStreamEvent : ContentStreamEventBase
{
    /// <summary>
    ///     The text delta.
    /// </summary>
    [JsonPropertyName("delta")]
    public string? Delta { get; set; }

    /// <summary>
    ///     The complete text content (for done events).
    /// </summary>
    [JsonPropertyName("text")]
    public string? Text { get; set; }
}

/// <summary>
///     Event for streaming audio transcript content.
/// </summary>
public class AudioTranscriptStreamEvent : ContentStreamEventBase
{
    /// <summary>
    ///     The transcript delta.
    /// </summary>
    [JsonPropertyName("delta")]
    public string? Delta { get; set; }

    /// <summary>
    ///     The complete transcript (for done events).
    /// </summary>
    [JsonPropertyName("transcript")]
    public string? Transcript { get; set; }
}

/// <summary>
///     Event for streaming audio content.
/// </summary>
public class AudioStreamEvent : ContentStreamEventBase
{
    /// <summary>
    ///     Base64-encoded audio data delta.
    /// </summary>
    [JsonPropertyName("delta")]
    public string? Delta { get; set; }
}

/// <summary>
///     Event for streaming function call arguments.
/// </summary>
public class FunctionCallStreamEvent : EventBase
{
    /// <summary>
    ///     The ID of the response.
    /// </summary>
    [JsonPropertyName("response_id")]
    public string ResponseId { get; set; }

    /// <summary>
    ///     The ID of the function call item.
    /// </summary>
    [JsonPropertyName("item_id")]
    public string ItemId { get; set; }

    /// <summary>
    ///     The index of the output item in the response.
    /// </summary>
    [JsonPropertyName("output_index")]
    public int OutputIndex { get; set; }

    /// <summary>
    ///     The ID of the function call.
    /// </summary>
    [JsonPropertyName("call_id")]
    public string CallId { get; set; }

    /// <summary>
    ///     The arguments delta as a JSON string.
    /// </summary>
    [JsonPropertyName("delta")]
    public string? Delta { get; set; }

    /// <summary>
    ///     The final arguments as a JSON string.
    /// </summary>
    [JsonPropertyName("arguments")]
    public string? Arguments { get; set; }

    /// <summary>
    ///     The final arguments as a JSON string.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }
}

/// <summary>
///     Event containing rate limit information.
/// </summary>
public class RateLimitsEvent : EventBase
{
    /// <summary>
    ///     List of rate limit information.
    /// </summary>
    [JsonPropertyName("rate_limits")]
    public List<RateLimit> RateLimits { get; set; }
}

/// <summary>
///     Information about a specific rate limit.
/// </summary>
public class RateLimit
{
    /// <summary>
    ///     The name of the rate limit ("requests", "tokens", "input_tokens", "output_tokens").
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; }

    /// <summary>
    ///     The maximum allowed value for the rate limit.
    /// </summary>
    [JsonPropertyName("limit")]
    public int Limit { get; set; }

    /// <summary>
    ///     The remaining value before the limit is reached.
    /// </summary>
    [JsonPropertyName("remaining")]
    public int Remaining { get; set; }

    /// <summary>
    ///     Seconds until the rate limit resets.
    /// </summary>
    [JsonPropertyName("reset_seconds")]
    public double ResetSeconds { get; set; }
}

#endregion

/// <summary>
///     Represents the maximum output tokens setting which can be either an integer or "inf"
/// </summary>
[JsonConverter(typeof(MaxOutputTokensConverter))]
public class MaxOutputTokens
{
    private readonly bool _isInfinite;
    private readonly int? _value;

    private MaxOutputTokens(int? value, bool isInfinite)
    {
        _value = value;
        _isInfinite = isInfinite;
    }

    /// <summary>
    ///     Gets the integer value if set, or null if infinite
    /// </summary>
    public int? Value => _value;

    /// <summary>
    ///     Gets whether this represents infinite tokens
    /// </summary>
    public bool IsInfinite => _isInfinite;

    /// <summary>
    ///     Creates a MaxOutputTokens instance with a specific token limit
    /// </summary>
    /// <param name="value">Token limit between 1 and 4096</param>
    /// <returns>MaxOutputTokens instance</returns>
    public static MaxOutputTokens FromInt(int value)
    {
        return new(value, false);
    }

    /// <summary>
    ///     Creates a MaxOutputTokens instance representing infinite tokens
    /// </summary>
    /// <returns>MaxOutputTokens instance</returns>
    public static MaxOutputTokens Infinite()
    {
        return new(null, true);
    }

    public override string ToString()
    {
        return IsInfinite ? "inf" : Value?.ToString() ?? "";
    }
}

/// <summary>
///     JSON converter for MaxOutputTokens to handle both integer and "inf" values
/// </summary>
public class MaxOutputTokensConverter : JsonConverter<MaxOutputTokens>
{
    public override MaxOutputTokens Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        // ReSharper disable once SwitchExpressionHandlesSomeKnownEnumValuesWithExceptionInDefault
        return reader.TokenType switch
        {
            JsonTokenType.String when reader.GetString() is "inf" => MaxOutputTokens.Infinite(),
            JsonTokenType.Number => MaxOutputTokens.FromInt(reader.GetInt32()),
            _ => throw new JsonException("Value must be an integer or 'inf'")
        };
    }

    public override void Write(Utf8JsonWriter writer, MaxOutputTokens value, JsonSerializerOptions options)
    {
        if (value.IsInfinite)
            writer.WriteStringValue("inf");
        else if (value.Value.HasValue)
            writer.WriteNumberValue(value.Value.Value);
        else
            writer.WriteNullValue();
    }
}