using System.Text.Json;
using System.Text.Json.Serialization;

namespace Betalgo.Ranul.OpenAI.Contracts.Types.Tools;

/// <summary>
///     A code interpreter tool that allows the model to run Python code.
///     <see href="https://platform.openai.com/docs/guides/tools">OpenAI Tools Guide</see>.
///     <see href="https://github.com/betalgo/openai/blob/master/Docs/openapi-split/components/schemas/codeinterpretertool.yml">
///         Source Definition
///     </see>
/// </summary>
public class CodeInterpreterTool : ITool
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="CodeInterpreterTool" /> class.
    /// </summary>
    public CodeInterpreterTool()
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="CodeInterpreterTool" /> class with a container.
    /// </summary>
    /// <param name="container">The container to use for the code interpreter.</param>
    public CodeInterpreterTool(string? container)
    {
        Container = container;
    }

    /// <summary>
    ///     The type of the code interpreter tool. Always <c>code_interpreter</c>.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type => "code_interpreter";

    /// <summary>
    ///     The container to use for the code interpreter.
    /// </summary>
    [JsonIgnore]
    public string? Container { get; set; }

    /// <summary>Object configuration such as auto; use instead of a string Container ID.</summary>
    [JsonIgnore]
    public JsonElement? ContainerConfiguration { get; set; }

    [JsonPropertyName("container")]
    public object? ContainerCalculated
    {
        get => ContainerConfiguration is JsonElement configuration ? configuration : Container;
        set
        {
            if (value is JsonElement element)
            {
                if (element.ValueKind == JsonValueKind.String) { Container = element.GetString(); ContainerConfiguration = null; }
                else { ContainerConfiguration = element.Clone(); Container = null; }
            }
            else { Container = value as string; ContainerConfiguration = null; }
        }
    }
}
