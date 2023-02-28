using System.Net;

namespace OpenAI.GPT3.EndpointProviders;

internal class AzureOpenAiEndpointProvider : IOpenAiEndpointProvider
{
    const string DeploymentsPrefix = "deployments";
    const string ApiPrefix = "openai";
    readonly string _apiVersion;
    readonly string _deploymentId;


    public AzureOpenAiEndpointProvider(string apiVersion, string deploymentId)
    {
        _apiVersion = apiVersion;
        _deploymentId = deploymentId;
    }

    string Prefix => $"/{ApiPrefix}/{DeploymentsPrefix}/{WebUtility.UrlEncode(_deploymentId)}";
    string QueryString => $"?api-version={_apiVersion}";


    public string ModelRetrieve(string model) => $"{Prefix}/models/{model}{QueryString}";

    public string FileDelete(string fileId) => $"{Prefix}/files/{fileId}{QueryString}";

    public string CompletionCreate() => $"{Prefix}/completions{QueryString}";

    public string EditCreate() => $"{Prefix}/edits{QueryString}";

    public string ModelsList() => $"{Prefix}/models{QueryString}";

    public string FilesList() => Files();

    public string FilesUpload() => Files();

    public string FileRetrieve(string fileId) => Files();

    public string FineTuneCreate() => $"{Prefix}/fine-tunes{QueryString}";

    public string FineTuneList() => $"{Prefix}/fine-tunes{QueryString}";

    public string FineTuneRetrieve(string fineTuneId) => $"{Prefix}/fine-tunes/{fineTuneId}{QueryString}";

    public string FineTuneCancel(string fineTuneId) => $"{Prefix}/fine-tunes/{fineTuneId}/cancel{QueryString}";

    public string FineTuneListEvents(string fineTuneId) => $"{Prefix}/fine-tunes/{fineTuneId}/events{QueryString}";

    public string FineTuneDelete(string fineTuneId) => $"{Prefix}/models/{fineTuneId}{QueryString}";

    public string EmbeddingCreate() => $"{Prefix}/embeddings{QueryString}";

    public string ModerationCreate() => $"{Prefix}/moderations{QueryString}";

    public string ImageCreate() => $"{Prefix}/images/generations{QueryString}";

    public string ImageEditCreate() => $"{Prefix}/images/edits{QueryString}";

    public string ImageVariationCreate() => $"{Prefix}/images/variations{QueryString}";

    string Files() => $"{Prefix}/files{QueryString}";
}