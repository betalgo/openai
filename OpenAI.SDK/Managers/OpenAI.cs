using Microsoft.Extensions.Options;
using OpenAI.GPT3.Extensions;
using OpenAI.GPT3.Interfaces;
using OpenAI.GPT3.Models.RequestModels;
using OpenAI.GPT3.Models.ResponseModels;

namespace OpenAI.GPT3.Managers
{
    //TODO Find a way to show default request values in documentation
    //TODO Move endpoints to a setting file
    public partial class OpenAISdk : IOpenAISdk, ISearch, IClassification, IAnswer
    {
        private readonly IOpenAiEndpointProvider _endpointProvider;
        private readonly HttpClient _httpClient;
        private string? _engineId;

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
        public async Task<AnswerCreateResponse> Answer(AnswerCreateRequest createAnswerRequest)
        {
            return await _httpClient.PostAndReadAsAsync<AnswerCreateResponse>(_endpointProvider.CreateAnswer(), createAnswerRequest);
        }

        //TODO Not tested yet
        public async Task<ClassificationCreateResponse> ClassificationsCreate(ClassificationCreateRequest createClassificationRequest)
        {
            return await _httpClient.PostAndReadAsAsync<ClassificationCreateResponse>(_endpointProvider.CreateClassification(), createClassificationRequest);
        }


        public IEngine Engine => this;
        public ICompletion Completions => this;
        public ISearch Searches => this;
        public IClassification Classifications => this;
        public IAnswer Answers => this;
        public IFile Files => this;
        public IFineTune FineTunes => this;

        public void SetDefaultEngineId(string engineId)
        {
            _engineId = engineId;
        }


        //TODO Not tested yet
        public async Task<SearchCreateResponse> SearchCreate(SearchCreateRequest createSearchRequest, string? engineId)
        {
            return await _httpClient.PostAndReadAsAsync<SearchCreateResponse>(_endpointProvider.CreateSearch(ProcessEngineId(engineId)), createSearchRequest);
        }

        private string ProcessEngineId(string? engineId)
        {
            return engineId ?? _engineId ?? throw new ArgumentNullException(nameof(engineId));
        }
    }
}