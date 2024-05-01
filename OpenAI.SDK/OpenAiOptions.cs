namespace OpenAI;

/// <summary>
///     Provider Type
/// </summary>
public enum ProviderType
{
    /// <summary>
    ///     OpenAi Provider
    /// </summary>
    OpenAi = 1,

    /// <summary>
    ///     Azure Provider
    /// </summary>
    Azure = 2
}

public class OpenAiOptions
{
    private const string OpenAiDefaultApiVersion = "v1";
    private const string OpenAiDefaultBaseDomain = "https://api.openai.com/";
    private const string AzureOpenAiDefaultApiVersion = "2023-12-01-preview";
    private const string OpenAiDefaultAssistantsApiVersion = "v2";

    /// <summary>
    ///     Setting key for Json Setting Bindings
    /// </summary>
    public static readonly string SettingKey = "OpenAIServiceOptions";


    private string? _apiVersion;
    private string? _baseDomain;

    /// <summary>
    ///     Get Provider Type
    /// </summary>
    public ProviderType ProviderType { get; set; } = ProviderType.OpenAi;

    /// <summary>
    /// Calls to the Assistants API require that you pass a beta HTTP header. 
    /// This is handled automatically if you’re using OpenAI’s official Python or Node.js SDKs.
    ///  <a href="https://platform.openai.com/docs/assistants/overview">assistants overview</a> page.
    /// </summary>
    public string? Assistants => $"assistants={OpenAiDefaultAssistantsApiVersion}";
    /// <summary>
    ///     For users who belong to multiple organizations, you can pass a header to specify which organization is used for an
    ///     API request. Usage from these API requests will count against the specified organization's subscription quota.
    ///     Organization IDs can be found on your
    ///     <a href="https://platform.openai.com/account/org-settings">Organization settings</a> page.
    /// </summary>
    public string? Organization { get; set; }

    /// <summary>
    ///     The OpenAI API uses API keys for authentication. Visit your
    ///     <a href="https://platform.openai.com/account/api-keys">API Keys page</a> to retrieve the API key you'll use in
    ///     your requests.
    ///     Remember that your API key is a secret! Do not share it with others or expose it in any client-side code(browsers,
    ///     apps). Production requests must be routed through your own backend server where your API key can be securely loaded
    ///     from an environment variable or key management service.
    /// </summary>
    public string ApiKey { get; set; } = null!;

    /// <summary>
    ///     Default Api Version
    /// </summary>
    public string ApiVersion
    {
        get
        {
            return _apiVersion ??= ProviderType switch
            {
                ProviderType.OpenAi => OpenAiDefaultApiVersion,
                ProviderType.Azure => AzureOpenAiDefaultApiVersion,
                _ => throw new ArgumentOutOfRangeException(nameof(ProviderType))
            };
        }
        set => _apiVersion = value;
    }

    /// <summary>
    ///     Base Domain
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public string BaseDomain
    {
        get
        {
            //we can ignore possible null value until resourceName is set
#pragma warning disable CS8603
            return _baseDomain ??= ProviderType switch
            {
                ProviderType.OpenAi => OpenAiDefaultBaseDomain,
                ProviderType.Azure => ResourceName == null ? null : $"https://{ResourceName}.openai.azure.com/",
                _ => throw new ArgumentOutOfRangeException(nameof(ProviderType))
            };
#pragma warning restore CS8603
        }
        set => _baseDomain = value;
    }

    /// <summary>
    ///     Azure Deployment Id
    /// </summary>
    public string? DeploymentId { get; set; }

    /// <summary>
    ///     Azure Resource Name
    /// </summary>
    public string? ResourceName { get; set; }

    /// <summary>
    ///     Default model id. If you are working with only one model, this will save you from few line extra code.
    /// </summary>
    [Obsolete("Use Default Model Id")]
    public string? DefaultEngineId
    {
        get => DefaultModelId;
        set => DefaultModelId = value;
    }

    public bool ValidateApiOptions { get; set; } = true;

    /// <summary>
    ///     Default model id. If you are working with only one model, this will save you from few line extra code.
    /// </summary>
    public string? DefaultModelId { get; set; }

    public bool UseBeta { get; set; } = false;

    /// <summary>
    ///     Create an instance of this class with the necessary information to connect to the azure open ai api
    /// </summary>
    /// <param name="resourceName">Resource Name of your Azure OpenAI resource</param>
    /// <param name="deploymentId">The id of your deployment of OpenAI</param>
    /// <param name="apiVersion">The azure open ai api version</param>
    /// <param name="apiKey">Token used for authentication</param>
    /// <returns>A valid OpenAiSettings instance configured with the method inputs</returns>
    private static OpenAiOptions CreateAzureSettings(string apiKey, string deploymentId, string resourceName, string? apiVersion)
    {
        return new OpenAiOptions
        {
            ProviderType = ProviderType.Azure,
            ResourceName = resourceName,
            DeploymentId = deploymentId,
            ApiKey = apiKey,
            ApiVersion = apiVersion ?? AzureOpenAiDefaultApiVersion
        };
    }

    /// <summary>
    ///     Create an instance of this class with the necessary information to connect to the azure open ai api
    /// </summary>
    /// <param name="deploymentId">The id of your deployment of OpenAI</param>
    /// <param name="baseDomain">Base Domain of your Azure OpenAI service</param>
    /// <param name="apiVersion">The azure open ai api version</param>
    /// <param name="apiKey">Token used for authentication</param>
    /// <returns>A valid OpenAiSettings instance configured with the method inputs</returns>
    private static OpenAiOptions CreateAzureSettingsWithBaseDomain(string apiKey, string deploymentId, string baseDomain, string? apiVersion)
    {
        return new OpenAiOptions
        {
            ProviderType = ProviderType.Azure,
            BaseDomain = baseDomain,
            DeploymentId = deploymentId,
            ApiKey = apiKey,
            ApiVersion = apiVersion ?? AzureOpenAiDefaultApiVersion
        };
    }

    /// <summary>
    ///     Validate Settings
    /// </summary>
    /// <exception cref="ArgumentNullException"></exception>
    public void Validate()
    {
        if (!ValidateApiOptions)
        {
            return;
        }

        if (string.IsNullOrEmpty(ApiKey))
        {
            throw new ArgumentNullException(nameof(ApiKey));
        }

        if (string.IsNullOrEmpty(ApiVersion))
        {
            throw new ArgumentNullException(nameof(ApiVersion));
        }

        if (string.IsNullOrEmpty(BaseDomain))
        {
            if (ProviderType == ProviderType.Azure && string.IsNullOrEmpty(ResourceName))
            {
                throw new ArgumentNullException(nameof(ResourceName));
            }

            throw new ArgumentNullException(nameof(BaseDomain));
        }

        if (ProviderType == ProviderType.Azure)
        {
            if (string.IsNullOrEmpty(DeploymentId))
            {
                throw new ArgumentNullException(nameof(DeploymentId));
            }

            if (BaseDomain.Equals("https://.openai.azure.com/"))
            {
                throw new ArgumentNullException(nameof(ResourceName));
            }
        }
        else if (ProviderType == ProviderType.OpenAi)
        {
            if (!string.IsNullOrEmpty(DeploymentId))
            {
                throw new ArgumentException(nameof(DeploymentId) + " is not supported for OpenAi Provider. Set ProviderType to Azure or remove " + nameof(DeploymentId));
            }

            if (!string.IsNullOrEmpty(ResourceName))
            {
                throw new ArgumentException(nameof(ResourceName) + " is not supported for OpenAi Provider. Set ProviderType to Azure or remove " + nameof(ResourceName));
            }
        }
    }
}