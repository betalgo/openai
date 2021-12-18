using System.Text.Json.Serialization;

namespace OpenAI.SDK.Models.ResponseModels
{
    public record RetrieveEngineResponse : Engine
    {
    }

    public record Engine : BaseResponse
    {
        [JsonPropertyName("id")] public string Id { get; set; }

        [JsonPropertyName("owner")] public string Owner { get; set; }

        [JsonPropertyName("ready")] public bool Ready { get; set; }
    }
}