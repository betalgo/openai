﻿using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Betalgo.Ranul.OpenAI.Contracts.Enums;
using Betalgo.Ranul.OpenAI.Interfaces;
using Betalgo.Ranul.OpenAI.ObjectModels.SharedModels;

namespace Betalgo.Ranul.OpenAI.ObjectModels.RequestModels;

public class ChatCompletionCreateRequest : IModelValidate, IOpenAIModels.ITemperature, IOpenAIModels.IModel, IOpenAIModels.IUser
{
    public enum ResponseFormats
    {
        Text,
        Json,
        JsonSchema
    }


    /// <summary>
    ///     The messages to generate chat completions for, in the chat format.
    ///     The main input is the messages parameter. Messages must be an array of message objects, where each object has a
    ///     role (either “system”, “user”, or “assistant”) and content (the content of the message). Conversations can be as
    ///     short as 1 message or fill many pages.
    /// </summary>
    [JsonPropertyName("messages")]
    public IList<ChatMessage> Messages { get; set; }

    /// <summary>
    ///     An alternative to sampling with temperature, called nucleus sampling, where the model considers the results of the
    ///     tokens with top_p probability mass. So 0.1 means only the tokens comprising the top 10% probability mass are
    ///     considered.
    ///     We generally recommend altering this or temperature but not both.
    /// </summary>
    [JsonPropertyName("top_p")]
    public float? TopP { get; set; }

    /// <summary>
    ///     How many chat completion choices to generate for each input message.
    /// </summary>
    [JsonPropertyName("n")]
    public int? N { get; set; }

    /// <summary>
    ///     If set, partial message deltas will be sent, like in ChatGPT. Tokens will be sent as data-only server-sent events
    ///     as they become available, with the stream terminated by a data: [DONE] message.
    /// </summary>
    [JsonPropertyName("stream")]
    public bool? Stream { get; set; }

    /// <summary>
    ///     Options for streaming response. Only set this when you set <see cref="Stream" />: <c>true</c>.
    /// </summary>
    [JsonPropertyName("stream_options")]
    public StreamOptions? StreamOptions { get; set; }

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
                return new List<string> { Stop };
            }

            return StopAsList;
        }
    }

    /// <summary>
    ///     The maximum number of tokens allowed for the generated answer. By default, the number of tokens the model can
    ///     return will be (4096 - prompt tokens).
    /// </summary>
    /// <see href="https://platform.openai.com/docs/api-reference/completions/create#completions/create-max_tokens" />
    [JsonPropertyName("max_tokens")]
    public int? MaxTokens { get; set; }


    /// <summary>
    ///     An upper bound for the number of tokens that can be generated for a completion,
    ///     including visible output tokens and reasoning tokens.
    /// </summary>
    /// <see href="https://platform.openai.com/docs/api-reference/chat/create#chat-create-max_completion_tokens" />
    [JsonPropertyName("max_completion_tokens")]
    public int? MaxCompletionTokens { get; set; }

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
    ///     A list of functions the model may generate JSON inputs for.
    /// </summary>
    [JsonIgnore]
    public IList<ToolDefinition>? Tools { get; set; }


    [JsonIgnore]
    public object? ToolsAsObject { get; set; }

    /// <summary>
    ///     A list of tools the model may call. Currently, only functions are supported as a tool. Use this to provide a list
    ///     of functions the model may generate JSON inputs for.
    /// </summary>
    [JsonPropertyName("tools")]
    public object? ToolsCalculated
    {
        get
        {
            if (ToolsAsObject != null && Tools != null)
            {
                throw new ValidationException("ToolsAsObject and Tools can not be assigned at the same time. One of them is should be null.");
            }

            return Tools ?? ToolsAsObject;
        }
    }

    /// <summary>
    ///     Controls which (if any) function is called by the model. none means the model will not call a function and instead
    ///     generates a message. auto means the model can pick between generating a message or calling a function. Specifying
    ///     a particular function via {"type: "function", "function": {"name": "my_function"}} forces the model to call that
    ///     function.
    ///     none is the default when no functions are present. auto is the default if functions are present.
    /// </summary>
    [JsonIgnore]
    public ToolChoice? ToolChoice { get; set; }

    [JsonPropertyName("tool_choice")]
    public object? ToolChoiceCalculated
    {
        get
        {
            if (ToolChoice != null && ToolChoice.Type != ToolChoiceType.Function && ToolChoice.Function != null)
            {
                throw new ValidationException("You cannot choose another type besides \"function\" while ToolChoice.Function is not null.");
            }

            if (ToolChoice?.Type == ToolChoiceType.Function)
            {
                return ToolChoice;
            }

            return ToolChoice?.Type;
        }
    }

    /// <summary>
    ///     The format that the model must output. Used to enable JSON mode.
    ///     Must be one of "text" or "json_object".<br />
    /// </summary>
    [JsonPropertyName("response_format")]
    public ResponseFormat? ResponseFormat { get; set; }

    /// <summary>
    ///     This feature is in Beta. If specified, our system will make a best effort to sample deterministically, such that
    ///     repeated requests with the same seed and parameters should return the same result. Determinism is not guaranteed,
    ///     and you should refer to the system_fingerprint response parameter to monitor changes in the backend.
    /// </summary>
    [JsonPropertyName("seed")]
    public int? Seed { get; set; }

    /// <summary>
    ///     Whether to return log probabilities of the output tokens or not. If true, returns the log probabilities of each
    ///     output token returned in the content of message.
    /// </summary>
    [JsonPropertyName("logprobs")]
    public bool? LogProbs { get; set; }


    /// <summary>
    ///     An integer between 0 and 20 specifying the number of most likely tokens to return at each token position, each with
    ///     an associated log probability. logprobs must be set to true if this parameter is used.
    /// </summary>
    [JsonPropertyName("top_logprobs")]
    public int? TopLogprobs { get; set; }

    /// <summary>
    ///     Whether to enable parallel <a href="https://platform.openai.com/docs/guides/function-calling/parallel-function-calling">function calling</a> during tool use.
    /// </summary>
    [JsonPropertyName("parallel_tool_calls")]
    public bool? ParallelToolCalls { get; set; }

    /// <summary>
    ///     ID of the model to use. For models supported see <see cref="OpenAI.ObjectModels.Models" /> start with <c>Gpt_</c>
    /// </summary>
    [JsonPropertyName("model")]
    public string? Model { get; set; }

    /// <summary>
    ///     Constrains effort on reasoning for reasoning models. Currently supported values are low, medium, and high.
    ///     Reducing reasoning effort can result in faster responses and fewer tokens used on reasoning in a response.
    /// </summary>
    [JsonPropertyName("reasoning_effort")]
    public ReasoningEffort? ReasoningEffort { get; set; }

    public IEnumerable<ValidationResult> Validate()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    ///     What sampling temperature to use, between 0 and 2. Higher values like 0.8 will make the output more random, while
    ///     lower values like 0.2 will make it more focused and deterministic.
    ///     We generally recommend altering this or top_p but not both.
    /// </summary>
    [JsonPropertyName("temperature")]
    public float? Temperature { get; set; }

    /// <summary>
    ///     A unique identifier representing your end-user, which can help OpenAI to monitor and detect abuse. Learn more.
    /// </summary>
    [JsonPropertyName("user")]
    public string User { get; set; }

    /// <summary>
    /// Specifies the latency tier to use for processing the request. This parameter is relevant for customers subscribed to the scale tier service:
    /// If set to 'auto', and the Project is Scale tier enabled, the system will utilize scale tier credits until they are exhausted.
    /// If set to 'auto', and the Project is not Scale tier enabled, the request will be processed using the default service tier with a lower uptime SLA and no latency guarentee.
    /// If set to 'default', the request will be processed using the default service tier with a lower uptime SLA and no latency guarentee.
    /// When not set, the default behavior is 'auto'.
    /// When this parameter is set, the response body will include the service_tier utilized.
    /// </summary>
    [JsonPropertyName("service_tier")]
    public string? ServiceTier { get; set; }


    /// <summary>
    /// Whether or not to store the output of this chat completion request for use in our model distillation or evals products.
    /// https://platform.openai.com/docs/api-reference/chat/create?lang=python#chat-create-store
    /// 
    /// <para /> 
    /// more about distillation: https://platform.openai.com/docs/guides/distillation
    /// <para /> 
    /// more about evals: https://platform.openai.com/docs/guides/evals
    /// </summary>
    [JsonPropertyName("store")]
    public bool? Store { get; set; }
}
