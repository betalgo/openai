using System.Net;
using System.Text;
using System.Text.Json;
using Betalgo.Ranul.OpenAI;
using Betalgo.Ranul.OpenAI.Contracts.Requests.Responses;
using Betalgo.Ranul.OpenAI.Managers;
using Shouldly;

namespace OpenAI.SDK.Tests;

public class AzureResponsesRouteTests
{
    [Theory]
    [InlineData("2025-04-01-preview", "/openai/responses", "?api-version=2025-04-01-preview")]
    [InlineData("v1", "/openai/v1/responses", "")]
    [InlineData("preview", "/openai/v1/responses", "?api-version=preview")]
    public async Task ResponsesUseResourceRoutesWithCallerSelectedVersionAndBodyDeployment(string version, string path, string query)
    {
        var handler = new Capture();
        using var service = new OpenAIService(new OpenAIOptions
        {
            ApiKey = "offline", ProviderType = ProviderType.Azure, ResourceName = "fixture",
            DeploymentId = "chat-deployment", ApiVersion = version
        }, new HttpClient(handler));
        await service.CreateResponse(new CreateResponse("response-deployment", "hello"));
        await service.RetrieveResponse("resp_1");
        await service.DeleteResponse("resp_1");
        await service.ListInputItems("resp_1", new InputItemsListRequest { Limit = 2 });
        handler.Requests.Select(x => x.Path).ShouldBe(new[] { path, path + "/resp_1", path + "/resp_1", path + "/resp_1/input_items" });
        handler.Requests.Take(3).ShouldAllBe(x => x.Query == query);
        handler.Requests[3].Query.ShouldBe(query.Length == 0 ? "?limit=2" : query + "&limit=2");
        using var body = JsonDocument.Parse(handler.Requests[0].Body!);
        body.RootElement.GetProperty("model").GetString().ShouldBe("response-deployment");
        handler.Requests.ShouldAllBe(x => x.HasAzureAuth);
    }

    [Fact]
    public async Task DatedChatRoutesRemainDeploymentScoped()
    {
        var handler = new Capture();
        using var service = new OpenAIService(new OpenAIOptions
        {
            ApiKey = "offline", ProviderType = ProviderType.Azure, ResourceName = "fixture",
            DeploymentId = "chat-deployment", ApiVersion = "2025-04-01-preview"
        }, new HttpClient(handler));
        await service.ChatCompletion.CreateCompletion(new Betalgo.Ranul.OpenAI.ObjectModels.RequestModels.ChatCompletionCreateRequest
        {
            Model = "chat-model", Messages = [Betalgo.Ranul.OpenAI.ObjectModels.RequestModels.ChatMessage.FromUser("hello")]
        });
        handler.Requests.Single().Path.ShouldBe("/openai/deployments/chat-deployment/chat/completions");
        handler.Requests.Single().Query.ShouldBe("?api-version=2025-04-01-preview");
    }

    private sealed class Capture : HttpMessageHandler
    {
        public List<(string Path, string Query, string? Body, bool HasAzureAuth)> Requests { get; } = [];
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Requests.Add((request.RequestUri!.AbsolutePath, request.RequestUri.Query,
                request.Content == null ? null : await request.Content.ReadAsStringAsync(cancellationToken), request.Headers.Contains("api-key")));
            return new(HttpStatusCode.OK) { Content = new StringContent("{\"id\":\"resp_1\",\"deleted\":true,\"data\":[],\"output\":[]}", Encoding.UTF8, "application/json") };
        }
    }
}
