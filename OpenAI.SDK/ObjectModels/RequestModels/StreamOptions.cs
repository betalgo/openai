using System.Text.Json.Serialization;

namespace OpenAI.ObjectModels.RequestModels;

/// <summary>
///     Options for streaming response.
/// </summary>
public sealed class StreamOptions
{
    /// <summary>
    ///     If set, an additional chunk will be streamed before the done message. 
    /// </summary>
    /// <remarks>
    ///     This usage field on this chunk shows the token usage statistics for the entire request, and the choices field will always be an empty array.   
    /// </remarks>
    [JsonPropertyName("include_usage")]
    public bool IncludeUsage { get; set; }
}
