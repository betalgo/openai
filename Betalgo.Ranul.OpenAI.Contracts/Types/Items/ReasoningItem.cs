using System.Text.Json.Serialization;
using Betalgo.Ranul.OpenAI.Contracts.Enums.Responses;

namespace Betalgo.Ranul.OpenAI.Contracts.Types.Items;

/// <summary>
///     A description of the chain of thought used by a reasoning model while generating a response.
///     Be sure to include these items in your <c>input</c> to the Responses API for subsequent turns of a conversation
///     if you are manually <see href="https://platform.openai.com/docs/guides/conversation-state">managing context</see>.
///     <see href="https://github.com/betalgo/openai/blob/master/Docs/openapi-split/components/schemas/reasoningitem.yml">
///         Source Definition
///     </see>
/// </summary>
public class ReasoningItem : IInputItem, IOutputItem
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="ReasoningItem" /> class.
    /// </summary>
    public ReasoningItem()
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="ReasoningItem" /> class with required parameters.
    /// </summary>
    /// <param name="id">The unique identifier of the reasoning content.</param>
    /// <param name="summary">Reasoning summary content.</param>
    public ReasoningItem(string id, List<SummaryText> summary)
    {
        Id = id;
        Summary = summary;
    }

    /// <summary>
    ///     The type of the object. Always <c>reasoning</c>.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type => "reasoning";

    /// <summary>
    ///     The unique identifier of the reasoning content.
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; set; } = null!;

    /// <summary>
    ///     The encrypted content of the reasoning item - populated when a response is generated with
    ///     <c>reasoning.encrypted_content</c> in the <c>include</c> parameter.
    /// </summary>
    [JsonPropertyName("encrypted_content")]
    public string? EncryptedContent { get; set; }

    /// <summary>
    ///     Reasoning summary content.
    /// </summary>
    [JsonPropertyName("summary")]
    public List<SummaryText> Summary { get; set; } = new();

    /// <summary>
    ///     Reasoning text content.
    /// </summary>
    [JsonPropertyName("content")]
    public List<ReasoningTextContent>? Content { get; set; }

    /// <summary>
    ///     The status of the item. One of <c>in_progress</c>, <c>completed</c>, or <c>incomplete</c>.
    ///     Populated when items are returned via API.
    /// </summary>
    [JsonPropertyName("status")]
    public ItemStatus? Status { get; set; }
}
