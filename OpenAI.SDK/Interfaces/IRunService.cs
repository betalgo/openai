using OpenAI.ObjectModels.RequestModels;
using OpenAI.ObjectModels.SharedModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAI.Interfaces
{
    public interface IRunService
    {
        /// <summary>
        /// Create a run.
        /// </summary>
        /// <param name="threadId"></param>
        /// <param name="request"></param>
        /// <param name="modelId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<RunResponse> RunCreate(string threadId, RunCreateRequest request, string? modelId = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a run.
        /// </summary>
        /// <param name="threadId"></param>
        /// <param name="runId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<RunResponse> RunRetrieve(string threadId, string runId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Cancels a run that is in_progress.
        /// </summary>
        /// <param name="threadId"></param>
        /// <param name="runId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<RunResponse> RunCancel(string threadId, string runId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Submit tool outputs to run
        /// <para>When a run has the status: "requires_action" and required_action.type is submit_tool_outputs, 
        /// this endpoint can be used to submit the outputs from the tool calls once they're all completed. 
        /// All outputs must be submitted in a single request.</para>
        /// </summary>
        /// <param name="threadId"></param>
        /// <param name="runId"></param>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<RunResponse> RunSubmitToolOutputs(string threadId, string runId, SubmitToolOutputsToRunRequest request, CancellationToken cancellationToken = default);
    }
}
