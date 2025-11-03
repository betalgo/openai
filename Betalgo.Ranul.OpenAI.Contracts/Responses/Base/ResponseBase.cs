using System.Net;
using System.Text.Json.Serialization;

namespace Betalgo.Ranul.OpenAI.Contracts.Responses.Base;

public abstract class ResponseBase
{
    /// <summary>
    ///     The type of the object returned by the API.
    /// </summary>
    [JsonPropertyName("object")]
    public string? ObjectTypeName { get; set; }

    [JsonPropertyName("StreamEvent")]
    public string? StreamEvent { get; set; }

    public bool IsDelta => StreamEvent?.EndsWith("delta") ?? false;

    public bool Successful => Error == null;

    [JsonPropertyName("error")]
    public ResponseError? Error { get; set; }

    public HttpStatusCode HttpStatusCode { get; set; }

    public ResponseBaseHeaderValues? HeaderValues { get; set; }
}

public class ResponseBase<T> : ResponseBase
{
    [JsonPropertyName("data")]
    public T? Data { get; set; }
}