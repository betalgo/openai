using System.Text.Json.Serialization;
using OpenAI.ObjectModels.SharedModels;

namespace OpenAI.ObjectModels.ResponseModels;

public record CreateModerationResponse : BaseResponse, IOpenAiModels.IModel, IOpenAiModels.IId
{
    [JsonPropertyName("results")] public List<Result> Results { get; set; } = new();

    [JsonPropertyName("id")] public string Id { get; set; } = string.Empty;

    [JsonPropertyName("model")] public string? Model { get; set; }
}

public record Result
{
    [JsonPropertyName("categories")] public Categories Categories { get; set; } = new();

    [JsonPropertyName("category_scores")] public CategoryScores CategoryScores { get; set; } = new();

    [JsonPropertyName("flagged")] public bool Flagged { get; set; }
}

public record Categories
{
    [JsonPropertyName("hate")] public bool Hate { get; set; }

    [JsonPropertyName("hatethreatening")] public bool HateThreatening { get; set; }

    [JsonPropertyName("selfharm")] public bool Selfharm { get; set; }

    [JsonPropertyName("sexual")] public bool Sexual { get; set; }

    [JsonPropertyName("sexualminors")] public bool SexualMinors { get; set; }

    [JsonPropertyName("violence")] public bool Violence { get; set; }

    [JsonPropertyName("violencegraphic")] public bool Violencegraphic { get; set; }
}

public record CategoryScores
{
    [JsonPropertyName("hate")] public float Hate { get; set; }

    [JsonPropertyName("hatethreatening")] public float HateThreatening { get; set; }

    [JsonPropertyName("selfharm")] public float Selfharm { get; set; }

    [JsonPropertyName("sexual")] public float Sexual { get; set; }

    [JsonPropertyName("sexualminors")] public float SexualMinors { get; set; }

    [JsonPropertyName("violence")] public float Violence { get; set; }

    [JsonPropertyName("violencegraphic")] public float Violencegraphic { get; set; }
}
