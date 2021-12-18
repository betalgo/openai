using System.Text.Json.Serialization;

namespace OpenAI.SDK.Models.ResponseModels
{
    public record DeleteResponseModel : BaseResponse
    {
        [JsonPropertyName("id")] public string Id { get; set; }

        [JsonPropertyName("Deleted")] public bool Deleted { get; set; }
    }
}