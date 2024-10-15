using System.Text.Json.Serialization;
using Betalgo.OpenAI.ObjectModels.SharedModels;

namespace Betalgo.OpenAI.ObjectModels.ResponseModels.FileResponseModels;

public record FileListResponse : BaseResponse
{
    [JsonPropertyName("data")]
    public List<FileResponse> Data { get; set; }
}