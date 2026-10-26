using System.Net;
using System.Text;
using Betalgo.Ranul.OpenAI;
using Betalgo.Ranul.OpenAI.Managers;
using Shouldly;

namespace OpenAI.SDK.Tests;

public class ResponseIdRouteTests
{
    private sealed class Capture : HttpMessageHandler
    {
        public List<Uri> Uris { get; } = new();
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Uris.Add(request.RequestUri!);
            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
            { Content = new StringContent("""{"id":"resp_fixture","deleted":true,"data":[],"output":[],"status":"completed"}""", Encoding.UTF8, "application/json") });
        }
    }

    [Theory]
    [InlineData(false, "../files/foo")]
    [InlineData(true, "../files/foo")]
    [InlineData(false, "resp_1?mode=x#fragment")]
    [InlineData(true, "resp_1?mode=x#fragment")]
    public async Task ResponseIdsRemainOneOpaqueSegmentForEveryOperation(bool azure, string id)
    {
        var handler = new Capture();
        var options = new OpenAIOptions { ApiKey = "offline" };
        if (azure)
        {
            options.ProviderType = ProviderType.Azure;
            options.ResourceName = "resource";
            options.DeploymentId = "deployment";
            options.ApiVersion = "2025-04-01-preview";
        }
        using var service = new OpenAIService(options, new HttpClient(handler));
        await service.RetrieveResponse(id);
        await service.DeleteResponse(id);
        await service.CancelResponse(id);
        await service.ListInputItems(id);
        var prefix = azure ? "/openai/responses/" : "/v1/responses/";
        var segment = Uri.EscapeDataString(id);
        handler.Uris.Select(uri => uri.AbsolutePath).ShouldBe(new[] { prefix + segment, prefix + segment, prefix + segment + "/cancel", prefix + segment + "/input_items" });
        handler.Uris.ShouldAllBe(uri => uri.Fragment == string.Empty);
        handler.Uris.ShouldAllBe(uri => uri.Query == (azure ? "?api-version=2025-04-01-preview" : string.Empty));
    }

    [Theory]
    [InlineData(".")]
    [InlineData("..")]
    public async Task BareDotSegmentsAreRejectedBeforeRequests(string id)
    {
        var handler = new Capture();
        using var service = new OpenAIService(new OpenAIOptions { ApiKey = "offline" }, new HttpClient(handler));
        await Should.ThrowAsync<ArgumentException>(() => service.RetrieveResponse(id));
        await Should.ThrowAsync<ArgumentException>(() => service.DeleteResponse(id));
        await Should.ThrowAsync<ArgumentException>(() => service.CancelResponse(id));
        await Should.ThrowAsync<ArgumentException>(() => service.ListInputItems(id));
        handler.Uris.ShouldBeEmpty();
    }
}
