namespace Betalgo.Ranul.OpenAI.Contracts.Responses.Base;

public class ResponseBaseHeaderValues
{
    public DateTimeOffset? Date { get; set; }
    public string? Connection { get; set; }
    public string? AccessControlAllowOrigin { get; set; }
    public string? CacheControl { get; set; }
    public string? Vary { get; set; }
    public string? XRequestId { get; set; }
    public string? StrictTransportSecurity { get; set; }

    // ReSharper disable once InconsistentNaming
    public string? CFCacheStatus { get; set; }
    public List<string>? SetCookie { get; set; }
    public string? Server { get; set; }

    // ReSharper disable once InconsistentNaming
    public string? CF_RAY { get; set; }
    public string? AltSvc { get; set; }
    public Dictionary<string, IEnumerable<string>>? All { get; set; }
    public RateLimitInfo? RateLimits { get; set; }
    public OpenAIInfo? OpenAI { get; set; }
}