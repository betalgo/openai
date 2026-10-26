using System.Net;
using System.Text.Json.Serialization;
using Betalgo.Ranul.OpenAI.Contracts.Enums.Responses;

namespace Betalgo.Ranul.OpenAI.Contracts.Requests.Responses;

/// <summary>
///     Request parameters for listing input items for a given response.
///     <see href="https://platform.openai.com/docs/api-reference/responses/list-input-items">OpenAI API documentation</see>.
///     <see href="https://github.com/betalgo/openai/blob/master/Docs/openapi-split/paths/responses/param-response_id/input_items/get/listinputitems.yml">
///         Source Definition
///     </see>
/// </summary>
public class InputItemsListRequest
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="InputItemsListRequest" /> class.
    /// </summary>
    public InputItemsListRequest()
    {
    }

    /// <summary>
    ///     A limit on the number of objects to be returned.
    ///     Limit can range between 1 and 100, and the default is 20.
    /// </summary>
    [JsonPropertyName("limit")]
    public int? Limit { get; set; }

    /// <summary>
    ///     The order to return the input items in. Default is <c>desc</c>.
    /// </summary>
    [JsonPropertyName("order")]
    public InputItemsOrder? Order { get; set; }

    /// <summary>
    ///     An item ID to list items after, used in pagination.
    /// </summary>
    [JsonPropertyName("after")]
    public string? After { get; set; }

    /// <summary>
    ///     Additional fields to include in the response.
    /// </summary>
    [JsonPropertyName("include")]
    public List<ResponsesInclude>? Include { get; set; }

    /// <summary>
    ///     Gets the query parameters string for the request.
    /// </summary>
    /// <returns>Query string or null if no parameters are set.</returns>
    public string? GetQueryParameters()
    {
        var build = new List<string>();

        if (Limit.HasValue)
        {
            build.Add($"limit={Limit.Value}");
        }

        if (Order.HasValue)
        {
            build.Add($"order={WebUtility.UrlEncode(Order.Value.Value)}");
        }

        if (!string.IsNullOrWhiteSpace(After))
        {
            build.Add($"after={WebUtility.UrlEncode(After)}");
        }

        if (Include is { Count: > 0 })
        {
            foreach (var include in Include)
            {
                build.Add($"include[]={WebUtility.UrlEncode(include.Value)}");
            }
        }

        return build.Count > 0 ? string.Join("&", build) : null;
    }
}

