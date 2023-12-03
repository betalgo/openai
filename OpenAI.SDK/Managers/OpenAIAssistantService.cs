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
    public partial class OpenAIService : IAssistantService
    {
        /// <summary>
        /// Create an assistant with a model and instructions.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="modelId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<AssistantResponse> AssistantCreate(AssistantCreateRequest request, string? modelId = null, CancellationToken cancellationToken = default)
        {
            request.ProcessModelId(modelId, _defaultModelId);
            return await _httpClient.PostAndReadAsAsync<AssistantResponse>(_endpointProvider.AssistantCreate(), request, cancellationToken);
        }

    }
}
