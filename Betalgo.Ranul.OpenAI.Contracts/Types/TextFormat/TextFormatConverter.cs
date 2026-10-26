using System.Text.Json;
using System.Text.Json.Serialization;

namespace Betalgo.Ranul.OpenAI.Contracts.Types.TextFormat;

/// <summary>
///     JSON converter for <see cref="TextFormat" /> that handles different format types.
/// </summary>
public class TextFormatConverter : JsonConverter<TextFormat>
{
    /// <inheritdoc />
    public override TextFormat? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
            throw new JsonException("Expected StartObject token for TextFormat");

        using var jsonDoc = JsonDocument.ParseValue(ref reader);
        if (!jsonDoc.RootElement.TryGetProperty("type", out var typeProp))
            throw new JsonException("Missing 'type' property in TextFormat");

        var type = typeProp.GetString();
        var rawText = jsonDoc.RootElement.GetRawText();

        return type switch
        {
            "json_schema" => JsonSerializer.Deserialize<JsonSchemaFormat>(rawText, options),
            _ => new TextFormat(type!)
        };
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, TextFormat value, JsonSerializerOptions options)
    {
        if (value is JsonSchemaFormat schema)
        {
            JsonSerializer.Serialize(writer, schema, options);
            return;
        }
        writer.WriteStartObject();
        writer.WriteString("type", value.Type);
        writer.WriteEndObject();
    }
}

