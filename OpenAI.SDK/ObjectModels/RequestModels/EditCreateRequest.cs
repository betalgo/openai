using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using OpenAI.Interfaces;
using OpenAI.ObjectModels.SharedModels;

namespace OpenAI.ObjectModels.RequestModels;

//TODO add model validation
//TODO check what is string or array for prompt,..
/// <summary>
///     Create Edit Request Model
/// </summary>
public record EditCreateRequest : IModelValidate, IOpenAiModels.ITemperature, IOpenAiModels.IModel
{
    /// <summary>
    ///     The input text to use as a starting point for the edit.
    /// </summary>
    [JsonPropertyName("input")]
    public string? Input { get; set; }

    /// <summary>
    ///     The instruction that tells the model how to edit the prompt.
    /// </summary>
    [JsonPropertyName("instruction")]
    public string Instruction { get; set; }

    /// <summary>
    ///     Defaults to 1
    ///     How many completions to generate for each prompt.
    ///     Note: Because this parameter generates many completions, it can quickly consume your token quota.Use carefully and
    ///     ensure that you have reasonable settings for max_tokens and stop.
    /// </summary>
    [JsonPropertyName("n")]
    public int? N { get; set; }

    /// <summary>
    ///     An alternative to sampling with temperature, called nucleus sampling, where the model considers the results of the
    ///     tokens with top_p probability mass. So 0.1 means only the tokens comprising the top 10% probability mass are
    ///     considered.
    ///     We generally recommend altering this or temperature but not both.
    /// </summary>
    [JsonPropertyName("top_p")]
    public float? TopP { get; set; }


    [JsonPropertyName("model")] public string? Model { get; set; }

    public IEnumerable<ValidationResult> Validate()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    ///     What
    ///     <a href="https://towardsdatascience.com/how-to-sample-from-language-models-682bceb97277">sampling temperature</a>
    ///     to use. Higher values means the model will take more risks. Try 0.9 for more creative
    ///     applications, and 0 (argmax sampling) for ones with a well-defined answer.
    ///     We generally recommend altering this or top_p but not both.
    /// </summary>
    /// <see href="https://platform.openai.com/docs/api-reference/completions/create#completions/create-temperature" />
    [JsonPropertyName("temperature")]
    public float? Temperature { get; set; }
}