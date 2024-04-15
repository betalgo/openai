using System.Text.Json.Serialization;
using OpenAI.ObjectModels.SharedModels;

namespace OpenAI.ObjectModels.ResponseModels;

public record AssistantFileListResponse : DataBaseResponse<List<AssistantFileResponse>>
{
    [JsonPropertyName("first_id")]
    public string FirstId { get; set; }

    [JsonPropertyName("last_id")]
    public string LastId { get; set; }

    [JsonPropertyName("has_more")]
    public bool IsHasMore { get; set; }
}