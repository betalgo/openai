using System.Text.Json.Serialization;
using OpenAI.GPT3.ObjectModels.SharedModels;

namespace OpenAI.GPT3.ObjectModels.ResponseModels;

public record ChatCompletionCreateResponse : BaseResponse, IOpenAiModels.IId, IOpenAiModels.ICreatedAt
{
    [JsonPropertyName("model")] public string? Model { get; set; }

    [JsonPropertyName("choices")] public List<ChatChoiceResponse> Choices { get; set; } = new();

    [JsonPropertyName("usage")] public UsageResponse Usage { get; set; } = new();

    [JsonPropertyName("created")] public int CreatedAt { get; set; }

    [JsonPropertyName("id")] public string Id { get; set; } = string.Empty;
}
