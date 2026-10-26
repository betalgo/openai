using System.Text.Json.Serialization;

namespace Betalgo.Ranul.OpenAI.Contracts.Types.Items;

/// <summary>
///     An internal identifier for an item to reference.
///     <see href="https://platform.openai.com/docs/api-reference/responses">OpenAI API documentation</see>.
///     <see href="https://github.com/betalgo/openai/blob/master/Docs/openapi-split/components/schemas/itemreferenceparam.yml">
///         Source Definition
///     </see>
/// </summary>
public class ItemReferenceItem : IInputItem
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="ItemReferenceItem" /> class.
    /// </summary>
    public ItemReferenceItem()
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="ItemReferenceItem" /> class with required parameters.
    /// </summary>
    /// <param name="id">The ID of the item to reference.</param>
    public ItemReferenceItem(string id)
    {
        Id = id;
    }

    /// <summary>
    ///     The type of item to reference. Always <c>item_reference</c>.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type => "item_reference";

    /// <summary>
    ///     The ID of the item to reference.
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; set; } = null!;
}
