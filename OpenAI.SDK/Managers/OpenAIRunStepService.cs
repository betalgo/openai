using Betalgo.Ranul.OpenAI.Extensions;
using Betalgo.Ranul.OpenAI.Interfaces;
using Betalgo.Ranul.OpenAI.ObjectModels.RequestModels;
using Betalgo.Ranul.OpenAI.ObjectModels.ResponseModels;

namespace Betalgo.Ranul.OpenAI.Managers;

public partial class OpenAIService : IRunStepService
{
    /// <inheritdoc />
    public async Task<RunStepListResponse> RunStepsList(string threadId, string runId, PaginationRequest? request = null, CancellationToken cancellationToken = default)
    {
        return await _httpClient.GetReadAsAsync<RunStepListResponse>(_endpointProvider.RunStepList(threadId, runId, request), cancellationToken);
    }

    /// <inheritdoc />
    public async Task<RunStepResponse> RunStepRetrieve(string threadId, string runId, string stepId, CancellationToken cancellationToken = default)
    {
        return await _httpClient.GetReadAsAsync<RunStepResponse>(_endpointProvider.RunStepRetrieve(threadId, runId, stepId), cancellationToken);
    }
}