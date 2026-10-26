using System.Text.Json.Serialization;

namespace Betalgo.Ranul.OpenAI.Contracts.Types.Content;

/// <summary>
///     A refusal from the model.
///     <see href="https://platform.openai.com/docs/api-reference/responses">OpenAI API documentation</see>.
///     <see href="https://github.com/betalgo/openai/blob/master/Docs/openapi-split/components/schemas/refusalcontent.yml">
///         Source Definition
///     </see>
/// </summary>
public class RefusalContent : IContent
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="RefusalContent" /> class.
    /// </summary>
    public RefusalContent()
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="RefusalContent" /> class with required parameters.
    /// </summary>
    /// <param name="refusal">The refusal explanation from the model.</param>
    public RefusalContent(string refusal)
    {
        Refusal = refusal;
    }

    /// <summary>
    ///     The type of the refusal. Always <c>refusal</c>.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type => "refusal";

    /// <summary>
    ///     The refusal explanation from the model.
    /// </summary>
    [JsonPropertyName("refusal")]
    public string Refusal { get; set; } = null!;
}

