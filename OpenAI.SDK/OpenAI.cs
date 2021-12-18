using Microsoft.Extensions.Options;
using OpenAI.SDK.Extensions;
using OpenAI.SDK.Interfaces;
using OpenAI.SDK.Models;
using OpenAI.SDK.Models.RequestModels;
using OpenAI.SDK.Models.ResponseModels;

namespace OpenAI.SDK
{
    //TODO Find a way to show default request values in documentation
    //TODO Move endpoints to a setting file
    public class OpenAISdk : IOpenAISdk, IEngine, ICompletions, ISearches, IClassifications, IAnswers, IFiles
    {
        private const string ApiVersion = "v1";
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

        //TODO Not tested yet
        public async Task<CreateAnswerResponse?> CreateAnswer(CreateAnswerRequest createAnswerRequest)
        {
            return await _httpClient.PostAndReadAsAsync<CreateAnswerResponse?>($"/{ApiVersion}/answers", createAnswerRequest);
        }

        //TODO Not tested yet
        public async Task<CreateClassificationResponse?> CreateClassification(CreateClassificationRequest createClassificationRequest)
        {
            return await _httpClient.PostAndReadAsAsync<CreateClassificationResponse?>($"/{ApiVersion}/classifications", createClassificationRequest);
        }


        public async Task<CreateCompletionResponse?> CreateCompletion(string engineId, CreateCompletionRequest createCompletionRequest)
        {
            return await _httpClient.PostAndReadAsAsync<CreateCompletionResponse?>($"/{ApiVersion}/engines/{engineId}/completions", createCompletionRequest);
        }

        public async Task<ListEngineResponse?> ListEngines()
        {
            return await _httpClient.GetFromJsonAsync<ListEngineResponse>($"/{ApiVersion}/engines");
        }

        public async Task<RetrieveEngineResponse?> RetrieveEngine(string engineId)
        {
            return await _httpClient.GetFromJsonAsync<RetrieveEngineResponse>($"/{ApiVersion}/engines/{engineId}");
        }

        public async Task<ListFilesResponse?> ListFiles()
        {
            return await _httpClient.GetFromJsonAsync<ListFilesResponse>($"/{ApiVersion}/files");
        }

        public async Task<UploadFilesResponse?> UploadFiles(string purpose, byte[] file, string fileName)
        {
            var multipartContent = new MultipartFormDataContent();
            multipartContent.Add(new StringContent(purpose), "purpose");
            multipartContent.Add(new ByteArrayContent(file), "file", fileName);

            return await _httpClient.PostFileAndReadAsAsync<UploadFilesResponse>($"/{ApiVersion}/files", multipartContent);
        }

        public async Task<DeleteResponseModel?> DeleteFile(string fileId)
        {
            return await _httpClient.DeleteAndReadAsAsync<DeleteResponseModel>($"/{ApiVersion}/files/{fileId}");
        }

        public async Task<RetrieveFileResponse?> RetrieveFile(string fileId)
        {
            return await _httpClient.GetFromJsonAsync<RetrieveFileResponse>($"/{ApiVersion}/files/{fileId}");
        }

        //TODO Not tested yet
        //TODO check if there undocumented response object
        // I couldn't figure out how this endpoint works..
        public async Task RetrieveFileContent(string fileId)
        {
            throw new NotImplementedException();
            //await _httpClient.GetFromJsonAsync<RetrieveFileResponse>($"/{ApiVersion}/files/{fileId}/content");
        }

        public IEngine Engine => this;
        public ICompletions Completions => this;
        public ISearches Searches => this;
        public IClassifications Classifications => this;
        public IAnswers Answers => this;
        public IFiles Files => this;

        //TODO Not tested yet
        public async Task<CreateCompletionResponse?> CreateSearch(string engineId, CreateSearchRequest createSearchRequest)
        {
            return await _httpClient.PostAndReadAsAsync<CreateCompletionResponse?>($"/{ApiVersion}/engines/{engineId}/search", createSearchRequest);
        }
        
    }
}