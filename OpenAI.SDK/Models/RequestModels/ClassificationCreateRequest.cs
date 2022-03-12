using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using OpenAI.GPT3.Interfaces;
using OpenAI.GPT3.Models.SharedModels;

namespace OpenAI.GPT3.Models.RequestModels
{
    public record ClassificationCreateRequest : IModelValidate, IOpenAiModels.ITemperature, IOpenAiModels.ILogitBias,IOpenAiModels.IModel
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="query">Query to be classified.</param>
        /// <param name="model">ID of the engine to use for completion.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public ClassificationCreateRequest(string query, string model)
        {
            Query = query ?? throw new ArgumentNullException(nameof(query));
            Model = model ?? throw new ArgumentNullException(nameof(model));
        }

        public ClassificationCreateRequest()
        {
            
        }

        /// <summary>
        ///     ID of the engine to use for completion.
        /// </summary>
        [JsonPropertyName("model")]
        public string Model { get; set; } = null!;

        /// <summary>
        ///     Query to be classified.
        /// </summary>
        [JsonPropertyName("query")]
        public string Query { get; set; } = null!;

        /// <summary>
        ///     A list of examples with labels, in the following format:
        ///     [["The movie is so interesting.", "Positive"], ["It is quite boring.", "Negative"], ...]
        ///     All the label strings will be normalized to be capitalized.
        ///     You should specify either examples or file, but not both.
        /// </summary>
        [JsonPropertyName("examples")]
        public List<List<string>>? Examples { get; set; }

        /// <summary>
        ///     The ID of the uploaded file that contains training examples. See upload file for how to upload a file of the
        ///     desired format and purpose.
        ///     You should specify either examples or file, but not both.
        /// </summary>
        [JsonPropertyName("file")]
        public string? File { get; set; }

        /// <summary>
        ///     The set of categories being classified. If not specified, candidate labels will be automatically collected from the
        ///     examples you provide. All the label strings will be normalized to be capitalized.
        /// </summary>
        [JsonPropertyName("labels")]
        public List<string>? Labels { get; set; }

        /// <summary>
        ///     ID of the engine to use for <a href="https://beta.openai.com/docs/api-reference/searches/create">Search.</a>
        /// </summary>
        [JsonPropertyName("search_model")]
        public string? SearchModel { get; set; }

        /// <summary>
        ///     Include the log probabilities on the logprobs most likely tokens, as well the chosen tokens. For example, if
        ///     logprobs is 10, the API will return a list of the 10 most likely tokens. the API will always return the logprob of
        ///     the sampled token, so there may be up to logprobs+1 elements in the response.
        ///     When logprobs is set, completion will be automatically added into expand to get the logprobs.
        /// </summary>
        [JsonPropertyName("logprobs")]
        public int? Logprobs { get; set; }


        /// <summary>
        ///     The maximum number of examples to be ranked by
        ///     <a href="https://beta.openai.com/docs/api-reference/searches/create">Search</a> when using file. Setting it to a
        ///     higher value leads to improved accuracy but with increased latency and cost.
        /// </summary>
        [JsonPropertyName("max_examples")]
        public int? MaxExamples { get; set; }


        /// <summary>
        ///     If set to true, the returned JSON will include a "prompt" field containing the final prompt that was used to
        ///     request a completion. This is mainly useful for debugging purposes.
        /// </summary>
        [JsonPropertyName("return_prompt")]
        public bool? ReturnPrompt { get; set; }

        /// <summary>
        ///     A special boolean flag for showing metadata. If set to true, each document entry in the returned JSON will contain
        ///     a "metadata" field.
        ///     This flag only takes effect when file is set.
        /// </summary>
        [JsonPropertyName("return_metadata")]
        public bool? ReturnMetadata { get; set; }

        /// <summary>
        ///     If an object name is in the list, we provide the full information of the object; otherwise, we only provide the
        ///     object ID. Currently we support completion and file objects for expansion.
        /// </summary>
        [JsonPropertyName("expand")]
        public List<string>? Expand { get; set; }

        /// <summary>
        ///     Modify the likelihood of specified tokens appearing in the completion.
        ///     Accepts a json object that maps tokens(specified by their token ID in the GPT tokenizer) to an associated bias
        ///     value from -100 to 100. You can use this <a href="https://beta.openai.com/tokenizer?view=bpe">tokenizer tool</a>
        ///     (which works for both GPT-2 and GPT-3) to convert text to
        ///     token IDs. Mathematically, the bias is added to the logits generated by the model prior to sampling. The exact
        ///     effect will vary per model, but values between -1 and 1 should decrease or increase likelihood of selection; values
        ///     like -100 or 100 should result in a ban or exclusive selection of the relevant token.
        ///     As an example, you can pass { "50256": -100}
        ///     to prevent the (endoftext) token from being generated.
        /// </summary>
        [JsonPropertyName("logit_bias")]
        public object? LogitBias { get; set; }

        public IEnumerable<ValidationResult> Validate()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     What sampling temperature to use. Higher values mean the model will take more risks. Try 0.9 for more creative
        ///     applications, and 0 (argmax sampling) for ones with a well-defined answer.
        /// </summary>
        [JsonPropertyName("temperature")]
        public float? Temperature { get; set; }
    }
}