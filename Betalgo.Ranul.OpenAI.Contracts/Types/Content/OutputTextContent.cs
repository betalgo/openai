using System.Text.Json.Serialization;

namespace Betalgo.Ranul.OpenAI.Contracts.Types.Content;

/// <summary>
///     A text output from the model.
///     <see href="https://platform.openai.com/docs/api-reference/responses">OpenAI API documentation</see>.
///     <see href="https://github.com/betalgo/openai/blob/master/Docs/openapi-split/components/schemas/outputtextcontent.yml">
///         Source Definition
///     </see>
/// </summary>
public class OutputTextContent : IContent
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="OutputTextContent" /> class.
    /// </summary>
    public OutputTextContent()
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="OutputTextContent" /> class with required parameters.
    /// </summary>
    /// <param name="text">The text output from the model.</param>
    /// <param name="annotations">The annotations of the text output.</param>
    public OutputTextContent(string text, List<Annotation>? annotations = null)
    {
        Text = text;
        Annotations = annotations ?? new List<Annotation>();
    }

    /// <summary>
    ///     The type of the output text. Always <c>output_text</c>.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type => "output_text";

    /// <summary>
    ///     The text output from the model.
    /// </summary>
    [JsonPropertyName("text")]
    public string Text { get; set; } = null!;

    /// <summary>
    ///     The annotations of the text output.
    /// </summary>
    [JsonPropertyName("annotations")]
    public List<Annotation> Annotations { get; set; } = new();

    /// <summary>
    ///     Log probability information for the output tokens, if requested.
    /// </summary>
    [JsonPropertyName("logprobs")]
    public List<LogProb>? Logprobs { get; set; }
}
