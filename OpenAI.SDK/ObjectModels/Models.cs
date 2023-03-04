namespace OpenAI.GPT3.ObjectModels;

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

        CodeSearchAdaCodeV1,
        CodeSearchBabbageCodeV1,

        CodeSearchAdaTextV1,
        CodeSearchBabbageTextV1,

        CodeDavinciV1,
        CodeCushmanV1,

        CodeDavinciV2,

        ChatGpt3_5Turbo,
        ChatGpt3_5Turbo0301,

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
        Edit
    }

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

    public static string ChatGpt3_5Turbo => "gpt-3.5-turbo";
    public static string ChatGpt3_5Turbo0301 => "gpt-3.5-turbo-0301";

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
            Model.ChatGpt3_5Turbo0301 => ChatGpt3_5Turbo0301,
            Model.WhisperV1 => WhisperV1,
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
            _ => throw new ArgumentOutOfRangeException(nameof(subject), subject, null)
        }, baseModel);
    }
}