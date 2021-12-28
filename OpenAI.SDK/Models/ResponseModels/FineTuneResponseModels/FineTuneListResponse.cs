using System.Text.Json.Serialization;

namespace OpenAI.GPT3.Models.ResponseModels.FineTuneResponseModels;

public record FineTuneListResponse : BaseResponse
{
    [JsonPropertyName("data")] public List<FineTuneResponse> Data { get; set; }
}