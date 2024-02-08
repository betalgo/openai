using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OpenAI.ObjectModels.SharedModels
{
    /// <summary>
    /// The text content that is part of a message.
    /// </summary>
    public record MessageText
    {
        /// <summary>
        /// The data that makes up the text.
        /// </summary>
        [JsonPropertyName("value")]
        public string Value { get; set; }

        /// <summary>
        /// annotations
        /// </summary>
        [JsonPropertyName("annotations")]
        public List<MessageAnnotation> Annotations { get; set; }
    }
}
