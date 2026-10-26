using System.Text.Json.Serialization;

namespace Betalgo.Ranul.OpenAI.Contracts.Types.Items;

/// <summary>
///     A pending safety check for the computer call.
///     <see href="https://platform.openai.com/docs/guides/tools-computer-use">Computer Use Guide</see>.
///     <see href="https://github.com/betalgo/openai/blob/master/Docs/openapi-split/components/schemas/computercallsafetycheckparam.yml">
///         Source Definition
///     </see>
/// </summary>
public class ComputerCallSafetyCheck
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="ComputerCallSafetyCheck" /> class.
    /// </summary>
    public ComputerCallSafetyCheck()
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="ComputerCallSafetyCheck" /> class with required parameters.
    /// </summary>
    /// <param name="id">The ID of the pending safety check.</param>
    public ComputerCallSafetyCheck(string id)
    {
        Id = id;
    }

    /// <summary>
    ///     The ID of the pending safety check.
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; set; } = null!;

    /// <summary>
    ///     The type of the pending safety check.
    /// </summary>
    [JsonPropertyName("code")]
    public string? Code { get; set; }

    /// <summary>
    ///     Details about the pending safety check.
    /// </summary>
    [JsonPropertyName("message")]
    public string? Message { get; set; }
}

