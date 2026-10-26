using System.Text.Json.Serialization;

namespace Betalgo.Ranul.OpenAI.Contracts.Types.Tools;

/// <summary>
///     A tool that allows the model to apply file patches.
///     <see href="https://platform.openai.com/docs/api-reference/responses">OpenAI API documentation</see>.
///     <see href="https://github.com/betalgo/openai/blob/master/Docs/openapi-split/components/schemas/applypatchtoolparam.yml">
///         Source Definition
///     </see>
/// </summary>
public class ApplyPatchTool : ITool
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="ApplyPatchTool" /> class.
    /// </summary>
    public ApplyPatchTool()
    {
    }

    /// <summary>
    ///     The type of the apply patch tool. Always <c>apply_patch</c>.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type => "apply_patch";
}




