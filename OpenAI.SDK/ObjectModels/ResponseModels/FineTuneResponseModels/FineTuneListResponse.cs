using System.Text.Json.Serialization;

namespace OpenAI.ObjectModels.ResponseModels.FineTuneResponseModels;

public record FineTuneListResponse : BaseResponse
{
    [JsonPropertyName("data")] public List<FineTuneResponse> Data { get; set; }
}