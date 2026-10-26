using System.Net;
using System.Reflection;
using System.Text;
using Betalgo.Ranul.OpenAI;
using Betalgo.Ranul.OpenAI.Contracts.Requests.Image;
using Betalgo.Ranul.OpenAI.Contracts.Requests.Responses;
using Betalgo.Ranul.OpenAI.Managers;
using Betalgo.Ranul.OpenAI.ObjectModels.RealtimeModels;
using Shouldly;

namespace OpenAI.SDK.Tests;

public class HttpJsonResponseLifetimeTests
{
    private sealed class TrackingContent(string json) : StringContent(json, Encoding.UTF8, "application/json")
    {
        public bool Disposed { get; private set; }
        protected override void Dispose(bool disposing) { Disposed = true; base.Dispose(disposing); }
    }
    private sealed class Handler(Func<HttpRequestMessage, HttpResponseMessage> send) : HttpMessageHandler
    {
        public int Requests { get; private set; }
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        { Requests++; return Task.FromResult(send(request)); }
    }

    [Fact]
    public async Task JsonCreateRetrieveDeleteDisposeContentAfterParsing()
    {
        var contents = new List<TrackingContent>();
        var handler = new Handler(_ =>
        {
            var content = new TrackingContent("""{"id":"resp_1","status":"completed","output":[],"deleted":true}""");
            contents.Add(content);
            return new(HttpStatusCode.OK) { Content = content };
        });
        using var service = new OpenAIService(new OpenAIOptions { ApiKey = "offline" }, new HttpClient(handler));
        await service.CreateResponse(new CreateResponse { Model = "fixture", Input = "hello" });
        await service.RetrieveResponse("resp_1");
        await service.DeleteResponse("resp_1");
        await service.ChatCompletion.CreateCompletion(new() { Model = "fixture", Messages = new[] { Betalgo.Ranul.OpenAI.ObjectModels.RequestModels.ChatMessage.FromUser("hello") } });
        contents.ShouldAllBe(content => content.Disposed);
    }


    [Theory]
    [InlineData(HttpStatusCode.BadRequest, "{\"error\":{\"message\":\"fixture\"}}")]
    [InlineData(HttpStatusCode.OK, "not-json")]
    public async Task ErrorAndMalformedJsonResponsesReleaseContent(HttpStatusCode status, string body)
    {
        var content = new TrackingContent(body);
        using var service = new OpenAIService(new OpenAIOptions { ApiKey = "offline" },
            new HttpClient(new Handler(_ => new(status) { Content = content })));
        if (status == HttpStatusCode.OK)
            await Should.ThrowAsync<System.Text.Json.JsonException>(() => service.RetrieveResponse("resp_1"));
        else (await service.RetrieveResponse("resp_1")).Successful.ShouldBeFalse();
        content.Disposed.ShouldBeTrue();
    }

    [Fact]
    public async Task ReturnedBinaryStreamRemainsOwnedByCaller()
    {
        using var payload = new MemoryStream(new byte[] { 1, 2, 3 });
        using var service = new OpenAIService(new OpenAIOptions { ApiKey = "offline" },
            new HttpClient(new Handler(_ => new(HttpStatusCode.OK) { Content = new StreamContent(payload) })));
        var response = await service.Audio.CreateSpeech<Stream>(new() { Model = "gpt-4o-mini-tts", Input = "fixture", Voice = "alloy" });
        using var stream = response.Data!;
        stream.ReadByte().ShouldBe(1);
        stream.ReadByte().ShouldBe(2);
    }
}
