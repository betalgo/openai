using System.Text.Json.Serialization;

namespace OpenAI.ObjectModels.RequestModels;

public class ToolChoiceFunction
{
    [JsonPropertyName("type")]
    public string? Type { get; set; } = StaticValues.CompletionStatics.ToolType.Function;

    [JsonPropertyName("function")]
    public FunctionTool? Function { get; set; }
}

public class FunctionTool
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }
}