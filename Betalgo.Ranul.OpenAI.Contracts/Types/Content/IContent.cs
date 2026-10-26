using System.Text.Json;
using System.Text.Json.Serialization;

namespace Betalgo.Ranul.OpenAI.Contracts.Types.Content;

/// <summary>
///     Base interface for all content types in the Responses API.
///     <see href="https://platform.openai.com/docs/api-reference/responses">OpenAI API documentation</see>.
///     <see href="https://github.com/betalgo/openai/blob/master/Docs/openapi-split/components/schemas/inputcontent.yml">
///         Source Definition
///     </see>
/// </summary>
[JsonConverter(typeof(ContentConverter))]
public interface IContent
{
    /// <summary>
    ///     The type discriminator for the content.
    /// </summary>
    [JsonPropertyName("type")]
    string Type { get; }
}

/// <summary>
///     JSON converter for <see cref="IContent" /> that handles polymorphic deserialization based on the type discriminator.
/// </summary>
public class ContentConverter : JsonConverter<IContent>
{
    /// <inheritdoc />
    public override IContent? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
            throw new JsonException("Expected StartObject token for Content");        using var jsonDoc = JsonDocument.ParseValue(ref reader);
        if (!jsonDoc.RootElement.TryGetProperty("type", out var typeProp))
            throw new JsonException("Missing 'type' property in Content");

        var type = typeProp.GetString();
        var rawText = jsonDoc.RootElement.GetRawText();

        return type switch
        {
            "input_text" => JsonSerializer.Deserialize<InputTextContent>(rawText, options),
            "input_image" => JsonSerializer.Deserialize<InputImageContent>(rawText, options),
            "input_file" => JsonSerializer.Deserialize<InputFileContent>(rawText, options),
            "input_audio" => JsonSerializer.Deserialize<InputAudioContent>(rawText, options),
            "output_text" => JsonSerializer.Deserialize<OutputTextContent>(rawText, options),
            "refusal" => JsonSerializer.Deserialize<RefusalContent>(rawText, options),
            _ => throw new JsonException($"Unknown Content type: {type}")
        };
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, IContent value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value, value.GetType(), options);
    }
}
