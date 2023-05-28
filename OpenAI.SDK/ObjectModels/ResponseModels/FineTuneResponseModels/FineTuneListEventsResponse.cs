using System.Text.Json.Serialization;
using OpenAI.ObjectModels.SharedModels;

namespace OpenAI.ObjectModels.ResponseModels.FineTuneResponseModels;

public record FineTuneListEventsResponse : BaseResponse
{
    [JsonPropertyName("data")] public List<EventResponse> Data { get; set; }
}