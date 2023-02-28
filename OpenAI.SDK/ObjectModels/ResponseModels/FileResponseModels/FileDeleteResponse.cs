using System.Text.Json.Serialization;
using OpenAI.GPT3.ObjectModels.SharedModels;

namespace OpenAI.GPT3.ObjectModels.ResponseModels.FileResponseModels;

public record FileDeleteResponse : BaseResponse, IOpenAiModels.IId
{
    [JsonPropertyName("deleted")] public bool Deleted { get; set; }
    [JsonPropertyName("id")] public string Id { get; set; }
}