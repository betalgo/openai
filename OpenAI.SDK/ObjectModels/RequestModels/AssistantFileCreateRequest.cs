using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OpenAI.ObjectModels.RequestModels
{
    public class AssistantFileCreateRequest
    {
        /// <summary>
        /// A File ID (with purpose="assistants") that the assistant should use. Useful for tools like retrieval and code_interpreter that can access files.
        /// </summary>
        [JsonPropertyName("file_id")]
        [Required]
        public string FileId { get; set; }
    }
}
