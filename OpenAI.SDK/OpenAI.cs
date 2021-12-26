using Microsoft.Extensions.Options;
using OpenAI.SDK.Extensions;
using OpenAI.SDK.Interfaces;
using OpenAI.SDK.Models.RequestModels;
using OpenAI.SDK.Models.ResponseModels;
using OpenAI.SDK.Models.ResponseModels.EngineResponseModels;

namespace OpenAI.SDK
{
    //TODO Find a way to show default request values in documentation
    //TODO Move endpoints to a setting file
    public class OpenAISdk : IOpenAISdk, IEngine, ICompletions, ISearches, IClassifications, IAnswers, IFiles
    {
        private readonly IOpenAiEndpointProvider _endpointProvider;
        private readonly HttpClient _httpClient;
        private string? _engineId = null;

        public OpenAISdk(HttpClient httpClient, IOptions<OpenAiSettings> settings)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(settings.Value.BaseDomain);
            var authKey = settings.Value.ApiKey;
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {authKey}");
            var organization = settings.Value.Organization;
            _httpClient.DefaultRequestHeaders.Add("OpenAI-Organization", $"{organization}");

            _endpointProvider = new OpenAiEndpointProvider(settings.Value.ApiVersion);
            _engineId = OpenAiSettings.DefaultEngineId;
        }

        //TODO Not tested yet
        public async Task<CreateAnswerResponse> CreateAnswer(CreateAnswerRequest createAnswerRequest)
        {
            return await _httpClient.PostAndReadAsAsync<CreateAnswerResponse>(_endpointProvider.CreateAnswer(), createAnswerRequest);
        }

        //TODO Not tested yet
        public async Task<CreateClassificationResponse> CreateClassification(CreateClassificationRequest createClassificationRequest)
        {
            return await _httpClient.PostAndReadAsAsync<CreateClassificationResponse>(_endpointProvider.CreateClassification(), createClassificationRequest);
        }


        public async Task<CreateCompletionResponse> CreateCompletion(CreateCompletionRequest createCompletionRequest, string? engineId = null)
        {
            return await _httpClient.PostAndReadAsAsync<CreateCompletionResponse>(_endpointProvider.CreateCompletion(ProcessEngineId(engineId)), createCompletionRequest);
        }

        public async Task<ListEngineResponse> ListEngines()
        {
            return await _httpClient.GetFromJsonAsync<ListEngineResponse>(_endpointProvider.ListEngines());
        }

        public async Task<RetrieveEngineResponse> RetrieveEngine(string? engineId = null)
        {
            return await _httpClient.GetFromJsonAsync<RetrieveEngineResponse>(_endpointProvider.RetrieveEngine(ProcessEngineId(engineId)));
        }

        public async Task<ListFilesResponse> ListFiles()
        {
            return await _httpClient.GetFromJsonAsync<ListFilesResponse>(_endpointProvider.ListFiles());
        }

        public async Task<UploadFilesResponse> UploadFiles(string purpose, byte[] file, string fileName)
        {
            var multipartContent = new MultipartFormDataContent();
            multipartContent.Add(new StringContent(purpose), "purpose");
            multipartContent.Add(new ByteArrayContent(file), "file", fileName);

            return await _httpClient.PostFileAndReadAsAsync<UploadFilesResponse>(_endpointProvider.UploadFiles(), multipartContent);
        }

        public async Task<DeleteResponseModel> DeleteFile(string fileId)
        {
            return await _httpClient.DeleteAndReadAsAsync<DeleteResponseModel>(_endpointProvider.DeleteFile(fileId));
        }

        public async Task<RetrieveFileResponse> RetrieveFile(string fileId)
        {
            return await _httpClient.GetFromJsonAsync<RetrieveFileResponse>(_endpointProvider.RetrieveFile(fileId));
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

        public void SetDefaultEngineId(string engineId)
        {
            _engineId = engineId;
        }

        private string ProcessEngineId(string? engineId)
        {
            return engineId ?? _engineId ?? throw new ArgumentNullException(nameof(engineId));
        }


        //TODO Not tested yet
        public async Task<CreateSearchResponse> CreateSearch(CreateSearchRequest createSearchRequest, string? engineId)
        {
            return await _httpClient.PostAndReadAsAsync<CreateSearchResponse>(_endpointProvider.CreateSearch(ProcessEngineId(engineId)), createSearchRequest);
        }
    }
}