using OpenAI.ObjectModels.RequestModels;
using OpenAI.ObjectModels.ResponseModels;

namespace OpenAI.Interfaces;

public interface IRunStepService
{
    /// <summary>
    ///     Returns a list of run steps belonging to a run.
    /// </summary>
    /// <param name="threadId">The ID of the thread the run and run steps belong to.</param>
    /// <param name="runId">The ID of the run steps belong to.</param>
    /// <param name="request">Paging</param>
    /// <param name="cancellationToken"></param>
    /// <returns>A list of [run step](/docs/api-reference/runs/step-object) objects.</returns>
    Task<RunStepListResponse> RunStepsList(string threadId, string runId, PaginationRequest? request = null, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Retrieves a run step.
    /// </summary>
    /// <param name="threadId">The ID of the thread to which the run and run step belongs.</param>
    /// <param name="runId">The ID of the run to which the run step belongs.</param>
    /// <param name="stepId">The ID of the run step to retrieve.</param>
    /// <param name="cancellationToken"></param>
    /// <returns>The [run step](/docs/api-reference/runs/step-object) object matching the specified ID.</returns>
    Task<RunStepResponse> RunStepRetrieve(string threadId, string runId, string stepId, CancellationToken cancellationToken = default);
}