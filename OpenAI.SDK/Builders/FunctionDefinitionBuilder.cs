using OpenAI.ObjectModels.RequestModels;
using OpenAI.ObjectModels.SharedModels;

namespace OpenAI.Builders;

/// <summary>
///     FunctionDefinitionBuilder is used to build and validate a FunctionDefinition object.
/// </summary>
public class FunctionDefinitionBuilder
{
    /// <summary>
    ///     String constant for validation of function name.
    /// </summary>
    private const string ValidNameChars =
        "abcdefghijklmnopqrstuvwxyz" +
        "ABCDEFGHIJKLMNOPQRSTUVWXYZ" +
        "0123456789-_";

    private readonly FunctionDefinition _definition;

    /// <summary>
    ///     Initializes a new instance of FunctionDefinitionBuilder.
    /// </summary>
    /// <param name="name">The name of the function</param>
    /// <param name="description">The optional description of the function</param>
    public FunctionDefinitionBuilder(string name, string? description = null)
    {
        _definition = new FunctionDefinition
        {
            Name = name,
            Description = description
        };
    }

    /// <summary>
    ///     Adds a parameter to the function definition with a type expressed as an enumeration.
    /// </summary>
    /// <param name="name">The name of the parameter</param>
    /// <param name="type">The type of the parameter</param>
    /// <param name="description">The optional description of the parameter</param>
    /// <param name="enum">The optional list of possible string values for the parameter</param>
    /// <param name="required">Whether this parameter is required (default is true)</param>
    /// <returns>The FunctionDefinitionBuilder instance for fluent configuration</returns>
    public FunctionDefinitionBuilder AddParameter(string name, FunctionParameters.FunctionObjectTypes type, string? description = null, IList<string>? @enum = null, bool required = true)
    {
        var typeStr = ConvertTypeToString(type);
        return AddParameter(name, typeStr, description, @enum, required);
    }

    /// <summary>
    ///     Adds a parameter to the function definition with a type expressed as a string.
    /// </summary>
    /// <param name="name">The name of the parameter</param>
    /// <param name="type">The type of the parameter</param>
    /// <param name="description">The optional description of the parameter</param>
    /// <param name="enum">The optional list of possible string values for the parameter</param>
    /// <param name="required">Whether this parameter is required (default is true)</param>
    /// <returns>The FunctionDefinitionBuilder instance for fluent configuration</returns>
    public FunctionDefinitionBuilder AddParameter(string name, string type, string? description = null, IList<string>? @enum = null, bool required = true)
    {
        _definition.Parameters ??= new FunctionParameters();
        _definition.Parameters.Properties ??= new Dictionary<string, FunctionParameters.FunctionParameterPropertyValue>();

        _definition.Parameters.Properties[name] =
            new FunctionParameters.FunctionParameterPropertyValue {Type = type, Description = description, Enum = @enum};

        if (required)
        {
            _definition.Parameters.Required ??= new List<string>();
            _definition.Parameters.Required.Add(name);
        }

        return this;
    }

    /// <summary>
    ///     Validates the function definition.
    /// </summary>
    /// <returns>The FunctionDefinitionBuilder instance for fluent configuration</returns>
    public FunctionDefinitionBuilder Validate()
    {
        ValidateName(_definition.Name);
        return this;
    }

    /// <summary>
    ///     Builds the FunctionDefinition object.
    /// </summary>
    /// <returns>The built FunctionDefinition object</returns>
    public FunctionDefinition Build()
    {
        return _definition;
    }

    /// <summary>
    ///     Validates the name of the function.
    /// </summary>
    /// <param name="functionName">The name of the function to validate</param>
    public static void ValidateName(string functionName)
    {
        var invalidChars = functionName.Where(ch => !ValidNameChars.Contains(ch)).ToList();
        if (functionName.Length > 64 || invalidChars.Count > 0)
        {
            var message = "The name of the function must be a-z, A-Z, 0-9, or contain underscores and dashes, with a maximum length of 64.";
            if (functionName.Length > 64)
            {
                message = "Function name is too long. " + message;
            }

            if (invalidChars.Count > 0)
            {
                message = $"Function name contains invalid characters: {string.Join(",", invalidChars)}. " + message;
            }

            throw new ArgumentOutOfRangeException(nameof(functionName), message);
        }
    }

    /// <summary>
    ///     Converts a FunctionObjectTypes enumeration value to its corresponding string representation.
    /// </summary>
    /// <param name="type">The type to convert</param>
    /// <returns>The string representation of the given type</returns>
    private static string ConvertTypeToString(FunctionParameters.FunctionObjectTypes type)
    {
        return type switch
        {
            FunctionParameters.FunctionObjectTypes.String => "string",
            FunctionParameters.FunctionObjectTypes.Number => "number",
            FunctionParameters.FunctionObjectTypes.Object => "object",
            FunctionParameters.FunctionObjectTypes.Array => "array",
            FunctionParameters.FunctionObjectTypes.Boolean => "boolean",
            FunctionParameters.FunctionObjectTypes.Null => "null",
            _ => throw new ArgumentOutOfRangeException(nameof(type), $"Unknown type: {type}")
        };
    }
}