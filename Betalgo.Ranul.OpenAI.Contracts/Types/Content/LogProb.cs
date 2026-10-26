using System.Text.Json.Serialization;

namespace Betalgo.Ranul.OpenAI.Contracts.Types.Content;

/// <summary>
///     Log probability information for a token.
///     <see href="https://platform.openai.com/docs/api-reference/responses">OpenAI API documentation</see>.
/// </summary>
public class LogProb
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="LogProb" /> class.
    /// </summary>
    public LogProb()
    {
    }

    /// <summary>
    ///     Initializes a new instance with parameters.
    /// </summary>
    /// <param name="token">The token.</param>
    /// <param name="logprob">The log probability of the token.</param>
    public LogProb(string? token = null, double? logprob = null)
    {
        Token = token;
        Logprob = logprob;
    }

    /// <summary>
    ///     The token.
    /// </summary>
    [JsonPropertyName("token")]
    public string? Token { get; set; }

    /// <summary>
    ///     The log probability of the token.
    /// </summary>
    [JsonPropertyName("logprob")]
    public double? Logprob { get; set; }

    /// <summary>
    ///     The bytes of the token.
    /// </summary>
    [JsonPropertyName("bytes")]
    public List<int>? Bytes { get; set; }
}

