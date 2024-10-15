using System.Runtime.CompilerServices;
using Betalgo.OpenAI.Extensions;
using Betalgo.OpenAI.Interfaces;
using Betalgo.OpenAI.ObjectModels.RequestModels;
using Betalgo.OpenAI.ObjectModels.ResponseModels;
using Betalgo.OpenAI.ObjectModels.SharedModels;

namespace Betalgo.OpenAI.Managers;

public partial class OpenAIService : IRunService
{
    /// <summary>
    ///     Create a run.
    /// </summary>
    /// <param name="threadId"></param>
    /// <param name="request"></param>
    /// <param name="modelId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public async Task<RunResponse> RunCreate(string threadId, RunCreateRequest request, string? modelId = null, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(threadId))
        {
            throw new ArgumentNullException(nameof(threadId));
        }

        request.ProcessModelId(modelId, _defaultModelId, true);
        return await _httpClient.PostAndReadAsAsync<RunResponse>(_endpointProvider.RunCreate(threadId), request, cancellationToken);
    }

    /// <summary>
    /// </summary>
    /// <param name="threadId"></param>
    /// <param name="request"></param>
    /// <param name="modelId"></param>
    /// <param name="justDataMode"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async IAsyncEnumerable<BaseResponse> RunCreateAsStream(string threadId, RunCreateRequest request, string? modelId = null, bool justDataMode = true, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        // Mark the request as streaming
        request.Stream = true;

        // Send the request to the CompletionCreate endpoint
        request.ProcessModelId(modelId, _defaultModelId, true);

        using var response = _httpClient.PostAsStreamAsync(_endpointProvider.RunCreate(threadId), request, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            yield return await response.HandleResponseContent<RunResponse>(cancellationToken);
            yield break;
        }

        await foreach (var baseResponse in response.AsStream(justDataMode: justDataMode,cancellationToken: cancellationToken))
        {
            yield return baseResponse;
        }
    }


    /// <inheritdoc />
    public async Task<RunResponse> RunModify(string threadId, string runId, RunModifyRequest request, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(threadId))
        {
            throw new ArgumentNullException(nameof(threadId));
        }

        if (string.IsNullOrWhiteSpace(runId))
        {
            throw new ArgumentNullException(nameof(runId));
        }

        return await _httpClient.PostAndReadAsAsync<RunResponse>(_endpointProvider.RunModify(threadId, runId), request, cancellationToken);
    }

    /// <summary>
    ///     Retrieves a run.
    /// </summary>
    /// <param name="threadId"></param>
    /// <param name="runId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task<RunResponse> RunRetrieve(string threadId, string runId, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(threadId))
        {
            throw new ArgumentNullException(nameof(threadId));
        }

        if (string.IsNullOrWhiteSpace(runId))
        {
            throw new ArgumentNullException(nameof(runId));
        }

        return await _httpClient.GetReadAsAsync<RunResponse>(_endpointProvider.RunRetrieve(threadId, runId), cancellationToken);
    }

    /// <summary>
    ///     Cancels a run that is in_progress.
    /// </summary>
    /// <param name="threadId"></param>
    /// <param name="runId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public async Task<RunResponse> RunCancel(string threadId, string runId, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(threadId))
        {
            throw new ArgumentNullException(nameof(threadId));
        }

        return await _httpClient.PostAndReadAsAsync<RunResponse>(_endpointProvider.RunCancel(threadId, runId), null, cancellationToken);
    }

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
    /// <exception cref="NotImplementedException"></exception>
    public async Task<RunResponse> RunSubmitToolOutputs(string threadId, string runId, SubmitToolOutputsToRunRequest request, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(threadId))
        {
            throw new ArgumentNullException(nameof(threadId));
        }

        if (string.IsNullOrWhiteSpace(runId))
        {
            throw new ArgumentNullException(nameof(runId));
        }

        return await _httpClient.PostAndReadAsAsync<RunResponse>(_endpointProvider.RunSubmitToolOutputs(threadId, runId), request, cancellationToken);
    }

    public async IAsyncEnumerable<BaseResponse> RunSubmitToolOutputsAsStream(string threadId, string runId, SubmitToolOutputsToRunRequest request, bool justDataMode = true, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        // Mark the request as streaming
        request.Stream = true;

        // Send the request to the CompletionCreate endpoint
        using var response = _httpClient.PostAsStreamAsync(_endpointProvider.RunSubmitToolOutputs(threadId, runId), request, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            yield return await response.HandleResponseContent<BaseResponse>(cancellationToken);
            yield break;
        }

        await foreach (var baseResponse in response.AsStream(justDataMode: justDataMode, cancellationToken: cancellationToken)) yield return baseResponse;
    }

    /// <inheritdoc />
    public async Task<RunResponse> CreateThreadAndRun(CreateThreadAndRunRequest requestBody, CancellationToken cancellationToken = default)
    {
        return await _httpClient.PostAndReadAsAsync<RunResponse>(_endpointProvider.ThreadAndRunCreate(), requestBody, cancellationToken);
    }

    public async IAsyncEnumerable<BaseResponse> CreateThreadAndRunAsStream(CreateThreadAndRunRequest createThreadAndRunRequest, string? modelId = null, bool justDataMode = true,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        // Mark the request as streaming
        createThreadAndRunRequest.Stream = true;

        // Send the request to the CompletionCreate endpoint
        createThreadAndRunRequest.ProcessModelId(modelId, _defaultModelId, true);

        using var response = _httpClient.PostAsStreamAsync(_endpointProvider.ThreadAndRunCreate(), createThreadAndRunRequest, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            yield return await response.HandleResponseContent<BaseResponse>(cancellationToken);
            yield break;
        }

        await foreach (var baseResponse in response.AsStream(justDataMode: justDataMode, cancellationToken: cancellationToken)) yield return baseResponse;
    }

    /// <inheritdoc />
    public async Task<RunListResponse> ListRuns(string threadId, PaginationRequest runListRequest, CancellationToken cancellationToken = default)
    {
        return await _httpClient.GetReadAsAsync<RunListResponse>(_endpointProvider.RunList(threadId, runListRequest), cancellationToken);
    }
}