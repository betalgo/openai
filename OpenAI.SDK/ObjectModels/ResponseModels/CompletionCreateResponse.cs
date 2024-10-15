using System.Text.Json.Serialization;
using Betalgo.OpenAI.ObjectModels.SharedModels;

namespace Betalgo.OpenAI.ObjectModels.ResponseModels;

public record CompletionCreateResponse : BaseResponse, IOpenAIModels.IId, IOpenAIModels.ICreatedAt
{
    [JsonPropertyName("model")]
    public string Model { get; set; }

    [JsonPropertyName("choices")]
    public List<ChoiceResponse> Choices { get; set; }

    [JsonPropertyName("usage")]
    public UsageResponse Usage { get; set; }

    [JsonPropertyName("created")] public long CreatedAt { get; set; }

    [JsonPropertyName("id")]
    public string Id { get; set; }
}