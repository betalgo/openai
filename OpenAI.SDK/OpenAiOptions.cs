namespace OpenAI.GPT3
{
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
        private const string AzureOpenAiDefaultApiVersion = "2022-12-01";


        /// <summary>
        ///     Setting key for Json Setting Bindings
        /// </summary>
        public static readonly string SettingKey = "OpenAIServiceOptions";

        private string? _apiVersion;
        private string? _baseDomain;
        private ProviderType? _providerType;

        /// <summary>
        ///     Get Provider Type
        /// </summary>
        public ProviderType ProviderType
        {
            get
            {
                _providerType ??= !string.IsNullOrEmpty(DeploymentId) || !string.IsNullOrEmpty(ResourceName) ? ProviderType.Azure : ProviderType.OpenAi;
                return _providerType.Value;
            }
            set => _providerType = value;
        }

        /// <summary>
        ///     For users who belong to multiple organizations, you can pass a header to specify which organization is used for an
        ///     API request. Usage from these API requests will count against the specified organization's subscription quota.
        ///     Organization IDs can be found on your
        ///     <a href="https://beta.openai.com/account/org-settings">Organization settings</a> page.
        /// </summary>
        public string? Organization { get; set; }

        /// <summary>
        ///     The OpenAI API uses API keys for authentication. Visit your
        ///     <a href="https://beta.openai.com/account/api-keys">API Keys page</a> to retrieve the API key you'll use in
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
                    _ => throw new ArgumentOutOfRangeException()
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
                return _baseDomain ??= ProviderType switch
                {
                    ProviderType.OpenAi => OpenAiDefaultBaseDomain,
                    ProviderType.Azure => $"https://{ResourceName}.openai.azure.com/",
                    _ => throw new ArgumentOutOfRangeException()
                };
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
        ///     Default engine id. If you are working with only one engine, this will save you from few line extra code.
        /// </summary>
        public static string? DefaultEngineId { get; set; }

        /// <summary>
        ///     Create an instance of this class with the necessary information to connect to the azure open ai api
        /// </summary>
        /// <param name="resourceName">Resource Name of your Azure OpenAI resource</param>
        /// <param name="deploymentId">The id of your deployment of OpenAI</param>
        /// <param name="apiVersion">The azure open ai api version</param>
        /// <param name="apiKey">Token used for authentication</param>
        /// <returns>A valid OpenAiSettings instance configured with the method inputs</returns>
        private static OpenAiOptions CreateAzureSettings(string apiKey, string resourceName, string deploymentId, string? apiVersion)
        {
            return new OpenAiOptions
            {
                _providerType = ProviderType.Azure,
                ResourceName = resourceName,
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
                throw new ArgumentNullException(nameof(BaseDomain));
            }

            if (ProviderType == ProviderType.Azure)
            {
                if (string.IsNullOrEmpty(DeploymentId))
                {
                    throw new ArgumentNullException(nameof(DeploymentId));
                }

                if (string.IsNullOrEmpty(ResourceName))
                {
                    throw new ArgumentNullException(nameof(ResourceName));
                }
            }
        }
    }
}