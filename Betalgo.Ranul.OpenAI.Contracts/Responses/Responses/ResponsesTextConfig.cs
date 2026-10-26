using System.Text.Json.Serialization;
using Betalgo.Ranul.OpenAI.Contracts.Types.TextFormat;

namespace Betalgo.Ranul.OpenAI.Contracts.Responses.Responses;

/// <summary>
///     Configuration for the response's text format.
///     <see href="https://platform.openai.com/docs/guides/structured-outputs">Structured Outputs Guide</see>.
/// </summary>
public class ResponsesTextConfig
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="ResponsesTextConfig" /> class.
    /// </summary>
    public ResponsesTextConfig()
    {
    }

    /// <summary>
    ///     Initializes a new instance with a format.
    /// </summary>
    /// <param name="format">The format configuration for text output.</param>
    public ResponsesTextConfig(TextFormat? format)
    {
        Format = format;
    }

    /// <summary>
    ///     The format configuration for text output.
    /// </summary>
    [JsonPropertyName("format")]
    public TextFormat? Format { get; set; }

    /// <summary>
    ///     The verbosity of the text output.
    /// </summary>
    [JsonPropertyName("verbosity")]
    public string? Verbosity { get; set; }
}

