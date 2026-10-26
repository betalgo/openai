using System.Text.Json;
using System.Text.Json.Serialization;
using Betalgo.Ranul.OpenAI.Contracts.Enums.Responses;
using Betalgo.Ranul.OpenAI.Contracts.Types.Content;

namespace Betalgo.Ranul.OpenAI.Contracts.Types.Items;

/// <summary>
///     A message input to the model with a role indicating instruction following hierarchy.
///     Instructions given with the <c>developer</c> or <c>system</c> role take precedence over instructions given with the
///     <c>user</c> role.
///     <see href="https://platform.openai.com/docs/api-reference/responses">OpenAI API documentation</see>.
///     <see href="https://github.com/betalgo/openai/blob/master/Docs/openapi-split/components/schemas/inputmessage.yml">
///         Source Definition
///     </see>
/// </summary>
public class InputMessageItem : IInputItem
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="InputMessageItem" /> class.
    /// </summary>
    public InputMessageItem()
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="InputMessageItem" /> class with required parameters.
    /// </summary>
    /// <param name="role">The role of the message input.</param>
    /// <param name="content">The content of the message.</param>
    public InputMessageItem(MessageRole role, List<IContent> content)
    {
        Role = role;
        Content = content;
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="InputMessageItem" /> class with simple text content.
    /// </summary>
    /// <param name="role">The role of the message input.</param>
    /// <param name="text">The text content of the message.</param>
    public InputMessageItem(MessageRole role, string text)
    {
        Role = role;
        Content = new List<IContent> { new InputTextContent(text) };
    }

    /// <summary>
    ///     The type of the message input. Always set to <c>message</c>.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type => "message";

    /// <summary>
    ///     The role of the message input. One of <c>user</c>, <c>system</c>, or <c>developer</c>.
    /// </summary>
    [JsonPropertyName("role")]
    public MessageRole Role { get; set; }

    /// <summary>
    ///     The content of the message.
    /// </summary>
    [JsonPropertyName("content")]
    public List<IContent>? Content { get; set; }

    /// <summary>
    ///     The status of item. One of <c>in_progress</c>, <c>completed</c>, or <c>incomplete</c>.
    ///     Populated when items are returned via API.
    /// </summary>
    [JsonPropertyName("status")]
    public ItemStatus? Status { get; set; }
    /// <summary>Assistant message phase, retained exactly when replaying history. Omit on user messages.</summary>
    [JsonPropertyName("phase")]
    public string? Phase { get; set; }

    /// <summary>Additional message metadata retained across response-history replay.</summary>
    [JsonExtensionData]
    public Dictionary<string, JsonElement>? AdditionalProperties { get; set; }

    /// <summary>The server-assigned message ID, retained when replaying response output.</summary>
    [JsonPropertyName("id")]
    public string? Id { get; set; }

}

