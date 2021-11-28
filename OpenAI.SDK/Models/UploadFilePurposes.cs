namespace OpenAI.SDK.Models
{
    public static class UploadFilePurposes
    {
        public static string Search => "search";
        public static string Answers => "answers";
        public static string Classifications => "classifications";
        public static string FineTune => "fine-tune";

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
    }

    public enum UploadFilePurpose
    {
        Search,
        Answers,
        Classifications,
        FineTune
    }
}