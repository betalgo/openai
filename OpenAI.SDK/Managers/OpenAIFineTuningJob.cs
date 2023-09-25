using System.Net;
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

        public async Task<FineTuningJobListResponse> ListFineTuningJobs(FineTuningJobListRequest? fineTuningJobListRequest, CancellationToken cancellationToken = default)
        {

            return (await _httpClient.GetFromJsonAsync<FineTuningJobListResponse>(_endpointProvider.FineTuningJobList(fineTuningJobListRequest), cancellationToken))!;
        }

        public async Task<FineTuningJobResponse> RetrieveFineTuningJob(string fineTuningJobId, CancellationToken cancellationToken = default)
        {
            return (await _httpClient.GetFromJsonAsync<FineTuningJobResponse>(_endpointProvider.FineTuningJobRetrieve(fineTuningJobId), cancellationToken))!;
        }

        public async Task<FineTuningJobResponse> CancelFineTuningJob(string fineTuningJobId, CancellationToken cancellationToken = default)
        {
            return await _httpClient.PostAndReadAsAsync<FineTuningJobResponse>(_endpointProvider.FineTuningJobCancel(fineTuningJobId),null, cancellationToken: cancellationToken);
        }

        public async Task<Stream> ListFineTuningJobEvents(FineTuningJobListEventsRequest fineTuningJobRequestModel, bool? stream = null, CancellationToken cancellationToken = default)
        {
            return await _httpClient.GetStreamAsync(_endpointProvider.FineTuningJobListEvents(fineTuningJobRequestModel.FineTuningJobId), cancellationToken);
        }
    }
}
