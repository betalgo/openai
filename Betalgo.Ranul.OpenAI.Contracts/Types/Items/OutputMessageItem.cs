using System.Text.Json;
using System.Text.Json.Serialization;
using Betalgo.Ranul.OpenAI.Contracts.Enums.Responses;
using Betalgo.Ranul.OpenAI.Contracts.Types.Content;

namespace Betalgo.Ranul.OpenAI.Contracts.Types.Items;

/// <summary>
///     An output message from the model.
///     <see href="https://platform.openai.com/docs/api-reference/responses">OpenAI API documentation</see>.
///     <see href="https://github.com/betalgo/openai/blob/master/Docs/openapi-split/components/schemas/outputmessage.yml">
///         Source Definition
///     </see>
/// </summary>
public class OutputMessageItem : IOutputItem
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="OutputMessageItem" /> class.
    /// </summary>
    public OutputMessageItem()
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="OutputMessageItem" /> class with required parameters.
    /// </summary>
    /// <param name="id">The unique ID of the output message.</param>
    /// <param name="content">The content of the output message.</param>
    /// <param name="status">The status of the message.</param>
    public OutputMessageItem(string id, List<IContent> content, ItemStatus status)
    {
        Id = id;
        Content = content;
        Status = status;
    }

    /// <summary>
    ///     The unique ID of the output message.
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; set; } = null!;

    /// <summary>
    ///     The type of the output message. Always <c>message</c>.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type => "message";

    /// <summary>
    ///     The role of the output message. Always <c>assistant</c>.
    /// </summary>
    [JsonPropertyName("role")]
    public string Role => "assistant";

    /// <summary>
    ///     The content of the output message.
    /// </summary>
    [JsonPropertyName("content")]
    public List<IContent> Content { get; set; } = new();

    /// <summary>
    ///     The status of the message input. One of <c>in_progress</c>, <c>completed</c>, or <c>incomplete</c>.
    ///     Populated when input items are returned via API.
    /// </summary>
    [JsonPropertyName("status")]
    public ItemStatus Status { get; set; }
    /// <summary>Assistant message phase, retained exactly when replaying history. Omit on user messages.</summary>
    [JsonPropertyName("phase")]
    public string? Phase { get; set; }

    /// <summary>Additional message metadata retained across response-history replay.</summary>
    [JsonExtensionData]
    public Dictionary<string, JsonElement>? AdditionalProperties { get; set; }

}

