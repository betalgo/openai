using System.Text.Json.Serialization;

namespace Betalgo.Ranul.OpenAI.Contracts.Responses.Responses;

/// <summary>
///     Configuration for reasoning in a response.
///     <see href="https://platform.openai.com/docs/guides/reasoning">Reasoning Guide</see>.
/// </summary>
public class ResponsesReasoningConfig
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="ResponsesReasoningConfig" /> class.
    /// </summary>
    public ResponsesReasoningConfig()
    {
    }

    /// <summary>
    ///     Initializes a new instance with parameters.
    /// </summary>
    /// <param name="effort">The effort level used for reasoning.</param>
    /// <param name="summary">Whether a summary of reasoning was included.</param>
    public ResponsesReasoningConfig(string? effort = null, string? summary = null)
    {
        Effort = effort;
        Summary = summary;
    }

    /// <summary>
    ///     The effort level used for reasoning.
    /// </summary>
    [JsonPropertyName("effort")]
    public string? Effort { get; set; }

    /// <summary>
    ///     Whether a summary of reasoning was included.
    /// </summary>
    [JsonPropertyName("summary")]
    public string? Summary { get; set; }
}

