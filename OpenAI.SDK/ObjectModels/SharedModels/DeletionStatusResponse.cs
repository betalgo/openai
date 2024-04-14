using System.Text.Json.Serialization;
using OpenAI.ObjectModels.ResponseModels;

namespace OpenAI.ObjectModels.SharedModels;

public record DeletionStatusResponse : BaseResponse, IOpenAiModels.IId
{
    /// <summary>
    ///     Deletion state
    /// </summary>
    [JsonPropertyName("deleted")]
    public bool IsDeleted { get; set; }

    /// <summary>
    ///     The identifier, which can be referenced in API endpoints.
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; set; }
}