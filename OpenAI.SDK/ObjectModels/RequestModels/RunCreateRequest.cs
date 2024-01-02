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
    public class RunCreateRequest : IOpenAiModels.IModel, IOpenAiModels.IMetaData
    {
        /// <summary>
        /// The ID of the assistant to use to execute this run.
        /// </summary>
        [Required]
        [JsonPropertyName("assistant_id")]
        public string AssistantId { get; set; }

        /// <summary>
        /// The ID of the Model to be used to execute this run. 
        /// If a value is provided here, it will override the model associated with the assistant. 
        /// If not, the model associated with the assistant will be used.
        /// </summary>
        [JsonPropertyName("model")]
        public string? Model { get; set; }

        /// <summary>
        /// Override the default system message of the assistant. 
        /// This is useful for modifying the behavior on a per-run basis.
        /// </summary>
        [JsonPropertyName("instructions")]
        public string? Instructions { get; set; }

        /// <summary>
        /// Override the tools the assistant can use for this run. 
        /// This is useful for modifying the behavior on a per-run basis.
        /// </summary>
        [JsonPropertyName("tools")]
        public List<ToolDefinition>? Tools { get; set; }

        /// <summary>
        /// Set of 16 key-value pairs that can be attached to an object.
        /// This can be useful for storing additional information about the object in a structured format. 
        /// Keys can be a maximum of 64 characters long and values can be a maxium of 512 characters long.
        /// </summary>
        [JsonPropertyName("metadata")]
        public Dictionary<string, string>? MetaData { get; set; }
    }
}
