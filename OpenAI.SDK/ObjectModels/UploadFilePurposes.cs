namespace OpenAI.GPT3.ObjectModels
{
    public static class UploadFilePurposes
    {
        public enum UploadFilePurpose
        {
            FineTune
        }

        public const string FineTune = "fine-tune";

        public static string EnumToString(this UploadFilePurpose uploadFilePurpose)
        {
            return uploadFilePurpose switch
            {
                UploadFilePurpose.FineTune => FineTune,
                _ => throw new ArgumentOutOfRangeException(nameof(uploadFilePurpose), uploadFilePurpose, null)
            };
        }

        public static UploadFilePurpose ToEnum(string filePurpose)
        {
            return filePurpose switch
            {
                FineTune => UploadFilePurpose.FineTune,
                _ => throw new ArgumentOutOfRangeException(nameof(filePurpose), filePurpose, null)
            };
        }
    }
}