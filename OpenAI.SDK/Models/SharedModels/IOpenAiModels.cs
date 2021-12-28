namespace OpenAI.GPT3.Models.SharedModels
{
    public interface IOpenAiModels
    {
        public interface IId
        {
            string Id { get; set; }
        }

        public interface IModel
        {
            string Model { get; set; }
        }

        public interface ITemperature
        {
            float? Temperature { get; set; }
        }

        public interface ILogitBias
        {
            object? LogitBias { get; set; }
        }

        public interface ILogprobs
        {
            int? Logprobs { get; set; }
        }

        public interface IMaxTokens
        {
            int? MaxTokens { get; set; }
        }

        public interface IStop
        {
            List<string>? Stop { get; set; }
        }

        public interface IReturnMetadata
        {
            bool? ReturnMetadata { get; set; }
        }

        public interface IReturnPrompt
        {
            bool? ReturnPrompt { get; set; }
        }

        public interface IExpand
        {
            List<string>? Expand { get; set; }
        }

        public interface IFileOrDocument : IFile
        {
            public List<string>? Documents { get; set; }
        }

        public interface IFile
        {
            /// <summary>
            ///     The ID of an uploaded file that contains documents to search over.
            ///     You should specify either documents or a file, but not both.
            /// </summary>
            public string? File { get; set; }
        }

        public interface ICreatedAt
        {
            public int? CreatedAt { get; set; }
        }

        public interface IFilePurpose
        {
            string Purpose { get; set; }
        }
    }
}