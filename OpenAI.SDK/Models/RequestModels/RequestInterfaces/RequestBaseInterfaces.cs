namespace OpenAI.SDK.Models.RequestModels.RequestInterfaces
{
    public interface IOpenAiRequest
    {
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
    }
}