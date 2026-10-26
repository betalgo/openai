using System.Text.Json.Serialization;

namespace Betalgo.Ranul.OpenAI.Contracts.Responses.Responses;

/// <summary>
///     The conversation that this response belongs to.
///     <see href="https://platform.openai.com/docs/api-reference/responses/object">OpenAI API documentation</see>.
///     <see href="https://github.com/betalgo/openai/blob/master/Docs/openapi-split/components/schemas/conversation-2.yml">
///         Source Definition
///     </see>
/// </summary>
public class ConversationResponse
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="ConversationResponse" /> class.
    /// </summary>
    public ConversationResponse()
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="ConversationResponse" /> class with the conversation ID.
    /// </summary>
    /// <param name="id">The unique ID of the conversation.</param>
    public ConversationResponse(string id)
    {
        Id = id;
    }

    /// <summary>
    ///     The unique ID of the conversation.
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; set; } = null!;
}

