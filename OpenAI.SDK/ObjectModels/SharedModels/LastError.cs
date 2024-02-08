using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OpenAI.ObjectModels.SharedModels
{
    public record LastError
    {
        /// <summary>
        /// One of server_error or rate_limit_exceeded.
        /// </summary>
        [JsonPropertyName("code")]
        public string Code {  get; set; }

        /// <summary>
        /// A human-readable description of the error.
        /// </summary>
        [JsonPropertyName("message")]
        public string Message { get; set; }
    }
}
