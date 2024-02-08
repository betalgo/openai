using OpenAI.Extensions;
using OpenAI.Interfaces;
using OpenAI.ObjectModels.RequestModels;
using OpenAI.ObjectModels.ResponseModels;
using OpenAI.ObjectModels.SharedModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OpenAI.Managers
{
    public partial class OpenAIService : IAssistantService
    {
        /// <inheritdoc />
        public async Task<AssistantResponse> AssistantCreate(AssistantCreateRequest request, string? modelId = null, CancellationToken cancellationToken = default)
        {
            request.ProcessModelId(modelId, _defaultModelId);
            return await _httpClient.PostAndReadAsAsync<AssistantResponse>(_endpointProvider.AssistantCreate(), request, cancellationToken);
        }

        /// <inheritdoc />
        public async Task<AssistantFileResponse> AssistantFileCreate(string assistantId, AssistantFileCreateRequest request, CancellationToken cancellationToken = default)
        {
            return await _httpClient.PostAndReadAsAsync<AssistantFileResponse>(_endpointProvider.AssistantFileCreate(assistantId), request, cancellationToken);
        }

        /// <inheritdoc />
        public async Task<AssistantListResponse> AssistantList(AssistantListRequest? request = null, CancellationToken cancellationToken = default)
        {
            return await _httpClient.GetReadAsAsync<AssistantListResponse>(_endpointProvider.AssistantList(request), cancellationToken);
        }

        /// <inheritdoc />
        public async Task<AssistantFileListResponse> AssistantFileList(string assistantId, AssistantFileListRequest? request = null, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(assistantId)) { throw new ArgumentNullException(nameof(assistantId)); }

            return await _httpClient.GetReadAsAsync<AssistantFileListResponse>(_endpointProvider.AssistantFileList(assistantId, request), cancellationToken);
        }

        /// <inheritdoc />
        public async Task<AssistantResponse> AssistantRetrieve(string assistantId, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(assistantId)) { throw new ArgumentNullException(nameof(assistantId)); }

            return await _httpClient.GetReadAsAsync<AssistantResponse>(_endpointProvider.AssistantRetrieve(assistantId), cancellationToken);
        }

        /// <inheritdoc />
        public async Task<AssistantFileResponse> AssistantFileRetrieve(string assistantId, string fileId, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(assistantId)) { throw new ArgumentNullException(nameof(assistantId)); }
            if (string.IsNullOrWhiteSpace(fileId)) { throw new ArgumentNullException(nameof(fileId)); }

            return await _httpClient.GetReadAsAsync<AssistantFileResponse>(_endpointProvider.AssistantFileRetrieve(assistantId, fileId), cancellationToken);
        }

        /// <inheritdoc />
        public async Task<AssistantResponse> AssistantModify(string assistantId, AssistantModifyRequest request, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(assistantId)) { throw new ArgumentNullException(nameof(assistantId)); }

            return await _httpClient.PostAndReadAsAsync<AssistantResponse>(_endpointProvider.AssistantModify(assistantId), request, cancellationToken);
        }

        /// <inheritdoc />
        public async Task<DeletionStatusResponse> AssistantDelete(string assistantId, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(assistantId)) { throw new ArgumentNullException(nameof(assistantId)); }

            return await _httpClient.DeleteAndReadAsAsync<DeletionStatusResponse>(_endpointProvider.AssistantDelete(assistantId), cancellationToken);
        }

        /// <inheritdoc />
        public async Task<DeletionStatusResponse> AssistantFileDelete(string assistantId, string fileId, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(assistantId)) { throw new ArgumentNullException(nameof(assistantId)); }
            if (string.IsNullOrWhiteSpace(fileId)) { throw new ArgumentNullException(nameof(fileId)); }

            return await _httpClient.DeleteAndReadAsAsync<DeletionStatusResponse>(_endpointProvider.AssistantFileDelete(assistantId, fileId), cancellationToken);
        }









    }
}
