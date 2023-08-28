using OpenAI.ObjectModels.SharedModels;
using System.Text.Json.Serialization;

namespace OpenAI.ObjectModels.ResponseModels.FineTuningJobResponseModels;

public record FineTuningJobResponse : BaseResponse, IOpenAiModels.IId, IOpenAiModels.IModel, IOpenAiModels.ICreatedAt
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("created_at")]
    public int CreatedAt { get; set; }

    [JsonPropertyName("finished_at")]
    public int? FinishedAt { get; set; }

    [JsonPropertyName("model")]
    public string Model { get; set; }

    [JsonPropertyName("fine_tuned_model")]
    public string? FineTunedModel { get; set; }

    [JsonPropertyName("organization_id")]
    public string OrganizationId { get; set; }

    [JsonPropertyName("status")]
    public string Status { get; set; }

    [JsonPropertyName("hyperparameters")]
    public HyperParametersResponse HyperParameters { get; set; }

    [JsonPropertyName("validation_file")]
    public string? ValidationFile { get; set; }

    [JsonPropertyName("training_file")]
    public string TrainingFile { get; set; }

    [JsonPropertyName("result_files")]
    public List<string> ResultFiles { get; set; }

    [JsonPropertyName("trained_tokens")]
    public int? TrainedTokens { get; set; }

    [JsonPropertyName("updated_at")]
    public int? UpdatedAt { get; set; }
}
