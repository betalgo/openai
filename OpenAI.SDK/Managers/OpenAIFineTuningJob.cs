using OpenAI.Extensions;
using OpenAI.Interfaces;
using OpenAI.ObjectModels.RequestModels;
using OpenAI.ObjectModels.ResponseModels.FineTuningJobResponseModels;
using System.Net.Http.Json;

namespace OpenAI.Managers
{
    public partial class OpenAIService : IFineTuningJobService
    {
        public async Task<FineTuningJobResponse> CreateFineTuningJob(FineTuningJobCreateRequest createFineTuningJobRequest, CancellationToken cancellationToken = default)
        {
            return await _httpClient.PostAndReadAsAsync<FineTuningJobResponse>(_endpointProvider.FineTuningJobCreate(), createFineTuningJobRequest, cancellationToken);
        }

        public async Task<FineTuningJobListResponse> ListFineTuningJobs(CancellationToken cancellationToken = default)
        {
            return (await _httpClient.GetFromJsonAsync<FineTuningJobListResponse>(_endpointProvider.FineTuningJobList(), cancellationToken))!;
        }

        public async Task<FineTuningJobResponse> RetrieveFineTuningJob(string FineTuningJobId, CancellationToken cancellationToken = default)
        {
            return (await _httpClient.GetFromJsonAsync<FineTuningJobResponse>(_endpointProvider.FineTuningJobRetrieve(FineTuningJobId), cancellationToken))!;
        }

        public async Task<FineTuningJobResponse> CancelFineTuningJob(string FineTuningJobId, CancellationToken cancellationToken = default)
        {
            return await _httpClient.PostAndReadAsAsync<FineTuningJobResponse>(_endpointProvider.FineTuningJobCancel(FineTuningJobId), new FineTuningJobCancelRequest
            {
                FineTuningJobId = FineTuningJobId
            }, cancellationToken);
        }

        public async Task<Stream> ListFineTuningJobEvents(string FineTuningJobId, bool? stream = null, CancellationToken cancellationToken = default)
        {
            return await _httpClient.GetStreamAsync(_endpointProvider.FineTuningJobListEvents(FineTuningJobId), cancellationToken);
        }

        public async Task DeleteFineTuningJob(string FineTuningJobId, CancellationToken cancellationToken = default)
        {
            await _httpClient.DeleteAsync(_endpointProvider.FineTuningJobDelete(FineTuningJobId), cancellationToken);
        }
    }
}
