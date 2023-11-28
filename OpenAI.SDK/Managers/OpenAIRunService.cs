using OpenAI.Extensions;
using OpenAI.Interfaces;
using OpenAI.ObjectModels.RequestModels;
using OpenAI.ObjectModels.SharedModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAI.Managers
{
    public partial class OpenAIService : IRunService
    {
        /// <summary>
        /// Create a run.
        /// </summary>
        /// <param name="threadId"></param>
        /// <param name="request"></param>
        /// <param name="modelId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<RunResponse> RunCreate(string threadId, RunCreateRequest request, string? modelId = null, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(threadId)) { throw new ArgumentNullException(nameof(threadId)); }

            //request.ProcessModelId(modelId, _defaultModelId);
            return await _httpClient.PostAndReadAsAsync<RunResponse>(_endpointProvider.RunCreate(threadId), request, cancellationToken);
        }

        /// <summary>
        ///  Retrieves a run.
        /// </summary>
        /// <param name="threadId"></param>
        /// <param name="runId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<RunResponse> RunRetrieve(string threadId, string runId, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(threadId)) { throw new ArgumentNullException(nameof(threadId)); }
            if (string.IsNullOrWhiteSpace(runId)) { throw new ArgumentNullException(nameof(runId)); }

            return await _httpClient.GetReadAsAsync<RunResponse>(_endpointProvider.RunRetrieve(threadId, runId), cancellationToken);
        }

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
        /// <exception cref="NotImplementedException"></exception>
        public async Task<RunResponse> RunSubmitToolOutputs(string threadId, string runId, SubmitToolOutputsToRunRequest request, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(threadId)) { throw new ArgumentNullException(nameof(threadId)); }
            if (string.IsNullOrWhiteSpace(runId)) { throw new ArgumentNullException(nameof(runId)); }

            return await _httpClient.PostAndReadAsAsync<RunResponse>(_endpointProvider.RunSubmitToolOutputs(threadId, runId), request, cancellationToken);
        }
    }
}
