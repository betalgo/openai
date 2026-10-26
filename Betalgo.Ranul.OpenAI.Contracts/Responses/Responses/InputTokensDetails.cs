using System.Text.Json.Serialization;

namespace Betalgo.Ranul.OpenAI.Contracts.Responses.Responses;

/// <summary>
///     A detailed breakdown of the input tokens.
///     <see href="https://platform.openai.com/docs/api-reference/responses/object">OpenAI API documentation</see>.
/// </summary>
public class InputTokensDetails
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="InputTokensDetails" /> class.
    /// </summary>
    public InputTokensDetails()
    {
    }

    /// <summary>
    ///     Initializes a new instance with cached tokens count.
    /// </summary>
    /// <param name="cachedTokens">The number of tokens that were retrieved from the cache.</param>
    public InputTokensDetails(int cachedTokens)
    {
        CachedTokens = cachedTokens;
    }

    /// <summary>
    ///     The number of tokens that were retrieved from the cache.
    /// </summary>
    [JsonPropertyName("cached_tokens")]
    public int CachedTokens { get; set; }
}

