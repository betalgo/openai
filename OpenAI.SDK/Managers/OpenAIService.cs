using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OpenAI.GPT3.Interfaces;

namespace OpenAI.GPT3.Managers
{
    //TODO Find a way to show default request values in documentation
    public partial class OpenAIService : IOpenAIService
    {
        private readonly IOpenAiEndpointProvider _endpointProvider;
        private readonly HttpClient _httpClient;
        private string? _engineId;

        [ActivatorUtilitiesConstructor]
        public OpenAIService(HttpClient httpClient, IOptions<OpenAiOptions> settings)
        {
            settings.Value.Validate();

            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(settings.Value.BaseDomain);
            var authKey = settings.Value.ApiKey;
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {authKey}");
            var organization = settings.Value.Organization;
            if (!string.IsNullOrEmpty(organization))
            {
                _httpClient.DefaultRequestHeaders.Add("OpenAI-Organization", $"{organization}");
            }

            _endpointProvider = new OpenAiEndpointProvider(settings.Value.ApiVersion);
            _engineId = OpenAiOptions.DefaultEngineId;
        }

        public OpenAIService(OpenAiOptions settings, HttpClient? httpClient = null)
        {
            settings.Validate();

            _httpClient = httpClient ?? HttpClientFactory.Create();
            _httpClient.BaseAddress = new Uri(settings.BaseDomain);
            var authKey = settings.ApiKey;
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {authKey}");
            var organization = settings.Organization;
            if (!string.IsNullOrEmpty(organization))
            {
                _httpClient.DefaultRequestHeaders.Add("OpenAI-Organization", $"{organization}");
            }

            _endpointProvider = new OpenAiEndpointProvider(settings.ApiVersion);
            _engineId = OpenAiOptions.DefaultEngineId;
        }


        public IModelService Models => this;
        public ICompletionService Completions => this;
        public IEmbeddingService Embeddings => this;
        public IFileService Files => this;
        public IFineTuneService FineTunes => this;
        public IModerationService Moderation => this;

        public void SetDefaultEngineId(string engineId)
        {
            _engineId = engineId;
        }

        private string ProcessEngineId(string? engineId)
        {
            return engineId ?? _engineId ?? throw new ArgumentNullException(nameof(engineId));
        }
    }
}