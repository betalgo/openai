using OpenAI.ObjectModels.RequestModels;

namespace OpenAI.EndpointProviders;

internal interface IOpenAiEndpointProvider
{
    string ModelRetrieve(string model);
    string CompletionCreate();
    string EditCreate();
    string ModelsList();
    string ModelsDelete(string modelId);
    string FilesList();
    string FilesUpload();
    string FileDelete(string fileId);
    string FileRetrieve(string fileId);
    string FileRetrieveContent(string fileId);
    string FineTuneCreate();
    string FineTuneList();
    string FineTuneRetrieve(string fineTuneId);
    string FineTuneCancel(string fineTuneId);
    string FineTuneListEvents(string fineTuneId);
    string FineTuneDelete(string fineTuneId);
    string FineTuningJobCreate();
    string FineTuningJobList(FineTuningJobListRequest? fineTuningJobListRequest);
    string FineTuningJobRetrieve(string fineTuningJobId);
    string FineTuningJobCancel(string fineTuningJobId);
    string FineTuningJobListEvents(string fineTuningJobId);
    string EmbeddingCreate();
    string ModerationCreate();
    string ImageCreate();
    string ImageEditCreate();
    string ImageVariationCreate();
    string ChatCompletionCreate();
    string AudioCreateTranscription();
    string AudioCreateTranslation();
    string AudioCreateSpeech();
    string BatchCreate();
    string BatchRetrieve(string batchId);
    string BatchCancel(string batchId);

    string AssistantCreate();
    string AssistantRetrieve(string assistantId);
    string AssistantModify(string assistantId);
    string AssistantDelete(string assistantId);
    string AssistantList(AssistantListRequest? assistantListRequest);
    string AssistantFileCreate(string assistantId);
    string AssistantFileRetrieve(string assistantId, string fileId);
    string AssistantFileDelete(string assistantId, string fileId);
    string AssistantFileList(string assistantId, AssistantFileListRequest? assistantFileListRequest);
    string ThreadCreate();
    string ThreadRetrieve(string threadId);
    string ThreadModify(string threadId);
    string ThreadDelete(string threadId);
    string MessageCreate(string threadId);
    string MessageRetrieve(string threadId,string messageId);
    string MessageModify(string threadId, string messageId);
    string MessageList(string threadId, MessageListRequest? messageListRequest);
    string MessageFileRetrieve(string threadId, string messageId, string fileId);
    string MessageFileList(string threadId, string messageId, MessageFileListRequest? messageFileListRequest);
    string RunCreate(string threadId);
    string RunRetrieve(string threadId, string runId);
    string RunModify(string threadId, string runId);
    string RunList(string threadId, RunListRequest? runListRequest);
    string RunSubmitToolOutputs(string threadId, string runId);
    string RunCancel(string threadId, string runId);
    string ThreadAndRunCreate();
    string RunStepRetrieve(string threadId, string runId,string stepId);
    string RunStepList(string threadId, string runId, RunStepListRequest? runStepListRequest);
}