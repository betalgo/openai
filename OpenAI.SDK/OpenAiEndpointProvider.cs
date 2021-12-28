namespace OpenAI.SDK
{
    internal interface IOpenAiEndpointProvider
    {
        string RetrieveEngine(string engineId);
        string CreateAnswer();
        string CreateClassification();
        string CreateCompletion(string engineId);
        string ListEngines();
        string ListFiles();
        string UploadFiles();
        string DeleteFile(string fileId);
        string RetrieveFile(string fileId);
        string CreateSearch(string engineId);
        string FineTuneCreate();
        string FineTuneList();
        string FineTuneRetrieve(string fineTuneId);
        string FineTuneCancel(string fineTuneId);
        string FineTuneListEvents(string fineTuneId);
        string FineTuneDelete(string fineTuneId);
        string FineTuneCompletions();
    }

    internal class OpenAiEndpointProvider : IOpenAiEndpointProvider
    {
        private readonly string _apiVersion;

        public OpenAiEndpointProvider(string apiVersion)
        {
            _apiVersion = apiVersion;
        }

        public string RetrieveEngine(string engineId)
        {
            return $"/{_apiVersion}/engines/{engineId}";
        }

        public string DeleteFile(string fileId)
        {
            return $"/{_apiVersion}/files/{fileId}";
        }

        public string CreateAnswer()
        {
            return $"/{_apiVersion}/answers";
        }

        public string CreateClassification()
        {
            return $"/{_apiVersion}/classifications";
        }

        public string CreateCompletion(string engineId)
        {
            return $"/{_apiVersion}/engines/{engineId}/completions";
        }

        public string ListEngines()
        {
            return $"/{_apiVersion}/engines";
            ;
        }

        public string ListFiles()
        {
            return Files();
        }

        public string UploadFiles()
        {
            return Files();
        }

        public string RetrieveFile(string fileId)
        {
            return Files();
        }

        public string CreateSearch(string engineId)
        {
            return $"/{_apiVersion}/engines/{engineId}/search";
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

        public string FineTuneCompletions()
        {
            return $"/{_apiVersion}/completions";
        }

        private string Files()
        {
            return $"/{_apiVersion}/files";
        }
    }
}