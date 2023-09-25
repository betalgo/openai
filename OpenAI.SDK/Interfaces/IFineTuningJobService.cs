using OpenAI.ObjectModels.RequestModels;
using OpenAI.ObjectModels.ResponseModels.FineTuningJobResponseModels;

namespace OpenAI.Interfaces;

/// <summary>
///     Manage fine-tuning jobs to tailor a model to your specific training data.
///     Related guide: <a href="https://platform.openai.com/docs/guides/fine-tuning">Fine-tune models</a>
/// </summary>
public interface IFineTuningJobService
{
    /// <summary>
    ///     Creates a job that fine-tunes a specified model from a given dataset.
    ///     Response includes details of the enqueued job including job status and the name of the fine-tuned models once
    ///     complete.
    /// </summary>
    /// <param name="createFineTuningJobRequest"></param>
    /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
    /// <returns></returns>
    Task<FineTuningJobResponse> CreateFineTuningJob(FineTuningJobCreateRequest createFineTuningJobRequest, CancellationToken cancellationToken = default);

    /// <summary>
    ///     List your organization's fine-tuning jobs
    /// </summary>
    /// <param name="fineTuningJobListRequest"></param>
    /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
    /// <returns></returns>
    Task<FineTuningJobListResponse> ListFineTuningJobs(FineTuningJobListRequest? fineTuningJobListRequest =null,CancellationToken cancellationToken = default);

    /// <summary>
    ///     Gets info about the fine-tuning job.
    /// </summary>
    /// <param name="fineTuningJobId">The ID of the fine-tuning job</param>
    /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
    /// <returns></returns>
    Task<FineTuningJobResponse> RetrieveFineTuningJob(string fineTuningJobId, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Immediately cancel a fine-tuning job.
    /// </summary>
    /// <param name="fineTuningJobId">The ID of the fine-tuning job to cancel</param>
    /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
    /// <returns></returns>
    Task<FineTuningJobResponse> CancelFineTuningJob(string fineTuningJobId, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Get fine-grained status updates for a fine-tuning job.
    /// </summary>
    /// <param name="model"></param>
    /// <param name="stream">
    ///     Whether to stream events for the fine-tuning job. If set to true, events will be sent as data-only server-sent events
    ///     as they become available. The stream will terminate with a data: [DONE] message when the job is finished
    ///     (succeeded, cancelled, or failed).
    ///     If set to false, only events generated so far will be returned.
    /// </param>
    /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
    /// <returns></returns>
    Task<Stream> ListFineTuningJobEvents(FineTuningJobListEventsRequest model, bool? stream = null, CancellationToken cancellationToken = default);
}
