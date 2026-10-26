using System.Text.Json.Serialization;
using Betalgo.Ranul.OpenAI.Contracts.Interfaces;
using Betalgo.Ranul.OpenAI.Contracts.Responses.Base;

namespace Betalgo.Ranul.OpenAI.Contracts.Responses.Responses;

/// <summary>
///     The response object containing input token counts.
///     <see href="https://platform.openai.com/docs/api-reference/responses/input-tokens">OpenAI API documentation</see>.
///     <see href="https://github.com/betalgo/openai/blob/master/Docs/openapi-split/components/schemas/tokencountsresource.yml">
///         Source Definition
///     </see>
/// </summary>
public class TokenCountsResponse : ResponseBase, IDefaultResult<TokenCountsResponse>
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="TokenCountsResponse" /> class.
    /// </summary>
    public TokenCountsResponse()
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="TokenCountsResponse" /> class.
    /// </summary>
    /// <param name="inputTokens">The number of input tokens.</param>
    public TokenCountsResponse(int inputTokens)
    {
        InputTokens = inputTokens;
    }

    /// <summary>
    ///     Gets this instance as the result for <see cref="IDefaultResult{T}" />.
    /// </summary>
    [JsonIgnore]
    public TokenCountsResponse? Result => Successful ? this : null;

    /// <summary>
    ///     The number of input tokens.
    /// </summary>
    [JsonPropertyName("input_tokens")]
    public int InputTokens { get; set; }
}

