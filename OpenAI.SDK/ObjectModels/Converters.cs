using System.Text.Json;
using System.Text.Json.Serialization;
using OpenAI.ObjectModels.RequestModels;

namespace OpenAI.ObjectModels;

public class MessageContentConverter : JsonConverter<MessageContentOneOfType>
{
    public override MessageContentOneOfType? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.TokenType switch
        {
            JsonTokenType.String => new() { AsString = reader.GetString() },
            JsonTokenType.StartArray => new() { AsList = JsonSerializer.Deserialize<List<MessageContent>>(ref reader, options) },
            _ => throw new JsonException()
        };
    }

    public override void Write(Utf8JsonWriter writer, MessageContentOneOfType? value, JsonSerializerOptions options)
    {
        if (value?.AsString != null)
        {
            writer.WriteStringValue(value.AsString);
        }
        else if (value?.AsList != null)
        {
            JsonSerializer.Serialize(writer, value.AsList, options);
        }
        else
        {
            writer.WriteNullValue();
        }
    }
}

public class ResponseFormatOptionConverter : JsonConverter<ResponseFormatOneOfType>
{
    public override ResponseFormatOneOfType? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.TokenType switch
        {
            JsonTokenType.String => new() { AsString = reader.GetString() },
            JsonTokenType.StartObject => new() { AsObject = JsonSerializer.Deserialize<ResponseFormat>(ref reader, options) },
            _ => throw new JsonException()
        };
    }

    public override void Write(Utf8JsonWriter writer, ResponseFormatOneOfType? value, JsonSerializerOptions options)
    {
        if (value?.AsString != null)
        {
            writer.WriteStringValue(value.AsString);
        }
        else if (value?.AsObject != null)
        {
            JsonSerializer.Serialize(writer, value.AsObject, options);
        }
        else
        {
            writer.WriteNullValue();
        }
    }
}

public class AssistantsApiToolChoiceConverter : JsonConverter<AssistantsApiToolChoiceOneOfType>
{
    public override AssistantsApiToolChoiceOneOfType? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.TokenType switch
        {
            JsonTokenType.String => new() { AsString= reader.GetString() },
            JsonTokenType.StartObject => new() { AsObject= JsonSerializer.Deserialize<ToolChoice>(ref reader, options) },
            _ => throw new JsonException()
        };
    }

    public override void Write(Utf8JsonWriter writer, AssistantsApiToolChoiceOneOfType? value, JsonSerializerOptions options)
    {
        if (value?.AsString != null)
        {
            writer.WriteStringValue(value.AsString);
        }
        else if (value?.AsObject != null)
        {
            JsonSerializer.Serialize(writer, value.AsObject, options);
        }
        else
        {
            writer.WriteNullValue();
        }
    }
}