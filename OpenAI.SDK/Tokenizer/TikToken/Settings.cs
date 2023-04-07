using System;
using System.Collections.Generic;

namespace OpenAI.GPT3.Tokenizer.TikToken;

/// <summary>
/// Parameters of the model for the tokenizer
/// </summary>
public class ModelParams
{
    /// <summary>
    /// Name of the model
    /// </summary>
    public string Name { get; }
    /// <summary>
    ///  Explicit number of vocabulary(ranks)
    /// </summary>
    public int? ExplicitNVocab { get; }
    /// <summary>
    ///  Pattern string for splitting
    /// </summary>
    public string PatStr { get; }
    /// <summary>
    ///  Mergeable ranks(vocabulary)
    /// </summary>
    public Dictionary<byte[], int> MergeableRanks { get; }
    /// <summary>
    ///  Special tokens to split the text
    /// </summary>
    public Dictionary<string, int> SpecialTokens { get; }


    private const string EndOfText = "<|endoftext|>";
    private const string FimPrefix = "<|fim_prefix|>";
    private const string FimMiddle = "<|fim_middle|>";
    private const string FimSuffix = "<|fim_suffix|>";
    private const string EndOfPrompt = "<|endofprompt|>";


    /// <summary>
    ///  Constructor
    /// </summary>
    /// <param name="name"></param>
    /// <param name="explicitNVocab"></param>
    /// <param name="patStr"></param>
    /// <param name="mergeableRanks"></param>
    /// <param name="specialTokens"></param>
    public ModelParams(
        string name,
        int? explicitNVocab = null,
        string patStr = null!,
        Dictionary<byte[], int> mergeableRanks = null!,
        Dictionary<string, int>? specialTokens = null)
    {
        Name = name;
        ExplicitNVocab = explicitNVocab;
        PatStr = patStr;
        MergeableRanks = mergeableRanks;
        SpecialTokens = specialTokens ?? new Dictionary<string, int>();
    }

    /// The following parameters come from [tiktoken/model.py](https://github.com/openai/tiktoken/blob/main/tiktoken/model.py)

    /// <summary>
    ///  R50KBase
    /// </summary>
    /// <returns></returns>

    public static ModelParams R50KBase()
    {
        var mergeableRanks = EmbeddedResource.LoadRanks("ranks.r50k_base.tiktoken");

        return new ModelParams
        (
            "r50k_base",
            50257,
            @"'s|'t|'re|'ve|'m|'ll|'d| ?\p{L}+| ?\p{N}+| ?[^\s\p{L}\p{N}]+|\s+(?!\S)|\s+",
            mergeableRanks,
            new Dictionary<string, int> { { EndOfText, 50256 } }
        );
    }

    /// <summary>
    ///  P50KBase
    /// </summary>
    /// <returns></returns>

    public static ModelParams P50KBase()
    {
        var mergeableRanks = EmbeddedResource.LoadRanks("ranks.p50k_base.tiktoken");

        return new ModelParams
        (
            "p50k_base",
            50281,
            @"'s|'t|'re|'ve|'m|'ll|'d| ?\p{L}+| ?\p{N}+| ?[^\s\p{L}\p{N}]+|\s+(?!\S)|\s+",
            mergeableRanks,
            new Dictionary<string, int> { { EndOfText, 50256 } }
        );
    }

    /// <summary>
    ///  P50KEdit
    /// </summary>
    /// <returns></returns>
    public static ModelParams P50KEdit()
    {
        var mergeableRanks = EmbeddedResource.LoadRanks("ranks.p50k_base.tiktoken");

        var specialTokens = new Dictionary<string, int>
        {
            {EndOfText, 50256}, {FimPrefix, 50281}, {FimMiddle, 50282}, {FimSuffix, 50283}
        };

        return new ModelParams
        (
            name: "p50k_base",
            patStr: @"'s|'t|'re|'ve|'m|'ll|'d| ?\p{L}+| ?\p{N}+| ?[^\s\p{L}\p{N}]+|\s+(?!\S)|\s+",
            mergeableRanks: mergeableRanks,
            specialTokens: specialTokens
        );
    }

    /// <summary>
    ///  Cl100KBase
    /// </summary>
    /// <returns></returns>

    public static ModelParams Cl100KBase()
    {
        var mergeableRanks = EmbeddedResource.LoadRanks("ranks.cl100k_base.tiktoken");

        var specialTokens = new Dictionary<string, int>
        {
            {EndOfText, 100257},
            {FimPrefix, 100258},
            {FimMiddle, 100259},
            {FimSuffix, 100260},
            {EndOfPrompt, 100276}
        };

        return new ModelParams
        (
            "cl100k_base",
            patStr:
            @"(?i:'s|'t|'re|'ve|'m|'ll|'d)|[^\r\n\p{L}\p{N}]?\p{L}+|\p{N}{1,3}| ?[^\s\p{L}\p{N}]+[\r\n]*|\s*[\r\n]+|\s+(?!\S)|\s+",
            mergeableRanks: mergeableRanks,
            specialTokens: specialTokens
        );
    }
}

/// <summary>
///  Mapping model to encoding
/// </summary>
public static class ModelEncoding
{
    private static readonly Dictionary<string, string> ModelPrefixToEncoding = new Dictionary<string, string>
    {
        // chat
        { "gpt-4-", "cl100k_base" }, // e.g., gpt-4-0314, etc., plus gpt-4-32k
        { "gpt-3.5-turbo-", "cl100k_base" } // e.g, gpt-3.5-turbo-0301, -0401, etc.
    };

    private static readonly Dictionary<string, string> ModelToEncoding = new Dictionary<string, string>
    {
        // chat
        { "gpt-4", "cl100k_base" },
        { "gpt-3.5-turbo", "cl100k_base" },
        //text
        { "text-davinci-003", "p50k_base"},
        { "text-davinci-002", "p50k_base"},
        { "text-davinci-001", "r50k_base"},
        { "text-curie-001", "r50k_base"},
        { "text-babbage-001", "r50k_base"},
        { "text-ada-001", "r50k_base"},
        { "davinci", "r50k_base"},
        { "curie", "r50k_base"},
        { "babbage", "r50k_base"},
        { "ada", "r50k_base"},
       //code
        { "code-davinci-002", "p50k_base"},
        { "code-davinci-001", "p50k_base"},
        { "code-cushman-002", "p50k_base"},
        { "code-cushman-001", "p50k_base"},
        { "davinci-codex", "p50k_base"},
        { "cushman-codex", "p50k_base"},
        //edit
        { "text-davinci-edit-001", "p50k_edit"},
        { "code-davinci-edit-001", "p50k_edit"},
        //embeddings
        { "text-embedding-ada-002", "cl100k_base"},
        //old embeddings
        { "text-similarity-davinci-001", "r50k_base"},
        { "text-similarity-curie-001", "r50k_base"},
        { "text-similarity-babbage-001", "r50k_base"},
        { "text-similarity-ada-001", "r50k_base"},
        { "text-search-davinci-doc-001", "r50k_base"},
        { "text-search-curie-doc-001", "r50k_base"},
        { "text-search-babbage-doc-001", "r50k_base"},
        { "text-search-ada-doc-001", "r50k_base"},
        { "code-search-babbage-code-001", "r50k_base"},
        { "code-search-ada-code-001", "r50k_base"},
        //open source
        { "gpt2", "gpt2"},
    };

    /// <summary>
    ///  Get the encoding name from a model name
    /// </summary>
    /// <param name="modelName"></param>
    /// <returns></returns>
    /// <exception cref="KeyNotFoundException"></exception>
    public static string GetModelEncoding(string modelName)
    {
        if (ModelToEncoding.TryGetValue(modelName, out string? encodingName))
        {
            return encodingName;
        }

        foreach (KeyValuePair<string, string> prefixToEncoding in ModelPrefixToEncoding)
        {
            if (modelName.StartsWith(prefixToEncoding.Key))
            {
                return prefixToEncoding.Value;
            }
        }

        throw new KeyNotFoundException($"Could not automatically map {modelName} to a tokeniser. Please explicitly specify the encoding name to get the tokeniser you expect.");
    }
}

/// <summary>
///  Settings
/// </summary>
public static class Settings
{
    /// <summary>
    ///  Get model params from encoding name
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static ModelParams GetModelParams(string name)
    {
        //未支持gpt2
        return name.ToLower() switch
        {
            "r50k_base" => ModelParams.R50KBase(),
            "p50k_base" => ModelParams.P50KBase(),
            "p50k_edit" => ModelParams.P50KEdit(),
            "cl100k_base" => ModelParams.Cl100KBase(),
            _ => throw new ArgumentException($"Unknown model name: {name}")
        };
    }

    /// <summary>
    ///  Get model params from model name
    /// </summary>
    /// <param name="modelName"></param>
    /// <returns></returns>
    public static ModelParams GetModelParamsFromModel(string modelName)
    {
        string name=ModelEncoding.GetModelEncoding(modelName);
        return GetModelParams(name);
    }

}
