using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using OpenAI.Interfaces;
using OpenAI.ObjectModels.SharedModels;

namespace OpenAI.ObjectModels.RequestModels;

//TODO add model validation
/// <summary>
///     Create Completion Request Model
/// </summary>
public record CompletionCreateRequest : IModelValidate, IOpenAiModels.ITemperature, IOpenAiModels.IModel, IOpenAiModels.ILogProbsRequest, IOpenAiModels.IUser
{
    /// <summary>
    ///     The prompt(s) to generate completions for, encoded as a string, a list of strings, or a list of token lists.
    ///     Note that endoftext is the document separator that the model sees during training, so if a prompt is not specified
    ///     the model will generate as if from the beginning of a new document.
    /// </summary>
    /// <see href="https://platform.openai.com/docs/api-reference/completions/create#completions/create-prompt" />
    [JsonIgnore]
    public string? Prompt { get; set; }

    /// <summary>
    ///     The prompt(s) to generate completions for, encoded as a string, a list of strings, or a list of token lists.
    ///     Note that endoftext is the document separator that the model sees during training, so if a prompt is not specified
    ///     the model will generate as if from the beginning of a new document.
    /// </summary>
    /// <see href="https://platform.openai.com/docs/api-reference/completions/create#completions/create-prompt" />
    [JsonIgnore]
    public IList<string>? PromptAsList { get; set; }

    [JsonPropertyName("prompt")]
    public IList<string>? PromptCalculated
    {
        get
        {
            if (Prompt != null && PromptAsList != null)
            {
                throw new ValidationException("Prompt and PromptAsList can not be assigned at the same time. One of them is should be null.");
            }

            if (Prompt != null)
            {
                return new List<string> {Prompt};
            }


            return PromptAsList;
        }
    }

    /// <summary>
    ///     The suffix that comes after a completion of inserted text.
    /// </summary>
    [JsonPropertyName("suffix")]
    public string? Suffix { get; set; }

    /// <summary>
    ///     The maximum number of <a href="https://platform.openai.com/tokenizer">tokens</a> to generate in the completion.
    ///     The token count of your prompt plus max_tokens cannot exceed the model's context length. Most models have a context
    ///     length of 2048 tokens (except davinci-codex, which supports 4096).
    /// </summary>
    /// <see href="https://platform.openai.com/docs/api-reference/completions/create#completions/create-max_tokens" />
    [JsonPropertyName("max_tokens")]
    public int? MaxTokens { get; set; }

    /// <summary>
    ///     An alternative to sampling with temperature, called nucleus sampling, where the model considers the results of the
    ///     tokens with top_p probability mass. So 0.1 means only the tokens comprising the top 10% probability mass are
    ///     considered.
    ///     We generally recommend altering this or temperature but not both.
    /// </summary>
    [JsonPropertyName("top_p")]
    public float? TopP { get; set; }

    /// <summary>
    ///     Defaults to 1
    ///     How many completions to generate for each prompt.
    ///     Note: Because this parameter generates many completions, it can quickly consume your token quota.Use carefully and
    ///     ensure that you have reasonable settings for max_tokens and stop.
    /// </summary>
    [JsonPropertyName("n")]
    public int? N { get; set; }

    /// <summary>
    ///     Whether to stream back partial progress. If set, tokens will be sent as data-only
    ///     <a
    ///         href="https://developer.mozilla.org/en-US/docs/Web/API/Server-sent_events/Using_server-sent_events#Event_stream_format">
    ///         server-sent events
    ///     </a>
    ///     as they become available, with the stream terminated by a data: [DONE] message.
    /// </summary>
    [JsonPropertyName("stream")]
    public bool? Stream { get; set; }

    /// <summary>
    ///     Echo back the prompt in addition to the completion
    /// </summary>
    [JsonPropertyName("echo")]
    public bool? Echo { get; set; }

    /// <summary>
    ///     Up to 4 sequences where the API will stop generating further tokens. The returned text will not contain the stop
    ///     sequence.
    /// </summary>
    [JsonIgnore]
    public string? Stop { get; set; }

    /// <summary>
    ///     Up to 4 sequences where the API will stop generating further tokens. The returned text will not contain the stop
    ///     sequence.
    /// </summary>
    [JsonIgnore]
    public IList<string>? StopAsList { get; set; }

    [JsonPropertyName("stop")]
    public IList<string>? StopCalculated
    {
        get
        {
            if (Stop != null && StopAsList != null)
            {
                throw new ValidationException("Stop and StopAsList can not be assigned at the same time. One of them is should be null.");
            }

            if (Stop != null)
            {
                return new List<string> {Stop};
            }

            return StopAsList;
        }
    }

    /// <summary>
    ///     Number between -2.0 and 2.0. Positive values penalize new tokens based on whether they appear in the text so far,
    ///     increasing the model's likelihood to talk about new topics.
    /// </summary>
    /// <seealso href="https://platform.openai.com/docs/api-reference/parameter-details" />
    [JsonPropertyName("presence_penalty")]
    public float? PresencePenalty { get; set; }


    /// <summary>
    ///     Number between -2.0 and 2.0. Positive values penalize new tokens based on their existing frequency in the text so
    ///     far, decreasing the model's likelihood to repeat the same line verbatim.
    /// </summary>
    /// <seealso href="https://platform.openai.com/docs/api-reference/parameter-details" />
    [JsonPropertyName("frequency_penalty")]
    public float? FrequencyPenalty { get; set; }

    /// <summary>
    ///     Generates best_of completions server-side and returns the "best" (the one with the lowest log probability per
    ///     token). Results cannot be streamed.
    ///     When used with n, best_of controls the number of candidate completions and n specifies how many to return – best_of
    ///     must be greater than n.
    ///     Note: Because this parameter generates many completions, it can quickly consume your token quota.Use carefully and
    ///     ensure that you have reasonable settings for max_tokens and stop.
    /// </summary>
    [JsonPropertyName("best_of")]
    public int? BestOf { get; set; }

    /// <summary>
    ///     Modify the likelihood of specified tokens appearing in the completion.
    ///     Accepts a json object that maps tokens(specified by their token ID in the GPT tokenizer) to an associated bias
    ///     value from -100 to 100. You can use this tokenizer tool (which works for both GPT-2 and GPT-3) to convert text to
    ///     token IDs. Mathematically, the bias is added to the logits generated by the model prior to sampling. The exact
    ///     effect will vary per model, but values between -1 and 1 should decrease or increase likelihood of selection; values
    ///     like -100 or 100 should result in a ban or exclusive selection of the relevant token.
    ///     As an example, you can pass { "50256": -100}
    ///     to prevent the endoftext token from being generated.
    /// </summary>
    /// <seealso href="https://platform.openai.com/tokenizer?view=bpe" />
    [JsonPropertyName("logit_bias")]
    public object? LogitBias { get; set; }

    /// <summary>
    ///     Include the log probabilities on the logprobs most likely tokens, as well the chosen tokens. For example, if
    ///     logprobs is 5, the API will return a list of the 5 most likely tokens. The API will always return the logprob of
    ///     the sampled token, so there may be up to logprobs+1 elements in the response.
    ///     The maximum value for logprobs is 5. If you need more than this, please contact support@openai.com and describe
    ///     your use case.
    /// </summary>
    [JsonPropertyName("logprobs")]
    public int? LogProbs { get; set; }

    [JsonPropertyName("model")] public string? Model { get; set; }

    public IEnumerable<ValidationResult> Validate()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    ///     What
    ///     <a href="https://towardsdatascience.com/how-to-sample-from-language-models-682bceb97277">sampling temperature</a>
    ///     to use. Higher values means the model will take more risks. Try 0.9 for more creative
    ///     applications, and 0 (argmax sampling) for ones with a well-defined answer.
    ///     We generally recommend altering this or top_p but not both.
    /// </summary>
    /// <see href="https://platform.openai.com/docs/api-reference/completions/create#completions/create-temperature" />
    [JsonPropertyName("temperature")]
    public float? Temperature { get; set; }

    /// <summary>
    ///     A unique identifier representing your end-user, which will help OpenAI to monitor and detect abuse.
    /// </summary>
    [JsonPropertyName("user")]
    public string? User { get; set; }
}