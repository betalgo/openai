using System.Text.Json.Serialization;

namespace OpenAI.Enums;

public enum OrderEnum
{
    [JsonPropertyName("asc")]
    ASC,
    [JsonPropertyName("desc")]
    DESC
}