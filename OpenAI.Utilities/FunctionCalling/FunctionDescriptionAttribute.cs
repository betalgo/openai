namespace OpenAI.Utilities.FunctionCalling;

/// <summary>
///     Attribute to mark a method as a function, and provide a description for the function. Can also be used to override
///     the Name of the function
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class FunctionDescriptionAttribute : Attribute
{
    /// <summary>
    ///     Creates a new instance of the <see cref="FunctionDescriptionAttribute" /> with the provided description
    /// </summary>
    public FunctionDescriptionAttribute(string? description = null)
    {
        Description = description;
    }

    /// <summary>
    ///     Name of the function. If not provided, the name of the method will be used.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    ///     Description of the function
    /// </summary>
    public string? Description { get; set; }
}
