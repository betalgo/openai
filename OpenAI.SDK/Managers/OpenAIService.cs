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
        private string? _defaultModelId;

        [ActivatorUtilitiesConstructor]
        public OpenAIService(HttpClient httpClient, IOptions<OpenAiOptions> settings)
            : this(settings.Value, httpClient)
        {
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
            _defaultModelId = OpenAiOptions.DefaultEngineId;
        }


        public IModelService Models => this;
        public ICompletionService Completions => this;
        public IEmbeddingService Embeddings => this;
        public IFileService Files => this;
        public IFineTuneService FineTunes => this;
        public IModerationService Moderation => this;
        public IImageService Image => this;
        public IEditService Edit => this;

        /// <summary>
        ///     Sets default Model Id
        /// </summary>
        /// <param name="modelId"></param>
        public void SetDefaultModelId(string modelId)
        {
            _defaultModelId = modelId;
        }

        /// <summary>
        ///     Get default Model Id
        /// </summary>
        /// <returns></returns>
        public string? GetDefaultModelId()
        {
            return _defaultModelId;
        }
    }
}