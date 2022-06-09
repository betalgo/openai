namespace OpenAI.GPT3.ObjectModels
{
    public static class UploadFilePurposes
    {
        public enum UploadFilePurpose
        {
            Search,
            Answers,
            Classifications,
            FineTune
        }

        public const string Search = "search";
        public const string Answers = "answers";
        public const string Classifications = "classifications";
        public const string FineTune = "fine-tune";

        public static string EnumToString(this UploadFilePurpose uploadFilePurpose)
        {
            return uploadFilePurpose switch
            {
                UploadFilePurpose.Search => Search,
                UploadFilePurpose.Answers => Answers,
                UploadFilePurpose.Classifications => Classifications,
                UploadFilePurpose.FineTune => FineTune,
                _ => throw new ArgumentOutOfRangeException(nameof(uploadFilePurpose), uploadFilePurpose, null)
            };
        }

        public static UploadFilePurpose ToEnum(string filePurpose)
        {
            return filePurpose switch
            {
                Search => UploadFilePurpose.Search,
                Answers => UploadFilePurpose.Answers,
                Classifications => UploadFilePurpose.Classifications,
                FineTune => UploadFilePurpose.FineTune,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}