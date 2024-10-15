using System.Text.Json.Serialization;
using Betalgo.OpenAI.ObjectModels.ResponseModels;

namespace Betalgo.OpenAI.ObjectModels.SharedModels;

public record DeletionStatusResponse : BaseResponse, IOpenAIModels.IId
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