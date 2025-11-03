namespace Betalgo.Ranul.OpenAI.Contracts.Responses.Base;

public class OpenAIInfo
{
    public string? Model { get; set; }
    public string? Organization { get; set; }
    public string? ProcessingMs { get; set; }
    public string? Version { get; set; }
}