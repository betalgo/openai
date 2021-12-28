using System.Text.Json.Serialization;
using OpenAI.SDK.Models.ResponseModels;

namespace OpenAI.SDK.Models.SharedModels
{
    public record Engine : BaseResponse
    {
        [JsonPropertyName("id")] public string Id { get; set; }

        [JsonPropertyName("owner")] public string Owner { get; set; }

        [JsonPropertyName("ready")] public bool Ready { get; set; }
    }
}