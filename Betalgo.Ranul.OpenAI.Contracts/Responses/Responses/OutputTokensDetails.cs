using System.Text.Json.Serialization;

namespace Betalgo.Ranul.OpenAI.Contracts.Responses.Responses;

/// <summary>
///     A detailed breakdown of the output tokens.
///     <see href="https://platform.openai.com/docs/api-reference/responses/object">OpenAI API documentation</see>.
/// </summary>
public class OutputTokensDetails
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="OutputTokensDetails" /> class.
    /// </summary>
    public OutputTokensDetails()
    {
    }

    /// <summary>
    ///     Initializes a new instance with reasoning tokens count.
    /// </summary>
    /// <param name="reasoningTokens">The number of reasoning tokens.</param>
    public OutputTokensDetails(int reasoningTokens)
    {
        ReasoningTokens = reasoningTokens;
    }

    /// <summary>
    ///     The number of reasoning tokens.
    /// </summary>
    [JsonPropertyName("reasoning_tokens")]
    public int ReasoningTokens { get; set; }
}

