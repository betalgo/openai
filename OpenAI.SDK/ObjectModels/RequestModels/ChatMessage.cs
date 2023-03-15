using System.Text.Json.Serialization;

namespace OpenAI.GPT3.ObjectModels.RequestModels;

/// <summary>
///     Messages must be an array of message objects, where each object has a role (either “system”, “user”, or
///     “assistant”) and content (the content of the message) and an optional name
/// </summary>
public class ChatMessage
{
    public ChatMessage(string role, string content, string? name = null)
    {
        Role = role;
        Content = content;
        Name = name;
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

    /// <summary>
    ///     Optional Message user Name
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    public static ChatMessage FromAssistant(string content)
    {
        return new ChatMessage(StaticValues.ChatMessageRoles.Assistant, content);
    }

    public static ChatMessage FromAssistant(string name, string content)
    {
        return new ChatMessage(StaticValues.ChatMessageRoles.Assistant, content, name);
    }

    public static ChatMessage FromUser(string content)
    {
        return new ChatMessage(StaticValues.ChatMessageRoles.User, content);
    }

    public static ChatMessage FromUser(string name, string content)
    {
        return new ChatMessage(StaticValues.ChatMessageRoles.User, content, name);
    }

    public static ChatMessage FromSystem(string content)
    {
        return new ChatMessage(StaticValues.ChatMessageRoles.System, content);
    }
    public static ChatMessage FromSystem(string name, string content)
    {
        return new ChatMessage(StaticValues.ChatMessageRoles.System, content, name);
    }
}