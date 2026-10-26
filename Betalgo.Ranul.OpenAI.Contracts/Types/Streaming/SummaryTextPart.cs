using System.Text.Json.Serialization;

namespace Betalgo.Ranul.OpenAI.Contracts.Types.Streaming;

/// <summary>
///     A summary text part used in reasoning summary events.
///     <see href="https://platform.openai.com/docs/api-reference/responses-streaming">OpenAI API documentation</see>.
///     <see href="https://github.com/betalgo/openai/blob/master/Docs/openapi-split/components/schemas/responsereasoningsummarypartaddedevent.yml">
///         Source Definition
///     </see>
/// </summary>
public sealed class SummaryTextPart
{
    /// <summary>
    ///     Initializes a new instance for deserialization.
    /// </summary>
    public SummaryTextPart()
    {
    }

    /// <summary>
    ///     Initializes a new instance with required parameters.
    /// </summary>
    /// <param name="text">The text of the summary part.</param>
    public SummaryTextPart(string text)
    {
        Text = text;
    }

    /// <summary>
    ///     The type of the summary part. Always "summary_text".
    /// </summary>
    [JsonPropertyName("type")]
    public string Type => "summary_text";

    /// <summary>
    ///     The text of the summary part.
    /// </summary>
    [JsonPropertyName("text")]
    public string Text { get; set; } = null!;
}
