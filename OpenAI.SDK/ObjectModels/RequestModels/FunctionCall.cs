using System.Text.Json;
using System.Text.Json.Serialization;

namespace OpenAI.ObjectModels.RequestModels;

/// <summary>
///     Describes a function call returned from GPT.
///     A function call contains a function name, and a dictionary
///     mapping function argument names to their values.
/// </summary>
public class FunctionCall
{
    /// <summary>
    ///     Function name
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    /// <summary>
    ///     Function arguments, returned as a JSON-encoded dictionary mapping
    ///     argument names to argument values.
    /// </summary>
    [JsonPropertyName("arguments")]
    public string? Arguments { get; set; }

    public Dictionary<string, object> ParseArguments()
    {
        var result = !string.IsNullOrWhiteSpace(Arguments) ? JsonSerializer.Deserialize<Dictionary<string, object>>(Arguments) : null;
        return result ?? new Dictionary<string, object>();
    }
}