using System.Text.Json.Serialization;

namespace Betalgo.Ranul.OpenAI.Contracts.Types.ToolChoice;

/// <summary>
///     Represents a specific tool choice that forces the model to use a particular tool.
///     <see href="https://platform.openai.com/docs/api-reference/responses/create">OpenAI API documentation</see>.
///     <see href="https://github.com/betalgo/openai/blob/master/Docs/openapi-split/components/schemas/toolchoicefunction.yml">
///         Source Definition
///     </see>
/// </summary>
public class SpecificToolChoice
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="SpecificToolChoice" /> class.
    /// </summary>
    public SpecificToolChoice()
    {
    }

    /// <summary>
    ///     Initializes a new instance for a hosted tool type.
    /// </summary>
    /// <param name="type">The type of tool.</param>
    public SpecificToolChoice(string type)
    {
        Type = type;
    }

    /// <summary>
    ///     Initializes a new instance for a function tool.
    /// </summary>
    /// <param name="type">The type (should be "function").</param>
    /// <param name="name">The name of the function to call.</param>
    public SpecificToolChoice(string type, string name)
    {
        Type = type;
        Name = name;
    }

    /// <summary>
    ///     The type of the tool choice.
    ///     For hosted tools: "file_search", "web_search_preview", "computer_use_preview", "code_interpreter",
    ///     "image_generation".
    ///     For functions: "function".
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = null!;

    /// <summary>
    ///     The name of the function to call (only for function tool choice).
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }
}

