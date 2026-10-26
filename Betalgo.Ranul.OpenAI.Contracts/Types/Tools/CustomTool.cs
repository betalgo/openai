using System.Text.Json.Serialization;

namespace Betalgo.Ranul.OpenAI.Contracts.Types.Tools;

/// <summary>
///     A custom tool that processes input using a specified format.
///     <see href="https://platform.openai.com/docs/guides/function-calling#custom-tools">Custom Tools Guide</see>.
///     <see href="https://github.com/betalgo/openai/blob/master/Docs/openapi-split/components/schemas/customtoolparam.yml">
///         Source Definition
///     </see>
/// </summary>
public class CustomTool : ITool
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="CustomTool" /> class.
    /// </summary>
    public CustomTool()
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="CustomTool" /> class with required parameters.
    /// </summary>
    /// <param name="name">The name of the custom tool.</param>
    public CustomTool(string name)
    {
        Name = name;
    }

    /// <summary>
    ///     The type of the custom tool. Always <c>custom</c>.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type => "custom";

    /// <summary>
    ///     The name of the custom tool, used to identify it in tool calls.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;

    /// <summary>
    ///     Optional description of the custom tool, used to provide more context.
    /// </summary>
    [JsonPropertyName("description")]
    public string? Description { get; set; }

    /// <summary>
    ///     The input format for the custom tool. Default is unconstrained text.
    /// </summary>
    [JsonPropertyName("format")]
    public object? Format { get; set; }
}




