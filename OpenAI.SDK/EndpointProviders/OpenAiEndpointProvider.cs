using System.Net;
using OpenAI.ObjectModels.RequestModels;

namespace OpenAI.EndpointProviders;

internal class OpenAiEndpointProvider : IOpenAiEndpointProvider
{
    private readonly string _apiVersion;

    public OpenAiEndpointProvider(string apiVersion)
    {
        _apiVersion = apiVersion;
    }

    public string ModelRetrieve(string model)
    {
        return $"{_apiVersion}/models/{model}";
    }

    public string FileDelete(string fileId)
    {
        return $"{_apiVersion}/files/{fileId}";
    }

    public string CompletionCreate()
    {
        return $"{_apiVersion}/completions";
    }

    public string ChatCompletionCreate()
    {
        return $"{_apiVersion}/chat/completions";
    }

    public string AudioCreateTranscription()
    {
        return $"{_apiVersion}/audio/transcriptions";
    }

    public string AudioCreateTranslation()
    {
        return $"{_apiVersion}/audio/translations";
    }

    public string AudioCreateSpeech()
    {
        return $"{_apiVersion}/audio/speech";
    }

    public string EditCreate()
    {
        return $"{_apiVersion}/edits";
    }

    public string ModelsList()
    {
        return $"{_apiVersion}/models";
    }

    public string FilesList()
    {
        return $"{_apiVersion}/files";
    }

    public string FilesUpload()
    {
        return $"{_apiVersion}/files";
    }

    public string FileRetrieve(string fileId)
    {
        return $"{_apiVersion}/files/{fileId}";
    }

    public string FileRetrieveContent(string fileId)
    {
        return $"{_apiVersion}/files/{fileId}/content";
    }

    public string FineTuneCreate()
    {
        return $"{_apiVersion}/fine-tunes";
    }

    public string FineTuneList()
    {
        return $"{_apiVersion}/fine-tunes";
    }

    public string FineTuneRetrieve(string fineTuneId)
    {
        return $"{_apiVersion}/fine-tunes/{fineTuneId}";
    }

    public string FineTuneCancel(string fineTuneId)
    {
        return $"{_apiVersion}/fine-tunes/{fineTuneId}/cancel";
    }

    public string FineTuneListEvents(string fineTuneId)
    {
        return $"{_apiVersion}/fine-tunes/{fineTuneId}/events";
    }

    public string FineTuneDelete(string fineTuneId)
    {
        return $"{_apiVersion}/models/{fineTuneId}";
    }

    public string FineTuningJobCreate()
    {
        return $"{_apiVersion}/fine_tuning/jobs";
    }

    public string FineTuningJobList(FineTuningJobListRequest? fineTuningJobListRequest)
    {
        var url = $"{_apiVersion}/fine_tuning/jobs";
        if (fineTuningJobListRequest != null)
        {
            var queryParams = new List<string>();
            if (fineTuningJobListRequest.After != null)
                queryParams.Add($"after={WebUtility.UrlEncode(fineTuningJobListRequest.After)}");
            if (fineTuningJobListRequest.Limit.HasValue)
                queryParams.Add($"limit={fineTuningJobListRequest.Limit.Value}");

            if (queryParams.Any())
                url = $"{url}?{string.Join("&", queryParams)}";
        }

        return url;
    }

    public string FineTuningJobRetrieve(string fineTuningJobId)
    {
        return $"{_apiVersion}/fine_tuning/jobs/{fineTuningJobId}";
    }

    public string FineTuningJobCancel(string fineTuningJobId)
    {
        return $"{_apiVersion}/fine_tuning/jobs/{fineTuningJobId}/cancel";
    }

    public string FineTuningJobListEvents(string fineTuningJobId)
    {
        return $"{_apiVersion}/fine_tuning/jobs/{fineTuningJobId}/events";
    }

    public string ModelsDelete(string modelId)
    {
        return $"{_apiVersion}/models/{modelId}";
    }

    public string EmbeddingCreate()
    {
        return $"{_apiVersion}/embeddings";
    }

    public string ModerationCreate()
    {
        return $"{_apiVersion}/moderations";
    }

    public string ImageCreate()
    {
        return $"{_apiVersion}/images/generations";
    }

    public string ImageEditCreate()
    {
        return $"{_apiVersion}/images/edits";
    }

    public string ImageVariationCreate()
    {
        return $"{_apiVersion}/images/variations";
    }

    public string AssistantCreate()
    {
        return $"{_apiVersion}/assistants";
    }

    public string AssistantRetrieve(string assistantId)
    {
        return $"{_apiVersion}/assistants/{assistantId}";
    }

    public string AssistantModify(string assistantId)
    {
        return $"{_apiVersion}/assistants/{assistantId}";
    }

    public string AssistantDelete(string assistantId)
    {
        return $"{_apiVersion}/assistants/{assistantId}";
    }

    public string AssistantList(AssistantListRequest? assistantListRequest)
    {
        var url = $"{_apiVersion}/assistants";

        var query = assistantListRequest?.GetQueryParameters();
        if (!string.IsNullOrWhiteSpace(query))
        {
            url = $"{url}?{query}";
        }

        return url;
    }

    public string AssistantFileCreate(string assistantId)
    {
        return $"{_apiVersion}/assistants/{assistantId}/files";
    }

    public string AssistantFileRetrieve(string assistantId, string fileId)
    {
        return $"{_apiVersion}/assistants/{assistantId}/files/{fileId}";
    }

    public string AssistantFileDelete(string assistantId, string fileId)
    {
        return $"{_apiVersion}/assistants/{assistantId}/files/{fileId}";
    }

    public string AssistantFileList(string assistantId, AssistantFileListRequest? assistantFileListRequest)
    {
        var url = $"{_apiVersion}/assistants/{assistantId}/files";

        var query = assistantFileListRequest?.GetQueryParameters();
        if (!string.IsNullOrWhiteSpace(query))
        {
            url = $"{url}?{query}";
        }

        return url;
    }

    public string ThreadCreate()
    {
        return $"{_apiVersion}/threads";
    }

    public string ThreadRetrieve(string threadId)
    {
        return $"{_apiVersion}/threads/{threadId}";
    }

    public string ThreadModify(string threadId)
    {
        return $"{_apiVersion}/threads/{threadId}";
    }

    public string ThreadDelete(string threadId)
    {
        return $"{_apiVersion}/threads/{threadId}";
    }

    public string MessageCreate(string threadId)
    {
        return $"{_apiVersion}/threads/{threadId}/messages";
    }

    public string MessageRetrieve(string threadId, string messageId)
    {
        return $"{_apiVersion}/threads/{threadId}/messages/{messageId}";
    }

    public string MessageModify(string threadId, string messageId)
    {
        return $"{_apiVersion}/threads/{threadId}/messages/{messageId}";
    }

    public string MessageList(string threadId, MessageListRequest? messageListRequest)
    {
        var url = $"{_apiVersion}/threads/{threadId}/messages";

        var query = messageListRequest?.GetQueryParameters();
        if (!string.IsNullOrWhiteSpace(query))
        {
            url = $"{url}?{query}";
        }

        return url;
    }

    public string MessageFileRetrieve(string threadId, string messageId, string fileId)
    {
        return $"{_apiVersion}/threads/{threadId}/messages/{messageId}/files/{fileId}";
    }

    public string MessageFileList(string threadId, string messageId, MessageFileListRequest? messageFileListRequest)
    {
        var url = $"{_apiVersion}/threads/{threadId}/messages/{messageId}/files";

        var query = messageFileListRequest?.GetQueryParameters();
        if (!string.IsNullOrWhiteSpace(query))
        {
            url = $"{url}?{query}";
        }

        return url;
    }

    public string RunCreate(string threadId)
    {
        return $"{_apiVersion}/threads/{threadId}/runs";
    }

    public string RunRetrieve(string threadId, string runId)
    {
        return $"{_apiVersion}/threads/{threadId}/runs/{runId}";
    }

    public string RunModify(string threadId, string runId)
    {
        return $"{_apiVersion}/threads/{threadId}/runs/{runId}";
    }

    public string RunList(string threadId, RunListRequest? runListRequest)
    {
        var url = $"{_apiVersion}/threads/{threadId}/runs";

        var query = runListRequest?.GetQueryParameters();
        if (!string.IsNullOrWhiteSpace(query))
        {
            url = $"{url}?{query}";
        }

        return url;
    }

    public string RunSubmitToolOutputs(string threadId, string runId)
    {
        return $"{_apiVersion}/threads/{threadId}/runs/{runId}/submit_tool_outputs";
    }

    public string RunCancel(string threadId, string runId)
    {
        return $"{_apiVersion}/threads/{threadId}/runs/{runId}/cancel";
    }

    public string ThreadAndRunCreate()
    {
        return $"{_apiVersion}/threads/runs";
    }

    public string RunStepRetrieve(string threadId, string runId, string stepId)
    {
        return $"{_apiVersion}/threads/{threadId}/runs/{runId}/steps/{stepId}";
    }

    public string RunStepList(string threadId, string runId, RunStepListRequest? runStepListRequest)
    {
        var url = $"{_apiVersion}/threads/{threadId}/runs/{runId}/steps";

        var query = runStepListRequest?.GetQueryParameters();
        if (!string.IsNullOrWhiteSpace(query))
        {
            url = $"{url}?{query}";
        }

        return url;
    }
}