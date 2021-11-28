using System.Text.Json.Serialization;

namespace OpenAI.SDK.Models.ResponseModels
{
    public record ListEngineResponse : BaseResponse
    {
        [JsonPropertyName("data")] public List<Engine> Engines { get; set; }
    }
}