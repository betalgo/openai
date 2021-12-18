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
            return Answer();
        }

        public string CreateClassification()
        {
            return Classification();
        }

        public string CreateCompletion(string engineId)
        {
            return Completion(engineId);
        }

        public string ListEngines()
        {
            return Engine();
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

        private string Answer()
        {
            return $"/{_apiVersion}/answers";
        }

        private string Classification()
        {
            return $"/{_apiVersion}/classifications";
        }

        private string Engine()
        {
            return $"/{_apiVersion}/engines";
        }

        private string Completion(string engineId)
        {
            return $"/{_apiVersion}/engines/{engineId}/completions";
        }

        private string Files()
        {
            return $"/{_apiVersion}/files";
        }
    }
}