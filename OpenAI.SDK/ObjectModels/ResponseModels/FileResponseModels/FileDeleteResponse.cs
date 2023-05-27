using System.Text.Json.Serialization;
using OpenAI.ObjectModels.SharedModels;

namespace OpenAI.ObjectModels.ResponseModels.FileResponseModels;

public record FileDeleteResponse : BaseResponse, IOpenAiModels.IId
{
    [JsonPropertyName("deleted")] public bool Deleted { get; set; }
    [JsonPropertyName("id")] public string Id { get; set; }
}