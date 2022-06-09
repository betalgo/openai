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

            CurieInstructBeta,
            DavinciInstructBeta,

            CurieSimilarityFast,

            CodeDavinciV1,
            CodeCushmanV1,

            CodeDavinciV2
        }

        public enum Subject
        {
            Text,
            InstructBeta,
            SimilarityFast,
            Code
        }

        public static string Ada => "ada";
        public static string Babbage => "babbage";
        public static string Curie => "curie";
        public static string Davinci => "davinci";

        public static string CurieInstructBeta => ModelNameBuilder(BaseEngine.Curie, Subject.InstructBeta);
        public static string DavinciInstructBeta => ModelNameBuilder(BaseEngine.Davinci, Subject.InstructBeta);

        public static string TextDavinciV1 => ModelNameBuilder(BaseEngine.Davinci, Subject.Text, "001");
        public static string TextDavinciV2 => ModelNameBuilder(BaseEngine.Davinci, Subject.Text, "002");
        public static string TextAdaV1 => ModelNameBuilder(BaseEngine.Ada, Subject.Text, "001");
        public static string TextBabbageV1 => ModelNameBuilder(BaseEngine.Babbage, Subject.Text, "001");
        public static string TextCurieV1 => ModelNameBuilder(BaseEngine.Curie, Subject.Text, "001");

        public static string CurieSimilarityFast => ModelNameBuilder(BaseEngine.Curie, Subject.SimilarityFast);

        public static string CodeDavinciV1 => ModelNameBuilder(BaseEngine.Davinci, Subject.Code, "001");
        public static string CodeCushmanV1 => ModelNameBuilder(BaseEngine.Cushman, Subject.Code, "001");
        public static string CodeDavinciV2 => ModelNameBuilder(BaseEngine.Davinci, Subject.Code, "002");

        /// <summary>
        ///     This method does not guarantee returned model exists.
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="version"></param>
        /// <param name="baseEngine"></param>
        /// <returns></returns>
        public static string ModelNameBuilder(this BaseEngine baseEngine, Subject? subject = null, string? version = null)
        {
            return ModelNameBuilder(baseEngine.EnumToString(), subject?.EnumToString(), version);
        }

        public static string ModelNameBuilder(string baseEngine, string? subject, string? version)
        {
            var response = baseEngine;
            if (!string.IsNullOrEmpty(subject))
            {
                response += $"-{subject}";
            }

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
                Model.TextAdaV1 => TextAdaV1,
                Model.TextBabbageV1 => TextBabbageV1,
                Model.TextCurieV1 => TextCurieV1,
                Model.CurieSimilarityFast => CurieSimilarityFast,
                Model.CodeDavinciV1 => CodeDavinciV1,
                Model.CodeCushmanV1 => CodeCushmanV1,
                Model.CodeDavinciV2 => CodeDavinciV2,
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

        private static string EnumToString(this Subject subject)
        {
            return subject switch
            {
                Subject.Text => "text",
                Subject.InstructBeta => "instruct-beta",
                Subject.SimilarityFast => "similarity-fast",
                Subject.Code => "code",
                _ => throw new ArgumentOutOfRangeException(nameof(subject), subject, null)
            };
        }
    }
}