using System.Text.Json.Serialization;

namespace OpenAI.ObjectModels.SharedModels;

public class TruncationStrategy
{
    /// <summary>
    ///     The truncation strategy to use for the thread.
    ///     The default is "auto". If set to "last_messages", the thread will be truncated to the n most recent messages in the
    ///     thread.
    ///     When set to "auto", messages in the middle of the thread will be dropped to fit the context length of the model,
    ///     max_prompt_tokens.
    /// </summary>
    [JsonPropertyName("type")]
    public string? Type { get; set; }

    /// <summary>
    ///     The number of most recent messages from the thread when constructing the context for the run.
    /// </summary>
    [JsonPropertyName("last_messages")]
    public int? LastMessages { get; set; }
}