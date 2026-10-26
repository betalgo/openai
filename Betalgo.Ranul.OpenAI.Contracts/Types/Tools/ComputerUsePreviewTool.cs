using System.Text.Json.Serialization;
using Betalgo.Ranul.OpenAI.Contracts.Enums.Responses;

namespace Betalgo.Ranul.OpenAI.Contracts.Types.Tools;

/// <summary>
///     A tool that controls a virtual computer. Learn more about the
///     <see href="https://platform.openai.com/docs/guides/tools-computer-use">computer tool</see>.
///     <see href="https://github.com/betalgo/openai/blob/master/Docs/openapi-split/components/schemas/computerusepreviewtool.yml">
///         Source Definition
///     </see>
/// </summary>
public class ComputerUsePreviewTool : ITool
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="ComputerUsePreviewTool" /> class.
    /// </summary>
    public ComputerUsePreviewTool()
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="ComputerUsePreviewTool" /> class with required parameters.
    /// </summary>
    /// <param name="environment">The environment of the computer to control.</param>
    /// <param name="displayWidth">The width of the computer display.</param>
    /// <param name="displayHeight">The height of the computer display.</param>
    public ComputerUsePreviewTool(ComputerEnvironment environment, int displayWidth, int displayHeight)
    {
        Environment = environment;
        DisplayWidth = displayWidth;
        DisplayHeight = displayHeight;
    }

    /// <summary>
    ///     The type of the computer use tool. Always <c>computer_use_preview</c>.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type => "computer_use_preview";

    /// <summary>
    ///     The environment of the computer to control.
    /// </summary>
    [JsonPropertyName("environment")]
    public ComputerEnvironment Environment { get; set; }

    /// <summary>
    ///     The width of the computer display.
    /// </summary>
    [JsonPropertyName("display_width")]
    public int DisplayWidth { get; set; }

    /// <summary>
    ///     The height of the computer display.
    /// </summary>
    [JsonPropertyName("display_height")]
    public int DisplayHeight { get; set; }
}

