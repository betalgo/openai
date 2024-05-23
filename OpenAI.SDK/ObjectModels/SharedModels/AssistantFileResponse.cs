using System.Text.Json.Serialization;
using Betalgo.Ranul.OpenAI.ObjectModels.ResponseModels;

namespace Betalgo.Ranul.OpenAI.ObjectModels.SharedModels;

public record AssistantFileResponse : BaseResponse, IOpenAIModels.IId, IOpenAIModels.ICreatedAt
{
    /// <summary>
    ///     The Unix timestamp (in seconds) for when the assistant file was created.
    /// </summary>
    [JsonPropertyName("assistant_id")]
    public string AssistantId { get; set; }

    /// <summary>
    ///     The Unix timestamp (in seconds) for when the assistant file was created.
    /// </summary>
    [JsonPropertyName("created_at")]
    public long CreatedAt { get; set; }

    /// <summary>
    ///     The identifier, which can be referenced in API endpoints.
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; set; }
}