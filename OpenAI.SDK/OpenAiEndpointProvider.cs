namespace OpenAI.GPT3
{
    internal interface IOpenAiEndpointProvider
    {
        string RetrieveModel(string model);
        string CreateCompletion(string engineId);
        string ListModels();
        string ListFiles();
        string UploadFiles();
        string DeleteFile(string fileId);
        string RetrieveFile(string fileId);
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

        public string RetrieveModel(string model)
        {
            return $"/{_apiVersion}/models/{model}";
        }

        public string DeleteFile(string fileId)
        {
            return $"/{_apiVersion}/files/{fileId}";
        }

        public string CreateCompletion(string engineId)
        {
            return $"/{_apiVersion}/engines/{engineId}/completions";
        }

        public string ListModels()
        {
            return $"/{_apiVersion}/models";
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