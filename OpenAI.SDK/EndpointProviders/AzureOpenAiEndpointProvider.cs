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
    private string AzureVersionQueryString => $"?api-version={_apiVersion}";

    public string ModelRetrieve(string model)
    {
        return $"{Prefix}/models/{model}{AzureVersionQueryString}";
    }

    public string FileDelete(string fileId)
    {
        return $"{Prefix}/files/{fileId}{AzureVersionQueryString}";
    }

    public string CompletionCreate()
    {
        return $"{Prefix}/completions{AzureVersionQueryString}";
    }

    public string EditCreate()
    {
        return $"{Prefix}/edits{AzureVersionQueryString}";
    }

    public string ModelsList()
    {
        return $"{Prefix}/models{AzureVersionQueryString}";
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
        return $"{Prefix}/files/{fileId}{AzureVersionQueryString}";
    }

    public string FileRetrieveContent(string fileId)
    {
        return $"{Prefix}/files/{fileId}/content{AzureVersionQueryString}";
    }

    public string FineTuneCreate()
    {
        return $"{Prefix}/fine-tunes{AzureVersionQueryString}";
    }

    public string FineTuneList()
    {
        return $"{Prefix}/fine-tunes{AzureVersionQueryString}";
    }

    public string FineTuneRetrieve(string fineTuneId)
    {
        return $"{Prefix}/fine-tunes/{fineTuneId}{AzureVersionQueryString}";
    }

    public string FineTuneCancel(string fineTuneId)
    {
        return $"{Prefix}/fine-tunes/{fineTuneId}/cancel{AzureVersionQueryString}";
    }

    public string FineTuneListEvents(string fineTuneId)
    {
        return $"{Prefix}/fine-tunes/{fineTuneId}/events{AzureVersionQueryString}";
    }

    public string FineTuneDelete(string fineTuneId)
    {
        return $"{Prefix}/models/{fineTuneId}{AzureVersionQueryString}";
    }

    public string FineTuningJobCreate()
    {
        return $"{Prefix}/fine_tuning/jobs{AzureVersionQueryString}";
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
                url = $"{url}{AzureVersionQueryString}&{string.Join("&", queryParams)}";
        }

        return url;
    }

    public string FineTuningJobRetrieve(string fineTuningJobId)
    {
        return $"{Prefix}/fine_tuning/jobs/{fineTuningJobId}{AzureVersionQueryString}";
    }

    public string FineTuningJobCancel(string fineTuningJobId)
    {
        return $"{Prefix}/fine_tuning/jobs/{fineTuningJobId}/cancel{AzureVersionQueryString}";
    }

    public string FineTuningJobListEvents(string fineTuningJobId)
    {
        return $"{Prefix}/fine_tuning/jobs/{fineTuningJobId}/events{AzureVersionQueryString}";
    }

    public string ModelsDelete(string modelId)
    {
        return $"{Prefix}/models/{modelId}{AzureVersionQueryString}";
    }

    public string EmbeddingCreate()
    {
        return $"{Prefix}/embeddings{AzureVersionQueryString}";
    }

    public string ModerationCreate()
    {
        return $"{Prefix}/moderations{AzureVersionQueryString}";
    }

    public string ImageCreate()
    {
        return $"{Prefix}/images/generations{AzureVersionQueryString}";
    }

    public string ImageEditCreate()
    {
        return $"{Prefix}/images/edits{AzureVersionQueryString}";
    }

    public string ImageVariationCreate()
    {
        return $"{Prefix}/images/variations{AzureVersionQueryString}";
    }

    public string ChatCompletionCreate()
    {
        return $"{Prefix}/chat/completions{AzureVersionQueryString}";
    }

    public string AudioCreateTranscription()
    {
        return $"{Prefix}/audio/transcriptions{AzureVersionQueryString}";
    }

    public string AudioCreateTranslation()
    {
        return $"{Prefix}/audio/translation{AzureVersionQueryString}";
    }

    public string AudioCreateSpeech()
    {
        return $"{Prefix}/audio/speech{AzureVersionQueryString}";
    }

    public string AssistantCreate()
    {
        return $"{Prefix}/assistants{AzureVersionQueryString}";
    }

    public string AssistantRetrieve(string assistantId)
    {
        return $"{Prefix}/assistants/{assistantId}{AzureVersionQueryString}";
    }

    public string AssistantModify(string assistantId)
    {
        return $"{Prefix}/assistants/{assistantId}{AzureVersionQueryString}";
    }

    public string AssistantDelete(string assistantId)
    {
        return $"{Prefix}/assistants/{assistantId}{AzureVersionQueryString}";
    }

    public string AssistantList(PaginationRequest? assistantListRequest)
    {
        var url = $"{Prefix}/assistants{AzureVersionQueryString}";

        var query = assistantListRequest?.GetQueryParameters();
        if (!string.IsNullOrWhiteSpace(query))
        {
            url = $"{url}{query}";
        }

        return url;
    }

    public string AssistantFileCreate(string assistantId)
    {
        return $"{Prefix}/assistants/{assistantId}/files{AzureVersionQueryString}";
    }

    public string AssistantFileRetrieve(string assistantId, string fileId)
    {
        return $"{Prefix}/assistants/{assistantId}/files/{fileId}{AzureVersionQueryString}";
    }

    public string AssistantFileDelete(string assistantId, string fileId)
    {
        return $"{Prefix}/assistants/{assistantId}/files/{fileId}{AzureVersionQueryString}";
    }

    public string AssistantFileList(string assistantId, PaginationRequest? assistantFileListRequest)
    {
        var url = $"{Prefix}/assistants/files{AzureVersionQueryString}";

        var query = assistantFileListRequest?.GetQueryParameters();
        if (!string.IsNullOrWhiteSpace(query))
        {
            url = $"{url}{query}";
        }

        return url;
    }

    public string ThreadCreate()
    {
        return $"{Prefix}/threads{AzureVersionQueryString}";
    }

    public string ThreadRetrieve(string threadId)
    {
        return $"{Prefix}/threads/{threadId}{AzureVersionQueryString}";
    }

    public string ThreadModify(string threadId)
    {
        return $"{Prefix}/threads/{threadId}{AzureVersionQueryString}";
    }

    public string ThreadDelete(string threadId)
    {
        return $"{Prefix}/threads/{threadId}{AzureVersionQueryString}";
    }

    public string MessageCreate(string threadId)
    {
        return $"{Prefix}/threads/{threadId}/messages{AzureVersionQueryString}";
    }

    public string MessageRetrieve(string threadId, string messageId)
    {
        return $"{Prefix}/threads/{threadId}/messages/{messageId}{AzureVersionQueryString}";
    }

    public string MessageModify(string threadId, string messageId)
    {
        return $"{Prefix}/threads/{threadId}/messages/{messageId}{AzureVersionQueryString}";
    }

    public string MessageList(string threadId, PaginationRequest? messageListRequest)
    {
        var url = $"{Prefix}/threads/{threadId}/messages{AzureVersionQueryString}";

        var query = messageListRequest?.GetQueryParameters();
        if (!string.IsNullOrWhiteSpace(query))
        {
            url = $"{url}{query}";
        }

        return url;
    }
    public string MessageDelete(string threadId, string messageId)
    {
        return $"{Prefix}/threads/{threadId}/messages/{messageId}{AzureVersionQueryString}";
    }

    public string RunCreate(string threadId)
    {
        return $"{Prefix}/threads/{threadId}/runs{AzureVersionQueryString}";
    }

    public string RunRetrieve(string threadId, string runId)
    {
        return $"{Prefix}/threads/{threadId}/runs/{runId}{AzureVersionQueryString}";
    }

    public string RunModify(string threadId, string runId)
    {
        return $"{Prefix}/threads/{threadId}/runs/{runId}{AzureVersionQueryString}";
    }

    public string RunList(string threadId, PaginationRequest? runListRequest)
    {
        var url = $"{Prefix}/threads/{threadId}/runs{AzureVersionQueryString}";

        var query = runListRequest?.GetQueryParameters();
        if (!string.IsNullOrWhiteSpace(query))
        {
            url = $"{url}{query}";
        }

        return url;
    }

    public string RunSubmitToolOutputs(string threadId, string runId)
    {
        return $"{Prefix}/threads/{threadId}/runs/{runId}/submit_tool_outputs{AzureVersionQueryString}";
    }

    public string RunCancel(string threadId, string runId)
    {
        return $"{Prefix}/threads/{threadId}/runs/{runId}/cancel{AzureVersionQueryString}";
    }

    public string ThreadAndRunCreate()
    {
        return $"{Prefix}/threads/runs{AzureVersionQueryString}";
    }

    public string RunStepRetrieve(string threadId, string runId, string stepId)
    {
        return $"{Prefix}/threads/{threadId}/runs/{runId}/steps/{stepId}{AzureVersionQueryString}";
    }

    public string RunStepList(string threadId, string runId, PaginationRequest? runStepListRequest)
    {
        var url = $"{Prefix}/threads/{threadId}/runs/{runId}/steps{AzureVersionQueryString}";

        var query = runStepListRequest?.GetQueryParameters();
        if (!string.IsNullOrWhiteSpace(query))
        {
            url = $"{url}{query}";
        }

        return url;
    }


    public string VectorStoreCreate()
    {
        return $"{Prefix}/vector_stores{AzureVersionQueryString}";
    }

    public string VectorStoreList(PaginationRequest baseListRequest)
    {
        var url = $"{Prefix}/vector_stores{AzureVersionQueryString}";

        var query = baseListRequest?.GetQueryParameters();
        if (!string.IsNullOrWhiteSpace(query))
        {
            url = $"{url}?{query}";
        }

        return url;
    }

    public string VectorStoreRetrieve(string vectorStoreId)
    {
        return $"{Prefix}/vector_stores/{vectorStoreId}{AzureVersionQueryString}";
    }

    public string VectorStoreModify(string vectorStoreId)
    {
        return $"{Prefix}/vector_stores/{vectorStoreId}{AzureVersionQueryString}";
    }

    public string VectorStoreDelete(string vectorStoreId)
    {
        return $"{Prefix}/vector_stores/{vectorStoreId}{AzureVersionQueryString}";
    }

    public string VectorStoreFileCreate(string vectorStoreId)
    {
        return $"{Prefix}/vector_stores/{vectorStoreId}/files{AzureVersionQueryString}";
    }

    public string VectorStoreFileRetrieve(string vectorStoreId, string fileId)
    {
        return $"{Prefix}/vector_stores/{vectorStoreId}/files/{fileId}{AzureVersionQueryString}";
    }

    public string VectorStoreFileDelete(string vectorStoreId, string fileId)
    {
        return $"{Prefix}/vector_stores/{vectorStoreId}/files/{fileId}{AzureVersionQueryString}";
    }

    public string VectorStoreFileList(string vectorStoreId, VectorStoreFileListRequest? baseListRequest)
    {
        var url = $"{Prefix}/vector_stores/{vectorStoreId}/files{AzureVersionQueryString}";

        var query = baseListRequest?.GetQueryParameters();
        if (!string.IsNullOrWhiteSpace(query))
        {
            url = $"{url}?{query}";
        }

        return url;
    }

    public string VectorStoreFileBatchCreate(string vectorStoreId)
    {
        return $"{Prefix}/vector_stores/{vectorStoreId}/files/batches{AzureVersionQueryString}";
    }

    public string VectorStoreFileBatchRetrieve(string vectorStoreId, string batchId)
    {
        return $"{Prefix}/vector_stores/{vectorStoreId}/files/batches/{batchId}{AzureVersionQueryString}";
    }

    public string VectorStoreFileBatchCancel(string vectorStoreId, string batchId)
    {
        return $"{Prefix}/vector_stores/{vectorStoreId}/files/batches/{batchId}/cancel{AzureVersionQueryString}";
    }

    public string VectorStoreFileBatchList(string vectorStoreId, string batchId, PaginationRequest? baseListRequest)
    {
        var url = $"{Prefix}/vector_stores/{vectorStoreId}/files/batches{AzureVersionQueryString}";

        var query = baseListRequest?.GetQueryParameters();
        if (!string.IsNullOrWhiteSpace(query))
        {
            url = $"{url}?{query}";
        }

        return url;
    }

    public string FineTuningJobList()
    {
        return $"{Prefix}/fine_tuning/jobs{AzureVersionQueryString}";
    }

    public string BatchCreate()
    {
        return $"{Prefix}/batches{AzureVersionQueryString}";
    }

    public string BatchRetrieve(string batchId)
    {
        return $"{Prefix}/batches/{batchId}{AzureVersionQueryString}";
    }

    public string BatchCancel(string batchId)
    {
        return $"{Prefix}/batches/{batchId}/cancel{AzureVersionQueryString}";
    }

    private string Files()
    {
        return $"{Prefix}/files{AzureVersionQueryString}";
    }


}