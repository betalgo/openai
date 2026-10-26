using System.Text.Json.Serialization;

namespace Betalgo.Ranul.OpenAI.Contracts.Types.Streaming;

/// <summary>
///     A logprob is the logarithmic probability that the model assigns to producing
///     a particular token at a given position in the sequence. Less-negative (higher)
///     logprob values indicate greater model confidence in that token choice.
///     <see href="https://platform.openai.com/docs/api-reference/responses-streaming">OpenAI API documentation</see>.
///     <see href="https://github.com/betalgo/openai/blob/master/Docs/openapi-split/components/schemas/responselogprob.yml">
///         Source Definition
///     </see>
/// </summary>
public sealed class ResponseLogProb
{
    /// <summary>
    ///     Initializes a new instance for deserialization.
    /// </summary>
    public ResponseLogProb()
    {
    }

    /// <summary>
    ///     Initializes a new instance with required parameters.
    /// </summary>
    /// <param name="token">The text token.</param>
    /// <param name="logprob">The log probability of this token.</param>
    public ResponseLogProb(string token, double logprob)
    {
        Token = token;
        Logprob = logprob;
    }

    /// <summary>
    ///     A possible text token.
    /// </summary>
    [JsonPropertyName("token")]
    public string Token { get; set; } = null!;

    /// <summary>
    ///     The log probability of this token.
    /// </summary>
    [JsonPropertyName("logprob")]
    public double Logprob { get; set; }

    /// <summary>
    ///     The log probability of the top 20 most likely tokens.
    /// </summary>
    [JsonPropertyName("top_logprobs")]
    public List<TopLogprobItem>? TopLogprobs { get; set; }
}

/// <summary>
///     An item in the top logprobs list, representing a possible token and its log probability.
/// </summary>
public sealed class TopLogprobItem
{
    /// <summary>
    ///     Initializes a new instance for deserialization.
    /// </summary>
    public TopLogprobItem()
    {
    }

    /// <summary>
    ///     Initializes a new instance with required parameters.
    /// </summary>
    /// <param name="token">The text token.</param>
    /// <param name="logprob">The log probability of this token.</param>
    public TopLogprobItem(string token, double logprob)
    {
        Token = token;
        Logprob = logprob;
    }

    /// <summary>
    ///     A possible text token.
    /// </summary>
    [JsonPropertyName("token")]
    public string Token { get; set; } = null!;

    /// <summary>
    ///     The log probability of this token.
    /// </summary>
    [JsonPropertyName("logprob")]
    public double Logprob { get; set; }
}
