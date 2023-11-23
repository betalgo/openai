using System.Text.Json.Serialization;
using OpenAI.ObjectModels.SharedModels.BetaSharedModels;

namespace OpenAI.ObjectModels.ResponseModels.AssistantResponseModels;

public record AssistantFileListResponse : BaseListResponse
{
    /// <summary>
    /// A list of Files attached to an assistant.
    /// </summary>
    [JsonPropertyName("data")]
    public List<AssistantFileRetrieveResponse> AssistantFiles { get; set; }
}