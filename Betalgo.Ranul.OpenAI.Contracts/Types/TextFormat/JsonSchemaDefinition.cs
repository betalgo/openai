using System.Text.Json.Serialization;

namespace Betalgo.Ranul.OpenAI.Contracts.Types.TextFormat;

/// <summary>
///     A JSON schema definition for structured outputs.
///     <see href="https://platform.openai.com/docs/guides/structured-outputs">Structured Outputs Guide</see>.
///     <see href="https://github.com/betalgo/openai/blob/master/Docs/openapi-split/components/schemas/responseformatjsonschemaschema.yml">
///         Source Definition
///     </see>
/// </summary>
public class JsonSchemaDefinition
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="JsonSchemaDefinition" /> class.
    /// </summary>
    public JsonSchemaDefinition()
    {
    }

    /// <summary>
    ///     Initializes a new instance with required parameters.
    /// </summary>
    /// <param name="name">The name of the schema.</param>
    /// <param name="schema">The JSON schema object.</param>
    /// <param name="strict">Whether to enforce strict schema validation.</param>
    public JsonSchemaDefinition(string name, Dictionary<string, object> schema, bool? strict = null)
    {
        Name = name;
        Schema = schema;
        Strict = strict;
    }

    /// <summary>
    ///     The name of the schema.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;

    /// <summary>
    ///     A description of the schema.
    /// </summary>
    [JsonPropertyName("description")]
    public string? Description { get; set; }

    /// <summary>
    ///     The JSON schema object.
    /// </summary>
    [JsonPropertyName("schema")]
    public Dictionary<string, object> Schema { get; set; } = new();

    /// <summary>
    ///     Whether to enforce strict schema validation.
    /// </summary>
    [JsonPropertyName("strict")]
    public bool? Strict { get; set; }
}

