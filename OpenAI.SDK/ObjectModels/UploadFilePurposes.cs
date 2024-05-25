namespace OpenAI.ObjectModels;

public static class UploadFilePurposes
{
    public enum UploadFilePurpose
    {
        Assistants,
        Vision,
        Batch,
        FineTune,
        [Obsolete("Not supported by the API")]
        FineTuneResults
    }

    // REF: https://platform.openai.com/docs/api-reference/files/create
    public const string Assistants = "assistants";
    public const string Vision = "vision";
    public const string Batch = "batch";
    public const string FineTune = "fine-tune";
    [Obsolete("Not supported by the API")]
    public const string FineTuneResults = "fine-tune-results";

    public static string EnumToString(this UploadFilePurpose uploadFilePurpose)
    {
        return uploadFilePurpose switch
        {
            UploadFilePurpose.Assistants => Assistants,
            UploadFilePurpose.Vision => Vision,
            UploadFilePurpose.Batch => Batch,
            UploadFilePurpose.FineTune => FineTune,
            _ => throw new ArgumentOutOfRangeException(nameof(uploadFilePurpose), uploadFilePurpose, null)
        };
    }

    public static UploadFilePurpose ToEnum(string filePurpose)
    {
        return filePurpose switch
        {
            Assistants => UploadFilePurpose.Assistants,
            Vision => UploadFilePurpose.Vision,
            Batch => UploadFilePurpose.Batch,
            FineTune => UploadFilePurpose.FineTune,
            _ => throw new ArgumentOutOfRangeException(nameof(filePurpose), filePurpose, null)
        };
    }
}