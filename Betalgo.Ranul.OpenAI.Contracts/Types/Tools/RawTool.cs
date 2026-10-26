using System.Text.Json;
using System.Text.Json.Serialization;

namespace Betalgo.Ranul.OpenAI.Contracts.Types.Tools;

/// <summary>
///     Represents any tool as raw JSON for zero-cost pass-through.
///     Use this when you want to pass a tool definition directly without any parsing or validation.
///     <para>
///         This is useful for:
///         <list type="bullet">
///             <item>Passing tool definitions from external sources</item>
///             <item>Using tool configurations from documentation/examples</item>
///             <item>Maximum flexibility without type constraints</item>
///         </list>
///     </para>
/// </summary>
/// <example>
///     <code>
/// // Any tool type as raw JSON
/// var tool = new RawTool("""
///     {
///         "type": "function",
///         "name": "get_weather",
///         "description": "Get weather",
///         "parameters": { ... }
///     }
///     """);
/// 
/// // Web search tool as raw JSON
/// var webSearch = new RawTool("""
///     {
///         "type": "web_search",
///         "search_context_size": "high"
///     }
///     """);
/// 
/// // Mix with typed tools
/// var tools = new List&lt;ITool&gt;
/// {
///     new FunctionTool { Name = "typed", ... },
///     new RawTool("""{ "type": "function", ... }"""),
///     new WebSearchTool()
/// };
///     </code>
/// </example>
[JsonConverter(typeof(RawToolConverter))]
public class RawTool : ITool
{
    private readonly string _rawJson;
    private readonly string _type;

    /// <summary>
    ///     Initializes a new instance of the <see cref="RawTool" /> class from a raw JSON string.
    /// </summary>
    /// <param name="rawJson">The raw JSON string containing the tool definition.</param>
    /// <exception cref="ArgumentException">Thrown when the JSON is invalid or missing the 'type' property.</exception>
    public RawTool(string rawJson)
    {
        if (string.IsNullOrWhiteSpace(rawJson))
        {
            throw new ArgumentException("Raw JSON cannot be null or empty", nameof(rawJson));
        }

        _rawJson = rawJson;

        // Extract the type for the ITool interface requirement
        try
        {
            using var doc = JsonDocument.Parse(rawJson);
            _type = doc.RootElement.TryGetProperty("type", out var typeProp)
                ? typeProp.GetString() ?? "function"
                : "function";
        }
        catch (JsonException ex)
        {
            throw new ArgumentException("Invalid JSON format", nameof(rawJson), ex);
        }
    }

    /// <summary>
    ///     Gets the type of the tool extracted from the raw JSON.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type => _type;

    /// <summary>
    ///     Gets the raw JSON string.
    /// </summary>
    [JsonIgnore]
    public string RawJson => _rawJson;

    /// <summary>
    ///     Implicit conversion from a raw JSON string to <see cref="RawTool" />.
    /// </summary>
    /// <param name="rawJson">The raw JSON string.</param>
    public static implicit operator RawTool(string rawJson) => new(rawJson);
}

/// <summary>
///     JSON converter for <see cref="RawTool" /> that uses <see cref="Utf8JsonWriter.WriteRawValue" />
///     for zero-cost pass-through serialization.
/// </summary>
public class RawToolConverter : JsonConverter<RawTool>
{
    /// <inheritdoc />
    public override RawTool? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
        {
            return null;
        }

        using var doc = JsonDocument.ParseValue(ref reader);
        return new RawTool(doc.RootElement.GetRawText());
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, RawTool value, JsonSerializerOptions options)
    {
        // Zero-cost pass-through - write raw JSON directly without any processing!
        writer.WriteRawValue(value.RawJson);
    }
}



