using System.Text.Json.Serialization;

namespace OpenAI.ObjectModels.ResponseModels;

public record EmbeddingCreateResponse : BaseResponse
{
    [JsonPropertyName("model")] public string Model { get; set; }

    [JsonPropertyName("data")] public List<EmbeddingResponse> Data { get; set; }

    [JsonPropertyName("usage")] public UsageResponse Usage { get; set; }
}

public record EmbeddingResponse
{
    [JsonPropertyName("index")] public int? Index { get; set; }

    [JsonPropertyName("embedding")] public List<double> Embedding { get; set; }
}