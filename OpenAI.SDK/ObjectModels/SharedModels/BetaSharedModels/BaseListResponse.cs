using System.Text.Json.Serialization;
using OpenAI.ObjectModels.ResponseModels;

namespace OpenAI.ObjectModels.SharedModels.BetaSharedModels;

public record BaseListResponse : BaseResponse
{
    /// <summary>
    /// ID of first assistant in the list
    /// </summary>
    [JsonPropertyName("first_id")]
    public string FirstId { get; set; }
    
    /// <summary>
    /// ID of last assistant in the list
    /// </summary>
    [JsonPropertyName("last_id")]
    public string LastId { get; set; }
    
    /// <summary>
    /// A boolean value for checking list has more assistants or not
    /// </summary>
    [JsonPropertyName("has_more")]
    public bool HasMore { get; set; }
}