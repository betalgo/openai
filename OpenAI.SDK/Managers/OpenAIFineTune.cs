using System.Net.Http.Json;
using OpenAI.Extensions;
using OpenAI.Interfaces;
using OpenAI.ObjectModels.RequestModels;
using OpenAI.ObjectModels.ResponseModels.FineTuneResponseModels;

namespace OpenAI.Managers;

public partial class OpenAIService : IFineTuneService
{
    public async Task<FineTuneResponse> CreateFineTune(FineTuneCreateRequest createFineTuneRequest, CancellationToken cancellationToken = default)
    {
        return await _httpClient.PostAndReadAsAsync<FineTuneResponse>(_endpointProvider.FineTuneCreate(), createFineTuneRequest, cancellationToken);
    }

    public async Task<FineTuneListResponse> ListFineTunes(CancellationToken cancellationToken = default)
    {
        return (await _httpClient.GetFromJsonAsync<FineTuneListResponse>(_endpointProvider.FineTuneList(), cancellationToken))!;
    }

    public async Task<FineTuneResponse> RetrieveFineTune(string fineTuneId, CancellationToken cancellationToken = default)
    {
        return (await _httpClient.GetFromJsonAsync<FineTuneResponse>(_endpointProvider.FineTuneRetrieve(fineTuneId), cancellationToken))!;
    }

    public async Task<FineTuneResponse> CancelFineTune(string fineTuneId, CancellationToken cancellationToken = default)
    {
        return await _httpClient.PostAndReadAsAsync<FineTuneResponse>(_endpointProvider.FineTuneCancel(fineTuneId), new FineTuneCancelRequest
        {
            FineTuneId = fineTuneId
        }, cancellationToken);
    }

    public async Task<Stream> ListFineTuneEvents(string fineTuneId, bool? stream = null, CancellationToken cancellationToken = default)
    {
        return await _httpClient.GetStreamAsync(_endpointProvider.FineTuneListEvents(fineTuneId), cancellationToken);
        //return await _httpClient.GetFromJsonAsync<ListFineTuneEventsResponse>(_endpointProvider.FineTuneListEvents(fineTuneId));
    }

    public async Task DeleteFineTune(string fineTuneId, CancellationToken cancellationToken = default)
    {
        await _httpClient.DeleteAsync(_endpointProvider.FineTuneDelete(fineTuneId), cancellationToken);
    }
}