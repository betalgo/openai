using System.Text.Json.Serialization;
using Betalgo.Ranul.OpenAI.Contracts.Interfaces;
using Betalgo.Ranul.OpenAI.Contracts.Responses.Base;
using Betalgo.Ranul.OpenAI.Contracts.Types;

namespace Betalgo.Ranul.OpenAI.Contracts.Responses.Responses;

/// <summary>
///     A list of Response items.
///     <see href="https://platform.openai.com/docs/api-reference/responses/list-input-items">OpenAI API documentation</see>.
///     <see href="https://github.com/betalgo/openai/blob/master/Docs/openapi-split/components/schemas/responseitemlist.yml">
///         Source Definition
///     </see>
/// </summary>
public class ResponseItemList : ResponseBase, IDefaultResult<ResponseItemList>
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="ResponseItemList" /> class.
    /// </summary>
    public ResponseItemList()
    {
    }

    /// <summary>
    ///     Gets this instance as the result for <see cref="IDefaultResult{T}" />.
    /// </summary>
    [JsonIgnore]
    public ResponseItemList? Result => Successful ? this : null;

    /// <summary>
    ///     A list of items used to generate this response.
    /// </summary>
    [JsonPropertyName("data")]
    public List<IInputItem>? Data { get; set; }

    /// <summary>
    ///     Whether there are more items available.
    /// </summary>
    [JsonPropertyName("has_more")]
    public bool HasMore { get; set; }

    /// <summary>
    ///     The ID of the first item in the list.
    /// </summary>
    [JsonPropertyName("first_id")]
    public string? FirstId { get; set; }

    /// <summary>
    ///     The ID of the last item in the list.
    /// </summary>
    [JsonPropertyName("last_id")]
    public string? LastId { get; set; }
}

