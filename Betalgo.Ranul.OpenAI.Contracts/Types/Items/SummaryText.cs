using System.Text.Json.Serialization;

namespace Betalgo.Ranul.OpenAI.Contracts.Types.Items;

/// <summary>
///     A summary text from the model.
///     <see href="https://platform.openai.com/docs/api-reference/responses">OpenAI API documentation</see>.
///     <see href="https://github.com/betalgo/openai/blob/master/Docs/openapi-split/components/schemas/summary.yml">
///         Source Definition
///     </see>
/// </summary>
public class SummaryText
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="SummaryText" /> class.
    /// </summary>
    public SummaryText()
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="SummaryText" /> class with required parameters.
    /// </summary>
    /// <param name="text">A summary of the reasoning output from the model so far.</param>
    public SummaryText(string text)
    {
        Text = text;
    }

    /// <summary>
    ///     The type of the object. Always <c>summary_text</c>.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type => "summary_text";

    /// <summary>
    ///     A summary of the reasoning output from the model so far.
    /// </summary>
    [JsonPropertyName("text")]
    public string Text { get; set; } = null!;
}

