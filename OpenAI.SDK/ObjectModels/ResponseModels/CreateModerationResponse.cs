using System.Text.Json.Serialization;
using OpenAI.ObjectModels.SharedModels;

namespace OpenAI.ObjectModels.ResponseModels;

public record CreateModerationResponse : BaseResponse, IOpenAiModels.IModel, IOpenAiModels.IId
{
    [JsonPropertyName("results")] public List<Result> Results { get; set; }

    [JsonPropertyName("id")] public string Id { get; set; }

    [JsonPropertyName("model")] public string Model { get; set; }
}

public record Result
{
    [JsonPropertyName("categories")] public Categories Categories { get; set; }

    [JsonPropertyName("category_scores")] public CategoryScores CategoryScores { get; set; }

    [JsonPropertyName("flagged")] public bool Flagged { get; set; }
}

public record Categories
{
    [JsonPropertyName("hate")] public bool Hate { get; set; }

    [JsonPropertyName("hate/threatening")] public bool HateThreatening { get; set; }

    [JsonPropertyName("harassment")] public bool Harassment { get; set; }

    [JsonPropertyName("harassment/threatening")]
    public bool HarassmentThreatening { get; set; }

    [JsonPropertyName("self-harm")] public bool SelfHarm { get; set; }
    [JsonPropertyName("self-harm/intent")] public bool SelfHarmIntent { get; set; }

    [JsonPropertyName("self-harm/instructions")]
    public bool SelfHarmInstructions { get; set; }

    [JsonPropertyName("sexual")] public bool Sexual { get; set; }

    [JsonPropertyName("sexual/minors")] public bool SexualMinors { get; set; }

    [JsonPropertyName("violence")] public bool Violence { get; set; }

    [JsonPropertyName("violence/graphic")] public bool ViolenceGraphic { get; set; }
}

public record CategoryScores
{
    [JsonPropertyName("hate")] public float Hate { get; set; }

    [JsonPropertyName("hate/threatening")] public float HateThreatening { get; set; }

    [JsonPropertyName("harassment")] public float Harassment { get; set; }

    [JsonPropertyName("harassment/threatening")]
    public float HarassmentThreatening { get; set; }

    [JsonPropertyName("self-harm")] public float SelfHarm { get; set; }
    [JsonPropertyName("self-harm/intent")] public float SelfHarmIntent { get; set; }

    [JsonPropertyName("self-harm/instructions")]
    public float SelfHarmInstructions { get; set; }

    [JsonPropertyName("sexual")] public float Sexual { get; set; }

    [JsonPropertyName("sexual/minors")] public float SexualMinors { get; set; }

    [JsonPropertyName("violence")] public float Violence { get; set; }

    [JsonPropertyName("violence/graphic")] public float ViolenceGraphic { get; set; }
}