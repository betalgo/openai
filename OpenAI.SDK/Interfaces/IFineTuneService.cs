using OpenAI.ObjectModels.RequestModels;
using OpenAI.ObjectModels.ResponseModels.FineTuneResponseModels;

namespace OpenAI.Interfaces;

/// <summary>
///     Manage fine-tuning jobs to tailor a model to your specific training data.
///     Related guide: <a href="https://platform.openai.com/docs/guides/fine-tuning">Fine-tune models</a>
/// </summary>
public interface IFineTuneService
{
    /// <summary>
    ///     Creates a job that fine-tunes a specified model from a given dataset.
    ///     Response includes details of the enqueued job including job status and the name of the fine-tuned models once
    ///     complete.
    /// </summary>
    /// <param name="createFineTuneRequest"></param>
    /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
    /// <returns></returns>
    Task<FineTuneResponse> CreateFineTune(FineTuneCreateRequest createFineTuneRequest, CancellationToken cancellationToken = default);

    /// <summary>
    ///     List your organization's fine-tuning jobs
    /// </summary>
    /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
    /// <returns></returns>
    Task<FineTuneListResponse> ListFineTunes(CancellationToken cancellationToken = default);

    /// <summary>
    ///     Gets info about the fine-tune job.
    /// </summary>
    /// <param name="fineTuneId">The ID of the fine-tune job</param>
    /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
    /// <returns></returns>
    Task<FineTuneResponse> RetrieveFineTune(string fineTuneId, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Immediately cancel a fine-tune job.
    /// </summary>
    /// <param name="fineTuneId">The ID of the fine-tune job to cancel</param>
    /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
    /// <returns></returns>
    Task<FineTuneResponse> CancelFineTune(string fineTuneId, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Get fine-grained status updates for a fine-tune job.
    /// </summary>
    /// <param name="fineTuneId">The ID of the fine-tune job to get events for.</param>
    /// <param name="stream">
    ///     Whether to stream events for the fine-tune job. If set to true, events will be sent as data-only server-sent events
    ///     as they become available. The stream will terminate with a data: [DONE] message when the job is finished
    ///     (succeeded, cancelled, or failed).
    ///     If set to false, only events generated so far will be returned.
    /// </param>
    /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
    /// <returns></returns>
    Task<Stream> ListFineTuneEvents(string fineTuneId, bool? stream = null, CancellationToken cancellationToken = default);

    Task DeleteFineTune(string fineTuneId, CancellationToken cancellationToken = default);
}