using System.Text.Json.Serialization;
using OpenAI.ObjectModels.SharedModels.BetaSharedModels;

namespace OpenAI.ObjectModels.ResponseModels.AssistantResponseModels;

public record AssistantListResponse : BaseListResponse
{
    /// <summary>
    /// Represents an assistant that can call the model and use tools.
    /// </summary>
    [JsonPropertyName("data")]
    public List<AssistantRetrieveResponse> Assistants { get; set; }
}