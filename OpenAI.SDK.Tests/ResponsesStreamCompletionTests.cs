using System.Net;
using Betalgo.Ranul.OpenAI;
using System.Text;
using System.Text.Json;
using Betalgo.Ranul.OpenAI.Contracts.Requests.Responses;
using Betalgo.Ranul.OpenAI.Contracts.Types.Streaming;
using Betalgo.Ranul.OpenAI.Contracts.Types.Streaming.Events.Lifecycle;
using Betalgo.Ranul.OpenAI.Managers;
using Shouldly;

namespace OpenAI.SDK.Tests;

public class ResponsesStreamCompletionTests
{
    [Theory]
    [InlineData("")]
    [InlineData("data: [DONE]\n\n")]
    [InlineData("data: {\"type\":\"response.output_text.delta\",\"delta\":\"partial\"}\n\n")]
    public async Task CreateStreamRejectsEndOfStreamWithoutTerminalResponse(string payload)
    {
        using var service = CreateService(payload);
        await Should.ThrowAsync<EndOfStreamException>(() => ReadAll(service.CreateAsStreamAsync(Request())));
    }

    [Fact]
    public async Task CreateStreamSurfacesMalformedJson()
    {
        using var service = CreateService("data: {not-json}\n\n");
        await Should.ThrowAsync<JsonException>(() => ReadAll(service.CreateAsStreamAsync(Request())));
    }

    [Fact]
    public async Task CreateStreamRejectsMissingTerminalResponseObject()
    {
        using var service = CreateService("data: {\"type\":\"response.completed\",\"sequence_number\":1}\n\n");
        await Should.ThrowAsync<JsonException>(() => ReadAll(service.CreateAsStreamAsync(Request())));
    }

    [Theory]
    [InlineData("response.completed", "completed")]
    [InlineData("response.failed", "failed")]
    [InlineData("response.incomplete", "incomplete")]
    public async Task CreateStreamPreservesTerminalResponseForCaller(string type, string status)
    {
        using var service = CreateService("data: {\"type\":\"" + type + "\",\"sequence_number\":1,\"response\":{\"id\":\"resp_test\",\"status\":\"" + status + "\"}}\n\n");
        var values = await ReadAll(service.CreateAsStreamAsync(Request()));
        values.Single().Type.ShouldBe(type);
    }

    [Fact]
    public async Task RetrieveFullStreamRejectsTruncation()
    {
        using var service = CreateService("data: [DONE]\n\n");
        await Should.ThrowAsync<EndOfStreamException>(() => ReadAll(service.RetrieveAsStreamAsync("resp_test")));
    }

    [Fact]
    public async Task RetrieveTailMayBeEmptyWhenCallerStartsAfterLastEvent()
    {
        using var service = CreateService("data: [DONE]\n\n");
        var values = await ReadAll(service.RetrieveAsStreamAsync("resp_test", new RetrieveResponseRequest { StartingAfter = 42 }));
        values.ShouldBeEmpty();
    }

    private static CreateResponse Request() => new() { Model = "fixture-model", Input = "hello" };

    private static OpenAIService CreateService(string payload) =>
        new(new OpenAIOptions { ApiKey = "fixture-only" }, new HttpClient(new Handler(payload)));

    private static async Task<List<IResponseStreamEvent>> ReadAll(IAsyncEnumerable<IResponseStreamEvent> stream)
    {
        var values = new List<IResponseStreamEvent>();
        await foreach (var value in stream) values.Add(value);
        return values;
    }

    private sealed class Handler(string payload) : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken) =>
            Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(payload, Encoding.UTF8, "text/event-stream")
            });
    }
}
