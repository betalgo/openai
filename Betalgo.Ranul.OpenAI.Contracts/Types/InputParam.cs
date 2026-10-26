using System.Text.Json;
using System.Text.Json.Serialization;

namespace Betalgo.Ranul.OpenAI.Contracts.Types;

/// <summary>
///     Input parameter that can be either a text string or a list of input items.
///     <see href="https://platform.openai.com/docs/api-reference/responses/create">OpenAI API documentation</see>.
///     <see href="https://github.com/betalgo/openai/blob/master/Docs/openapi-split/components/schemas/inputparam.yml">
///         Source Definition
///     </see>
/// </summary>
[JsonConverter(typeof(InputParamConverter))]
public class InputParam
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="InputParam" /> class with a string value.
    /// </summary>
    /// <param name="value">A text input to the model, equivalent to a text input with the user role.</param>
    public InputParam(string value)
    {
        AsString = value;
        AsList = null;
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="InputParam" /> class with a list of input items.
    /// </summary>
    /// <param name="value">A list of one or many input items to the model, containing different content types.</param>
    public InputParam(List<IInputItem> value)
    {
        AsString = null;
        AsList = value;
    }

    /// <summary>
    ///     The string value when the input is a simple text string.
    /// </summary>
    public string? AsString { get; }

    /// <summary>
    ///     The list of input items when the input is a structured list.
    /// </summary>
    public List<IInputItem>? AsList { get; }

    /// <summary>
    ///     Implicitly converts a string to an <see cref="InputParam" />.
    /// </summary>
    public static implicit operator InputParam(string value) => new(value);

    /// <summary>
    ///     Implicitly converts a list of input items to an <see cref="InputParam" />.
    /// </summary>
    public static implicit operator InputParam(List<IInputItem> value) => new(value);
}

/// <summary>
///     JSON converter for <see cref="InputParam" /> that handles both string and array formats.
/// </summary>
public class InputParamConverter : JsonConverter<InputParam>
{
    /// <inheritdoc />
    public override InputParam? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.TokenType switch
        {
            JsonTokenType.String => new(reader.GetString()!),
            JsonTokenType.StartArray => new(JsonSerializer.Deserialize<List<IInputItem>>(ref reader, options)!),
            _ => throw new JsonException("Expected string or array for InputParam")
        };
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, InputParam value, JsonSerializerOptions options)
    {
        if (value.AsString != null)
        {
            writer.WriteStringValue(value.AsString);
        }
        else if (value.AsList != null)
        {
            JsonSerializer.Serialize(writer, value.AsList, options);
        }
        else
        {
            writer.WriteNullValue();
        }
    }
}

