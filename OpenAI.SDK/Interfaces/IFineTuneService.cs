using OpenAI.GPT3.ObjectModels.RequestModels;
using OpenAI.GPT3.ObjectModels.ResponseModels.FineTuneResponseModels;
using OpenAI.GPT3.ObjectModels.SharedModels;

namespace OpenAI.GPT3.Interfaces;

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
    /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
    /// <returns></returns>
    Task<FineTuneListEventsResponse> ListFineTuneEvents(string fineTuneId, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Get fine-grained status updates for a fine-tune job as a stream (blocks until a new event comes in).
    /// </summary>
    /// <param name="fineTuneId">The ID of the fine-tune job to get events for.</param>
    /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
    /// <returns></returns>
    IAsyncEnumerable<EventResponse> ListFineTuneEventsStream(string fineTuneId, CancellationToken cancellationToken = default);

    Task DeleteFineTune(string fineTuneId, CancellationToken cancellationToken = default);
}