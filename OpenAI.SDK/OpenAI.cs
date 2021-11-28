using Microsoft.Extensions.Options;
using OpenAI.SDK.Models.RequestModels;
using OpenAI.SDK.Models.ResponseModels;

namespace OpenAI.SDK
{
    //TODO Find a way to show default request values in documentation
    public class OpenAISdk : IOpenAISdk
    {
        private readonly HttpClient _httpClient;

        public OpenAISdk(HttpClient httpClient, IOptions<OpenAiSettings> settings)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://api.openai.com/");
            var authKey = settings.Value.ApiKey;
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {authKey}");
            var organization = settings.Value.Organization;
            _httpClient.DefaultRequestHeaders.Add("OpenAI-Organization", $"{organization}");
        }


        public async Task<ListEngineResponse?> ListEngines()
        {
            return await _httpClient.GetFromJsonAsync<ListEngineResponse>("/v1/engines");
        }

        public async Task<RetrieveEngineResponse?> RetrieveEngine(string engineId)
        {
            return await _httpClient.GetFromJsonAsync<RetrieveEngineResponse>($"/v1/engines/{engineId}");
        }

        public async Task<CreateCompletionResponse?> CreateCompletion(string engineId, CreateCompletionRequest createCompletionRequest)
        {
            return await _httpClient.PostAndReadAsAsync<CreateCompletionResponse?>($"v1/engines/{engineId}/completions", createCompletionRequest);
        }

        public async Task<CreateCompletionResponse?> CreateSearch(string engineId, CreateSearchRequest createSearchRequest)
        {
            return await _httpClient.PostAndReadAsAsync<CreateCompletionResponse?>($"v1/engines/{engineId}/search", createSearchRequest);
        }

        public async Task<CreateClassificationResponse?> CreateClassification(string engineId, CreateClassificationRequest createClassificationRequest)
        {
            return await _httpClient.PostAndReadAsAsync<CreateClassificationResponse?>("v1/classifications", createClassificationRequest);
        }
    }


    public interface IOpenAISdk
    {
        /// <summary>
        ///     Lists the currently available engines, and provides basic information about each one such as the owner and
        ///     availability.
        /// </summary>
        /// <returns></returns>
        Task<ListEngineResponse?> ListEngines();

        /// <summary>
        ///     Retrieves an engine instance, providing basic information about the engine such as the owner and availability.
        /// </summary>
        /// <param name="engineId">The ID of the engine to use for this request</param>
        /// <returns></returns>
        Task<RetrieveEngineResponse?> RetrieveEngine(string engineId);

        /// <summary>
        ///     Creates a new completion for the provided prompt and parameters
        /// </summary>
        /// <param name="engineId">The ID of the engine to use for this request</param>
        /// <param name="createCompletionModel"></param>
        /// <returns></returns>
        Task<CreateCompletionResponse?> CreateCompletion(string engineId, CreateCompletionRequest createCompletionModel);

        /// <summary>
        ///     The search endpoint computes similarity scores between provided query and documents. Documents can be passed
        ///     directly to the API if there are no more than 200 of them.
        ///     To go beyond the 200 document limit, documents can be processed offline and then used for efficient retrieval at
        ///     query time.When file is set, the search endpoint searches over all the documents in the given file and returns up
        ///     to the max_rerank number of documents.These documents will be returned along with their search scores.
        ///     The similarity score is a positive score that usually ranges from 0 to 300 (but can sometimes go higher), where a
        ///     score above 200 usually means the document is semantically similar to the query.
        /// </summary>
        /// <param name="engineId">The ID of the engine to use for this request</param>
        /// <param name="createSearchRequest"></param>
        /// <returns></returns>
        Task<CreateCompletionResponse?> CreateSearch(string engineId, CreateSearchRequest createSearchRequest);

        /// <summary>
        ///     Classifies the specified query using provided examples.
        ///     The endpoint first searches over the labeled examples to select the ones most relevant for the particular
        ///     query.Then, the relevant examples are combined with the query to construct a prompt to produce the final label via
        ///     the completions endpoint.
        ///     Labeled examples can be provided via an uploaded file, or explicitly listed in the request using the examples
        ///     parameter for quick tests and small scale use cases.
        /// </summary>
        /// <param name="engineId">The ID of the engine to use for this request</param>
        /// <param name="createClassificationRequest"></param>
        /// <returns></returns>
        Task<CreateClassificationResponse?> CreateClassification(string engineId, CreateClassificationRequest createClassificationRequest);
    }
}