using System.Text.Json.Serialization;

namespace Betalgo.Ranul.OpenAI.Contracts.Types.TextFormat;

/// <summary>
///     A JSON schema format configuration for structured outputs.
///     <see href="https://platform.openai.com/docs/guides/structured-outputs">Structured Outputs Guide</see>.
///     <see href="https://github.com/betalgo/openai/blob/master/Docs/openapi-split/components/schemas/textresponseformatjsonschema.yml">
///         Source Definition
///     </see>
/// </summary>
public class JsonSchemaFormat : TextFormat
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="JsonSchemaFormat" /> class.
    /// </summary>
    public JsonSchemaFormat() : base("json_schema")
    {
    }

    /// <summary>
    ///     Initializes a new instance with schema parameters.
    /// </summary>
    /// <param name="name">The name of the schema.</param>
    /// <param name="schema">The JSON schema definition.</param>
    /// <param name="strict">Whether to enforce strict schema validation.</param>
    public JsonSchemaFormat(string name, Dictionary<string, object> schema, bool? strict = null) : base("json_schema")
    {
        JsonSchema = new JsonSchemaDefinition
        {
            Name = name,
            Schema = schema,
            Strict = strict
        };
    }

    /// <summary>
    ///     The JSON schema definition.
    /// </summary>
    [JsonIgnore]
    public JsonSchemaDefinition? JsonSchema { get; set; }

    /// <summary>The response format name, serialized directly inside text.format.</summary>
    [JsonPropertyName("name")]
    public string? Name
    {
        get => JsonSchema?.Name;
        set => (JsonSchema ??= new()).Name = value!;
    }

    /// <summary>The JSON schema, serialized directly inside text.format.</summary>
    [JsonPropertyName("schema")]
    public Dictionary<string, object>? Schema
    {
        get => JsonSchema?.Schema;
        set => (JsonSchema ??= new()).Schema = value!;
    }

    /// <summary>An optional description of the response format.</summary>
    [JsonPropertyName("description")]
    public string? Description
    {
        get => JsonSchema?.Description;
        set => (JsonSchema ??= new()).Description = value;
    }

    /// <summary>Whether to enforce strict schema adherence.</summary>
    [JsonPropertyName("strict")]
    public bool? Strict
    {
        get => JsonSchema?.Strict;
        set => (JsonSchema ??= new()).Strict = value;
    }
}

