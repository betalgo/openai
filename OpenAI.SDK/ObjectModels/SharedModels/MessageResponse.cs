using System.Text.Json.Serialization;
using OpenAI.ObjectModels.RequestModels;
using OpenAI.ObjectModels.ResponseModels;

namespace OpenAI.ObjectModels.SharedModels;

/// <summary>
///     Represents a message within a thread.
/// </summary>
public record MessageResponse : BaseResponse, IOpenAiModels.IId, IOpenAiModels.ICreatedAt, IOpenAiModels.IMetaData,IOpenAiModels.IAssistantId
{
    /// <summary>
    ///     The thread ID that this message belongs to.
    /// </summary>
    [JsonPropertyName("thread_id")]
    public string ThreadId { get; set; }

    /// <summary>
    ///     The entity that produced the message. One of user or assistant.
    /// </summary>
    [JsonPropertyName("role")]
    public string Role { get; set; }

    /// <summary>
    ///     The content of the message in array of text and/or images.
    /// </summary>
    [JsonPropertyName("content")]
    public List<MessageContentResponse>? Content { get; set; }

    /// <summary>
    ///     The status of the message, which can be either in_progress, incomplete, or completed.
    /// </summary>
    [JsonPropertyName("status")]
    public string Status { get; set; }


    /// <summary>
    ///    On an incomplete message, details about why the message is incomplete.
    /// </summary>
    [JsonPropertyName("incomplete_details")]
    public IncompleteDetails? IncompleteDetails { get; set; }

    /// <summary>
    ///     The Unix timestamp (in seconds) for when the run was completed.
    /// </summary>
    [JsonPropertyName("completed_at")]
    public int? CompletedAt { get; set; }
    
    /// <summary>
    ///     The Unix timestamp (in seconds) for when the run was completed.
    /// </summary>
    [JsonPropertyName("incomplete_at")]
    public int? IncompleteAt { get; set; }

    /// <summary>
    ///     If applicable, the ID of the assistant that authored this message.
    /// </summary>
    [JsonPropertyName("assistant_id")]
    public string? AssistantId { get; set; }

    /// <summary>
    ///    The ID of the run associated with the creation of this message. Value is null when messages are created manually using the create message or create thread endpoints.
    /// </summary>
    [JsonPropertyName("run_id")]
    public string? RunId { get; set; }

    /// <summary>
    ///     A list of file IDs that the assistant should use.
    ///     Useful for tools like retrieval and code_interpreter that can access files.
    ///     A maximum of 10 files can be attached to a message.
    /// </summary>
    [JsonPropertyName("attachments")]
    public List<Attachment> Attachments { get; set; }

    /// <summary>
    ///     The Unix timestamp (in seconds) for when the message was created.
    /// </summary>
    [JsonPropertyName("created_at")]
    public long CreatedAt { get; set; }

    /// <summary>
    ///     The identifier, which can be referenced in API endpoints.
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; set; }

    /// <summary>
    ///     Set of 16 key-value pairs that can be attached to an object.
    /// </summary>
    [JsonPropertyName("metadata")]
    public Dictionary<string, string>? Metadata { get; set; }


    /// <summary>
    ///     The content of the message:  text and/or images.
    /// </summary>
    public record MessageContentResponse
    {
        /// <summary>
        ///     text and/or images. image_file text
        /// </summary>
        [JsonPropertyName("type")]
        public string Type { get; set; }

        /// <summary>
        ///     References an image File in the content of a message.
        /// </summary>
        [JsonPropertyName("image_file")]
        public MessageImageFile? ImageFile { get; set; }

        /// <summary>
        ///     The text content that is part of a message.
        /// </summary>
        [JsonPropertyName("text")]
        public MessageText? Text { get; set; }

        /// <summary>
        ///  References an image URL in the content of a message.
        /// </summary>
        [JsonPropertyName("image_url")]
        public MessageImageUrl? ImageUrl { get; set; }
    }
}