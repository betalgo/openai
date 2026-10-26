using System.Net;
using System.Text;
using System.Text.Json;
using Betalgo.Ranul.OpenAI;
using Betalgo.Ranul.OpenAI.Contracts.Enums.Responses;
using Betalgo.Ranul.OpenAI.Contracts.Requests.Responses;
using Betalgo.Ranul.OpenAI.Managers;
using Shouldly;

namespace OpenAI.SDK.Tests;

public class ResponseCacheOptionsTests
{
    [Fact]
    public async Task InMemoryCacheRetentionUsesApiWireName()
    {
        using var handler = new Handler();
        using var service = new OpenAIService(new OpenAIOptions { ApiKey = "fixture-only" }, new HttpClient(handler));
        await service.CreateResponse(new CreateResponse("fixture-model", "hello")
        {
            PromptCacheRetention = PromptCacheRetention.InMemory
        });
        using var json = JsonDocument.Parse(handler.Body!);
        json.RootElement.GetProperty("prompt_cache_retention").GetString().ShouldBe("in_memory");
    }

    private sealed class Handler : HttpMessageHandler
    {
        public string? Body { get; private set; }
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Body = await request.Content!.ReadAsStringAsync(cancellationToken);
            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("""{"id":"resp_fixture","status":"completed","output":[]}""", Encoding.UTF8, "application/json")
            };
        }
    }
}
