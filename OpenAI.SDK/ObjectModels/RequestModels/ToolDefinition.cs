using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace OpenAI.ObjectModels.RequestModels;

/// <summary>
///     Definition of a valid tool.
/// </summary>
public class ToolDefinition
{
    /// <summary>
    ///     Required. The type of the tool. Currently, only function is supported.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; }

  
    /// <summary>
    ///     A list of functions the model may generate JSON inputs for.
    /// </summary>
    [JsonIgnore]
    public FunctionDefinition? Function { get; set; }

    [JsonIgnore] 
    public object? FunctionsAsObject { get; set; }

    /// <summary>
    ///     Required. The description of what the function does.
    /// </summary>
    [JsonPropertyName("function")]
    public object? FunctionCalculated
    {
        get
        {
            if (FunctionsAsObject != null && Function != null)
            {
                throw new ValidationException("FunctionAsObject and Function can not be assigned at the same time. One of them is should be null.");
            }

            return Function ?? FunctionsAsObject;
        }
    }

    public static ToolDefinition DefineFunction(FunctionDefinition function) => new()
    {
        Type = StaticValues.CompletionStatics.ToolType.Function,
        Function = function
    };

    public static ToolDefinition DefineCodeInterpreter() => new()
    {
        Type = StaticValues.AssistantsStatics.ToolCallTypes.CodeInterpreter,
    };

    [Obsolete("Retrieval is now called FileSearch")]
    public static ToolDefinition DefineRetrieval() => new()
    {
        Type = StaticValues.AssistantsStatics.ToolCallTypes.FileSearch,
    };

    public static ToolDefinition DefineFileSearch() => new()
    {
        Type = StaticValues.AssistantsStatics.ToolCallTypes.FileSearch,
    };

}