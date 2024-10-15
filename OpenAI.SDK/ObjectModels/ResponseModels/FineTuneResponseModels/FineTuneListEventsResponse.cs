using System.Text.Json.Serialization;
using Betalgo.OpenAI.ObjectModels.SharedModels;

namespace Betalgo.OpenAI.ObjectModels.ResponseModels.FineTuneResponseModels;

public record FineTuneListEventsResponse : BaseResponse
{
    [JsonPropertyName("data")]
    public List<EventResponse> Data { get; set; }
}