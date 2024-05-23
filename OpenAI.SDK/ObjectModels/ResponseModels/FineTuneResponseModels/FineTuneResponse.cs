using System.Text.Json.Serialization;
using OpenAI.ObjectModels.SharedModels;

namespace OpenAI.ObjectModels.ResponseModels.FineTuneResponseModels;

public record FineTuneResponse : BaseResponse, IOpenAiModels.IId, IOpenAiModels.IModel, IOpenAiModels.ICreatedAt
{
    [JsonPropertyName("events")] public List<EventResponse> Events { get; set; }

    [JsonPropertyName("fine_tuned_model")] public string FineTunedModel { get; set; }

    [JsonPropertyName("hyperparams")] public HyperParametersResponse HyperParams { get; set; }

    [JsonPropertyName("organization_id")] public string OrganizationId { get; set; }

    [JsonPropertyName("result_files")] public List<FileResponse> ResultFiles { get; set; }

    [JsonPropertyName("status")] public string Status { get; set; }

    [JsonPropertyName("validation_files")] public List<FileResponse> ValidationFiles { get; set; }

    [JsonPropertyName("training_files")] public List<FileResponse> TrainingFiles { get; set; }

    [JsonPropertyName("updated_at")] public int? UpdatedAt { get; set; }

    [JsonPropertyName("created_at")] public long CreatedAt { get; set; }
    [JsonPropertyName("id")] public string Id { get; set; }

    [JsonPropertyName("model")] public string Model { get; set; }
}