namespace OpenAI.GPT3.ObjectModels
{
    public static class Models
    {
        public enum BaseEngine
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

            CodeDavinciV2
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

        public static string CurieInstructBeta => ModelNameBuilder(BaseEngine.Curie, Subject.InstructBeta);
        public static string DavinciInstructBeta => ModelNameBuilder(BaseEngine.Davinci, Subject.InstructBeta);

        public static string TextDavinciV1 => ModelNameBuilder(BaseEngine.Davinci, Subject.Text, "001");
        public static string TextDavinciV2 => ModelNameBuilder(BaseEngine.Davinci, Subject.Text, "002");
        public static string TextDavinciV3 => ModelNameBuilder(BaseEngine.Davinci, Subject.Text, "003");
        public static string TextAdaV1 => ModelNameBuilder(BaseEngine.Ada, Subject.Text, "001");
        public static string TextBabbageV1 => ModelNameBuilder(BaseEngine.Babbage, Subject.Text, "001");
        public static string TextCurieV1 => ModelNameBuilder(BaseEngine.Curie, Subject.Text, "001");

        public static string CurieSimilarityFast => ModelNameBuilder(BaseEngine.Curie, Subject.SimilarityFast);

        public static string CodeDavinciV1 => ModelNameBuilder(BaseEngine.Davinci, Subject.Code, "001");
        public static string CodeCushmanV1 => ModelNameBuilder(BaseEngine.Cushman, Subject.Code, "001");
        public static string CodeDavinciV2 => ModelNameBuilder(BaseEngine.Davinci, Subject.Code, "002");

        public static string TextSimilarityAdaV1 => ModelNameBuilder(BaseEngine.Ada, Subject.TextSimilarity, "001");
        public static string TextSimilarityBabbageV1 => ModelNameBuilder(BaseEngine.Babbage, Subject.TextSimilarity, "001");
        public static string TextSimilarityCurieV1 => ModelNameBuilder(BaseEngine.Curie, Subject.TextSimilarity, "001");
        public static string TextSimilarityDavinciV1 => ModelNameBuilder(BaseEngine.Davinci, Subject.TextSimilarity, "001");

        public static string TextSearchAdaDocV1 => ModelNameBuilder(BaseEngine.Ada, Subject.TextSearchDocument, "001");
        public static string TextSearchBabbageDocV1 => ModelNameBuilder(BaseEngine.Babbage, Subject.TextSearchDocument, "001");
        public static string TextSearchCurieDocV1 => ModelNameBuilder(BaseEngine.Curie, Subject.TextSearchDocument, "001");
        public static string TextSearchDavinciDocV1 => ModelNameBuilder(BaseEngine.Davinci, Subject.TextSearchDocument, "001");
        public static string TextSearchAdaQueryV1 => ModelNameBuilder(BaseEngine.Ada, Subject.TextSearchQuery, "001");
        public static string TextSearchBabbageQueryV1 => ModelNameBuilder(BaseEngine.Babbage, Subject.TextSearchQuery, "001");
        public static string TextSearchCurieQueryV1 => ModelNameBuilder(BaseEngine.Curie, Subject.TextSearchQuery, "001");
        public static string TextSearchDavinciQueryV1 => ModelNameBuilder(BaseEngine.Davinci, Subject.TextSearchQuery, "001");

        public static string TextEditDavinciV1 => ModelNameBuilder(BaseEngine.Davinci, Subject.Edit, "001");
        public static string CodeEditDavinciV1 => ModelNameBuilder(BaseEngine.Davinci, Subject.CodeEdit, "001");

        public static string CodeSearchAdaCodeV1 => ModelNameBuilder(BaseEngine.Ada, Subject.CodeSearchCode, "001");
        public static string CodeSearchBabbageCodeV1 => ModelNameBuilder(BaseEngine.Babbage, Subject.CodeSearchCode, "001");
        public static string CodeSearchAdaTextV1 => ModelNameBuilder(BaseEngine.Ada, Subject.CodeSearchText, "001");
        public static string CodeSearchBabbageTextV1 => ModelNameBuilder(BaseEngine.Babbage, Subject.CodeSearchText, "001");


        /// <summary>
        ///     This method does not guarantee returned model exists.
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="version"></param>
        /// <param name="baseEngine"></param>
        /// <returns></returns>
        public static string ModelNameBuilder(this BaseEngine baseEngine, Subject? subject = null, string? version = null)
        {
            return ModelNameBuilder(baseEngine.EnumToString(), subject?.EnumToString(baseEngine.EnumToString()), version);
        }

        public static string ModelNameBuilder(string baseEngine, string? subject, string? version)
        {
            var response = subject ?? $"{baseEngine}";

            if (!string.IsNullOrEmpty(version))
            {
                response += $"-{version}";
            }

            return response;
        }


        public static string EnumToString(this Model engine)
        {
            return engine switch
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
                _ => throw new ArgumentOutOfRangeException(nameof(engine), engine, null)
            };
        }

        private static string EnumToString(this BaseEngine baseEngine)
        {
            return baseEngine switch
            {
                BaseEngine.Ada => Ada,
                BaseEngine.Babbage => Babbage,
                BaseEngine.Curie => Curie,
                BaseEngine.Davinci => Davinci,
                BaseEngine.Cushman => "cushman",
                _ => throw new ArgumentOutOfRangeException(nameof(baseEngine), baseEngine, null)
            };
        }

        public static string EnumToString(this Subject subject, string baseEngine)
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
            }, baseEngine);
        }
    }
}