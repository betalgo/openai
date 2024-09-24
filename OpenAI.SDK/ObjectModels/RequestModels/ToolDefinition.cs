using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace OpenAI.ObjectModels.RequestModels;

/// <summary>
///     Definition of a valid tool.
/// </summary>
public class ToolDefinition
{
    /// <summary>
    ///     Required. The type of the tool. Currently, only function is supported.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; }

    /// <summary>
    ///     Overrides for the file search tool.
    /// </summary>
    [JsonPropertyName("file_search")]
    public FileSearchTool? FileSearchTool { get; set; }

    /// <summary>
    ///     A list of functions the model may generate JSON inputs for.
    /// </summary>
    [JsonIgnore]
    public FunctionDefinition? Function { get; set; }

    [JsonIgnore]
    public object? FunctionsAsObject { get; set; }

    /// <summary>
    ///     Required. The description of what the function does.
    /// </summary>
    [JsonPropertyName("function")]
    public object? FunctionCalculated
    {
        get
        {
            if (FunctionsAsObject != null && Function != null)
            {
                throw new ValidationException("FunctionAsObject and Function can not be assigned at the same time. One of them is should be null.");
            }

            return Function ?? FunctionsAsObject;
        }
    }

    public static ToolDefinition DefineFunction(FunctionDefinition function)
    {
        return new()
        {
            Type = StaticValues.CompletionStatics.ToolType.Function,
            Function = function
        };
    }

    public static ToolDefinition DefineCodeInterpreter()
    {
        return new()
        {
            Type = StaticValues.AssistantsStatics.ToolCallTypes.CodeInterpreter
        };
    }

    [Obsolete("Retrieval is now called FileSearch")]
    public static ToolDefinition DefineRetrieval()
    {
        return new()
        {
            Type = StaticValues.AssistantsStatics.ToolCallTypes.FileSearch
        };
    }

    public static ToolDefinition DefineFileSearch(FileSearchTool? fileSearchTool = null)
    {
        return new()
        {
            Type = StaticValues.AssistantsStatics.ToolCallTypes.FileSearch,
            FileSearchTool = fileSearchTool
        };
    }
}

public class FileSearchTool
{
    /// <summary>
    ///     The maximum number of results the file search tool should output. The default is 20 for gpt-4* models and 5 for
    ///     gpt-3.5-turbo. This number should be between 1 and 50 inclusive.
    ///     Note that the file search tool may output fewer than max_num_results results.
    ///     <a href="https://platform.openai.com/docs/assistants/tools/file-search/customizing-file-search-settings">
    ///         See the
    ///         file search tool documentation
    ///     </a>
    ///     for more information.
    /// </summary>
    [JsonPropertyName("max_num_results")]
    public int? MaxNumberResults { get; set; }

    /// <summary>
    ///     The ranking options for the file search. If not specified, the file search tool will use the auto ranker and a
    ///     score_threshold of 0.
    ///     See the
    ///     <a href="https://platform.openai.com/docs/assistants/tools/file-search/customizing-file-search-settings">
    ///         file
    ///         search tool documentation
    ///     </a>
    ///     for more information.
    /// </summary>
    [JsonPropertyName("ranking_options")]
    public RankingOptions? RankingOptions { get; set; }
}

public class RankingOptions
{
    /// <summary>
    ///     The ranker to use for the file search. If not specified will use the auto ranker.
    /// </summary>
    [JsonPropertyName("ranker")]
    public string? Ranker { get; set; }

    /// <summary>
    ///     The score threshold for the file search. All values must be a floating point number between 0 and 1.
    /// </summary>
    [JsonPropertyName("score_threshold")]
    public float ScoreThreshold { get; set; }
}