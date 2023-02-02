using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OpenAI.GPT3.Extensions
{
    public static class HttpClientExtensions
    {
        public static async Task<T> GetFromJsonAsync<T>(this HttpClient client, string uri, CancellationToken cancellationToken = default)
        {
            var response = await client.GetAsync(uri, cancellationToken);
            return await response.Content.ReadFromJsonAsync<T>(cancellationToken: cancellationToken) ?? throw new InvalidOperationException();
        }

        public static async Task<TResponse> PostAndReadAsAsync<TResponse>(this HttpClient client, string uri, object requestModel, CancellationToken cancellationToken = default)
        {
            var response = await client.PostAsJsonAsync(uri, requestModel, new JsonSerializerOptions()
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault
            }, cancellationToken: cancellationToken);
            return await response.Content.ReadFromJsonAsync<TResponse>(cancellationToken: cancellationToken) ?? throw new InvalidOperationException();
        }

        public static HttpResponseMessage PostAsStreamAsync(this HttpClient client, string uri, object requestModel, CancellationToken cancellationToken = default)
        {
            var settings = new JsonSerializerOptions()
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault
            };

            var content = JsonContent.Create(requestModel, null, settings);

            using var request = new HttpRequestMessage(HttpMethod.Post, uri);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("text/event-stream"));
            request.Content = content;

            return client.Send(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
        }

        public static async Task<TResponse> PostFileAndReadAsAsync<TResponse>(this HttpClient client, string uri, HttpContent content, CancellationToken cancellationToken = default)
        {
            var response = await client.PostAsync(uri, content, cancellationToken);
            return await response.Content.ReadFromJsonAsync<TResponse>(cancellationToken: cancellationToken) ?? throw new InvalidOperationException();
        }

        public static async Task<TResponse> DeleteAndReadAsAsync<TResponse>(this HttpClient client, string uri, CancellationToken cancellationToken = default)
        {
            var response = await client.DeleteAsync(uri, cancellationToken);
            return await response.Content.ReadFromJsonAsync<TResponse>(cancellationToken: cancellationToken) ?? throw new InvalidOperationException();
        }
    }
}