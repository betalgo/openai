using System.Text.Json.Serialization;
using OpenAI.SDK.Models.ResponseModels;

namespace OpenAI.SDK.Models.SharedModels
{
    public record FileResponse : BaseResponse, IOpenAiModels.IId, IOpenAiModels.ICreatedAt, IOpenAiModels.IFilePurpose
    {
        [JsonPropertyName("bytes")] public int? Bytes { get; set; }
        [JsonPropertyName("filename")] public string FileName { get; set; }
        public UploadFilePurposes.UploadFilePurpose PurposeEnum => UploadFilePurposes.ToEnum(Purpose);
        [JsonPropertyName("created_at")] public int? CreatedAt { get; set; }
        [JsonPropertyName("purpose")] public string Purpose { get; set; }
        [JsonPropertyName("id")] public string Id { get; set; }
    }
}