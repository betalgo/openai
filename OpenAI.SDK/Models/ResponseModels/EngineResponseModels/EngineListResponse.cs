using System.Text.Json.Serialization;

namespace OpenAI.GPT3.Models.ResponseModels.EngineResponseModels
{
    public record EngineListResponse : BaseResponse
    {
        [JsonPropertyName("data")] public List<EngineResponse> Engines { get; set; }
    }
}