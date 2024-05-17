using OpenAI.ObjectModels.RequestModels;
using OpenAI.ObjectModels.SharedModels;

namespace OpenAI.Interfaces;

public interface IRunService
{
    /// <summary>
    ///     Create a run.
    /// </summary>
    /// <param name="threadId"></param>
    /// <param name="request"></param>
    /// <param name="modelId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<RunResponse> RunCreate(string threadId, RunCreateRequest request, string? modelId = null, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Retrieves a run.
    /// </summary>
    /// <param name="threadId"></param>
    /// <param name="runId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<RunResponse> RunRetrieve(string threadId, string runId, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Cancels a run that is in_progress.
    /// </summary>
    /// <param name="threadId"></param>
    /// <param name="runId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<RunResponse> RunCancel(string threadId, string runId, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Submit tool outputs to run
    ///     <para>
    ///         When a run has the status: "requires_action" and required_action.type is submit_tool_outputs,
    ///         this endpoint can be used to submit the outputs from the tool calls once they're all completed.
    ///         All outputs must be submitted in a single request.
    ///     </para>
    /// </summary>
    /// <param name="threadId"></param>
    /// <param name="runId"></param>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<RunResponse> RunSubmitToolOutputs(string threadId, string runId, SubmitToolOutputsToRunRequest request, CancellationToken cancellationToken = default);
    
    /// <summary>
    ///     Modifies a run.
    /// </summary>
    /// <param name="threadId">The ID of the [thread](/docs/api-reference/threads) that was run.</param>
    /// <param name="runId">The ID of the run to modify.</param>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<RunResponse> RunModify(string threadId, string runId, RunModifyRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Create a thread and run it in one request.
    /// </summary>
    Task<RunResponse> CreateThreadAndRun(CreateThreadAndRunRequest requestBody, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Returns a list of runs belonging to a thread.
    /// </summary>
    Task<RunListResponse> ListRuns(string threadId, PaginationRequest runListRequest, CancellationToken cancellationToken = default);
}