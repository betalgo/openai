using System.Text.Json;
using System.Text.Json.Serialization;
using Betalgo.Ranul.OpenAI.ObjectModels.RequestModels;

namespace Betalgo.Ranul.OpenAI.ObjectModels;

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
            JsonTokenType.String => new() { AsString = reader.GetString() },
            JsonTokenType.StartObject => new() { AsObject = JsonSerializer.Deserialize<ToolChoice>(ref reader, options) },
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

public class SingleOrArrayToListConverter : JsonConverter<IList<string?>>
{
    public override IList<string?>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
        switch (reader.TokenType) {
            case JsonTokenType.String:
                return [reader.GetString()];
            case JsonTokenType.StartArray:
            {
                var list = new List<string?>();
                while (reader.Read() && reader.TokenType != JsonTokenType.EndArray) {
                    switch (reader.TokenType) {
                        case JsonTokenType.String:
                            list.Add(reader.GetString());
                            break;
                        case JsonTokenType.Null:
                            list.Add(null);
                            break;
                        default:
                            throw new JsonException($"Unexpected token in type array: {reader.TokenType}");
                    }
                }
                return list;
            }
            default:
                throw new JsonException($"Unexpected token parsing type: {reader.TokenType}");
        }
    }

    public override void Write(Utf8JsonWriter writer, IList<string?>? value, JsonSerializerOptions options) {
        if (value == null || value.Count == 0) {
            writer.WriteNullValue();
        } else if (value.Count == 1) {
            writer.WriteStringValue(value[0]);
        } else {
            writer.WriteStartArray();
            foreach (var item in value) {
                if (item == null) {
                    writer.WriteNullValue();
                } else {
                    writer.WriteStringValue(item);
                }
            }
            writer.WriteEndArray();
        }
    }
}
