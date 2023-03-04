namespace OpenAI.GPT3.ObjectModels.SharedModels;

public interface IOpenAiModels
{
    public interface IId
    {
        string Id { get; set; }
    }

    public interface IModel
    {
        string? Model { get; set; }
    }

    public interface ILogProbsRequest
    {
        int? LogProbs { get; set; }
    }

    public interface ILogProbsResponse
    {
        LogProbsResponse LogProbs { get; set; }
    }

    public interface ITemperature
    {
        float? Temperature { get; set; }
    }

    public interface ICreatedAt
    {
        public int CreatedAt { get; set; }
    }

    public interface IUser
    {
        public string User { get; set; }
    }

    public interface IFile
    {
        public byte[] File { get; set; }
        public string FileName { get; set; }
    }
}