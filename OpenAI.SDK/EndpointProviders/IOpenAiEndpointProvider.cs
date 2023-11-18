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
    string AssistantCreate();
    string AssistantRetrieve(string assistantId);
    string AssistantModify(string assistantId);
    string AssistantDelete(string assistantId);
    string AssistantList(AssistantListRequest? assistantListRequest);
    string AssistantFileCreate(string assistantId);
    string AssistantFileRetrieve(string assistantId, string fileId);
    string AssistantFileDelete(string assistantId, string fileId);
    string AssistantFileList(string assistantId, AssistantFileListRequest? assistantFileListRequest);
}