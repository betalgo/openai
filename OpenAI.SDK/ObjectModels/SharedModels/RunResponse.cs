using OpenAI.ObjectModels.RequestModels;
using OpenAI.ObjectModels.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OpenAI.ObjectModels.SharedModels
{
    public record RunResponse : BaseResponse, IOpenAiModels.IId, IOpenAiModels.IModel, IOpenAiModels.ICreatedAt, IOpenAiModels.IFileIds, IOpenAiModels.IMetaData
    {
        /// <summary>
        /// The identifier, which can be referenced in API endpoints.
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; set; }

        /// <summary>
        ///  The Unix timestamp (in seconds) for when the run was created.
        /// </summary>
        [JsonPropertyName("created_at")]
        public int CreatedAt { get; set; }

        /// <summary>
        /// The ID of the thread that was executed on as a part of this run.
        /// </summary>
        [JsonPropertyName("thread_id")]
        public string ThreadId { get; set; }

        /// <summary>
        /// The ID of the assistant used for execution of this run.
        /// </summary>
        [JsonPropertyName("assistant_id")]
        public string AssistantId { get; set; }

        /// <summary>
        /// The status of the run, which can be either queued, in_progress, requires_action, cancelling, cancelled, failed, completed, or expired.
        /// </summary>
        [JsonPropertyName("status")]
        public string Status { get; set; }

        /// <summary>
        /// Details on the action required to continue the run. 
        /// Will be null if no action is required.
        /// </summary>
        [JsonPropertyName("required_action")]
        public RequiredAction? RequiredAction { get; set; }


        private LastError _lastError;
        /// <summary>
        /// The last error associated with this run. Will be null if there are no errors.
        /// </summary>
        [JsonPropertyName("last_error")]
        public LastError? LastError
        {
            get => _lastError;
            set
            {
                _lastError = value;
                if (_lastError == null) return;

                this.Error = new Error()
                {
                    Code = _lastError.Code,
                    MessageObject = _lastError.Message,
                };
            }
        }

        /// <summary>
        /// The Unix timestamp (in seconds) for when the run will expire.
        /// </summary>
        [JsonPropertyName("expires_at")]
        public int? ExpiresAt { get; set; }

        /// <summary>
        /// The Unix timestamp (in seconds) for when the run was started.
        /// </summary>
        [JsonPropertyName("started_at")]
        public int? StartedAt { get; set; }

        /// <summary>
        /// The Unix timestamp (in seconds) for when the run was cancelled.
        /// </summary>
        [JsonPropertyName("cancelled_at")]
        public int? CancelledAt { get; set; }

        /// <summary>
        /// The Unix timestamp (in seconds) for when the run failed.
        /// </summary>
        [JsonPropertyName("failed_at")]
        public int? FailedAt { get; set; }

        /// <summary>
        /// The Unix timestamp (in seconds) for when the run was completed.
        /// </summary>
        [JsonPropertyName("completed_at")]
        public int? CompletedAt { get; set; }

        /// <summary>
        /// The model that the assistant used for this run.
        /// </summary>
        [JsonPropertyName("model")]
        public string Model { get; set; }

        /// <summary>
        ///The instructions that the assistant used for this run.
        /// </summary>
        [JsonPropertyName("instructions")]
        public string Instructions { get; set; }

        /// <summary>
        /// The list of tools that the assistant used for this run.
        /// </summary>
        [JsonPropertyName("tools")]
        public List<ToolDefinition>? Tools { get; set; }

        /// <summary>
        /// The list of File IDs the assistant used for this run.
        /// </summary>
        [JsonPropertyName("file_ids")]
        public List<string>? FileIds { get; set; }

        /// <summary>
        /// Set of 16 key-value pairs that can be attached to an object.
        /// This can be useful for storing additional information about the object in a structured format. 
        /// Keys can be a maximum of 64 characters long and values can be a maxium of 512 characters long.
        /// </summary>
        [JsonPropertyName("metadata")]
        public Dictionary<string, string>? MetaData { get; set; }
   
    }
}
