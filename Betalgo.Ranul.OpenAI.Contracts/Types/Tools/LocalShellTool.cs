using System.Text.Json.Serialization;

namespace Betalgo.Ranul.OpenAI.Contracts.Types.Tools;

/// <summary>
///     A tool that allows the model to execute shell commands in a local environment.
///     <see href="https://platform.openai.com/docs/api-reference/responses">OpenAI API documentation</see>.
///     <see href="https://github.com/betalgo/openai/blob/master/Docs/openapi-split/components/schemas/localshelltoolparam.yml">
///         Source Definition
///     </see>
/// </summary>
public class LocalShellTool : ITool
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="LocalShellTool" /> class.
    /// </summary>
    public LocalShellTool()
    {
    }

    /// <summary>
    ///     The type of the local shell tool. Always <c>local_shell</c>.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type => "local_shell";
}




