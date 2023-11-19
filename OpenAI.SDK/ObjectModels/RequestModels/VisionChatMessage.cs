using System.Text.Json.Serialization;

namespace OpenAI.ObjectModels.RequestModels;

/// <summary>
///     The contents of the message.
///     Messages must be an array of message objects, where each object has a role (either “system”, “user”, or
///     “assistant”) and content (the content of the message) and an optional name
/// </summary>
public class VisionChatMessage
{
    /// <summary>
    /// </summary>
    /// <param name="role">The role of the author of this message. One of system, user, or assistant.</param>
    /// <param name="content">The contents of the vision message.</param>
    public VisionChatMessage(string role, IList<VisionContent> content)
    {
        Role = role;
        Content = content;
    }

    /// <summary>
    /// </summary>
    /// <param name="role">The role of the author of this message. One of system, user, or assistant.</param>
    /// <param name="content">The contents of the vision message.</param>
    public VisionChatMessage(string role, VisionContent content)
    {
        Role = role;
        Content = new List<VisionContent>() {content};
    }

    /// <summary>
    ///     The role of the author of this message. One of system, user, or assistant.
    /// </summary>
    [JsonPropertyName("role")]
    public string Role { get; set; }

    /// <summary>
    ///     The contents of the message.
    ///     
    ///     The Chat Completions API is capable of taking in and processing multiple image inputs in both base64 encoded format or as an image URL. 
    ///     The model will process each image and use the information from all of them to answer the question.
    ///     
    ///     The content must contain a "text" type content and at least one "image_url" type content
    /// </summary>
    [JsonPropertyName("content")]
    public IList<VisionContent> Content { get; set; }

    public static VisionChatMessage FromAssistant(VisionContent content)
    {
        return new VisionChatMessage(StaticValues.ChatMessageRoles.Assistant, content);
    }

    public static VisionChatMessage FromUser(IList<VisionContent> content)
    {
        return new VisionChatMessage(StaticValues.ChatMessageRoles.User, content);
    }

    public static VisionChatMessage FromSystem(VisionContent content)
    {
        return new VisionChatMessage(StaticValues.ChatMessageRoles.System, content);
    }
}