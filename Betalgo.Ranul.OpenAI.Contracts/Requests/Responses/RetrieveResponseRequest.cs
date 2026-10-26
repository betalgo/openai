using System.Net;
using System.Text.Json.Serialization;
using Betalgo.Ranul.OpenAI.Contracts.Enums.Responses;

namespace Betalgo.Ranul.OpenAI.Contracts.Requests.Responses;

/// <summary>
///     Request parameters for retrieving a model response.
///     <see href="https://platform.openai.com/docs/api-reference/responses/get">OpenAI API documentation</see>.
///     <see href="https://github.com/betalgo/openai/blob/master/Docs/openapi-split/paths/responses/param-response_id/get/getresponse.yml">
///         Source Definition
///     </see>
/// </summary>
public class RetrieveResponseRequest
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="RetrieveResponseRequest" /> class.
    /// </summary>
    public RetrieveResponseRequest()
    {
    }

    /// <summary>
    ///     Additional fields to include in the response.
    /// </summary>
    [JsonPropertyName("include")]
    public List<ResponsesInclude>? Include { get; set; }

    /// <summary>
    ///     If set to true, the model response data will be streamed to the client
    ///     as it is generated using server-sent events.
    ///     <see href="https://platform.openai.com/docs/api-reference/responses-streaming">Streaming documentation</see>.
    /// </summary>
    [JsonPropertyName("stream")]
    public bool? Stream { get; set; }

    /// <summary>
    ///     The sequence number of the event after which to start streaming.
    /// </summary>
    [JsonPropertyName("starting_after")]
    public int? StartingAfter { get; set; }

    /// <summary>
    ///     When true, stream obfuscation will be enabled. Stream obfuscation adds
    ///     random characters to an <c>obfuscation</c> field on streaming delta events
    ///     to normalize payload sizes as a mitigation to certain side-channel attacks.
    ///     These obfuscation fields are included by default.
    /// </summary>
    [JsonPropertyName("include_obfuscation")]
    public bool? IncludeObfuscation { get; set; }

    /// <summary>
    ///     Gets the query parameters string for the request.
    /// </summary>
    /// <param name="includeStream">Whether to include the stream query parameter when present.</param>
    /// <returns>Query string or null if no parameters are set.</returns>
    public string? GetQueryParameters(bool includeStream = true)
    {
        var build = new List<string>();

        if (Include is { Count: > 0 })
        {
            foreach (var include in Include)
            {
                build.Add($"include[]={WebUtility.UrlEncode(include.Value)}");
            }
        }

        if (includeStream && Stream.HasValue)
        {
            build.Add($"stream={Stream.Value.ToString().ToLowerInvariant()}");
        }

        if (StartingAfter.HasValue)
        {
            build.Add($"starting_after={StartingAfter.Value}");
        }

        if (IncludeObfuscation.HasValue)
        {
            build.Add($"include_obfuscation={IncludeObfuscation.Value.ToString().ToLowerInvariant()}");
        }

        return build.Count > 0 ? string.Join("&", build) : null;
    }
}

