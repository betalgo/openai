using System.Text.Json.Serialization;

namespace OpenAI.ObjectModels.SharedModels;

public record EventResponse
{
    [JsonPropertyName("object")] public string? ObjectTypeName { get; set; }
    [JsonPropertyName("id")] public string? Id { get; set; }

    [JsonPropertyName("created_at")] public int? CreatedAt { get; set; }

    [JsonPropertyName("level")] public string Level { get; set; }

    [JsonPropertyName("message")] public string Message { get; set; }
    [JsonPropertyName("type")] public string Type { get; set; }
}