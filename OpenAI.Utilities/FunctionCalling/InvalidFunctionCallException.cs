namespace OpenAI.Utilities.FunctionCalling;

/// <summary>
///     Exception thrown when a function call is invalid
/// </summary>
public class InvalidFunctionCallException : Exception
{
    /// <summary>
    ///     Creates a new instance of the <see cref="InvalidFunctionCallException" /> with the provided message
    /// </summary>
    public InvalidFunctionCallException(string message) : base(message)
    {
    }
}
