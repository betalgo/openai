using System.Text.Json.Serialization;
using OpenAI.ObjectModels.ResponseModels.FineTuningJobResponseModels;
using OpenAI.ObjectModels.SharedModels;

namespace OpenAI.ObjectModels.RequestModels;

public record FineTuningJobCreateRequest : IOpenAiModels.IModel
{
    /// <summary>
    ///     The ID of an uploaded file that contains training data.
    ///     See <see href="https://platform.openai.com/docs/api-reference/files/upload">upload file</see> for how to upload a
    ///     file.
    ///     Your dataset must be formatted as a JSONL file. Additionally, you must upload your file with the purpose
    ///     <code>fine-tune</code>.
    ///     See the <see href="https://platform.openai.com/docs/guides/fine-tuning">fine-tuning guide</see> for more details.
    /// </summary>
    [JsonPropertyName("training_file")]
    public string TrainingFile { get; set; }

    /// <summary>
    ///     The ID of an uploaded file that contains validation data.
    ///     If you provide this file, the data is used to generate validation metrics periodically during fine-tuning.
    ///     These metrics can be viewed in the fine-tuning results file.
    ///     The same data should not be present in both train and validation files.
    ///     Your dataset must be formatted as a JSONL file. You must upload your file with the purpose <code>fine-tune</code>.
    ///     See the <see href="https://platform.openai.com/docs/guides/fine-tuning">fine-tuning guide</see> for more details.
    /// </summary>
    [JsonPropertyName("validation_file")]
    public string? ValidationFile { get; set; }

    /// <summary>
    ///     The hyperparameters used for the fine-tuning job.
    /// </summary>
    [JsonPropertyName("hyperparameters")]
    public HyperparametersRequestModel? Hyperparameters { get; set; } // This can be further detailed if the properties of the hyperparameters are known.

    /// <summary>
    ///     A string of up to 18 characters that will be added to your fine-tuned model name.
    ///     For example, a <code>suffix</code> of "custom-model-name" would produce a model name like
    ///     <code>ft:gpt-3.5-turbo:openai:custom-model-name:7p4lURel</code>.
    /// </summary>
    [JsonPropertyName("suffix")]
    public string? Suffix { get; set; } = null;

    /// <summary>
    ///     The name of the model to fine-tune. You can select one of the
    ///     <see href="https://platform.openai.com/docs/guides/fine-tuning/what-models-can-be-fine-tuned">supported models</see>
    ///     .
    /// </summary>
    [JsonPropertyName("model")]
    public string Model { get; set; }
}

public class HyperparametersRequestModel
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