// Inspired from @author: Devis Lucato.

using System.Collections.Concurrent;
using System.Text;
using System.Text.RegularExpressions;

namespace OpenAI.GPT3.Tokenizer.TikToken;

/// <summary>
///    Tokenizer porting from [Tiktoken](https://github.com/openai/tiktoken)
/// </summary>
public class Tokenizer
{
    private CoreBPE _coreBPE;

    /// <summary>
    /// 
    /// </summary>
    public Tokenizer()
    {
        ;
    }

    /// <summary>
    /// create a tokenizer from an encoding
    /// </summary>
    /// <param name="encoding">encoding name. e.g. cl100k_base </param>
    public Tokenizer(string encoding)
    {
        ModelParams modelParams = Settings.GetModelParams(encoding);
        _coreBPE = new CoreBPE(modelParams.MergeableRanks, modelParams.SpecialTokens, modelParams.PatStr);
    }

    /// <summary>
    /// create a tokenizer from a model name
    /// </summary>
    /// <param name="modelName">the name of model, e.g. gpt-3.5-turbo-0301</param>
    /// <returns></returns>
    public Tokenizer FromModelName(string modelName)
    {
        ModelParams modelParams = Settings.GetModelParamsFromModel(modelName);
        _coreBPE = new CoreBPE(modelParams.MergeableRanks, modelParams.SpecialTokens, modelParams.PatStr);
        return this;
    }

    /// <summary>
    /// create a tokenizer from a model
    /// </summary>
    /// <param name="model">the enum value of model, e.g. Models.Model.TextDavinciV3</param>
    /// <returns></returns>
    public Tokenizer FromModel(OpenAI.GPT3.ObjectModels.Models.Model model)
    {
        string modelName = OpenAI.GPT3.ObjectModels.Models.EnumToString(model);
        return this.FromModelName(modelName);
    }


    /// <summary>
    ///  Encode a string into a list of tokens
    /// </summary>
    /// <param name="text"> the string to encode</param>
    /// <param name="allowedSpecial"> a set of special tokens that are allowed to be split</param>
    /// <returns></returns>
    public int[] Encode(string text, HashSet<string>? allowedSpecial = null)
    {
        return _coreBPE.Encode(text, allowedSpecial ?? new HashSet<string>());
    }

    /// <summary>
    ///  Decode a list of tokens into a string
    /// </summary>
    /// <param name="tokens"></param>
    /// <returns></returns>

    public string Decode(int[] tokens)
    {
        return _coreBPE.Decode(tokens);
    }
}