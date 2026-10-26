using System.Text.Json;
using System.Text.Json.Serialization;

namespace Betalgo.Ranul.OpenAI.Contracts.Types.Tools;

/// <summary>
///     A function tool that can be called by the model.
///     <see href="https://platform.openai.com/docs/guides/function-calling">Function Calling Guide</see>.
///     <see href="https://github.com/betalgo/openai/blob/master/Docs/openapi-split/components/schemas/functiontool.yml">
///         Source Definition
///     </see>
/// </summary>
/// <example>
///     <code>
/// // Option 1: Raw JSON parameters (zero-cost pass-through)
/// var tool = new FunctionTool
/// {
///     Name = "get_weather",
///     Description = "Get current weather",
///     Parameters = """{"type":"object","properties":{"location":{"type":"string"}}}"""
/// };
/// 
/// // Option 2: Entire function from raw JSON
/// var tool = FunctionTool.FromJson("""
///     {
///         "type": "function",
///         "name": "get_weather",
///         "description": "Get current weather",
///         "parameters": { ... }
///     }
///     """);
///     </code>
/// </example>
public class FunctionTool : ITool
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="FunctionTool" /> class.
    /// </summary>
    public FunctionTool()
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="FunctionTool" /> class with required parameters.
    /// </summary>
    /// <param name="name">The name of the function to call.</param>
    /// <param name="strict">Whether to enforce strict parameter validation. Default <c>true</c>.</param>
    /// <param name="parameters">The JSON schema for the function parameters.</param>
    public FunctionTool(string name, bool? strict = null, FunctionParameters? parameters = null)
    {
        Name = name;
        Strict = strict;
        Parameters = parameters;
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="FunctionTool" /> class with name and description.
    /// </summary>
    /// <param name="name">The name of the function to call.</param>
    /// <param name="description">A description of the function.</param>
    /// <param name="parameters">The JSON schema for the function parameters.</param>
    /// <param name="strict">Whether to enforce strict parameter validation. Default <c>true</c>.</param>
    public FunctionTool(string name, string? description, FunctionParameters? parameters = null, bool? strict = null)
    {
        Name = name;
        Description = description;
        Parameters = parameters;
        Strict = strict;
    }

    /// <summary>
    ///     The type of the function tool. Always <c>function</c>.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type => "function";

    /// <summary>
    ///     The name of the function to call.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;

    /// <summary>
    ///     A description of the function. Used by the model to determine whether or not to call the function.
    /// </summary>
    [JsonPropertyName("description")]
    public string? Description { get; set; }

    /// <summary>
    ///     A JSON schema object describing the parameters of the function.
    ///     <para>
    ///         Accepts either a raw JSON string or a Dictionary. Raw JSON strings are passed through
    ///         without parsing for zero performance overhead.
    ///     </para>
    /// </summary>
    /// <example>
    ///     <code>
    /// // Raw JSON string (zero-cost)
    /// Parameters = """{"type":"object","properties":{"location":{"type":"string"}}}""";
    /// 
    /// // Dictionary
    /// Parameters = new Dictionary&lt;string, object&gt; { ["type"] = "object", ... };
    ///     </code>
    /// </example>
    [JsonPropertyName("parameters")]
    public FunctionParameters? Parameters { get; set; }

    /// <summary>
    ///     Whether to enforce strict parameter validation. Default <c>true</c>.
    /// </summary>
    [JsonPropertyName("strict")]
    public bool? Strict { get; set; }

    /// <summary>
    ///     Creates a <see cref="FunctionTool" /> from a raw JSON string.
    ///     The entire function definition is parsed from the JSON.
    /// </summary>
    /// <param name="json">The raw JSON string containing the function tool definition.</param>
    /// <returns>A new <see cref="FunctionTool" /> instance.</returns>
    /// <example>
    ///     <code>
    /// var tool = FunctionTool.FromJson("""
    ///     {
    ///         "type": "function",
    ///         "name": "get_weather",
    ///         "description": "Get current weather",
    ///         "parameters": {
    ///             "type": "object",
    ///             "properties": {
    ///                 "location": { "type": "string" }
    ///             }
    ///         },
    ///         "strict": true
    ///     }
    ///     """);
    ///     </code>
    /// </example>
    public static FunctionTool FromJson(string json)
    {
        return JsonSerializer.Deserialize<FunctionTool>(json) 
            ?? throw new ArgumentException("Invalid JSON for FunctionTool", nameof(json));
    }
}

