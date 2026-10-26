using Betalgo.Ranul.OpenAI.Contracts.Responses.Responses;
using Betalgo.Ranul.OpenAI.Contracts.Types.Streaming.Events.Lifecycle;

namespace Betalgo.Ranul.OpenAI.Extensions.Streaming;

/// <summary>A text convenience helper could not produce successfully completed output.</summary>
public sealed class ResponseStreamException : InvalidOperationException
{
    /// <summary>Initializes an exception retaining the response or error event for inspection.</summary>
    public ResponseStreamException(string message, Response? response = null, ResponseErrorEvent? errorEvent = null)
        : base(message)
    {
        Response = response;
        ErrorEvent = errorEvent;
    }

    /// <summary>The terminal response, including its status, error and incomplete details, when available.</summary>
    public Response? Response { get; }

    /// <summary>The stream error event, when available.</summary>
    public ResponseErrorEvent? ErrorEvent { get; }
}
