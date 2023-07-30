using System.Text.Json.Serialization;

namespace OpenAI.ObjectModels.SharedModels;

/// <summary>
///     Function parameter is a JSON Schema object.
///     https://json-schema.org/understanding-json-schema/reference/object.html
/// </summary>
public class PropertyDefinition
{
    public enum FunctionObjectTypes
    {
        String,
        Integer,
        Number,
        Object,
        Array,
        Boolean,
        Null
    }

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
    public IDictionary<string, PropertyDefinition>? Properties { get; set; }

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
    [JsonPropertyName("maxProperties")]
    public int? MaxProperties { get; set; }

    /// <summary>
    ///     If type is "array", this specifies the element type for all items in the array.
    ///     If type is not "array", this should be null.
    ///     For more details, see https://json-schema.org/understanding-json-schema/reference/array.html
    /// </summary>
    [JsonPropertyName("items")]
    public PropertyDefinition? Items { get; set; }

    public static PropertyDefinition DefineArray(PropertyDefinition? arrayItems = null)
    {
        return new PropertyDefinition
        {
            Items = arrayItems,
            Type = ConvertTypeToString(FunctionObjectTypes.Array)
        };
    }

    public static PropertyDefinition DefineEnum(List<string> enumList, string? description = null)
    {
        return new PropertyDefinition
        {
            Description = description,
            Enum = enumList,
            Type = ConvertTypeToString(FunctionObjectTypes.String)
        };
    }

    public static PropertyDefinition DefineInteger(string? description = null)
    {
        return new PropertyDefinition
        {
            Description = description,
            Type = ConvertTypeToString(FunctionObjectTypes.Integer)
        };
    }

    public static PropertyDefinition DefineNumber(string? description = null)
    {
        return new PropertyDefinition
        {
            Description = description,
            Type = ConvertTypeToString(FunctionObjectTypes.Number)
        };
    }

    public static PropertyDefinition DefineString(string? description = null)
    {
        return new PropertyDefinition
        {
            Description = description,
            Type = ConvertTypeToString(FunctionObjectTypes.String)
        };
    }

    public static PropertyDefinition DefineBoolean(string? description = null)
    {
        return new PropertyDefinition
        {
            Description = description,
            Type = ConvertTypeToString(FunctionObjectTypes.Boolean)
        };
    }

    public static PropertyDefinition DefineNull(string? description = null)
    {
        return new PropertyDefinition
        {
            Description = description,
            Type = ConvertTypeToString(FunctionObjectTypes.Null)
        };
    }

    public static PropertyDefinition DefineObject(IDictionary<string, PropertyDefinition>? properties, IList<string>? required, bool? additionalProperties, string? description, IList<string>? @enum)
    {
        return new PropertyDefinition
        {
            Properties = properties,
            Required = required,
            AdditionalProperties = additionalProperties,
            Description = description,
            Enum = @enum,
            Type = ConvertTypeToString(FunctionObjectTypes.Object)
        };
    }

    /// <summary>
    ///     Converts a FunctionObjectTypes enumeration value to its corresponding string representation.
    /// </summary>
    /// <param name="type">The type to convert</param>
    /// <returns>The string representation of the given type</returns>
    public static string ConvertTypeToString(FunctionObjectTypes type)
    {
        return type switch
        {
            FunctionObjectTypes.String => "string",
            FunctionObjectTypes.Integer => "integer",
            FunctionObjectTypes.Number => "number",
            FunctionObjectTypes.Object => "object",
            FunctionObjectTypes.Array => "array",
            FunctionObjectTypes.Boolean => "boolean",
            FunctionObjectTypes.Null => "null",
            _ => throw new ArgumentOutOfRangeException(nameof(type), $"Unknown type: {type}")
        };
    }
}