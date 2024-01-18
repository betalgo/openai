using OpenAI.ObjectModels.SharedModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OpenAI.ObjectModels.RequestModels
{
    public class MessageCreateRequest : IOpenAiModels.IFileIds, IOpenAiModels.IMetaData
    {
        /// <summary>
        /// The role of the entity that is creating the message. 
        /// Currently only user is supported.
        /// </summary>
        [Required]
        [JsonPropertyName("role")]
        public string Role { get; set; } = StaticValues.AssistantsStatics.MessageStatics.Roles.User;

        /// <summary>
        /// The content of the message.
        /// </summary>
        [Required]
        [JsonPropertyName("content")]
        public string Content { get; set; }

        /// <summary>
        /// A list of File IDs that the message should use.
        /// There can be a maximum of 10 files attached to a message. 
        /// Useful for tools like retrieval and code_interpreter that can access and use files.
        /// </summary>
        [JsonPropertyName("file_ids")]
        public List<string> FileIds { get; set; }

        /// <summary>
        /// Set of 16 key-value pairs that can be attached to an object.
        /// This can be useful for storing additional information about the object in a structured format. 
        /// Keys can be a maximum of 64 characters long and values can be a maxium of 512 characters long.
        /// </summary>
        [JsonPropertyName("metadata")]
        public Dictionary<string, string> MetaData { get; set; }

    }
}
