using System.Text.Json.Serialization;

namespace OpenAI.ObjectModels.ResponseModels.ModelResponseModels;

public record ModelRetrieveResponse : ModelResponse
    // ReSharper disable once RedundantRecordBody
{
}

public record ModelResponse : BaseResponse
{
    [JsonPropertyName("id")] public string Id { get; set; }

    [JsonPropertyName("owned_by")] public string Owner { get; set; }

    [JsonPropertyName("permission")] public List<Permission> Permissions { get; set; } = new();

    [JsonPropertyName("created")] public int Created { get; set; }
    public DateTimeOffset CreatedTime => DateTimeOffset.FromUnixTimeSeconds(Created);


    [JsonPropertyName("root")] public string? Root { get; set; }

    [JsonPropertyName("parent")] public string? Parent { get; set; }
}

public record Permission : BaseResponse
{
    [JsonPropertyName("id")] public string Id { get; set; }

    [JsonPropertyName("created")] public int Created { get; set; }
    public DateTimeOffset CreatedTime => DateTimeOffset.FromUnixTimeSeconds(Created);

    [JsonPropertyName("allow_create_engine")]
    public bool AllowCreateEngine { get; set; }

    [JsonPropertyName("allow_sampling")] public bool AllowSampling { get; set; }

    [JsonPropertyName("allow_logprobs")] public bool AllowLogprobs { get; set; }

    [JsonPropertyName("allow_search_indices")]
    public bool AllowSearchIndices { get; set; }

    [JsonPropertyName("allow_view")] public bool AllowView { get; set; }

    [JsonPropertyName("allow_fine_tuning")]
    public bool AllowFineTuning { get; set; }

    [JsonPropertyName("organization")] public string Organization { get; set; }

    [JsonPropertyName("group")] public object Group { get; set; }

    [JsonPropertyName("is_blocking")] public bool IsBlocking { get; set; }
}