using System.Text.Json.Serialization;
using OpenAI.ObjectModels.SharedModels;

namespace OpenAI.ObjectModels.ResponseModels;

public record ChatCompletionCreateResponse : BaseResponse, IOpenAIModels.IId, IOpenAIModels.ICreatedAt
{
    [JsonPropertyName("model")] public string Model { get; set; }
    [JsonPropertyName("object")] public string Type { get; set; }

    [JsonPropertyName("choices")] public List<ChatChoiceResponse> Choices { get; set; }

    [JsonPropertyName("usage")] public UsageResponse? Usage { get; set; }

    [JsonPropertyName("created")] public int CreatedAt { get; set; }

    [JsonPropertyName("id")] public string Id { get; set; }
}