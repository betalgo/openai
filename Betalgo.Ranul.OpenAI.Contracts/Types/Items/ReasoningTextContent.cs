using System.Text.Json.Serialization;

namespace Betalgo.Ranul.OpenAI.Contracts.Types.Items;

/// <summary>
///     Reasoning text content.
///     <see href="https://platform.openai.com/docs/api-reference/responses">OpenAI API documentation</see>.
/// </summary>
public class ReasoningTextContent
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="ReasoningTextContent" /> class.
    /// </summary>
    public ReasoningTextContent()
    {
    }

    /// <summary>
    ///     Initializes a new instance with parameters.
    /// </summary>
    /// <param name="type">The type of the content.</param>
    /// <param name="text">The reasoning text.</param>
    public ReasoningTextContent(string? type = null, string? text = null)
    {
        Type = type;
        Text = text;
    }

    /// <summary>
    ///     The type of the content.
    /// </summary>
    [JsonPropertyName("type")]
    public string? Type { get; set; }

    /// <summary>
    ///     The reasoning text.
    /// </summary>
    [JsonPropertyName("text")]
    public string? Text { get; set; }
}

