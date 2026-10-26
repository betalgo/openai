using System.Text.Json.Serialization;
using Betalgo.Ranul.OpenAI.Contracts.Enums;
using Betalgo.Ranul.OpenAI.Contracts.Enums.Responses;

namespace Betalgo.Ranul.OpenAI.Contracts.Requests.Responses;

/// <summary>
///     Configuration for reasoning models. <b>gpt-5 and o-series models only</b>.
///     <see href="https://platform.openai.com/docs/guides/reasoning">Reasoning Guide</see>.
///     <see href="https://github.com/betalgo/openai/blob/master/Docs/openapi-split/components/schemas/reasoning.yml">
///         Source Definition
///     </see>
/// </summary>
public class ReasoningConfig
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="ReasoningConfig" /> class.
    /// </summary>
    public ReasoningConfig()
    {
    }

    /// <summary>
    ///     Initializes a new instance with parameters.
    /// </summary>
    /// <param name="effort">The effort level for the reasoning model.</param>
    /// <param name="summary">Whether to include a summary of reasoning.</param>
    public ReasoningConfig(ReasoningEffort? effort = null, ReasoningSummary? summary = null)
    {
        Effort = effort;
        Summary = summary;
    }

    /// <summary>
    ///     The effort level for the reasoning model.
    ///     One of <c>low</c>, <c>medium</c>, or <c>high</c>.
    /// </summary>
    [JsonPropertyName("effort")]
    public ReasoningEffort? Effort { get; set; }

    /// <summary>
    ///     A summary of the reasoning performed by the model. This can be useful for debugging
    ///     and understanding the model's reasoning process.
    ///     One of <c>auto</c>, <c>concise</c>, or <c>detailed</c>.
    ///     <c>concise</c> is only supported for <c>computer-use-preview</c> models.
    /// </summary>
    [JsonPropertyName("summary")]
    public ReasoningSummary? Summary { get; set; }

    /// <summary>
    ///     <b>Deprecated:</b> use <see cref="Summary" /> instead.
    ///     A summary of the reasoning performed by the model.
    /// </summary>
    [Obsolete("Deprecated: use Summary instead.")]
    [JsonPropertyName("generate_summary")]
    public ReasoningSummary? GenerateSummary { get; set; }
}

