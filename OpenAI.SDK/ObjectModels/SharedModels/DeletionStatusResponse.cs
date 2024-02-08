using OpenAI.ObjectModels.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OpenAI.ObjectModels.SharedModels
{
    public record DeletionStatusResponse : BaseResponse, IOpenAiModels.IId
    {
        /// <summary>
        /// The identifier, which can be referenced in API endpoints.
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; set; }

        /// <summary>
        /// Deletion state
        /// </summary>
        [JsonPropertyName("deleted")]
        public bool IsDeleted { get; set; }
    }
}
