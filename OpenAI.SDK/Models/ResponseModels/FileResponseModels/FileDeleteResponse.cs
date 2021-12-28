using System.Text.Json.Serialization;
using OpenAI.SDK.Models.SharedModels;

namespace OpenAI.SDK.Models.ResponseModels.FileResponseModels
{
    public record FileDeleteResponse : BaseResponse, IOpenAiModels.IId
    {
        [JsonPropertyName("Deleted")] public bool Deleted { get; set; }
        [JsonPropertyName("id")] public string Id { get; set; }
    }
}