using System.Text.Json.Serialization;

namespace OpenAI.SDK.Models.ResponseModels.EngineResponseModels
{
    public record ListEngineResponse : BaseResponse
    {
        [JsonPropertyName("data")] public List<EngineResponse> Engines { get; set; }
    }
}