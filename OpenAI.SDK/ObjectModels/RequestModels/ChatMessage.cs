using System.Text.Json.Serialization;

namespace OpenAI.GPT3.ObjectModels.RequestModels;

/// <summary>
///     Messages must be an array of message objects, where each object has a role (either “system”, “user”, or
///     “assistant”) and content (the content of the message)
/// </summary>
public class ChatMessage
{
    public ChatMessage(string role, string content)
    {
        Role = role;
        Content = content;
    }

    /// <summary>
    ///     “system”, “user”, or “assistant”
    /// </summary>
    [JsonPropertyName("role")]
    public string Role { get; set; }

    /// <summary>
    ///     Message Content
    /// </summary>
    [JsonPropertyName("content")]
    public string Content { get; set; }

    public static ChatMessage FromAssistant(string content)
    {
        return new ChatMessage(StaticValues.ChatMessageRoles.Assistant, content);
    }

    public static ChatMessage FromUser(string content)
    {
        return new ChatMessage(StaticValues.ChatMessageRoles.User, content);
    }

    public static ChatMessage FromSystem(string content)
    {
        return new ChatMessage(StaticValues.ChatMessageRoles.System, content);
    }
}