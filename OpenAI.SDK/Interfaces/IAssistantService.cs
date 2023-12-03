using OpenAI.ObjectModels.RequestModels;
using OpenAI.ObjectModels.SharedModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAI.Interfaces
{
    public interface IAssistantService
    {
        /// <summary>
        /// Create an assistant with a model and instructions.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="modelId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<AssistantResponse> AssistantCreate(AssistantCreateRequest request, string? modelId = null, CancellationToken cancellationToken = default);
    }
}
