using System.Text.Json;
using System.Text.Json.Serialization;

namespace Betalgo.Ranul.OpenAI.ObjectModels.RealtimeModels;

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

[JsonConverter(typeof(StatusJsonConverter))]
public enum Status
{
    Unimplemented,
    Completed,
    InProgress,
    Incomplete,
    Cancelled,
    Failed
}

[JsonConverter(typeof(AudioFormatJsonConverter))]
public enum AudioFormat
{
    Unimplemented,
    PCM16,

    // ReSharper disable once InconsistentNaming
    G711_ULAW,

    // ReSharper disable once InconsistentNaming
    G711_ALAW
}

[JsonConverter(typeof(ContentTypeJsonConverter))]
public enum ContentType
{
    Unimplemented,
    InputText,
    InputAudio,
    Text,
    Audio
}

[JsonConverter(typeof(ItemTypeJsonConverter))]
public enum ItemType
{
    Unimplemented,
    Message,
    FunctionCall,
    FunctionCallOutput
}

[JsonConverter(typeof(RoleJsonConverter))]
public enum Role
{
    Unimplemented,
    User,
    Assistant,
    System
}