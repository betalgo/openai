using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using OpenAI.GPT3.Interfaces;
using OpenAI.GPT3.Models.SharedModels;

namespace OpenAI.GPT3.Models.RequestModels
{
    public record SearchCreateRequest : IModelValidate, IOpenAiModels.IFileOrDocument
    {
        /// <summary>
        ///     Query to search against the documents.
        /// </summary>
        [JsonPropertyName("query")]
        public string Query { get; set; } = null!;

        [JsonPropertyName("search_model")] public string SearchModel { get; set; } = null!;

        /// <summary>
        ///     The maximum number of documents to be re-ranked and returned by search.
        ///     This flag only takes effect when file is set.
        /// </summary>
        [JsonPropertyName("max_rerank")]
        public int? MaxRerank { get; set; }

        /// <summary>
        ///     A special boolean flag for showing metadata. If set to true, each document entry in the returned JSON will contain
        ///     a "metadata" field.
        ///     This flag only takes effect when file is set.
        /// </summary>
        [JsonPropertyName("return_metadata")]
        public bool? ReturnMetadata { get; set; }

        /// <summary>
        ///     Up to 200 documents to search over, provided as a list of strings.
        ///     The maximum document length(in tokens) is 2034 minus the number of tokens in the query.
        ///     You should specify either documents or a file, but not both.
        /// </summary>
        [JsonPropertyName("documents")]
        public List<string>? Documents { get; set; }

        /// <summary>
        ///     The ID of an uploaded file that contains documents to search over.
        ///     You should specify either documents or a file, but not both.
        /// </summary>
        [JsonPropertyName("file")]
        public string? File { get; set; }

        public IEnumerable<ValidationResult> Validate()
        {
            throw new NotImplementedException();
        }
    }
}