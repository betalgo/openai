using System.Text.Json.Serialization;
using OpenAI.ObjectModels.SharedModels;

namespace OpenAI.ObjectModels.ResponseModels.FineTuningJobResponseModels;

public record FineTuningJobResponse : BaseResponse, IOpenAiModels.IId, IOpenAiModels.IModel, IOpenAiModels.ICreatedAt
{
    /// <summary>
    ///     The Unix timestamp (in seconds) for when the fine-tuning job was finished. The value will be null if the
    ///     fine-tuning job is still running.
    /// </summary>
    [JsonPropertyName("finished_at")]
    public int? FinishedAt { get; set; }

    /// <summary>
    ///     The name of the fine-tuned model that is being created. The value will be null if the fine-tuning job is still
    ///     running.
    /// </summary>
    [JsonPropertyName("fine_tuned_model")]
    public string? FineTunedModel { get; set; }

    /// <summary>
    ///     The organization that owns the fine-tuning job.
    /// </summary>
    [JsonPropertyName("organization_id")]
    public string? OrganizationId { get; set; }

    /// <summary>
    ///     The current status of the fine-tuning job, which can be either created, pending, running, succeeded, failed, or
    ///     cancelled.
    /// </summary>
    [JsonPropertyName("status")]
    public string Status { get; set; }

    /// <summary>
    ///     The hyperparameters used for the fine-tuning job. See the fine-tuning guide for more details.
    /// </summary>
    [JsonPropertyName("hyperparameters")]
    public Hyperparameters HyperparametersData { get; set; }

    /// <summary>
    ///     The file ID used for training. You can retrieve the training data with the Files API.
    /// </summary>
    [JsonPropertyName("training_file")]
    public string TrainingFile { get; set; }

    /// <summary>
    ///     The file ID used for validation. You can retrieve the validation results with the Files API.
    /// </summary>
    [JsonPropertyName("validation_file")]
    public string? ValidationFile { get; set; }

    /// <summary>
    ///     The compiled results file ID(s) for the fine-tuning job. You can retrieve the results with the Files API.
    /// </summary>
    [JsonPropertyName("result_files")]
    public List<string> ResultFiles { get; set; }

    /// <summary>
    ///     The total number of billable tokens processed by this fine-tuning job. The value will be null if the fine-tuning
    ///     job is still running.
    /// </summary>
    [JsonPropertyName("trained_tokens")]
    public int? TrainedTokens { get; set; }

    /// <summary>
    ///     The Unix timestamp (in seconds) for when the fine-tuning job was created.
    /// </summary>
    [JsonPropertyName("created_at")]
    public long CreatedAt { get; set; }

    /// <summary>
    ///     The object identifier, which can be referenced in the API endpoints.
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; set; }

    /// <summary>
    ///     The base model that is being fine-tuned.
    /// </summary>
    [JsonPropertyName("model")]
    public string Model { get; set; }


    public class Hyperparameters
    {
        /// <summary>
        ///     The number of epochs to train the model for. An epoch refers to one full cycle through the training dataset.
        ///     "Auto" decides the optimal number of epochs based on the size of the dataset. If setting the number manually, we
        ///     support any number between 1 and 50 epochs.
        ///     "Auto" == -1 
        /// </summary>
        [JsonPropertyName("n_epochs")]
        [JsonConverter(typeof(NEpochsConverter))]
        public int? NEpochs { get; set; }
    }
}