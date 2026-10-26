using System.Text.Json.Serialization;

namespace Betalgo.Ranul.OpenAI.Contracts.Responses.Responses;

/// <summary>
///     Token usage details for a response.
///     <see href="https://platform.openai.com/docs/api-reference/responses/object">OpenAI API documentation</see>.
///     <see href="https://github.com/betalgo/openai/blob/master/Docs/openapi-split/components/schemas/responseusage.yml">
///         Source Definition
///     </see>
/// </summary>
public class ResponsesUsage
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="ResponsesUsage" /> class.
    /// </summary>
    public ResponsesUsage()
    {
    }

    /// <summary>
    ///     Initializes a new instance with required parameters.
    /// </summary>
    /// <param name="inputTokens">The number of input tokens.</param>
    /// <param name="outputTokens">The number of output tokens.</param>
    /// <param name="totalTokens">The total number of tokens used.</param>
    public ResponsesUsage(int inputTokens, int outputTokens, int totalTokens)
    {
        InputTokens = inputTokens;
        OutputTokens = outputTokens;
        TotalTokens = totalTokens;
    }

    /// <summary>
    ///     The number of input tokens.
    /// </summary>
    [JsonPropertyName("input_tokens")]
    public int InputTokens { get; set; }

    /// <summary>
    ///     A detailed breakdown of the input tokens.
    /// </summary>
    [JsonPropertyName("input_tokens_details")]
    public InputTokensDetails? InputTokensDetails { get; set; }

    /// <summary>
    ///     The number of output tokens.
    /// </summary>
    [JsonPropertyName("output_tokens")]
    public int OutputTokens { get; set; }

    /// <summary>
    ///     A detailed breakdown of the output tokens.
    /// </summary>
    [JsonPropertyName("output_tokens_details")]
    public OutputTokensDetails? OutputTokensDetails { get; set; }

    /// <summary>
    ///     The total number of tokens used.
    /// </summary>
    [JsonPropertyName("total_tokens")]
    public int TotalTokens { get; set; }
}

