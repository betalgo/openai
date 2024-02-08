using System.Text.Json.Serialization;

namespace OpenAI.ObjectModels.SharedModels.BetaSharedModels;

public class Tool
{
    /// <summary>
    /// The type of tool being defined
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; }

    /// <summary>
    /// Function definition, only used if type is "function"
    /// </summary>
    [JsonPropertyName("function")]
    public Function Function { get; set; }
}