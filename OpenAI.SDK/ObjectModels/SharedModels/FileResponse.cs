using System.Text.Json.Serialization;
using OpenAI.GPT3.ObjectModels.ResponseModels;

namespace OpenAI.GPT3.ObjectModels.SharedModels;

public record FileResponse : BaseResponse, IOpenAiModels.IId, IOpenAiModels.ICreatedAt
{
    [JsonPropertyName("bytes")] public int? Bytes { get; set; }
    [JsonPropertyName("filename")] public string FileName { get; set; }
    public UploadFilePurposes.UploadFilePurpose PurposeEnum => UploadFilePurposes.ToEnum(Purpose);
    [JsonPropertyName("purpose")] public string Purpose { get; set; }
    [JsonPropertyName("status")] public string Status { get; set; }
    [JsonPropertyName("created_at")] public int CreatedAt { get; set; }
    [JsonPropertyName("id")] public string Id { get; set; }
}