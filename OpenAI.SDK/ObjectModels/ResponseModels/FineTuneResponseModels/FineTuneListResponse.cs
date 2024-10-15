using System.Text.Json.Serialization;

namespace Betalgo.OpenAI.ObjectModels.ResponseModels.FineTuneResponseModels;

public record FineTuneListResponse : BaseResponse
{
    [JsonPropertyName("data")]
    public List<FineTuneResponse> Data { get; set; }
}