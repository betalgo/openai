using System.Text.Json;
using System.Text.Json.Serialization;

namespace Betalgo.Ranul.OpenAI.Contracts.Types.TextFormat;

/// <summary>
///     Configuration for the format of text output from the model.
///     <see href="https://platform.openai.com/docs/guides/structured-outputs">Structured Outputs Guide</see>.
///     <see href="https://github.com/betalgo/openai/blob/master/Docs/openapi-split/components/schemas/textresponseformatconfiguration.yml">
///         Source Definition
///     </see>
/// </summary>
[JsonConverter(typeof(TextFormatConverter))]
public class TextFormat
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="TextFormat" /> class.
    /// </summary>
    public TextFormat()
    {
    }

    /// <summary>
    ///     Initializes a new instance with a format type.
    /// </summary>
    /// <param name="type">The format type.</param>
    public TextFormat(string type)
    {
        Type = type;
    }

    /// <summary>
    ///     The type of text format. One of <c>text</c>, <c>json_object</c>, or <c>json_schema</c>.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = null!;

    /// <summary>
    ///     Creates a plain text format configuration.
    /// </summary>
    public static TextFormat Text => new("text");

    /// <summary>
    ///     Creates a JSON object format configuration (legacy, prefer json_schema).
    /// </summary>
    public static TextFormat JsonObject => new("json_object");

    /// <summary>
    ///     Creates a JSON schema format configuration for structured outputs.
    /// </summary>
    /// <param name="name">The name of the schema.</param>
    /// <param name="schema">The JSON schema definition.</param>
    /// <param name="strict">Whether to enforce strict schema validation.</param>
    public static JsonSchemaFormat JsonSchema(string name, Dictionary<string, object> schema, bool? strict = null)
        => new(name, schema, strict);
}

