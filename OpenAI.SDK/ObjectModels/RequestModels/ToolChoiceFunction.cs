using System.Text.Json.Serialization;

namespace OpenAI.ObjectModels.RequestModels;

public class ToolChoice
{
    public static ToolChoice None => new() { Type = StaticValues.CompletionStatics.ToolChoiceType.None };
    public static ToolChoice Auto => new() { Type = StaticValues.CompletionStatics.ToolChoiceType.Auto };
    public static ToolChoice FunctionChoice(string functionName) =>new()
    {
        Type = StaticValues.CompletionStatics.ToolChoiceType.Function,
        Function = new FunctionTool()
        {
            Name = functionName
        }
    };

    /// <summary>
    ///     "none" is the default when no functions are present.  <br />
    ///     "auto" is the default if functions are present.  <br />
    ///     "function" has to be assigned if user Function is not null<br />
    ///     <br />
    ///     Check <see cref="StaticValues.CompletionStatics.ToolChoiceType" /> for possible values.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("function")] public FunctionTool? Function { get; set; }

    public class FunctionTool
    {
        [JsonPropertyName("name")] public string Name { get; set; }
    }
}