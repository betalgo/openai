using System.Text.Json.Serialization;
using OpenAI.GPT3.Models.SharedModels;

namespace OpenAI.GPT3.Models.ResponseModels.FileResponseModels
{
    public record FileListResponse : BaseResponse
    {
        [JsonPropertyName("data")] public List<FileResponse> Data { get; set; }
    }
}