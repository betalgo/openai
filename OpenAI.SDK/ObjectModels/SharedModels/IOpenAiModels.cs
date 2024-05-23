using OpenAI.ObjectModels.RequestModels;
using System.Text.Json.Serialization;

namespace OpenAI.ObjectModels.SharedModels;

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

    public interface IAssistantId
    {
        string AssistantId { get; set; }
    }

    public interface ICreatedAt
    {
        public long CreatedAt { get; set; }
    }

    public interface ICompletedAt
    {
        public int CompletedAt { get; set; }
    }

    public interface IUser
    {
        public string User { get; set; }
    }

    public interface IFile
    {
        public byte[] File { get; set; }
        public Stream FileStream { get; set; }
        public string FileName { get; set; }
    }

    public interface IMetaData
    {
        public Dictionary<string, string> Metadata { get; set; }
    }

    public interface IFileIds
    {
        public List<string> FileIds { get; set;}
    }

    public interface ITools
    {
        public List<ToolDefinition> Tools { get; set; }
    }
}