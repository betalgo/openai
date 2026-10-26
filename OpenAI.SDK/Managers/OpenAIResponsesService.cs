using System.Runtime.CompilerServices;
using System.Text.Json;
using Betalgo.Ranul.OpenAI.Contracts.Types.Streaming.Events.Lifecycle;
using Betalgo.Ranul.OpenAI.Contracts.Requests.Responses;
using Betalgo.Ranul.OpenAI.Contracts.Responses.Responses;
using Betalgo.Ranul.OpenAI.Contracts.Types.Streaming;
using Betalgo.Ranul.OpenAI.Extensions;
using Betalgo.Ranul.OpenAI.Extensions.Streaming;
using Betalgo.Ranul.OpenAI.ExtensionsV2;
using Betalgo.Ranul.OpenAI.Interfaces;

namespace Betalgo.Ranul.OpenAI.Managers;

public partial class OpenAIService : IResponsesService, IResponsesProvider
{
    /// <inheritdoc />
    public async Task<Response> CreateResponse(CreateResponse createResponse, CancellationToken cancellationToken = default)
    {
        return await CreateResponse(createResponse, modelId: null, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<Response> CreateResponse(CreateResponse createResponse, string? modelId, CancellationToken cancellationToken = default)
    {
        createResponse.ProcessModelId(modelId, _defaultModelId);
        return await _httpClient.PostAndReadAsAsync<Response>(_endpointProvider.ResponsesCreate(), createResponse, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<Response> RetrieveResponse(string responseId, RetrieveResponseRequest? request = null, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(responseId))
        {
            throw new ArgumentNullException(nameof(responseId));
        }

        return await _httpClient.GetReadAsAsync<Response>(_endpointProvider.ResponsesRetrieve(responseId, request?.GetQueryParameters()), cancellationToken);
    }

    /// <inheritdoc />
    public async Task<DeletedResponse> DeleteResponse(string responseId, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(responseId))
        {
            throw new ArgumentNullException(nameof(responseId));
        }

        return await _httpClient.DeleteAndReadAsAsync<DeletedResponse>(_endpointProvider.ResponsesDelete(responseId), cancellationToken);
    }

    /// <inheritdoc />
    public async Task<Response> CancelResponse(string responseId, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(responseId))
        {
            throw new ArgumentNullException(nameof(responseId));
        }

        return await _httpClient.PostAndReadAsAsync<Response>(_endpointProvider.ResponsesCancel(responseId), null, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<ResponseItemList> ListInputItems(string responseId, InputItemsListRequest? request = null, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(responseId))
        {
            throw new ArgumentNullException(nameof(responseId));
        }

        return await _httpClient.GetReadAsAsync<ResponseItemList>(_endpointProvider.ResponsesInputItemsList(responseId, request?.GetQueryParameters()), cancellationToken);
    }

    /// <inheritdoc />
    public async Task<TokenCountsResponse> CountInputTokens(TokenCountsRequest request, CancellationToken cancellationToken = default)
    {
        return await CountInputTokens(request, modelId: null, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<TokenCountsResponse> CountInputTokens(TokenCountsRequest request, string? modelId, CancellationToken cancellationToken = default)
    {
        request.ProcessModelId(modelId, _defaultModelId);
        return await _httpClient.PostAndReadAsAsync<TokenCountsResponse>(_endpointProvider.ResponsesInputTokensCount(), request, cancellationToken);
    }

    /// <inheritdoc />
    public async IAsyncEnumerable<IResponseStreamEvent> CreateAsStreamAsync(CreateResponse createResponse, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        await foreach (var evt in CreateAsStreamAsync(createResponse, modelId: null, cancellationToken))
        {
            yield return evt;
        }
    }

    /// <inheritdoc />
    public async IAsyncEnumerable<IResponseStreamEvent> CreateAsStreamAsync(CreateResponse createResponse, string? modelId, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        // Ensure streaming is enabled
        createResponse.Stream = true;
        createResponse.ProcessModelId(modelId, _defaultModelId);

        using var response = await _httpClient.PostAndGetStreamAsync(_endpointProvider.ResponsesCreate(), createResponse, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
#if NET6_0_OR_GREATER
            var errorContent = await response.Content.ReadAsStringAsync(cancellationToken);
#else
            var errorContent = await response.Content.ReadAsStringAsync();
#endif
            throw new HttpRequestException($"Request failed with status code {response.StatusCode}: {errorContent}");
        }

        await foreach (var evt in ReadResponsesStreamAsync(response, requireTerminalEvent: true, cancellationToken))
        {
            yield return evt;
        }
    }

    /// <inheritdoc />
    public async IAsyncEnumerable<IResponseStreamEvent> RetrieveAsStreamAsync(string responseId, RetrieveResponseRequest? request = null, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(responseId))
        {
            throw new ArgumentNullException(nameof(responseId));
        }

        // Build query parameters with a single canonical stream=true entry.
        var existingParams = request?.GetQueryParameters(includeStream: false);
        var queryParams = string.IsNullOrEmpty(existingParams)
            ? "stream=true"
            : $"{existingParams}&stream=true";

        using var response = await _httpClient.GetAndGetStreamAsync(_endpointProvider.ResponsesRetrieve(responseId, queryParams), cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
#if NET6_0_OR_GREATER
            var errorContent = await response.Content.ReadAsStringAsync(cancellationToken);
#else
            var errorContent = await response.Content.ReadAsStringAsync();
#endif
            throw new HttpRequestException($"Request failed with status code {response.StatusCode}: {errorContent}");
        }

        await foreach (var evt in ReadResponsesStreamAsync(response, requireTerminalEvent: request?.StartingAfter == null, cancellationToken))
        {
            yield return evt;
        }
    }
    private static async IAsyncEnumerable<IResponseStreamEvent> ReadResponsesStreamAsync(
        HttpResponseMessage response, bool requireTerminalEvent,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var receivedTerminalEvent = false;
        await foreach (var evt in response.ParseSseStreamAsync<IResponseStreamEvent>(cancellationToken: cancellationToken))
        {
            switch (evt)
            {
                case ResponseCompletedEvent completed when completed.Response == null:
                case ResponseFailedEvent failed when failed.Response == null:
                case ResponseIncompleteEvent incomplete when incomplete.Response == null:
                    throw new JsonException("A terminal response event must contain its response object.");
                case ResponseCompletedEvent:
                case ResponseFailedEvent:
                case ResponseIncompleteEvent:
                case ResponseErrorEvent:
                    receivedTerminalEvent = true;
                    break;
            }
            yield return evt;
        }

        if (requireTerminalEvent && !receivedTerminalEvent)
        {
            throw new EndOfStreamException("The response stream ended before a terminal response event was received.");
        }
    }

}

