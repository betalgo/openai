using System.Text.Json.Serialization;

namespace Betalgo.Ranul.OpenAI.ObjectModels.ResponseModels.ModelResponseModels;

public record ModelListResponse : BaseResponse
{
    [JsonPropertyName("data")]
    public List<ModelResponse> Models { get; set; }
}