namespace OpenAI.GPT3
{
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
    }

    internal class OpenAiEndpointProvider : IOpenAiEndpointProvider
    {
        private readonly string _apiVersion;

        public OpenAiEndpointProvider(string apiVersion)
        {
            _apiVersion = apiVersion;
        }

        public string ModelRetrieve(string model) => $"/{_apiVersion}/models/{model}";

        public string FileDelete(string fileId) => $"/{_apiVersion}/files/{fileId}";

        public string CompletionCreate() => $"/{_apiVersion}/completions";

        public string EditCreate() => $"/{_apiVersion}/edits";

        public string ModelsList() => $"/{_apiVersion}/models";

        public string FilesList() => Files();

        public string FilesUpload() => Files();

        public string FileRetrieve(string fileId) => Files();

        public string FineTuneCreate() => $"/{_apiVersion}/fine-tunes";

        public string FineTuneList() => $"/{_apiVersion}/fine-tunes";

        public string FineTuneRetrieve(string fineTuneId) => $"/{_apiVersion}/fine-tunes/{fineTuneId}";

        public string FineTuneCancel(string fineTuneId) => $"/{_apiVersion}/fine-tunes/{fineTuneId}/cancel";

        public string FineTuneListEvents(string fineTuneId) => $"/{_apiVersion}/fine-tunes/{fineTuneId}/events";

        public string FineTuneDelete(string fineTuneId) => $"/{_apiVersion}/models/{fineTuneId}";

        public string EmbeddingCreate() => $"/{_apiVersion}/embeddings";

        public string ModerationCreate() => $"/{_apiVersion}/moderations";

        public string ImageCreate() => $"/{_apiVersion}/images/generations";

        public string ImageEditCreate() => $"/{_apiVersion}/images/edits";

        public string ImageVariationCreate() => $"/{_apiVersion}/images/variations";

        string Files() => $"/{_apiVersion}/files";
    }
}