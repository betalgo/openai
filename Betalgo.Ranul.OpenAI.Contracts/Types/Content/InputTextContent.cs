using System.Text.Json.Serialization;

namespace Betalgo.Ranul.OpenAI.Contracts.Types.Content;

/// <summary>
///     A text input to the model.
///     <see href="https://platform.openai.com/docs/api-reference/responses">OpenAI API documentation</see>.
///     <see href="https://github.com/betalgo/openai/blob/master/Docs/openapi-split/components/schemas/inputtextcontent.yml">
///         Source Definition
///     </see>
/// </summary>
public class InputTextContent : IContent
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="InputTextContent" /> class.
    /// </summary>
    public InputTextContent()
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="InputTextContent" /> class with required parameters.
    /// </summary>
    /// <param name="text">The text content.</param>
    public InputTextContent(string text)
    {
        Text = text;
    }

    /// <summary>
    ///     The type of the input text. Always <c>input_text</c>.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type => "input_text";

    /// <summary>
    ///     The text input to the model.
    /// </summary>
    [JsonPropertyName("text")]
    public string Text { get; set; } = null!;
}

