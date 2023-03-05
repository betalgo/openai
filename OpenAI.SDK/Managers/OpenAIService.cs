using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OpenAI.GPT3.EndpointProviders;
using OpenAI.GPT3.Interfaces;

namespace OpenAI.GPT3.Managers;

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

        _httpClient = httpClient ?? new HttpClient();
        _httpClient.BaseAddress = new Uri(settings.BaseDomain);

        switch (settings.ProviderType)
        {
            case ProviderType.Azure:
                _httpClient.DefaultRequestHeaders.Add("api-key", settings.ApiKey);
                break;
            case ProviderType.OpenAi:
            default:
                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {settings.ApiKey}");
                break;
        }

        if (!string.IsNullOrEmpty(settings.Organization))
        {
            _httpClient.DefaultRequestHeaders.Add("OpenAI-Organization", $"{settings.Organization}");
        }

        _endpointProvider = settings.ProviderType switch
        {
            ProviderType.Azure => new AzureOpenAiEndpointProvider(settings.ApiVersion, settings.DeploymentId!),
            _ => new OpenAiEndpointProvider(settings.ApiVersion)
        };

        _defaultModelId = settings.DefaultModelId;
    }


    /// <inheritdoc />
    public IModelService Models => this;

    /// <inheritdoc />
    public ICompletionService Completions => this;

    /// <inheritdoc />
    public IEmbeddingService Embeddings => this;

    /// <inheritdoc />
    public IFileService Files => this;

    /// <inheritdoc />
    public IFineTuneService FineTunes => this;

    /// <inheritdoc />
    public IModerationService Moderation => this;

    /// <inheritdoc />
    public IImageService Image => this;

    /// <inheritdoc />
    public IEditService Edit => this;

    /// <inheritdoc />
    public IChatCompletionService ChatCompletion => this;

    /// <inheritdoc />
    public IAudioService Audio => this;

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