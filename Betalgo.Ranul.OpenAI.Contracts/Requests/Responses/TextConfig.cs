using System.Text.Json.Serialization;
using Betalgo.Ranul.OpenAI.Contracts.Types.TextFormat;

namespace Betalgo.Ranul.OpenAI.Contracts.Requests.Responses;

/// <summary>
///     Configuration for text output format.
///     <see href="https://platform.openai.com/docs/guides/structured-outputs">Structured Outputs Guide</see>.
///     <see href="https://github.com/betalgo/openai/blob/master/Docs/openapi-split/components/schemas/responsetextparam.yml">
///         Source Definition
///     </see>
/// </summary>
public class TextConfig
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="TextConfig" /> class.
    /// </summary>
    public TextConfig()
    {
    }

    /// <summary>
    ///     Initializes a new instance with a format.
    /// </summary>
    /// <param name="format">The format of the text output.</param>
    public TextConfig(TextFormat? format)
    {
        Format = format;
    }

    /// <summary>
    ///     The format of the text output.
    ///     <see href="https://platform.openai.com/docs/guides/structured-outputs">Structured Outputs Guide</see>.
    /// </summary>
    [JsonPropertyName("format")]
    public TextFormat? Format { get; set; }

    /// <summary>
    ///     The verbosity of the text output.
    /// </summary>
    [JsonPropertyName("verbosity")]
    public string? Verbosity { get; set; }
}

