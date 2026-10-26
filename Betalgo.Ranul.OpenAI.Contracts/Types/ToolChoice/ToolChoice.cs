using System.Text.Json;
using System.Text.Json.Serialization;
using Betalgo.Ranul.OpenAI.Contracts.Enums.Responses;

namespace Betalgo.Ranul.OpenAI.Contracts.Types.ToolChoice;

/// <summary>
///     How the model should select which tool (or tools) to use when generating a response.
///     Can be a string option ("none", "auto", "required") or a specific tool choice object.
///     <see href="https://platform.openai.com/docs/api-reference/responses/create">OpenAI API documentation</see>.
///     <see href="https://github.com/betalgo/openai/blob/master/Docs/openapi-split/components/schemas/toolchoiceparam.yml">
///         Source Definition
///     </see>
/// </summary>
[JsonConverter(typeof(ToolChoiceConverter))]
public class ToolChoice
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="ToolChoice" /> class.
    /// </summary>
    public ToolChoice()
    {
    }

    /// <summary>
    ///     Initializes a new instance with a string option.
    /// </summary>
    /// <param name="option">The tool choice option (none, auto, required).</param>
    public ToolChoice(ToolChoiceOption option)
    {
        Option = option;
        SpecificTool = null;
    }

    /// <summary>
    ///     Initializes a new instance with a specific tool choice.
    /// </summary>
    /// <param name="specificTool">The specific tool to use.</param>
    public ToolChoice(SpecificToolChoice specificTool)
    {
        Option = null;
        SpecificTool = specificTool;
    }

    /// <summary>
    ///     The string option value when using simple tool choice.
    /// </summary>
    public ToolChoiceOption? Option { get; }

    /// <summary>
    ///     The specific tool choice when forcing a particular tool.
    /// </summary>
    public SpecificToolChoice? SpecificTool { get; }

    /// <summary>
    ///     Creates a ToolChoice that prevents the model from calling any tool.
    /// </summary>
    public static ToolChoice None => new(ToolChoiceOption.None);

    /// <summary>
    ///     Creates a ToolChoice that lets the model decide whether to call tools.
    /// </summary>
    public static ToolChoice Auto => new(ToolChoiceOption.Auto);

    /// <summary>
    ///     Creates a ToolChoice that requires the model to call at least one tool.
    /// </summary>
    public static ToolChoice Required => new(ToolChoiceOption.Required);

    /// <summary>
    ///     Creates a ToolChoice for a specific function by name.
    /// </summary>
    /// <param name="functionName">The name of the function to call.</param>
    public static ToolChoice Function(string functionName) => new(new SpecificToolChoice("function", functionName));

    /// <summary>
    ///     Creates a ToolChoice for a specific hosted tool type.
    /// </summary>
    /// <param name="type">The type of hosted tool (file_search, web_search_preview, etc.).</param>
    public static ToolChoice HostedTool(string type) => new(new SpecificToolChoice(type));

    /// <summary>
    ///     Implicitly converts a string to a ToolChoice.
    /// </summary>
    public static implicit operator ToolChoice(string value) => new(new ToolChoiceOption(value));

    /// <summary>
    ///     Implicitly converts a ToolChoiceOption to a ToolChoice.
    /// </summary>
    public static implicit operator ToolChoice(ToolChoiceOption option) => new(option);
}

