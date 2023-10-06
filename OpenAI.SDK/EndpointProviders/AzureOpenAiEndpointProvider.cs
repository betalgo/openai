using System.Net;
using OpenAI.ObjectModels.RequestModels;

namespace OpenAI.EndpointProviders;

internal class AzureOpenAiEndpointProvider : IOpenAiEndpointProvider
{
    private const string DeploymentsPrefix = "deployments";
    private const string ApiPrefix = "openai";
    private readonly string _apiVersion;
    private readonly string _deploymentId;


    public AzureOpenAiEndpointProvider(string apiVersion, string deploymentId)
    {
        _apiVersion = apiVersion;
        _deploymentId = deploymentId;
    }

    private string Prefix => $"{ApiPrefix}/{DeploymentsPrefix}/{WebUtility.UrlEncode(_deploymentId)}";
    private string QueryString => $"?api-version={_apiVersion}";


    public string ModelRetrieve(string model)
    {
        return $"{Prefix}/models/{model}{QueryString}";
    }

    public string FileDelete(string fileId)
    {
        return $"{Prefix}/files/{fileId}{QueryString}";
    }

    public string CompletionCreate()
    {
        return $"{Prefix}/completions{QueryString}";
    }

    public string EditCreate()
    {
        return $"{Prefix}/edits{QueryString}";
    }

    public string ModelsList()
    {
        return $"{Prefix}/models{QueryString}";
    }

    public string FilesList()
    {
        return Files();
    }

    public string FilesUpload()
    {
        return Files();
    }

    public string FileRetrieve(string fileId)
    {
        return $"{Prefix}/files/{fileId}{QueryString}";
    }

    public string FileRetrieveContent(string fileId)
    {
        return $"{Prefix}/files/{fileId}/content{QueryString}";
    }

    public string FineTuneCreate()
    {
        return $"{Prefix}/fine-tunes{QueryString}";
    }

    public string FineTuneList()
    {
        return $"{Prefix}/fine-tunes{QueryString}";
    }

    public string FineTuneRetrieve(string fineTuneId)
    {
        return $"{Prefix}/fine-tunes/{fineTuneId}{QueryString}";
    }

    public string FineTuneCancel(string fineTuneId)
    {
        return $"{Prefix}/fine-tunes/{fineTuneId}/cancel{QueryString}";
    }

    public string FineTuneListEvents(string fineTuneId)
    {
        return $"{Prefix}/fine-tunes/{fineTuneId}/events{QueryString}";
    }

    public string FineTuneDelete(string fineTuneId)
    {
        return $"{Prefix}/models/{fineTuneId}{QueryString}";
    }

    public string FineTuningJobCreate()
    {
        return $"{Prefix}/fine_tuning/jobs{QueryString}";
    }

    public string FineTuningJobList(FineTuningJobListRequest? fineTuningJobListRequest)
    {
        var url = $"{Prefix}/fine_tuning/jobs";
        if (fineTuningJobListRequest != null)
        {
            var queryParams = new List<string>();
            if (fineTuningJobListRequest.After != null)
                queryParams.Add($"after={WebUtility.UrlEncode(fineTuningJobListRequest.After)}");
            if (fineTuningJobListRequest.Limit.HasValue)
                queryParams.Add($"limit={fineTuningJobListRequest.Limit.Value}");
        
            if (queryParams.Any())
                url = $"{url}{QueryString}&{string.Join("&", queryParams)}";
        }
        return url;
    }

    public string FineTuningJobList()
    {
        return $"{Prefix}/fine_tuning/jobs{QueryString}";
    }

    public string FineTuningJobRetrieve(string fineTuningJobId)
    {
        return $"{Prefix}/fine_tuning/jobs/{fineTuningJobId}{QueryString}";
    }

    public string FineTuningJobCancel(string fineTuningJobId)
    {
        return $"{Prefix}/fine_tuning/jobs/{fineTuningJobId}/cancel{QueryString}";
    }

    public string FineTuningJobListEvents(string fineTuningJobId)
    {
        return $"{Prefix}/fine_tuning/jobs/{fineTuningJobId}/events{QueryString}";
    }

    public string ModelsDelete(string modelId)
    {
        return $"{Prefix}/models/{modelId}{QueryString}";
    }

    public string EmbeddingCreate()
    {
        return $"{Prefix}/embeddings{QueryString}";
    }

    public string ModerationCreate()
    {
        return $"{Prefix}/moderations{QueryString}";
    }

    public string ImageCreate()
    {
        return $"{Prefix}/images/generations{QueryString}";
    }

    public string ImageEditCreate()
    {
        return $"{Prefix}/images/edits{QueryString}";
    }

    public string ImageVariationCreate()
    {
        return $"{Prefix}/images/variations{QueryString}";
    }

    public string ChatCompletionCreate()
    {
        return $"{Prefix}/chat/completions{QueryString}";
    }

    public string AudioCreateTranscription()
    {
        return $"{Prefix}/audio/transcriptions{QueryString}";
    }

    public string AudioCreateTranslation()
    {
        return $"{Prefix}/audio/translation{QueryString}";
    }

    private string Files()
    {
        return $"{Prefix}/files{QueryString}";
    }
}