namespace OpenAI.GPT3
{
    internal interface IOpenAiEndpointProvider
    {
        string ModelRetrieve(string model);
        string CompletionCreate(string engineId);
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
    }

    internal class OpenAiEndpointProvider : IOpenAiEndpointProvider
    {
        private readonly string _apiVersion;

        public OpenAiEndpointProvider(string apiVersion)
        {
            _apiVersion = apiVersion;
        }

        public string ModelRetrieve(string model)
        {
            return $"/{_apiVersion}/models/{model}";
        }

        public string FileDelete(string fileId)
        {
            return $"/{_apiVersion}/files/{fileId}";
        }

        public string CompletionCreate(string engineId)
        {
            return $"/{_apiVersion}/engines/{engineId}/completions";
        }

        public string ModelsList()
        {
            return $"/{_apiVersion}/models";
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
            return Files();
        }

        public string FineTuneCreate()
        {
            return $"/{_apiVersion}/fine-tunes";
        }

        public string FineTuneList()
        {
            return $"/{_apiVersion}/fine-tunes";
        }

        public string FineTuneRetrieve(string fineTuneId)
        {
            return $"/{_apiVersion}/fine-tunes/{fineTuneId}";
        }

        public string FineTuneCancel(string fineTuneId)
        {
            return $"/{_apiVersion}/fine-tunes/{fineTuneId}/cancel";
        }

        public string FineTuneListEvents(string fineTuneId)
        {
            return $"/{_apiVersion}/fine-tunes/{fineTuneId}/events";
        }

        public string FineTuneDelete(string fineTuneId)
        {
            return $"/{_apiVersion}/models/{fineTuneId}";
        }

        public string EmbeddingCreate()
        {
            return $"/{_apiVersion}/embeddings";
        }

        public string ModerationCreate()
        {
            return $"/{_apiVersion}/moderations";
        }

        public string ImageCreate()
        {
            return $"/{_apiVersion}/images/generations";
        }

        private string Files()
        {
            return $"/{_apiVersion}/files";
        }
    }
}