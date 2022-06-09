using System.Text.Json.Serialization;

namespace OpenAI.GPT3.ObjectModels.ResponseModels.FineTuneResponseModels;

public record FineTuneListResponse : BaseResponse
{
    [JsonPropertyName("data")] public List<FineTuneResponse> Data { get; set; }
}