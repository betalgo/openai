namespace OpenAI.GPT3.Models
{
    public static class Engines
    {
        public enum Engine
        {
            Ada,
            Babbage,
            Curie,
            CurieInstructBeta,
            CushmanCodex,
            Davinci,
            DavinciCodex,
            DavinciInstructBeta,
            DavinciInstructBetaV3,
            DavinciTextV1,
            DavinciTextV2
        }

        public static string Ada => "ada";
        public static string Babbage => "babbage";
        public static string Curie => "curie";
        public static string CurieInstructBeta => "curie-instruct-beta";
        public static string CushmanCodex => "cushman-codex";
        public static string Davinci => "davinci";
        public static string DavinciCodex => "davinci-codex";
        public static string DavinciInstructBeta => "davinci-instruct-beta";
        public static string DavinciInstructBetaV3 => "davinci-instruct-beta-v3";
        public static string DavinciTextV1 => "text-davinci-001";
        public static string DavinciTextV2 => "text-davinci-002";

        public static string EnumToString(this Engine engine)
        {
            return engine switch
            {
                Engine.Ada => "ada",
                Engine.Babbage => "babbage",
                Engine.Curie => "curie",
                Engine.CurieInstructBeta => "curie-instruct-beta",
                Engine.CushmanCodex => "cushman-codex",
                Engine.Davinci => "davinci",
                Engine.DavinciCodex => "davinci-codex",
                Engine.DavinciInstructBeta => "davinci-instruct-beta",
                Engine.DavinciTextV1 => "text-davinci-001",
                Engine.DavinciTextV2 => "text-davinci-002",

                _ => throw new ArgumentOutOfRangeException(nameof(engine), engine, null)
            };
        }
    }
}