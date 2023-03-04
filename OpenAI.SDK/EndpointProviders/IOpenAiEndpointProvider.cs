namespace OpenAI.GPT3.EndpointProviders;

internal interface IOpenAiEndpointProvider
{
    string ModelRetrieve(string model);
    string CompletionCreate();
    string EditCreate();
    string ModelsList();
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
    string EmbeddingCreate();
    string ModerationCreate();
    string ImageCreate();
    string ImageEditCreate();
    string ImageVariationCreate();
    string ChatCompletionCreate();
    string AudioCreateTranscription();
    string AudioCreateTranslation();
}