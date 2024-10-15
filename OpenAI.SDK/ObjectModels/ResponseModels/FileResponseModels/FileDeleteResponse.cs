using System.Text.Json.Serialization;
using Betalgo.OpenAI.ObjectModels.SharedModels;

namespace Betalgo.OpenAI.ObjectModels.ResponseModels.FileResponseModels;

public record FileDeleteResponse : BaseResponse, IOpenAIModels.IId
{
    [JsonPropertyName("deleted")]
    public bool Deleted { get; set; }

    [JsonPropertyName("id")]
    public string Id { get; set; }
}