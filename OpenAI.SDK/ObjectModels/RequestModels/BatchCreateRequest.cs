using System.Text.Json.Serialization;
using OpenAI.ObjectModels.SharedModels;

namespace OpenAI.ObjectModels.RequestModels;

public record BatchCreateRequest : IOpenAiModels.IMetaData
{
    /// <summary>
    ///     The ID of an uploaded file that contains requests for the new batch.
    ///     See [upload file](/docs/api-reference/files/create) for how to upload a file.
    ///     Your input file must be formatted as a JSONL file, and must be uploaded with the purpose `batch`.
    /// </summary>
    [JsonPropertyName("input_file_id")]
    public string InputFileId { get; set; }

    /// <summary>
    ///     The endpoint to be used for all requests in the batch. Currently only `/v1/chat/completions` is supported.
    /// </summary>
    [JsonPropertyName("endpoint")]
    public string Endpoint { get; set; }

    /// <summary>
    ///     The time frame within which the batch should be processed. Currently only `24h` is supported.
    /// </summary>
    [JsonPropertyName("completion_window")]
    public string CompletionWindow { get; set; }

    /// <summary>
    ///     Optional custom metadata for the batch.
    /// </summary>
    [JsonPropertyName("metadata")]
    public Dictionary<string, string>? Metadata { get; set; }
}