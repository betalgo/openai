using System.Text.Json.Serialization;

namespace OpenAI.ObjectModels.RequestModels;

public class AssistantFileListRequest : AssistantListRequest
{
    
}

public class AssistantListRequest
{
    private string _order;
    
    /// <summary>
    /// A limit on the number of objects to be returned.
    /// Limit can range between 1 and 100, and the default is 20.
    /// </summary>
    [JsonPropertyName("limit")]
    public int? Limit { get; set; }

    /// <summary>
    /// Sort order by the created_at timestamp of the objects.
    /// "asc" for ascending order and "desc" for descending order.
    /// </summary>
    [JsonPropertyName("order")]
    public string? Order
    {
        get => _order;
        set => _order = value switch
        {
            "asc" => value,
            "desc" => value,
            _ => throw new ArgumentException("Invalid order value. Only 'asc' or 'desc' are allowed")
        };
    }
    
    /// <summary>
    /// A cursor for use in pagination. after is an object ID that defines your place in the list.
    /// For instance, if you make a list request and receive 100 objects, ending with obj_foo,
    /// your subsequent call can include after=obj_foo in order to fetch the next page of the list.
    /// </summary>
    [JsonPropertyName("after")]
    public string? After { get; set; }
    
    /// <summary>
    /// A cursor for use in pagination. before is an object ID that defines your place in the list.
    /// For instance, if you make a list request and receive 100 objects, ending with obj_foo, your
    /// subsequent call can include before=obj_foo in order to fetch the previous page of the list.
    /// </summary>
    [JsonPropertyName("before")]
    public string? Before { get; set; }
    
    
}