using System.Text.Json.Serialization;
using Betalgo.Ranul.OpenAI.Contracts.Enums;
using Betalgo.Ranul.OpenAI.ObjectModels.SharedModels;

namespace Betalgo.Ranul.OpenAI.ObjectModels.RequestModels;

public class MessageCreateRequest : IOpenAIModels.IMetaData
{
    public MessageCreateRequest()
    {
    }

    public MessageCreateRequest(AssistantMessageRole role, MessageContentOneOfType content, List<Attachment>? attachments = null, Dictionary<string, string>? metaData = null)
    {
        Role = role;
        Content = content;
        Attachments = attachments;
        Metadata = metaData;
    }

    /// <summary>
    ///     The role of the entity that is creating the message.
    ///     Currently only user is supported.
    /// </summary>
    [JsonPropertyName("role")]
    public AssistantMessageRole Role { get; set; } = AssistantMessageRole.User;

    /// <summary>
    ///     The content of the message.
    /// </summary>
    [JsonPropertyName("content")]

    public MessageContentOneOfType Content { get; set; }

    /// <summary>
    ///     A list of files attached to the message, and the tools they should be added to.
    /// </summary>
    [JsonPropertyName("attachments")]
    public List<Attachment>? Attachments { get; set; }

    /// <summary>
    ///     Set of 16 key-value pairs that can be attached to an object.
    ///     This can be useful for storing additional information about the object in a structured format.
    ///     Keys can be a maximum of 64 characters long and values can be a maxium of 512 characters long.
    /// </summary>
    [JsonPropertyName("metadata")]
    public Dictionary<string, string>? Metadata { get; set; }
}

public class Attachment
{
    /// <summary>
    ///     The ID of the file to attach.
    ///     <a href="https://platform.openai.com/docs/assistants/tools/file-search/supported-files">
    ///         See list of supported file
    ///         extensions
    ///     </a>
    ///     .
    /// </summary>
    [JsonPropertyName("file_id")]
    public string FileId { get; set; }

    /// <summary>
    ///     The tools to add this file to.
    /// </summary>
    [JsonPropertyName("tools")]
    public List<ToolDefinition> Tools { get; set; }
}