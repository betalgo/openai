using OpenAI.Extensions;
using OpenAI.Interfaces;
using OpenAI.ObjectModels.RequestModels;
using OpenAI.ObjectModels.ResponseModels;
using OpenAI.ObjectModels.SharedModels;

namespace OpenAI.Managers;

public partial class OpenAIService : IAssistantService
{
    /// <inheritdoc />
    public async Task<AssistantResponse> AssistantCreate(AssistantCreateRequest request, string? modelId = null, CancellationToken cancellationToken = default)
    {
        request.ProcessModelId(modelId, _defaultModelId);
        return await _httpClient.PostAndReadAsAsync<AssistantResponse>(_endpointProvider.AssistantCreate(), request, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<AssistantListResponse> AssistantList(PaginationRequest? request = null, CancellationToken cancellationToken = default)
    {
        return await _httpClient.GetReadAsAsync<AssistantListResponse>(_endpointProvider.AssistantList(request), cancellationToken);
    }

    /// <inheritdoc />
    public async Task<AssistantResponse> AssistantRetrieve(string assistantId, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(assistantId))
        {
            throw new ArgumentNullException(nameof(assistantId));
        }

        return await _httpClient.GetReadAsAsync<AssistantResponse>(_endpointProvider.AssistantRetrieve(assistantId), cancellationToken);
    }

    /// <inheritdoc />
    public async Task<AssistantResponse> AssistantModify(string assistantId, AssistantModifyRequest request, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(assistantId))
        {
            throw new ArgumentNullException(nameof(assistantId));
        }

        return await _httpClient.PostAndReadAsAsync<AssistantResponse>(_endpointProvider.AssistantModify(assistantId), request, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<DeletionStatusResponse> AssistantDelete(string assistantId, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(assistantId))
        {
            throw new ArgumentNullException(nameof(assistantId));
        }

        return await _httpClient.DeleteAndReadAsAsync<DeletionStatusResponse>(_endpointProvider.AssistantDelete(assistantId), cancellationToken);
    }
}