using System.Diagnostics.CodeAnalysis;

#pragma warning disable CS1591
namespace OpenAI.ObjectModels;

[SuppressMessage("ReSharper", "InconsistentNaming")]
[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
public static class Models
{
    public enum BaseModel
    {
        Ada,
        Babbage,
        Curie,
        Davinci,
        Cushman
    }

    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public enum Model
    {
        Ada,
        Babbage,
        Curie,
        Davinci,

        TextAdaV1,
        TextBabbageV1,
        TextCurieV1,
        TextDavinciV1,

        TextDavinciV2,
        TextDavinciV3,

        CurieInstructBeta,
        DavinciInstructBeta,

        CurieSimilarityFast,

        TextSimilarityAdaV1,
        TextSimilarityBabbageV1,
        TextSimilarityCurieV1,
        TextSimilarityDavinciV1,

        TextSearchAdaDocV1,
        TextSearchBabbageDocV1,
        TextSearchCurieDocV1,
        TextSearchDavinciDocV1,

        TextSearchAdaQueryV1,
        TextSearchBabbageQueryV1,
        TextSearchCurieQueryV1,
        TextSearchDavinciQueryV1,

        TextEditDavinciV1,
        CodeEditDavinciV1,

        TextEmbeddingAdaV2,

        CodeSearchAdaCodeV1,
        CodeSearchBabbageCodeV1,

        CodeSearchAdaTextV1,
        CodeSearchBabbageTextV1,

        CodeDavinciV1,
        CodeCushmanV1,

        CodeDavinciV2,

        [Obsolete("Use Gpt_3_5_Turbo instead")]
        ChatGpt3_5Turbo,
        Gpt_3_5_Turbo,

        [Obsolete("Use Gpt_3_5_Turbo_0301 instead")]
        ChatGpt3_5Turbo0301,
        Gpt_3_5_Turbo_0301,

        Gpt_3_5_Turbo_16k,
        Gpt_3_5_Turbo_16k_0613,
        Gpt_3_5_Turbo_0613,

        Gpt_4,
        Gpt_4_0314,
        Gpt_4_0613,
        Gpt_4_32k,
        Gpt_4_32k_0314,
        Gpt_4_32k_0613,

        WhisperV1
    }

    public enum Subject
    {
        Text,
        InstructBeta,
        SimilarityFast,
        TextSimilarity,
        TextSearchDocument,
        TextSearchQuery,
        CodeSearchCode,
        CodeSearchText,
        Code,
        CodeEdit,
        Edit,
        TextEmbedding
    }

    /// <summary>
    ///     More capable than any GPT-3.5 model, able to do more complex tasks, and optimized for chat. Will be updated with
    ///     our latest model iteration.
    ///     8,192 tokens	Up to Sep 2021
    /// </summary>
    public static string Gpt_4 => "gpt-4";

    /// <summary>
    ///     Same capabilities as the base gpt-4 mode but with 4x the context length. Will be updated with our latest model
    ///     iteration.
    ///     32,768 tokens	Up to Sep 2021
    /// </summary>
    public static string Gpt_4_32k => "gpt-4-32k";

    /// <summary>
    ///     Snapshot of gpt-4 from March 14th 2023. Unlike gpt-4, this model will not receive updates, and will only be
    ///     supported for a three month period ending on June 14th 2023.
    ///     8,192 tokens	Up to Sep 2021
    /// </summary>
    public static string Gpt_4_0314 => "gpt-4-0314";

    /// <summary>
    ///     Snapshot of gpt-4-32 from March 14th 2023. Unlike gpt-4-32k, this model will not receive updates, and will only be
    ///     supported for a three month period ending on June 14th 2023.
    ///     32,768 tokens	Up to Sep 2021
    /// </summary>
    public static string Gpt_4_32k_0314 => "gpt-4-32k-0314";

    /// <summary>
    ///     Snapshot of gpt-4 from June 13th 2023 with function calling data. Unlike gpt-4, this model will not receive
    ///     updates,
    ///     and will be deprecated 3 months after a new version is released.
    ///     8,192 tokens	Up to Sep 2021
    /// </summary>
    public static string Gpt_4_0613 => "gpt-4-0613";

    /// <summary>
    ///     Snapshot of gpt-4-32 from June 13th 2023. Unlike gpt-4-32k, this model will not receive updates,
    ///     and will be deprecated 3 months after a new version is released.
    ///     32,768 tokens	Up to Sep 2021
    /// </summary>
    public static string Gpt_4_32k_0613 => "gpt-4-32k-0613";


    public static string Ada => "ada";
    public static string Babbage => "babbage";
    public static string Curie => "curie";
    public static string Davinci => "davinci";

    public static string CurieInstructBeta => ModelNameBuilder(BaseModel.Curie, Subject.InstructBeta);
    public static string DavinciInstructBeta => ModelNameBuilder(BaseModel.Davinci, Subject.InstructBeta);

    public static string TextDavinciV1 => ModelNameBuilder(BaseModel.Davinci, Subject.Text, "001");
    public static string TextDavinciV2 => ModelNameBuilder(BaseModel.Davinci, Subject.Text, "002");
    public static string TextDavinciV3 => ModelNameBuilder(BaseModel.Davinci, Subject.Text, "003");
    public static string TextAdaV1 => ModelNameBuilder(BaseModel.Ada, Subject.Text, "001");
    public static string TextBabbageV1 => ModelNameBuilder(BaseModel.Babbage, Subject.Text, "001");
    public static string TextCurieV1 => ModelNameBuilder(BaseModel.Curie, Subject.Text, "001");

    public static string CurieSimilarityFast => ModelNameBuilder(BaseModel.Curie, Subject.SimilarityFast);

    public static string CodeDavinciV1 => ModelNameBuilder(BaseModel.Davinci, Subject.Code, "001");
    public static string CodeCushmanV1 => ModelNameBuilder(BaseModel.Cushman, Subject.Code, "001");
    public static string CodeDavinciV2 => ModelNameBuilder(BaseModel.Davinci, Subject.Code, "002");

    public static string TextSimilarityAdaV1 => ModelNameBuilder(BaseModel.Ada, Subject.TextSimilarity, "001");
    public static string TextSimilarityBabbageV1 => ModelNameBuilder(BaseModel.Babbage, Subject.TextSimilarity, "001");
    public static string TextSimilarityCurieV1 => ModelNameBuilder(BaseModel.Curie, Subject.TextSimilarity, "001");
    public static string TextSimilarityDavinciV1 => ModelNameBuilder(BaseModel.Davinci, Subject.TextSimilarity, "001");

    public static string TextSearchAdaDocV1 => ModelNameBuilder(BaseModel.Ada, Subject.TextSearchDocument, "001");
    public static string TextSearchBabbageDocV1 => ModelNameBuilder(BaseModel.Babbage, Subject.TextSearchDocument, "001");
    public static string TextSearchCurieDocV1 => ModelNameBuilder(BaseModel.Curie, Subject.TextSearchDocument, "001");
    public static string TextSearchDavinciDocV1 => ModelNameBuilder(BaseModel.Davinci, Subject.TextSearchDocument, "001");
    public static string TextSearchAdaQueryV1 => ModelNameBuilder(BaseModel.Ada, Subject.TextSearchQuery, "001");
    public static string TextSearchBabbageQueryV1 => ModelNameBuilder(BaseModel.Babbage, Subject.TextSearchQuery, "001");
    public static string TextSearchCurieQueryV1 => ModelNameBuilder(BaseModel.Curie, Subject.TextSearchQuery, "001");
    public static string TextSearchDavinciQueryV1 => ModelNameBuilder(BaseModel.Davinci, Subject.TextSearchQuery, "001");

    public static string TextEditDavinciV1 => ModelNameBuilder(BaseModel.Davinci, Subject.Edit, "001");
    public static string CodeEditDavinciV1 => ModelNameBuilder(BaseModel.Davinci, Subject.CodeEdit, "001");

    public static string CodeSearchAdaCodeV1 => ModelNameBuilder(BaseModel.Ada, Subject.CodeSearchCode, "001");
    public static string CodeSearchBabbageCodeV1 => ModelNameBuilder(BaseModel.Babbage, Subject.CodeSearchCode, "001");
    public static string CodeSearchAdaTextV1 => ModelNameBuilder(BaseModel.Ada, Subject.CodeSearchText, "001");
    public static string CodeSearchBabbageTextV1 => ModelNameBuilder(BaseModel.Babbage, Subject.CodeSearchText, "001");

    public static string TextEmbeddingAdaV2 => ModelNameBuilder(BaseModel.Ada, Subject.TextEmbedding, "002");

    /// <summary>
    ///     Most capable GPT-3.5 model and optimized for chat at 1/10th the cost of text-davinci-003. Will be updated with our
    ///     latest model iteration.
    ///     4,096 tokens	Up to Sep 2021
    /// </summary>
    [Obsolete("Use Gpt_3_5_Turbo instead, this is just a naming change,the field will be removed in next versions")]
    public static string ChatGpt3_5Turbo => "gpt-3.5-turbo";

    /// <summary>
    ///     Most capable GPT-3.5 model and optimized for chat at 1/10th the cost of text-davinci-003. Will be updated with our
    ///     latest model iteration.
    ///     4,096 tokens	Up to Sep 2021
    /// </summary>
    public static string Gpt_3_5_Turbo => "gpt-3.5-turbo";

    /// <summary>
    ///     Same capabilities as the standard gpt-3.5-turbo model but with 4 times the context.
    ///     16,384 tokens	Up to Sep 2021
    /// </summary>
    public static string Gpt_3_5_Turbo_16k => "gpt-3.5-turbo-16k";

    /// <summary>
    ///     Snapshot of gpt-3.5-turbo from March 1st 2023. Unlike gpt-3.5-turbo, this model will not receive updates, and will
    ///     only be supported for a three month period ending on June 1st 2023.
    ///     4,096 tokens	Up to Sep 2021
    /// </summary>
    [Obsolete("Use Gpt_3_5_Turbo_0301 instead, this is just a naming change,the field will be removed in next versions")]
    public static string ChatGpt3_5Turbo0301 => "gpt-3.5-turbo-0301";

    /// <summary>
    ///     Snapshot of gpt-3.5-turbo from March 1st 2023. Unlike gpt-3.5-turbo, this model will not receive updates, and will
    ///     only be supported for a three month period ending on June 1st 2023.
    ///     4,096 tokens	Up to Sep 2021
    /// </summary>
    public static string Gpt_3_5_Turbo_0301 => "gpt-3.5-turbo-0301";

    /// <summary>
    ///     Snapshot of gpt-3.5-turbo from June 13th 2023 with function calling data. Unlike gpt-3.5-turbo,
    ///     this model will not receive updates, and will be deprecated 3 months after a new version is released.
    ///     4,096 tokens	Up to Sep 2021
    /// </summary>
    public static string Gpt_3_5_Turbo_0613 => "gpt-3.5-turbo-0613";

    /// <summary>
    ///     Snapshot of gpt-3.5-turbo from June 13th 2023 with function calling data. Unlike gpt-3.5-turbo,
    ///     this model will not receive updates, and will be deprecated 3 months after a new version is released.
    ///     4,096 tokens	Up to Sep 2021
    /// </summary>
    public static string Gpt_3_5_Turbo_16k_0613 => "gpt-3.5-turbo-16k-0613";


    public static string WhisperV1 => "whisper-1";

    /// <summary>
    ///     This method does not guarantee returned model exists.
    /// </summary>
    /// <param name="subject"></param>
    /// <param name="version"></param>
    /// <param name="baseModel"></param>
    /// <returns></returns>
    public static string ModelNameBuilder(this BaseModel baseModel, Subject? subject = null, string? version = null)
    {
        return ModelNameBuilder(baseModel.EnumToString(), subject?.EnumToString(baseModel.EnumToString()), version);
    }

    public static string ModelNameBuilder(string baseModel, string? subject, string? version)
    {
        var response = subject ?? $"{baseModel}";

        if (!string.IsNullOrEmpty(version))
        {
            response += $"-{version}";
        }

        return response;
    }


    public static string EnumToString(this Model model)
    {
        return model switch
        {
            Model.Ada => Ada,
            Model.Babbage => Babbage,
            Model.Curie => Curie,
            Model.CurieInstructBeta => CurieInstructBeta,
            Model.Davinci => Davinci,
            Model.DavinciInstructBeta => DavinciInstructBeta,
            Model.TextDavinciV1 => TextDavinciV1,
            Model.TextDavinciV2 => TextDavinciV2,
            Model.TextDavinciV3 => TextDavinciV3,
            Model.TextAdaV1 => TextAdaV1,
            Model.TextBabbageV1 => TextBabbageV1,
            Model.TextCurieV1 => TextCurieV1,
            Model.CurieSimilarityFast => CurieSimilarityFast,
            Model.CodeDavinciV1 => CodeDavinciV1,
            Model.CodeCushmanV1 => CodeCushmanV1,
            Model.CodeDavinciV2 => CodeDavinciV2,
            Model.TextSimilarityAdaV1 => TextSimilarityAdaV1,
            Model.TextSimilarityBabbageV1 => TextSimilarityBabbageV1,
            Model.TextSimilarityCurieV1 => TextSimilarityCurieV1,
            Model.TextSimilarityDavinciV1 => TextSimilarityDavinciV1,
            Model.TextSearchAdaDocV1 => TextSearchAdaDocV1,
            Model.TextSearchBabbageDocV1 => TextSearchBabbageDocV1,
            Model.TextSearchCurieDocV1 => TextSearchCurieDocV1,
            Model.TextSearchDavinciDocV1 => TextSearchDavinciDocV1,
            Model.TextSearchAdaQueryV1 => TextSearchAdaQueryV1,
            Model.TextSearchBabbageQueryV1 => TextSearchBabbageQueryV1,
            Model.TextSearchCurieQueryV1 => TextSearchCurieQueryV1,
            Model.TextSearchDavinciQueryV1 => TextSearchDavinciQueryV1,
            Model.CodeSearchAdaCodeV1 => CodeSearchAdaCodeV1,
            Model.CodeSearchBabbageCodeV1 => CodeSearchBabbageCodeV1,
            Model.CodeSearchAdaTextV1 => CodeSearchAdaTextV1,
            Model.CodeSearchBabbageTextV1 => CodeSearchBabbageTextV1,
            Model.TextEditDavinciV1 => TextEditDavinciV1,
            Model.CodeEditDavinciV1 => CodeEditDavinciV1,
            Model.ChatGpt3_5Turbo => ChatGpt3_5Turbo,
            Model.Gpt_3_5_Turbo => Gpt_3_5_Turbo,
            Model.ChatGpt3_5Turbo0301 => ChatGpt3_5Turbo0301,
            Model.Gpt_3_5_Turbo_0301 => Gpt_3_5_Turbo_0301,
            Model.Gpt_3_5_Turbo_0613 => Gpt_3_5_Turbo_0613,
            Model.Gpt_3_5_Turbo_16k_0613 => Gpt_3_5_Turbo_16k_0613,
            Model.Gpt_3_5_Turbo_16k => Gpt_3_5_Turbo_16k,
            Model.WhisperV1 => WhisperV1,
            Model.TextEmbeddingAdaV2 => TextEmbeddingAdaV2,
            Model.Gpt_4 => Gpt_4,
            Model.Gpt_4_0314 => Gpt_4_0314,
            Model.Gpt_4_32k => Gpt_4_32k,
            Model.Gpt_4_32k_0314 => Gpt_4_32k_0314,
            Model.Gpt_4_0613 => Gpt_4_0613,
            Model.Gpt_4_32k_0613 => Gpt_4_32k_0613,
            _ => throw new ArgumentOutOfRangeException(nameof(model), model, null)
        };
    }

    private static string EnumToString(this BaseModel baseModel)
    {
        return baseModel switch
        {
            BaseModel.Ada => Ada,
            BaseModel.Babbage => Babbage,
            BaseModel.Curie => Curie,
            BaseModel.Davinci => Davinci,
            BaseModel.Cushman => "cushman",
            _ => throw new ArgumentOutOfRangeException(nameof(baseModel), baseModel, null)
        };
    }

    public static string EnumToString(this Subject subject, string baseModel)
    {
        return string.Format(subject switch
        {
            //{0}-{1}
            Subject.Text => "text-{0}",
            Subject.InstructBeta => "{0}-instruct-beta",
            Subject.SimilarityFast => "{0}-similarity-fast",
            Subject.TextSimilarity => "text-similarity-{0}",
            Subject.TextSearchDocument => "text-search-{0}-doc",
            Subject.TextSearchQuery => "text-search-{0}-query",
            Subject.CodeSearchCode => "code-search-{0}-code",
            Subject.CodeSearchText => "code-search-{0}-text",
            Subject.Code => "code-{0}",
            Subject.CodeEdit => "code-{0}-edit",
            Subject.Edit => "text-{0}-edit",
            Subject.TextEmbedding => "text-embedding-{0}",
            _ => throw new ArgumentOutOfRangeException(nameof(subject), subject, null)
        }, baseModel);
    }
}