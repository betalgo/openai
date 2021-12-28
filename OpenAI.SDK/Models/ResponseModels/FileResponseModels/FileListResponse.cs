using System.Text.Json.Serialization;
using OpenAI.SDK.Models.SharedModels;

namespace OpenAI.SDK.Models.ResponseModels.FileResponseModels
{
    public record FileListResponse : BaseResponse
    {
        [JsonPropertyName("data")] public List<FileResponse> Data { get; set; }
    }
}