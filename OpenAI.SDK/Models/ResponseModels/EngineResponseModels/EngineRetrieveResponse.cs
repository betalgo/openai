using System.Text.Json.Serialization;

namespace OpenAI.GPT3.Models.ResponseModels.EngineResponseModels
{
    public record EngineRetrieveResponse : EngineResponse
    {
    }

    public record EngineResponse : BaseResponse
    {
        [JsonPropertyName("id")] public string Id { get; set; }

        [JsonPropertyName("owner")] public string Owner { get; set; }

        [JsonPropertyName("ready")] public bool? Ready { get; set; }

    }
}