using System.Text.Json;
using System.Text.Json.Serialization;

namespace Betalgo.Ranul.OpenAI.ObjectModels.RealtimeModels;

/// <summary>
/// Converts between JSON strings and Status enum values for the OpenAI Realtime API.
/// </summary>
internal class StatusJsonConverter : JsonConverter<Status>
{
    public override Status Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();
        return value switch
        {
            "completed" => Status.Completed,
            "in_progress" => Status.InProgress,
            "incomplete" => Status.Incomplete,
            "cancelled" => Status.Cancelled,
            "failed" => Status.Failed,
            _ => Status.Unimplemented // Return Unimplemented for unknown values
        };
    }

    public override void Write(Utf8JsonWriter writer, Status value, JsonSerializerOptions options)
    {
        var stringValue = value switch
        {
            Status.Completed => "completed",
            Status.InProgress => "in_progress",
            Status.Incomplete => "incomplete",
            Status.Cancelled => "cancelled",
            Status.Failed => "failed",
            _ => "unimplemented"
        };
        writer.WriteStringValue(stringValue);
    }
}

/// <summary>
/// Converts between JSON strings and AudioFormat enum values for the OpenAI Realtime API.
/// </summary>
internal class AudioFormatJsonConverter : JsonConverter<AudioFormat>
{
    public override AudioFormat Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();
        return value switch
        {
            RealtimeConstants.Audio.FormatPcm16 => AudioFormat.PCM16,
            RealtimeConstants.Audio.FormatG711Ulaw => AudioFormat.G711_ULAW,
            RealtimeConstants.Audio.FormatG711Alaw => AudioFormat.G711_ALAW,
            _ => AudioFormat.Unimplemented
        };
    }

    public override void Write(Utf8JsonWriter writer, AudioFormat value, JsonSerializerOptions options)
    {
        var stringValue = value switch
        {
            AudioFormat.PCM16 => RealtimeConstants.Audio.FormatPcm16,
            AudioFormat.G711_ULAW => RealtimeConstants.Audio.FormatG711Ulaw,
            AudioFormat.G711_ALAW => RealtimeConstants.Audio.FormatG711Alaw,
            _ => "unimplemented"
        };
        writer.WriteStringValue(stringValue);
    }
}

/// <summary>
/// Converts between JSON strings and ContentType enum values for the OpenAI Realtime API.
/// </summary>
internal class ContentTypeJsonConverter : JsonConverter<ContentType>
{
    public override ContentType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();
        return value switch
        {
            "input_text" => ContentType.InputText,
            "input_audio" => ContentType.InputAudio,
            "text" => ContentType.Text,
            "audio" => ContentType.Audio,
            _ => ContentType.Unimplemented
        };
    }

    public override void Write(Utf8JsonWriter writer, ContentType value, JsonSerializerOptions options)
    {
        var stringValue = value switch
        {
            ContentType.InputText => "input_text",
            ContentType.InputAudio => "input_audio",
            ContentType.Text => "text",
            ContentType.Audio => "audio",
            _ => "unimplemented"
        };
        writer.WriteStringValue(stringValue);
    }
}

/// <summary>
/// Converts between JSON strings and ItemType enum values for the OpenAI Realtime API.
/// </summary>
internal class ItemTypeJsonConverter : JsonConverter<ItemType>
{
    public override ItemType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();
        return value switch
        {
            "message" => ItemType.Message,
            "function_call" => ItemType.FunctionCall,
            "function_call_output" => ItemType.FunctionCallOutput,
            _ => ItemType.Unimplemented
        };
    }

    public override void Write(Utf8JsonWriter writer, ItemType value, JsonSerializerOptions options)
    {
        var stringValue = value switch
        {
            ItemType.Message => "message",
            ItemType.FunctionCall => "function_call",
            ItemType.FunctionCallOutput => "function_call_output",
            _ => "unimplemented"
        };
        writer.WriteStringValue(stringValue);
    }
}

/// <summary>
/// Converts between JSON strings and Role enum values for the OpenAI Realtime API.
/// </summary>
internal class RoleJsonConverter : JsonConverter<Role>
{
    public override Role Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();
        return value switch
        {
            "user" => Role.User,
            "assistant" => Role.Assistant,
            "system" => Role.System,
            _ => Role.Unimplemented
        };
    }

    public override void Write(Utf8JsonWriter writer, Role value, JsonSerializerOptions options)
    {
        var stringValue = value switch
        {
            Role.User => "user",
            Role.Assistant => "assistant",
            Role.System => "system",
            _ => "unimplemented"
        };
        writer.WriteStringValue(stringValue);
    }
}

/// <summary>
/// Represents the status of items in the OpenAI Realtime API responses.
/// </summary>
[JsonConverter(typeof(StatusJsonConverter))]
public enum Status
{
    /// <summary>
    /// Represents an unimplemented or unknown status.
    /// </summary>
    Unimplemented,

    /// <summary>
    /// Indicates that the item has been completed successfully.
    /// </summary>
    Completed,

    /// <summary>
    /// Indicates that the item is currently in progress.
    /// </summary>
    InProgress,

    /// <summary>
    /// Indicates that the item is incomplete, which can occur when a response is interrupted.
    /// </summary>
    Incomplete,

    /// <summary>
    /// Indicates that the item was cancelled by the client.
    /// </summary>
    Cancelled,

    /// <summary>
    /// Indicates that the item failed to complete due to an error.
    /// </summary>
    Failed
}

/// <summary>
/// Represents the audio format options supported by the OpenAI Realtime API.
/// </summary>
[JsonConverter(typeof(AudioFormatJsonConverter))]
public enum AudioFormat
{
    /// <summary>
    /// Represents an unimplemented or unknown audio format.
    /// </summary>
    Unimplemented,

    /// <summary>
    /// 16-bit PCM audio format.
    /// </summary>
    PCM16,

    /// <summary>
    /// G.711 µ-law audio format.
    /// </summary>
    G711_ULAW,

    /// <summary>
    /// G.711 A-law audio format.
    /// </summary>
    G711_ALAW
}

/// <summary>
/// Represents the content types supported in the OpenAI Realtime API messages.
/// </summary>
[JsonConverter(typeof(ContentTypeJsonConverter))]
public enum ContentType
{
    /// <summary>
    /// Represents an unimplemented or unknown content type.
    /// </summary>
    Unimplemented,

    /// <summary>
    /// Represents input text content from the user.
    /// </summary>
    InputText,

    /// <summary>
    /// Represents input audio content from the user.
    /// </summary>
    InputAudio,

    /// <summary>
    /// Represents text content in the response.
    /// </summary>
    Text,

    /// <summary>
    /// Represents audio content in the response.
    /// </summary>
    Audio
}

/// <summary>
/// Represents the types of items that can appear in the OpenAI Realtime API conversation.
/// </summary>
[JsonConverter(typeof(ItemTypeJsonConverter))]
public enum ItemType
{
    /// <summary>
    /// Represents an unimplemented or unknown item type.
    /// </summary>
    Unimplemented,

    /// <summary>
    /// Represents a message item containing text or audio content.
    /// </summary>
    Message,

    /// <summary>
    /// Represents a function call item made by the assistant.
    /// </summary>
    FunctionCall,

    /// <summary>
    /// Represents the output of a function call.
    /// </summary>
    FunctionCallOutput
}

/// <summary>
/// Represents the possible roles in the OpenAI Realtime API conversation.
/// </summary>
[JsonConverter(typeof(RoleJsonConverter))]
public enum Role
{
    /// <summary>
    /// Represents an unimplemented or unknown role.
    /// </summary>
    Unimplemented,

    /// <summary>
    /// Represents the user role in the conversation.
    /// </summary>
    User,

    /// <summary>
    /// Represents the assistant role in the conversation.
    /// </summary>
    Assistant,

    /// <summary>
    /// Represents the system role in the conversation.
    /// </summary>
    System
}