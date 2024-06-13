using System.Text.Json.Serialization;
using OpenAI.ObjectModels.SharedModels;

namespace OpenAI.ObjectModels.ResponseModels;

public record CompletionCreateResponse : BaseResponse, IOpenAiModels.IId, IOpenAiModels.ICreatedAt
{
    [JsonPropertyName("model")] public string Model { get; set; }

    [JsonPropertyName("choices")] public List<ChoiceResponse> Choices { get; set; }

    [JsonPropertyName("usage")] public UsageResponse Usage { get; set; }

    [JsonPropertyName("created")] public long CreatedAt { get; set; }

    [JsonPropertyName("id")] public string Id { get; set; }
}