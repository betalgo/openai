using System.Text.Json.Serialization;
using OpenAI.SDK.Models.RequestModels.RequestInterfaces;

namespace OpenAI.SDK.Models.RequestModels
{
    //TODO validate fields types with documentation
    //TODO create common fields interfaces
    public record CreateAnswerRequest : IOpenAiRequest.IModel, IOpenAiRequest.ITemperature, IOpenAiRequest.ILogitBias, IOpenAiRequest.ILogprobs, IOpenAiRequest.IMaxTokens, IOpenAiRequest.IStop, IOpenAiRequest.IReturnPrompt,
        IOpenAiRequest.IReturnMetadata,
        IOpenAiRequest.IExpand
    {
        /// <summary>
        ///     Question to get answered.
        /// </summary>
        [JsonPropertyName("question")]
        public string Question { get; set; }

        /// <summary>
        ///     List of (question, answer) pairs that will help steer the model towards the tone and answer format you'd like. We
        ///     recommend adding 2 to 3 examples.
        /// </summary>
        [JsonPropertyName("examples")]
        public List<List<string>> Examples { get; set; }

        /// <summary>
        ///     A text snippet containing the contextual information used to generate the answers for the examples you provide.
        /// </summary>
        [JsonPropertyName("examples_context")]
        public string ExamplesContext { get; set; }


        /// <summary>
        ///     List of documents from which the answer for the input question should be derived. If this is an empty list, the
        ///     question will be answered based on the question-answer examples.
        ///     You should specify either documents or a file, but not both.
        /// </summary>
        [JsonPropertyName("documents")]
        public List<string>? Documents { get; set; }

        /// <summary>
        ///     The ID of an uploaded file that contains documents to search over. See
        ///     <a href="https://beta.openai.com/docs/api-reference/files/upload">upload file</a> for how to upload a file of the
        ///     desired format and purpose.
        ///     You should specify either documents or a file, but not both.
        /// </summary>
        [JsonPropertyName("file")]
        public string? File { get; set; }

        /// <summary>
        ///     ID of the engine to use for <a href="https://beta.openai.com/docs/api-reference/searches/create">Search</a>.
        /// </summary>
        [JsonPropertyName("search_model")]
        public string? SearchModel { get; set; }

        /// <summary>
        ///     The maximum number of documents to be ranked by Search when using file. Setting it to a higher value leads to
        ///     improved accuracy but with increased latency and cost.
        /// </summary>
        [JsonPropertyName("max_rerank")]
        public int? MaxRerank { get; set; }

        /// <summary>
        ///     How many answers to generate for each question.
        /// </summary>
        [JsonPropertyName("n")]
        public bool? N { get; set; }


        [JsonPropertyName("expand")] public List<string>? Expand { get; set; }


        /// <summary>
        /// </summary>
        [JsonPropertyName("logit_bias")]
        public object? LogitBias { get; set; }

        /// <summary>
        ///     Include the log probabilities on the logprobs most likely tokens, as well the chosen tokens. For example, if
        ///     logprobs is 10, the API will return a list of the 10 most likely tokens. the API will always return the logprob of
        ///     the sampled token, so there may be up to logprobs+1 elements in the response.
        ///     When logprobs is set, completion will be automatically added into expand to get the logprobs.
        /// </summary>
        [JsonPropertyName("logprobs")]
        public int? Logprobs { get; set; }

        /// <summary>
        ///     The maximum number of tokens allowed for the generated answer
        /// </summary>
        [JsonPropertyName("max_tokens")]
        public int? MaxTokens { get; set; }

        /// <summary>
        ///     ID of the engine to use for completion.
        /// </summary>
        [JsonPropertyName("model")]
        public string Model { get; set; }

        /// <summary>
        ///     A special boolean flag for showing metadata. If set to true, each document entry in the returned JSON will contain
        ///     a "metadata" field.
        ///     This flag only takes effect when file is set.
        /// </summary>
        [JsonPropertyName("return_metadata")]
        public bool? ReturnMetadata { get; set; }

        /// <summary>
        ///     If set to true, the returned JSON will include a "prompt" field containing the final prompt that was used to
        ///     request a completion. This is mainly useful for debugging purposes.
        /// </summary>
        [JsonPropertyName("return_prompt")]
        public bool? ReturnPrompt { get; set; }

        /// <summary>
        ///     Up to 4 sequences where the API will stop generating further tokens. The returned text will not contain the stop
        ///     sequence.
        /// </summary>
        public List<string>? Stop { get; set; }

        /// <summary>
        ///     What
        ///     <a href="https://towardsdatascience.com/how-to-sample-from-language-models-682bceb97277">sampling temperature</a>
        ///     to use. Higher values mean the model will take more risks and value 0 (argmax sampling) works better for scenarios
        ///     with a well-defined answer.
        /// </summary>
        [JsonPropertyName("temperature")]
        public float? Temperature { get; set; }
    }
}