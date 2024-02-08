using OpenAI.ObjectModels.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OpenAI.ObjectModels.SharedModels
{
    public record AssistatntFileResponse : BaseResponse, IOpenAiModels.IId, IOpenAiModels.ICreatedAt
    {
        /// <summary>
        /// The identifier, which can be referenced in API endpoints.
        /// </summary>
        [JsonPropertyName("id")]
        public string Id {  get; set; }

        /// <summary>
        /// The Unix timestamp (in seconds) for when the assistant was created.
        /// </summary>
        [JsonPropertyName("created_at")]
        public int CreatedAt { get; set; }

        /// <summary>
        /// The assistant ID that the file is attached to.
        /// </summary>
        [JsonPropertyName("assistant_id")]
        public string AssistantId {  get; set; }
    }
}
