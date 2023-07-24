using System.Text.Json.Serialization;

namespace OpenAI.ObjectModels.SharedModels;

/// <summary>
///     Function parameter is a JSON Schema object.
///     https://json-schema.org/understanding-json-schema/reference/object.html
/// </summary>
public class FunctionParameters
{
    /// <summary>
    ///     Required. Function parameter object type. Default value is "object".
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = "object";

    /// <summary>
    ///     Optional. List of "function arguments", as a dictionary that maps from argument name
    ///     to an object that describes the type, maybe possible enum values, and so on.
    /// </summary>
    [JsonPropertyName("properties")]
    public IDictionary<string, FunctionParameterPropertyValue>? Properties { get; set; }

    /// <summary>
    ///     Optional. List of "function arguments" which are required.
    /// </summary>
    [JsonPropertyName("required")]
    public IList<string>? Required { get; set; }

    /// <summary>
    ///     Optional. Whether additional properties are allowed. Default value is true.
    /// </summary>
    [JsonPropertyName("additionalProperties")]
    public bool? AdditionalProperties { get; set; }

    /// <summary>
    ///     Each property value is a JSON Schema object with its own keys and values.
    ///     The documentation (https://platform.openai.com/docs/guides/gpt/function-calling)
    ///     suggests that only a few specific keys are used: type, description, and sometimes enum.
    /// </summary>
    public class FunctionParameterPropertyValue
    {
     

        /// <summary>
        ///     Argument type (e.g. string, integer, and so on).
        ///     For examples, see https://json-schema.org/understanding-json-schema/reference/object.html
        /// </summary>
        [JsonPropertyName("type")]
        public string Type { get; set; } = "string";

        /// <summary>
        ///     Optional. Argument description.
        /// </summary>
        [JsonPropertyName("description")]
        public string? Description { get; set; }

        /// <summary>
        ///     Optional. List of allowed values for this argument.
        /// </summary>
        [JsonPropertyName("enum")]
        public IList<string>? Enum { get; set; }

        /// <summary>
        ///     The number of properties on an object can be restricted using the minProperties and maxProperties keywords. Each of
        ///     these must be a non-negative integer.
        /// </summary>
        [JsonPropertyName("minProperties")]
        public int? MinProperties { get; set; }

        /// <summary>
        ///     The number of properties on an object can be restricted using the minProperties and maxProperties keywords. Each of
        ///     these must be a non-negative integer.
        /// </summary>
        [JsonPropertyName("maxProperties")] public int? MaxProperties { get; set; }
    }

    public enum FunctionObjectTypes
    {
        String,
        Number,
        Object,
        Array,
        Boolean,
        Null
    }
}