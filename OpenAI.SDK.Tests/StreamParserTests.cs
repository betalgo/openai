using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Betalgo.Ranul.OpenAI.Contracts.Interfaces;
using Betalgo.Ranul.OpenAI.Contracts.Types.Streaming;
using Betalgo.Ranul.OpenAI.Contracts.Types.Streaming.Events;
using Betalgo.Ranul.OpenAI.Contracts.Types.Streaming.Events.Text;
using Betalgo.Ranul.OpenAI.Extensions.Streaming;
using Shouldly;

namespace OpenAI.SDK.Tests;

public class StreamParserTests
{
    [Fact]
    public async Task ParseSseStreamAsyncParsesMultilineEventsAndStopsOnDone()
    {
        using var response = CreateEventStreamResponse("""
: comment
event: response.output_text.delta
data: {"type":"response.output_text.delta","sequence_number":1,
data: "item_id":"item_1","output_index":0,"content_index":0,"delta":"Hello"}

data: [DONE]

data: {"type":"response.output_text.delta","sequence_number":2,"item_id":"item_2","output_index":0,"content_index":0,"delta":"Ignored"}

""");

        var events = await ToListAsync(response.ParseSseStreamAsync<IResponseStreamEvent>());

        events.Count.ShouldBe(1);
        var delta = events[0].ShouldBeOfType<ResponseTextDeltaEvent>();
        delta.Delta.ShouldBe("Hello");
    }

    [Fact]
    public async Task ParseSseStreamAsyncDoesNotSilentlyDiscardMalformedEvents()
    {
        using var response = CreateEventStreamResponse("""
data: {"type":"response.output_text.delta","sequence_number":1,
data: bad-json}

data: {"type":"response.output_text.delta","sequence_number":2,"item_id":"item_2","output_index":0,"content_index":0,"delta":"Recovered"}

data: [DONE]

""");

        await Should.ThrowAsync<JsonException>(() => ToListAsync(response.ParseSseStreamAsync<IResponseStreamEvent>()));
    }

    [Fact]
    public async Task ParseSseStreamAsyncRejectsNullEventPayload()
    {
        using var response = CreateEventStreamResponse("data: null\n\n");
        await Should.ThrowAsync<JsonException>(() => ToListAsync(response.ParseSseStreamAsync<IResponseStreamEvent>()));
    }

    [Theory]
    [InlineData("{}")]
    [InlineData("{\"type\":\"\"}")]
    [InlineData("{\"type\":\"   \"}")]
    public async Task ParseSseStreamAsyncRejectsMissingConcreteEventDiscriminators(string payload)
    {
        using var response = CreateEventStreamResponse($"data: {payload}\n\n");
        await Should.ThrowAsync<JsonException>(() => ToListAsync(response.ParseSseStreamAsync<ConcreteEvent>()));
    }

    public sealed class ConcreteEvent : IStreamEvent
    {
        [JsonPropertyName("type")]
        public string Type { get; set; } = null!;
        public int SequenceNumber { get; set; }
    }

    [Fact]
    public async Task ParseSseStreamAsyncPreservesUnknownFutureEvents()
    {
        using var response = CreateEventStreamResponse("data: {\"type\":\"response.future_event\",\"sequence_number\":8,\"new_field\":\"value\"}\n\n");
        var events = await ToListAsync(response.ParseSseStreamAsync<IResponseStreamEvent>());
        var unknown = events.Single().ShouldBeOfType<UnknownResponseStreamEvent>();
        unknown.Type.ShouldBe("response.future_event");
        unknown.SequenceNumber.ShouldBe(8);
        using var json = JsonDocument.Parse(unknown.RawJson);
        json.RootElement.GetProperty("new_field").GetString().ShouldBe("value");
    }

    [Fact]
    public async Task ParseSseStreamAsyncReadsFinalCompleteJsonWithoutTrailingBlankLine()
    {
        using var response = CreateEventStreamResponse("data: {\"type\":\"response.output_text.delta\",\"sequence_number\":1,\"item_id\":\"item_1\",\"output_index\":0,\"content_index\":0,\"delta\":\"Last\"}");
        var events = await ToListAsync(response.ParseSseStreamAsync<IResponseStreamEvent>());
        events.Single().ShouldBeOfType<ResponseTextDeltaEvent>().Delta.ShouldBe("Last");
    }

    [Fact]
    public async Task ParseSseStreamAsyncObservesCancellationWhileReadIsPending()
    {
        var stream = new BlockingReadStream();
        using var response = new HttpResponseMessage(HttpStatusCode.OK) { Content = new StreamContent(stream) };
        using var cancellation = new CancellationTokenSource();
        var reading = ToListAsync(response.ParseSseStreamAsync<IResponseStreamEvent>(cancellationToken: cancellation.Token));
        await stream.ReadStarted.Task.WaitAsync(TimeSpan.FromSeconds(5));
        cancellation.Cancel();
        await Should.ThrowAsync<OperationCanceledException>(() => reading.WaitAsync(TimeSpan.FromSeconds(5)));
    }

    private static HttpResponseMessage CreateEventStreamResponse(string payload)
    {
        return new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(payload, Encoding.UTF8, "text/event-stream")
        };
    }

    private static async Task<List<T>> ToListAsync<T>(IAsyncEnumerable<T> values)
    {
        var results = new List<T>();
        await foreach (var value in values)
        {
            results.Add(value);
        }

        return results;
    }

    private sealed class BlockingReadStream : Stream
    {
        private readonly CancellationTokenSource _disposed = new();
        public TaskCompletionSource<bool> ReadStarted { get; } = new(TaskCreationOptions.RunContinuationsAsynchronously);
        public override bool CanRead => true;
        public override bool CanSeek => false;
        public override bool CanWrite => false;
        public override long Length => throw new NotSupportedException();
        public override long Position { get => throw new NotSupportedException(); set => throw new NotSupportedException(); }
        public override int Read(byte[] buffer, int offset, int count) => throw new NotSupportedException();
        public override async Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            ReadStarted.TrySetResult(true);
            using var linked = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, _disposed.Token);
            await Task.Delay(Timeout.Infinite, linked.Token);
            return 0;
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing) _disposed.Cancel();
            base.Dispose(disposing);
        }
        public override void Flush() => throw new NotSupportedException();
        public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException();
        public override void SetLength(long value) => throw new NotSupportedException();
        public override void Write(byte[] buffer, int offset, int count) => throw new NotSupportedException();
    }
}
