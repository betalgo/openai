using System.Text.Json.Serialization;
using Betalgo.Ranul.OpenAI.Contracts.Enums.Responses;
using Betalgo.Ranul.OpenAI.Contracts.Interfaces;
using Betalgo.Ranul.OpenAI.Contracts.Types;
using Betalgo.Ranul.OpenAI.Contracts.Types.ToolChoice;
using Betalgo.Ranul.OpenAI.Contracts.Types.Tools;

namespace Betalgo.Ranul.OpenAI.Contracts.Requests.Responses;

/// <summary>
///     Request parameters for counting input tokens before creating a response.
///     <see href="https://platform.openai.com/docs/api-reference/responses/input-tokens">OpenAI API documentation</see>.
///     <see href="https://github.com/betalgo/openai/blob/master/Docs/openapi-split/components/schemas/tokencountsbody.yml">
///         Source Definition
///     </see>
/// </summary>
public class TokenCountsRequest : IRequest
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="TokenCountsRequest" /> class.
    /// </summary>
    public TokenCountsRequest()
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="TokenCountsRequest" /> class with a model.
    /// </summary>
    /// <param name="model">Model ID used to generate the response.</param>
    public TokenCountsRequest(string model)
    {
        Model = model;
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="TokenCountsRequest" /> class with a model and text input.
    /// </summary>
    /// <param name="model">Model ID used to generate the response.</param>
    /// <param name="input">The text input to the model.</param>
    public TokenCountsRequest(string model, string input)
    {
        Model = model;
        Input = input;
    }

    /// <summary>
    ///     Model ID used to generate the response, like <c>gpt-4o</c> or <c>o3</c>.
    ///     OpenAI offers a wide range of models with different capabilities, performance characteristics, and price points.
    ///     <see href="https://platform.openai.com/docs/models">Refer to the model guide</see>.
    /// </summary>
    [JsonPropertyName("model")]
    public string? Model { get; set; }

    /// <summary>
    ///     Text, image, or file inputs to the model, used to generate a response.
    /// </summary>
    [JsonPropertyName("input")]
    public InputParam? Input { get; set; }

    /// <summary>
    ///     The unique ID of the previous response to the model. Use this to create multi-turn conversations.
    ///     <see href="https://platform.openai.com/docs/guides/conversation-state">Learn more about conversation state</see>.
    ///     Cannot be used in conjunction with <see cref="Conversation" />.
    /// </summary>
    [JsonPropertyName("previous_response_id")]
    public string? PreviousResponseId { get; set; }

    /// <summary>
    ///     An array of tools the model may call while generating a response.
    ///     You can specify which tool to use by setting the <see cref="ToolChoice" /> parameter.
    /// </summary>
    [JsonPropertyName("tools")]
    public List<ITool>? Tools { get; set; }

    /// <summary>
    ///     Configuration for the response's text format.
    /// </summary>
    [JsonPropertyName("text")]
    public TextConfig? Text { get; set; }

    /// <summary>
    ///     Configuration for <see href="https://platform.openai.com/docs/guides/reasoning">reasoning models</see>.
    /// </summary>
    [JsonPropertyName("reasoning")]
    public ReasoningConfig? Reasoning { get; set; }

    /// <summary>
    ///     The truncation strategy to use for the model response.
    /// </summary>
    [JsonPropertyName("truncation")]
    public TruncationStrategy? Truncation { get; set; }

    /// <summary>
    ///     A system (or developer) message inserted into the model's context.
    ///     When using along with <see cref="PreviousResponseId" />, the instructions from a previous
    ///     response will not be carried over to the next response.
    /// </summary>
    [JsonPropertyName("instructions")]
    public string? Instructions { get; set; }

    /// <summary>
    ///     The conversation that this response belongs to. Items from this conversation are prepended to input items
    ///     for this response request. Cannot be used in conjunction with <see cref="PreviousResponseId" />.
    /// </summary>
    [JsonPropertyName("conversation")]
    public ConversationParam? Conversation { get; set; }

    /// <summary>
    ///     How the model should select which tool to call.
    ///     <see href="https://platform.openai.com/docs/guides/function-calling">Learn more about function calling</see>.
    /// </summary>
    [JsonPropertyName("tool_choice")]
    public ToolChoice? ToolChoice { get; set; }

    /// <summary>
    ///     Whether to allow the model to run tool calls in parallel.
    /// </summary>
    [JsonPropertyName("parallel_tool_calls")]
    public bool? ParallelToolCalls { get; set; }
}

