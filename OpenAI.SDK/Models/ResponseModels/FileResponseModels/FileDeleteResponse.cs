using System.Text.Json.Serialization;
using OpenAI.GPT3.Models.SharedModels;

namespace OpenAI.GPT3.Models.ResponseModels.FileResponseModels
{
    public record FileDeleteResponse : BaseResponse, IOpenAiModels.IId
    {
        [JsonPropertyName("Deleted")] public bool Deleted { get; set; }
        [JsonPropertyName("id")] public string Id { get; set; }
    }
}