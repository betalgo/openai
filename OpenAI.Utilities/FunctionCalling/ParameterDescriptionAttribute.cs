namespace OpenAI.Utilities.FunctionCalling;

/// <summary>
///     Attribute to describe a parameter of a function. Can also be used to override the Name of the parameter
/// </summary>
[AttributeUsage(AttributeTargets.Parameter)]
public class ParameterDescriptionAttribute : Attribute
{
    /// <summary>
    ///     Creates a new instance of the <see cref="ParameterDescriptionAttribute" /> with the provided description
    /// </summary>
    public ParameterDescriptionAttribute(string? description = null)
    {
        Description = description;
    }

    /// <summary>
    ///     Name of the parameter. If not provided, the name of the parameter will be used.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    ///     Description of the parameter
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    ///     Type of the parameter. If not provided, the type of the parameter will be inferred from the parameter type
    /// </summary>
    public string? Type { get; set; }

    /// <summary>
    ///     Enum values of the parameter in a comma separated string. If not provided, the enum values will be inferred from
    ///     the parameter type
    /// </summary>
    public string? Enum { get; set; }

    /// <summary>
    ///     Whether the parameter is required. If not provided, the parameter will be required. Default is true
    /// </summary>
    public bool Required { get; set; } = true;
}
