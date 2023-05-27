using System.Net.Http.Json;
using System.Text.Json;
using OpenAI.GPT3.Extensions;
using OpenAI.GPT3.Interfaces;
using OpenAI.GPT3.ObjectModels.RequestModels;
using OpenAI.GPT3.ObjectModels.ResponseModels.FineTuneResponseModels;
using OpenAI.GPT3.ObjectModels.SharedModels;

namespace OpenAI.GPT3.Managers;

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

    public async Task<FineTuneListEventsResponse> ListFineTuneEvents(string fineTuneId, CancellationToken cancellationToken = default)
    {
        return (await _httpClient.GetFromJsonAsync<FineTuneListEventsResponse>(_endpointProvider.FineTuneListEvents(fineTuneId, false), cancellationToken))!;
    }

    public async IAsyncEnumerable<EventResponse> ListFineTuneEventsStream(string fineTuneId, CancellationToken cancellationToken = default)
    {
        var eventCount = 0;
        var uriPath = _endpointProvider.FineTuneListEvents(fineTuneId, true);
        var done = false;
        while (!done)
        {
            await using var stream = await _httpClient.GetStreamAsync(uriPath, cancellationToken);
            using var streamReader = new StreamReader(stream);
            var currentEventCount = 0;
            while (true)
            {

                var endOfStream = TryEndOfStreamOperation(streamReader);
                if (!endOfStream.HasValue)
                {
                    break;
                }
                if (endOfStream.Value)
                {
                    done = true;
                    break;
                }

                if (currentEventCount < eventCount)
                {
                    await streamReader.ReadLineAsync();
                    currentEventCount++;
                    continue;
                }

                currentEventCount++;
                eventCount++;
                var eventData = await streamReader.ReadLineAsync();

                if (string.IsNullOrEmpty(eventData))
                {
                    continue;
                }
                var startJson = eventData.IndexOf('{');
                var endJson = eventData.LastIndexOf('}');
                if (startJson == -1 || endJson == -1)
                {
                    continue;
                }

                var json = eventData[startJson..(endJson + 1)];
                yield return JsonSerializer.Deserialize<EventResponse>(json)!;
            }
        }
    }

    private bool? TryEndOfStreamOperation(StreamReader streamReader)
    {
        try
        {
            return streamReader.EndOfStream;
        }
        catch (IOException e) when(e.Message.Contains("The response ended prematurely", StringComparison.OrdinalIgnoreCase))
        {
            return null;
        }
    }

    public async Task DeleteFineTune(string fineTuneId, CancellationToken cancellationToken = default)
    {
        await _httpClient.DeleteAsync(_endpointProvider.FineTuneDelete(fineTuneId), cancellationToken);
    }
}