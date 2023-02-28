using System.Text.Json.Serialization;
using OpenAI.GPT3.ObjectModels.SharedModels;

namespace OpenAI.GPT3.ObjectModels.ResponseModels;

public record EditCreateResponse : BaseResponse, IOpenAiModels.ICreatedAt
{
    [JsonPropertyName("model")] public string Model { get; set; }

    [JsonPropertyName("choices")] public List<ChoiceResponse> Choices { get; set; }

    [JsonPropertyName("usage")] public UsageResponse Usage { get; set; }

    [JsonPropertyName("created")] public int CreatedAt { get; set; }
}