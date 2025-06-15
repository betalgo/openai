using System.Text.Json.Serialization;

namespace Betalgo.Ranul.OpenAI.ObjectModels.SharedModels;

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
    public string? Type { get; set; }
    
    /// <summary>
    ///     An instance validates successfully against this keyword if its value is equal to the value of the keyword
    ///     https://json-schema.org/draft/2020-12/json-schema-validation#name-const
    /// </summary>
    [JsonPropertyName("const")]
    public string? Constant { get; set; }

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
    ///     Optional. Title of the schema.
    ///     https://json-schema.org/draft/2020-12/json-schema-validation#name-title-and-description
    /// </summary>
    [JsonPropertyName("title")]
    public string? Title { get; set; }
    
    /// <summary>
    ///     Optional. Description of the schema.
    ///     https://json-schema.org/draft/2020-12/json-schema-validation#name-title-and-description
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
    ///     The value of "minimum" MUST be a number, representing an inclusive lower limit for a numeric instance.
    ///     https://json-schema.org/draft/2020-12/json-schema-validation#name-minimum
    /// </summary>
    [JsonPropertyName("minimum")]
    public float? Minimum { get; set; }
    
    /// <summary>
    ///     The value of "maximum" MUST be a number, representing an inclusive upper limit for a numeric instance.
    ///     https://json-schema.org/draft/2020-12/json-schema-validation#name-maximum
    /// </summary>
    [JsonPropertyName("maximum")]
    public float? Maximum { get; set; }

    /// <summary>
    ///     If type is "array", this specifies the element type for all items in the array.
    ///     If type is not "array", this should be null.
    ///     For more details, see https://json-schema.org/understanding-json-schema/reference/array.html
    /// </summary>
    [JsonPropertyName("items")]
    public PropertyDefinition? Items { get; set; }
    
    /// <summary>
    ///     Definitions of schemas (2020-12 and newer specifications).
    ///     For more details, see https://json-schema.org/understanding-json-schema/structuring#defs
    /// </summary>
    [JsonPropertyName("$defs")]
    public IDictionary<string, PropertyDefinition>? Defs { get; set; }
    
    /// <summary>
    ///     Definitions of schemas (draft-07 specification).
    /// </summary>
    [JsonPropertyName("definitions")]
    public IDictionary<string, PropertyDefinition>? Definitions { get; set; }
    
    /// <summary>
    ///     Reference to another schema definition.
    ///     For more details, see https://json-schema.org/understanding-json-schema/structuring#dollarref
    /// </summary>
    [JsonPropertyName("$ref")]
    public string? Ref { get; set; }
    
    /// <summary>
    ///     To validate against anyOf, the given data must be valid against any (one or more) of the given subschemas.
    ///     For more details, see https://json-schema.org/understanding-json-schema/reference/combining#anyOf
    /// </summary>
    [JsonPropertyName("anyOf")]
    public IList<PropertyDefinition>? AnyOf { get; set; }
    
    /// <summary>
    ///     To validate against oneOf, the given data must be valid against exactly one of the given subschemas.
    ///     For more details, see https://json-schema.org/understanding-json-schema/reference/combining#oneOf
    /// </summary>
    [JsonPropertyName("oneOf")]
    public IList<PropertyDefinition>? OneOf { get; set; }

    public static PropertyDefinition DefineArray(PropertyDefinition? arrayItems = null)
    {
        return new()
        {
            Items = arrayItems,
            Type = ConvertTypeToString(FunctionObjectTypes.Array)
        };
    }

    public static PropertyDefinition DefineEnum(List<string> enumList, string? description = null)
    {
        return new()
        {
            Description = description,
            Enum = enumList,
            Type = ConvertTypeToString(FunctionObjectTypes.String)
        };
    }

    public static PropertyDefinition DefineInteger(string? description = null)
    {
        return new()
        {
            Description = description,
            Type = ConvertTypeToString(FunctionObjectTypes.Integer)
        };
    }

    public static PropertyDefinition DefineNumber(string? description = null)
    {
        return new()
        {
            Description = description,
            Type = ConvertTypeToString(FunctionObjectTypes.Number)
        };
    }

    public static PropertyDefinition DefineString(string? description = null)
    {
        return new()
        {
            Description = description,
            Type = ConvertTypeToString(FunctionObjectTypes.String)
        };
    }

    public static PropertyDefinition DefineBoolean(string? description = null)
    {
        return new()
        {
            Description = description,
            Type = ConvertTypeToString(FunctionObjectTypes.Boolean)
        };
    }

    public static PropertyDefinition DefineNull(string? description = null)
    {
        return new()
        {
            Description = description,
            Type = ConvertTypeToString(FunctionObjectTypes.Null)
        };
    }

    public static PropertyDefinition DefineObject(IDictionary<string, PropertyDefinition>? properties, IList<string>? required, bool? additionalProperties, string? description, IList<string>? @enum)
    {
        return new()
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
