using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OpenAI.ObjectModels.RequestModels
{
    /// <summary>
    ///   Definition of a valid tool call.
    /// </summary>
    public class ToolDefinition
    {
        /// <summary>
        ///     The type of the tool.  Type can be either: code_interpreter、retrieval、function
        /// </summary>
        [Required]
        [JsonPropertyName("type")]
        public string Type { get; set; }

        /// <summary>
        ///     Definition of a valid function call.
        /// </summary>
        [JsonPropertyName("function")]
        public FunctionDefinition? Function { get; set; }

    }
}
