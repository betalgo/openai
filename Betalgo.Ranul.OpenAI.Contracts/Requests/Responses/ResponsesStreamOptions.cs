using System.Text.Json.Serialization;

namespace Betalgo.Ranul.OpenAI.Contracts.Requests.Responses;

/// <summary>
///     Options for streaming responses. Only set this when you set <c>stream: true</c>.
///     <see href="https://platform.openai.com/docs/api-reference/responses/create">OpenAI API documentation</see>.
///     <see href="https://github.com/betalgo/openai/blob/master/Docs/openapi-split/components/schemas/responsestreamoptions.yml">
///         Source Definition
///     </see>
/// </summary>
public class ResponsesStreamOptions
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="ResponsesStreamOptions" /> class.
    /// </summary>
    public ResponsesStreamOptions()
    {
    }

    /// <summary>
    ///     When true, stream obfuscation will be enabled. Stream obfuscation adds random characters to an
    ///     <c>obfuscation</c> field on streaming delta events to normalize payload sizes as a mitigation
    ///     to certain side-channel attacks. These obfuscation fields are included by default, but add a
    ///     small amount of overhead to the data stream. You can set <c>include_obfuscation</c> to false
    ///     to optimize for bandwidth if you trust the network links between your application and the OpenAI API.
    /// </summary>
    [JsonPropertyName("include_obfuscation")]
    public bool? IncludeObfuscation { get; set; }
}

