using System.Text.Json.Serialization;
using Betalgo.Ranul.OpenAI.ObjectModels.SharedModels;

namespace Betalgo.Ranul.OpenAI.ObjectModels.ResponseModels.FileResponseModels;

public record FileUploadResponse : BaseResponse, IOpenAIModels.ICreatedAt
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("bytes")]
    public int Bytes { get; set; }

    [JsonPropertyName("filename")]
    public string FileName { get; set; }

    [JsonPropertyName("purpose")]
    public string Purpose { get; set; }

    [JsonPropertyName("created_at")] public long CreatedAt { get; set; }
}