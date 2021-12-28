using System.Text.Json.Serialization;
using OpenAI.SDK.Models.SharedModels;

namespace OpenAI.SDK.Models.ResponseModels.FineTuneResponseModels;

public record FineTuneListEventsResponse : BaseResponse
{
    [JsonPropertyName("data")] public List<EventResponse> Data { get; set; }
}