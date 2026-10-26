using System.Text.Json;
using System.Text.Json.Serialization;

namespace Betalgo.Ranul.OpenAI.Contracts.Types.Tools;

/// <summary>
///     Represents function parameters that can be either a typed PropertyDefinition or raw JSON string.
///     Supports zero-cost pass-through for raw JSON - no unnecessary parsing/serialization.
///     <para>
///         <b>Usage Options:</b>
///         <list type="bullet">
///             <item>Raw JSON string: <c>Parameters = """{"type":"object",...}"""</c></item>
///             <item>Dictionary: <c>Parameters = new Dictionary&lt;string, object&gt; { ... }</c></item>
///         </list>
///     </para>
/// </summary>
/// <example>
///     <code>
/// // Option 1: Raw JSON string (zero-cost pass-through)
/// var tool = new FunctionTool
/// {
///     Name = "get_weather",
///     Parameters = """{"type":"object","properties":{"location":{"type":"string"}}}"""
/// };
/// 
/// // Option 2: Dictionary
/// var tool = new FunctionTool
/// {
///     Name = "get_weather",
///     Parameters = new Dictionary&lt;string, object&gt;
///     {
///         ["type"] = "object",
///         ["properties"] = new Dictionary&lt;string, object&gt; { ... }
///     }
/// };
///     </code>
/// </example>
[JsonConverter(typeof(FunctionParametersConverter))]
public class FunctionParameters
{
    private readonly string? _rawJson;
    private readonly Dictionary<string, object>? _dictionary;

    /// <summary>
    ///     Creates FunctionParameters from a raw JSON string.
    ///     The JSON is passed through without parsing - zero performance cost.
    /// </summary>
    /// <param name="rawJson">The raw JSON schema string.</param>
    private FunctionParameters(string rawJson)
    {
        _rawJson = rawJson;
    }

    /// <summary>
    ///     Creates FunctionParameters from a dictionary.
    /// </summary>
    /// <param name="dictionary">The parameters dictionary.</param>
    private FunctionParameters(Dictionary<string, object>? dictionary)
    {
        _dictionary = dictionary;
    }

    /// <summary>
    ///     Gets whether this instance contains raw JSON.
    /// </summary>
    public bool IsRawJson => _rawJson != null;

    /// <summary>
    ///     Gets the raw JSON string if available.
    /// </summary>
    public string? RawJson => _rawJson;

    /// <summary>
    ///     Gets the dictionary if available.
    /// </summary>
    public Dictionary<string, object>? Dictionary => _dictionary;

    /// <summary>
    ///     Creates FunctionParameters from a raw JSON string.
    ///     The JSON is passed through without parsing - zero performance cost.
    /// </summary>
    /// <param name="rawJson">The raw JSON schema string.</param>
    /// <returns>A new FunctionParameters instance.</returns>
    public static FunctionParameters FromJson(string rawJson) => new(rawJson);

    /// <summary>
    ///     Creates FunctionParameters from a dictionary.
    /// </summary>
    /// <param name="dictionary">The parameters dictionary.</param>
    /// <returns>A new FunctionParameters instance.</returns>
    public static FunctionParameters FromDictionary(Dictionary<string, object>? dictionary) => new(dictionary);

    /// <summary>
    ///     Implicit conversion from raw JSON string.
    /// </summary>
    public static implicit operator FunctionParameters(string rawJson) => FromJson(rawJson);

    /// <summary>
    ///     Implicit conversion from dictionary.
    /// </summary>
    public static implicit operator FunctionParameters(Dictionary<string, object>? dictionary) => FromDictionary(dictionary);
}

/// <summary>
///     JSON converter for <see cref="FunctionParameters" /> that handles both raw JSON and dictionary serialization.
///     Uses WriteRawValue for zero-cost raw JSON pass-through.
/// </summary>
public class FunctionParametersConverter : JsonConverter<FunctionParameters>
{
    /// <inheritdoc />
    public override FunctionParameters? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
        {
            return null;
        }

        // Read as raw JSON string for maximum compatibility
        using var doc = JsonDocument.ParseValue(ref reader);
        return FunctionParameters.FromJson(doc.RootElement.GetRawText());
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, FunctionParameters value, JsonSerializerOptions options)
    {
        if (value.IsRawJson && value.RawJson != null)
        {
            // Zero-cost pass-through - write raw JSON directly without parsing!
            writer.WriteRawValue(value.RawJson);
        }
        else if (value.Dictionary != null)
        {
            JsonSerializer.Serialize(writer, value.Dictionary, options);
        }
        else
        {
            writer.WriteNullValue();
        }
    }
}

