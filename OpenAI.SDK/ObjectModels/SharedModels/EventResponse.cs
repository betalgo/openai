using System.Text.Json.Serialization;

namespace Betalgo.Ranul.OpenAI.ObjectModels.SharedModels;

public record EventResponse:IOpenAIModels.ICreatedAt
{
    [JsonPropertyName("object")]
    public string? ObjectTypeName { get; set; }

    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("created_at")]
    public long CreatedAtUnix { get; set; }
    public DateTimeOffset CreatedAt => DateTimeOffset.FromUnixTimeSeconds(CreatedAtUnix);


    [JsonPropertyName("level")]
    public string Level { get; set; }

    [JsonPropertyName("message")]
    public string Message { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }
}