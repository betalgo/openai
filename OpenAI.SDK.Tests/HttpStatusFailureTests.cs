using System.Net;
using System.Text;
using Betalgo.Ranul.OpenAI;
using Betalgo.Ranul.OpenAI.Managers;
using Betalgo.Ranul.OpenAI.ObjectModels.RequestModels;
using Shouldly;

namespace OpenAI.SDK.Tests;

public class HttpStatusFailureTests
{
    private sealed class Handler(HttpStatusCode status, string json) : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken) =>
            Task.FromResult(new HttpResponseMessage(status) { Content = new StringContent(json, Encoding.UTF8, "application/json") });
    }

    [Theory]
    [InlineData(HttpStatusCode.BadRequest, "{}")]
    [InlineData(HttpStatusCode.TooManyRequests, "{\"error\":null}")]
    public async Task NonSuccessHttpStatusCannotBecomeSuccessWithoutAnErrorEnvelope(HttpStatusCode status, string json)
    {
        using var service = new OpenAIService(new OpenAIOptions { ApiKey = "offline" }, new HttpClient(new Handler(status, json)));
        var response = await service.RetrieveResponse("resp_fixture");
        response.Successful.ShouldBeFalse();
        response.HttpStatusCode.ShouldBe(status);
        response.Error!.Type.ShouldBe("http_error");
        response.Error.Code.ShouldBe(((int)status).ToString(System.Globalization.CultureInfo.InvariantCulture));
        var chat = await service.ChatCompletion.CreateCompletion(new() { Model = "test", Messages = new[] { ChatMessage.FromUser("hello") } });
        chat.Successful.ShouldBeFalse();
        chat.Error!.Type.ShouldBe("http_error");
    }
}
