using OpenAI.ObjectModels.RequestModels;
using OpenAI.ObjectModels.ResponseModels.BatchResponseModel;

namespace OpenAI.Interfaces;

public interface IBatchService
{
    /// <summary>
    ///     Creates and executes a batch from an uploaded file of requests.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>The created Batch object.</returns>
    Task<BatchResponse> BatchCreate(BatchCreateRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Retrieves a batch.
    /// </summary>
    /// <param name="batchId">The ID of the batch to retrieve.</param>
    /// <param name="cancellationToken"></param>
    /// <returns>The Batch object matching the specified ID.</returns>
    Task<BatchResponse?> BatchRetrieve(string batchId, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Cancels an in-progress batch.
    /// </summary>
    /// <param name="batchId">The ID of the batch to cancel.</param>
    /// <param name="cancellationToken"></param>
    /// <returns>The Batch object matching the specified ID.</returns>
    Task<BatchResponse> BatchCancel(string batchId, CancellationToken cancellationToken = default);

}