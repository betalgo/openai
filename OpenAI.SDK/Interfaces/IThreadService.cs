using OpenAI.ObjectModels.RequestModels;
using OpenAI.ObjectModels.SharedModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAI.Interfaces
{
    public interface IThreadService
    {
        /// <summary>
        /// Create a thread.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="modelId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ThreadResponse> ThreadCreate(ThreadCreateRequest? request = null, string? modelId = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a thread.
        /// </summary>
        /// <param name="threadId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ThreadResponse> ThreadRetrieve(string threadId, CancellationToken cancellationToken = default);
    }
}
