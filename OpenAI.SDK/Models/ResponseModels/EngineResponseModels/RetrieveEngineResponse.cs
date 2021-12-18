using System.Text.Json.Serialization;
using OpenAI.SDK.Interfaces;

namespace OpenAI.SDK.Models.ResponseModels.EngineResponseModels
{
    public record RetrieveEngineResponse : EngineResponse
    {
    }
    public record EngineResponse : BaseResponse
    {
        [JsonPropertyName("id")] public string Id { get; set; }

        [JsonPropertyName("owner")] public string Owner { get; set; }

        [JsonPropertyName("ready")] public bool Ready { get; set; }
    }

}