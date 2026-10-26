using System.Text.Json.Serialization;

namespace Betalgo.Ranul.OpenAI.Contracts.Requests.Responses;

/// <summary>
///     Reference to a prompt template and its variables.
///     <see href="https://platform.openai.com/docs/guides/text?api-mode=responses#reusable-prompts">Learn more</see>.
///     <see href="https://github.com/betalgo/openai/blob/master/Docs/openapi-split/components/schemas/prompt.yml">
///         Source Definition
///     </see>
/// </summary>
public class PromptConfig
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="PromptConfig" /> class.
    /// </summary>
    public PromptConfig()
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="PromptConfig" /> class with the required ID.
    /// </summary>
    /// <param name="id">The unique identifier of the prompt template to use.</param>
    public PromptConfig(string id)
    {
        Id = id;
    }

    /// <summary>
    ///     The unique identifier of the prompt template to use.
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; set; } = null!;

    /// <summary>
    ///     Optional version of the prompt template.
    /// </summary>
    [JsonPropertyName("version")]
    public string? Version { get; set; }

    /// <summary>
    ///     Variables to substitute into the prompt template.
    /// </summary>
    [JsonPropertyName("variables")]
    public Dictionary<string, string>? Variables { get; set; }
}

