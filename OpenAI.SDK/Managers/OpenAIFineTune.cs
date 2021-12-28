using OpenAI.SDK.Extensions;
using OpenAI.SDK.Interfaces;
using OpenAI.SDK.Models.RequestModels;
using OpenAI.SDK.Models.ResponseModels;
using OpenAI.SDK.Models.ResponseModels.FineTuneResponseModels;

namespace OpenAI.SDK.Managers;

public partial class OpenAISdk : IFineTune
{
    public async Task<FineTuneResponse> CreateFineTune(FineTuneCreateRequest createFineTuneRequest)
    {
        return await _httpClient.PostAndReadAsAsync<FineTuneResponse>(_endpointProvider.FineTuneCreate(), createFineTuneRequest);
    }

    public async Task<FineTuneListResponse> ListFineTunes()
    {
        return await _httpClient.GetFromJsonAsync<FineTuneListResponse>(_endpointProvider.FineTuneList());
    }

    public async Task<FineTuneResponse> RetrieveFineTune(string fineTuneId)
    {
        return await _httpClient.GetFromJsonAsync<FineTuneResponse>(_endpointProvider.FineTuneRetrieve(fineTuneId));
    }

    public async Task<FineTuneResponse> CancelFineTune(string fineTuneId)
    {
        return await _httpClient.PostAndReadAsAsync<FineTuneResponse>(_endpointProvider.FineTuneCancel(fineTuneId), new FineTuneCancelRequest
        {
            FineTuneId = fineTuneId
        });
    }

    public async Task<Stream> ListFineTuneEvents(string fineTuneId, bool? stream = null)
    {
        return await _httpClient.GetStreamAsync(_endpointProvider.FineTuneListEvents(fineTuneId));
        //return await _httpClient.GetFromJsonAsync<ListFineTuneEventsResponse>(_endpointProvider.FineTuneListEvents(fineTuneId));
    }

    public async Task<CompletionCreateResponse> FineTuneCompletions(FineTuneCompletionsRequest fineTuneCompletionsRequest)
    {
        return await _httpClient.PostAndReadAsAsync<CompletionCreateResponse>(_endpointProvider.FineTuneCompletions(), fineTuneCompletionsRequest);
    }

    public async Task DeleteFineTune(string fineTuneId)
    {
        await _httpClient.DeleteAsync(_endpointProvider.FineTuneDelete(fineTuneId));
    }
}