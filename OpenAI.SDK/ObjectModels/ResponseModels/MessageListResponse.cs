using OpenAI.ObjectModels.SharedModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OpenAI.ObjectModels.ResponseModels
{
    public record MessageListResponse : DataBaseResponse<List<MessageResponse>>
    {
        [JsonPropertyName("first_id")]
        public string FirstId {  get; set; }

        [JsonPropertyName("last_id")]
        public string LastId { get; set; }

        [JsonPropertyName("has_more")]
        public bool IsHasMore { get; set; }
    }
}
