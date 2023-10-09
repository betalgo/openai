using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OpenAI.Extensions;

public static class HttpClientExtensions
{
    public static async Task<TResponse> PostAndReadAsAsync<TResponse>(this HttpClient client, string uri, object? requestModel, CancellationToken cancellationToken = default)
    {
        var response = await client.PostAsJsonAsync(uri, requestModel, new JsonSerializerOptions
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault
        }, cancellationToken);
        return await response.Content.ReadFromJsonAsync<TResponse>(cancellationToken: cancellationToken) ?? throw new InvalidOperationException();
    }

    public static HttpResponseMessage PostAsStreamAsync(this HttpClient client, string uri, object requestModel, CancellationToken cancellationToken = default)
    {
        var settings = new JsonSerializerOptions
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault
        };

        var content = JsonContent.Create(requestModel, null, settings);

        using var request = CreatePostEventStreamRequest(uri, content);

#if NET6_0_OR_GREATER
        try
        {
            return client.Send(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
        }
        catch (PlatformNotSupportedException)
        {
            using var newRequest = CreatePostEventStreamRequest(uri, content);

            return SendRequestPreNet6(client, newRequest, cancellationToken);
        }
#else
        return SendRequestPreNet6(client, request, cancellationToken);
#endif
    }

    private static HttpResponseMessage SendRequestPreNet6(HttpClient client, HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var responseTask = client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
        var response = responseTask.GetAwaiter().GetResult();
        return response;
    }

    private static HttpRequestMessage CreatePostEventStreamRequest(string uri, HttpContent content)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, uri);
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("text/event-stream"));
        request.Content = content;

        return request;
    }

    public static async Task<TResponse> PostFileAndReadAsAsync<TResponse>(this HttpClient client, string uri, HttpContent content, CancellationToken cancellationToken = default)
    {
        var response = await client.PostAsync(uri, content, cancellationToken);
        return await response.Content.ReadFromJsonAsync<TResponse>(cancellationToken: cancellationToken) ?? throw new InvalidOperationException();
    }

    public static async Task<string> PostFileAndReadAsStringAsync(this HttpClient client, string uri, HttpContent content, CancellationToken cancellationToken = default)
    {
        var response = await client.PostAsync(uri, content, cancellationToken);
        return await response.Content.ReadAsStringAsync(cancellationToken) ?? throw new InvalidOperationException();
    }

    public static async Task<TResponse> DeleteAndReadAsAsync<TResponse>(this HttpClient client, string uri, CancellationToken cancellationToken = default)
    {
        var response = await client.DeleteAsync(uri, cancellationToken);
        return await response.Content.ReadFromJsonAsync<TResponse>(cancellationToken: cancellationToken) ?? throw new InvalidOperationException();
    }

#if NETSTANDARD2_0
    public static async Task<string> ReadAsStringAsync(this HttpContent content, CancellationToken cancellationToken)
    {
        var stream = await content.ReadAsStreamAsync().WithCancellation(cancellationToken);
        using var sr = new StreamReader(stream);
        return await sr.ReadToEndAsync().WithCancellation(cancellationToken);
    }

    public static async Task<AsyncDisposableStream> ReadAsStreamAsync(this HttpContent content, CancellationToken cancellationToken)
    {
        var stream = await content.ReadAsStreamAsync().WithCancellation(cancellationToken);
        return new AsyncDisposableStream(stream);
    }

    public static async Task<byte[]> ReadAsByteArrayAsync(this HttpContent content, CancellationToken cancellationToken)
    {
        return await content.ReadAsByteArrayAsync().WithCancellation(cancellationToken);
    }

    public static async Task<Stream> GetStreamAsync(this HttpClient client, string requestUri, CancellationToken cancellationToken)
    {
        var response = await client.GetAsync(requestUri, cancellationToken);
        return await response.Content.ReadAsStreamAsync(cancellationToken);
    }

    public static async Task<T> WithCancellation<T>(this Task<T> task, CancellationToken cancellationToken)
    {
        var tcs = new TaskCompletionSource<bool>();
        using (cancellationToken.Register(s => ((TaskCompletionSource<bool>)s).TrySetResult(true), tcs))
        {
            if (task != await Task.WhenAny(task, tcs.Task))
            {
                throw new OperationCanceledException(cancellationToken);
            }
        }

        return await task;
    }
#endif
}