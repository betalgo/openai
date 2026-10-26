using System.Text.Json;
using System.Text.Json.Serialization;
using Betalgo.Ranul.OpenAI.Contracts.Enums.Responses;

namespace Betalgo.Ranul.OpenAI.Contracts.Types.ToolChoice;

/// <summary>
///     JSON converter for <see cref="ToolChoice" /> that handles both string and object formats.
/// </summary>
public class ToolChoiceConverter : JsonConverter<ToolChoice>
{
    /// <inheritdoc />
    public override ToolChoice? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.TokenType switch
        {
            JsonTokenType.String => new ToolChoice(new ToolChoiceOption(reader.GetString()!)),
            JsonTokenType.StartObject => new ToolChoice(JsonSerializer.Deserialize<SpecificToolChoice>(ref reader, options)!),
            _ => throw new JsonException("Expected string or object for ToolChoice")
        };
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, ToolChoice value, JsonSerializerOptions options)
    {
        if (value.Option.HasValue)
        {
            writer.WriteStringValue(value.Option.Value.Value);
        }
        else if (value.SpecificTool != null)
        {
            JsonSerializer.Serialize(writer, value.SpecificTool, options);
        }
        else
        {
            writer.WriteNullValue();
        }
    }
}

