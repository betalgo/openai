using System.Text.Json.Serialization;
using Betalgo.Ranul.OpenAI.Contracts.Enums.Responses;
using Betalgo.Ranul.OpenAI.Contracts.Interfaces;
using Betalgo.Ranul.OpenAI.Contracts.Types;
using Betalgo.Ranul.OpenAI.Contracts.Types.ToolChoice;
using Betalgo.Ranul.OpenAI.Contracts.Types.Tools;

#pragma warning disable CS0618 // Type or member is obsolete

namespace Betalgo.Ranul.OpenAI.Contracts.Requests.Responses;

/// <summary>
///     Creates a model response. Provide <see cref="Input" /> as text or a list of content items, and the model
///     will generate a response based on the instructions and conversation history.
///     <see href="https://platform.openai.com/docs/api-reference/responses/create">OpenAI API documentation</see>.
///     <see href="https://github.com/betalgo/openai/blob/master/Docs/openapi-split/components/schemas/createresponse.yml">
///         Source Definition
///     </see>
/// </summary>
public class CreateResponse : IRequest
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="CreateResponse" /> class.
    /// </summary>
    public CreateResponse()
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="CreateResponse" /> class with required parameters.
    /// </summary>
    /// <param name="model">Model ID used to generate the response.</param>
    public CreateResponse(string model)
    {
        Model = model;
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="CreateResponse" /> class with a model and text input.
    /// </summary>
    /// <param name="model">Model ID used to generate the response.</param>
    /// <param name="input">The text input to the model.</param>
    public CreateResponse(string model, string input)
    {
        Model = model;
        Input = input;
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="CreateResponse" /> class with a model and list of input items.
    /// </summary>
    /// <param name="model">Model ID used to generate the response.</param>
    /// <param name="input">The list of input items to the model.</param>
    public CreateResponse(string model, List<IInputItem> input)
    {
        Model = model;
        Input = new InputParam(input);
    }

    /// <summary>
    ///     Model ID used to generate the response, like <c>gpt-4o</c> or <c>o1</c>.
    ///     OpenAI offers a wide range of models with different capabilities, performance characteristics, and price points.
    ///     <see href="https://platform.openai.com/docs/models">Learn more about models</see>.
    /// </summary>
    [JsonPropertyName("model")]
    public string Model { get; set; } = null!;

    /// <summary>
    ///     Text, image, or file inputs to the model, used to generate a response.
    ///     <see href="https://platform.openai.com/docs/guides/text">Text inputs</see>,
    ///     <see href="https://platform.openai.com/docs/guides/images">Image inputs</see>,
    ///     <see href="https://platform.openai.com/docs/guides/pdf-files">File inputs</see>.
    /// </summary>
    [JsonPropertyName("input")]
    public InputParam? Input { get; set; }

    /// <summary>
    ///     Specify additional output data to include in the model response.
    /// </summary>
    [JsonPropertyName("include")]
    public List<ResponsesInclude>? Include { get; set; }

    /// <summary>
    ///     A system (or developer) message inserted into the model's context.
    ///     When using along with <see cref="PreviousResponseId" />, the instructions from a previous
    ///     response will not be carried over to the next response.
    /// </summary>
    [JsonPropertyName("instructions")]
    public string? Instructions { get; set; }

    /// <summary>
    ///     An upper bound for the number of tokens that can be generated for a response,
    ///     including visible output tokens and
    ///     <see href="https://platform.openai.com/docs/guides/reasoning">reasoning tokens</see>.
    /// </summary>
    [JsonPropertyName("max_output_tokens")]
    public int? MaxOutputTokens { get; set; }

    /// <summary>
    ///     The maximum number of total calls to built-in tools that can be processed in a response.
    ///     This maximum number applies across all built-in tool calls, not per individual tool.
    /// </summary>
    [JsonPropertyName("max_tool_calls")]
    public int? MaxToolCalls { get; set; }

    /// <summary>
    ///     Set of 16 key-value pairs that can be attached to an object.
    ///     Keys must be strings with a maximum length of 64 characters.
    ///     Values must be strings with a maximum length of 512 characters.
    /// </summary>
    [JsonPropertyName("metadata")]
    public Dictionary<string, string>? Metadata { get; set; }

    /// <summary>
    ///     Whether to allow the model to run tool calls in parallel. Defaults to <c>true</c>.
    /// </summary>
    [JsonPropertyName("parallel_tool_calls")]
    public bool? ParallelToolCalls { get; set; }

    /// <summary>
    ///     The unique ID of the previous response to the model. Use this to create multi-turn conversations.
    ///     <see href="https://platform.openai.com/docs/guides/conversation-state">Learn more about conversation state</see>.
    /// </summary>
    [JsonPropertyName("previous_response_id")]
    public string? PreviousResponseId { get; set; }

    /// <summary>
    ///     Configuration for
    ///     <see href="https://platform.openai.com/docs/guides/reasoning">reasoning models</see>.
    /// </summary>
    [JsonPropertyName("reasoning")]
    public ReasoningConfig? Reasoning { get; set; }

    /// <summary>
    ///     Whether to store the generated model response for later retrieval via API. Defaults to <c>true</c>.
    /// </summary>
    [JsonPropertyName("store")]
    public bool? Store { get; set; }

    /// <summary>
    ///     If set to true, the model response data will be streamed to the client as it is generated using server-sent events.
    /// </summary>
    [JsonPropertyName("stream")]
    public bool? Stream { get; set; }

    /// <summary>
    ///     What sampling temperature to use, between 0 and 2. Higher values like 0.8 will make the output more random,
    ///     while lower values like 0.2 will make it more focused and deterministic.
    /// </summary>
    [JsonPropertyName("temperature")]
    public double? Temperature { get; set; }

    /// <summary>
    ///     Configuration for the response's text format.
    /// </summary>
    [JsonPropertyName("text")]
    public TextConfig? Text { get; set; }

    /// <summary>
    ///     How the model should select which tool to call. See the
    ///     <see href="https://platform.openai.com/docs/guides/function-calling">function calling guide</see> for more
    ///     information.
    /// </summary>
    [JsonPropertyName("tool_choice")]
    public ToolChoice? ToolChoice { get; set; }

    /// <summary>
    ///     An array of tools the model may call while generating a response.
    /// </summary>
    [JsonPropertyName("tools")]
    public List<ITool>? Tools { get; set; }

    /// <summary>
    ///     An integer between 0 and 20 specifying the number of most likely tokens to return at each token position.
    /// </summary>
    [JsonPropertyName("top_logprobs")]
    public int? TopLogprobs { get; set; }

    /// <summary>
    ///     An alternative to sampling with temperature, called nucleus sampling,
    ///     where the model considers the results of the tokens with top_p probability mass.
    /// </summary>
    [JsonPropertyName("top_p")]
    public double? TopP { get; set; }

    /// <summary>
    ///     The truncation strategy to use for the model response.
    ///     <c>auto</c>: If the input exceeds the context window, truncate by dropping items from the beginning.
    ///     <c>disabled</c> (default): Fail with a 400 error if the input size exceeds the context window.
    /// </summary>
    [JsonPropertyName("truncation")]
    public TruncationStrategy? Truncation { get; set; }

    /// <summary>
    ///     A unique identifier for your end-user.
    ///     This field is being replaced by <see cref="SafetyIdentifier" /> and <see cref="PromptCacheKey" />.
    ///     Use <see cref="PromptCacheKey" /> instead to maintain caching optimizations.
    ///     <see href="https://platform.openai.com/docs/guides/safety-best-practices#end-user-ids">Learn more</see>.
    /// </summary>
    [Obsolete("This field is being replaced by SafetyIdentifier and PromptCacheKey. Use PromptCacheKey instead to maintain caching optimizations.")]
    [JsonPropertyName("user")]
    public string? User { get; set; }

    /// <summary>
    ///     A stable identifier used to help detect users of your application that may be violating OpenAI's usage policies.
    ///     <see href="https://platform.openai.com/docs/guides/safety-best-practices#safety-identifiers">Learn more</see>.
    /// </summary>
    [JsonPropertyName("safety_identifier")]
    public string? SafetyIdentifier { get; set; }

    /// <summary>
    ///     Used by OpenAI to cache responses for similar requests to optimize your cache hit rates. Replaces the User field.
    ///     <see href="https://platform.openai.com/docs/guides/prompt-caching">Learn more</see>.
    /// </summary>
    [JsonPropertyName("prompt_cache_key")]
    public string? PromptCacheKey { get; set; }

    /// <summary>
    ///     The retention policy for the prompt cache.
    ///     <see href="https://platform.openai.com/docs/guides/prompt-caching#prompt-cache-retention">Learn more</see>.
    /// </summary>
    [JsonPropertyName("prompt_cache_retention")]
    public PromptCacheRetention? PromptCacheRetention { get; set; }

    /// <summary>
    ///     Whether to run the model response in the background.
    ///     <see href="https://platform.openai.com/docs/guides/background">Learn more</see>.
    /// </summary>
    [JsonPropertyName("background")]
    public bool? Background { get; set; }

    /// <summary>
    ///     Specifies the latency tier to use for processing the request.
    ///     <c>auto</c>: System will use scale tier credits if available, otherwise default tier.
    ///     <c>default</c>: Use the default tier with faster response time.
    /// </summary>
    [JsonPropertyName("service_tier")]
    public ServiceTier? ServiceTier { get; set; }

    /// <summary>
    ///     Options for streaming responses. Only set this when you set <c>stream: true</c>.
    /// </summary>
    [JsonPropertyName("stream_options")]
    public ResponsesStreamOptions? StreamOptions { get; set; }

    /// <summary>
    ///     The conversation that this response belongs to. Items from this conversation are prepended to input items
    ///     for this response request. Cannot be used in conjunction with <see cref="PreviousResponseId" />.
    /// </summary>
    [JsonPropertyName("conversation")]
    public ConversationParam? Conversation { get; set; }

    /// <summary>
    ///     Reference to a prompt template and its variables.
    ///     <see href="https://platform.openai.com/docs/guides/text?api-mode=responses#reusable-prompts">Learn more</see>.
    /// </summary>
    [JsonPropertyName("prompt")]
    public PromptConfig? Prompt { get; set; }
}
