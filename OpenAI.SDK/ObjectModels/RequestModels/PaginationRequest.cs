using System.Net;
using System.Text.Json.Serialization;

namespace OpenAI.ObjectModels.RequestModels;

public class VectorStoreFileListRequest : PaginationRequest
{
    /// <summary>
    ///     Filter by file status. One of in_progress, completed, failed, cancelled.
    /// </summary>
    [JsonPropertyName("filter")]
    public string? Filter { get; set; }

    public override string? GetQueryParameters()
    {
        var querystring = base.GetQueryParameters();
        if (Filter == null)
        {
            return querystring;
        }

        return querystring == null ? $"filter={WebUtility.UrlEncode(Filter)}" : $"{querystring}&filter={WebUtility.UrlEncode(Filter)}";
    }
}

public class PaginationRequest
{
    /// <summary>
    ///     A limit on the number of objects to be returned.
    ///     Limit can range between 1 and 100, and the default is 20.
    /// </summary>
    [JsonPropertyName("limit")]
    public int? Limit { get; set; }

    /// <summary>
    ///     Sort order by the created_at timestamp of the objects.
    ///     "asc" for ascending order and "desc" for descending order.
    /// </summary>
    [JsonPropertyName("order")]
    public string? Order { get; set; }


    /// <summary>
    ///     A cursor for use in pagination. after is an object ID that defines your place in the list.
    ///     For instance, if you make a list request and receive 100 objects, ending with obj_foo,
    ///     your subsequent call can include after=obj_foo in order to fetch the next page of the list.
    /// </summary>
    [JsonPropertyName("after")]
    public string? After { get; set; }

    /// <summary>
    ///     A cursor for use in pagination. before is an object ID that defines your place in the list.
    ///     For instance, if you make a list request and receive 100 objects, ending with obj_foo, your
    ///     subsequent call can include before=obj_foo in order to fetch the previous page of the list.
    /// </summary>
    [JsonPropertyName("before")]
    public string? Before { get; set; }

    public virtual string? GetQueryParameters()
    {
        var build = new List<string>();
        if (Limit != null)
        {
            build.Add($"limit={Limit}");
        }

        if (Order != null)
        {
            build.Add($"order={WebUtility.UrlEncode(Order)}");
        }

        if (After != null)
        {
            build.Add($"after={WebUtility.UrlEncode(After)}");
        }

        if (Before != null)
        {
            build.Add($"before={WebUtility.UrlEncode(Before)}");
        }

        if (build.Count <= 0) return null;

        return string.Join("&", build);
    }
}