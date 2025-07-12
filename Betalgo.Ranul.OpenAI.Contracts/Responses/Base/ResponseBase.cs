////using System.Net;
////using System.Text.Json.Serialization;

////namespace Betalgo.Ranul.OpenAI.Contracts.Responses.Base;

////public abstract class ResponseObjectBase
////{
////    /// <summary>
////    ///     The type of the object returned by the API.
////    /// </summary>
////    [JsonPropertyName("object")]
////    public string? ObjectTypeName { get; set; }
////}

////public abstract class ResponseBase : ResponseObjectBase
////{
////    [JsonPropertyName("StreamEvent")]
////    public string? StreamEvent { get; set; }

////    public bool IsDelta => StreamEvent?.EndsWith("delta") ?? false;

////    public bool Successful => Error == null;

////    [JsonPropertyName("error")]
////    public ResponseError? Error { get; set; }

////    public HttpStatusCode HttpStatusCode { get; set; }
////    public ResponseHeaderValues? HeaderValues { get; set; }
////}