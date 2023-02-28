using System.Text.Json.Serialization;
using OpenAI.GPT3.ObjectModels.SharedModels;

namespace OpenAI.GPT3.ObjectModels.ResponseModels.FileResponseModels;

public record FileUploadResponse : BaseResponse, IOpenAiModels.ICreatedAt
{
    [JsonPropertyName("id")] public string Id { get; set; }

    [JsonPropertyName("bytes")] public int Bytes { get; set; }

    [JsonPropertyName("filename")] public string FileName { get; set; }

    [JsonPropertyName("purpose")] public string Purpose { get; set; }

    [JsonPropertyName("created_at")] public int CreatedAt { get; set; }
}