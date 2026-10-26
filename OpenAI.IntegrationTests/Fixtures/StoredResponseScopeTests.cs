using System.Net;
using System.Text;
using Betalgo.Ranul.OpenAI.Contracts.Requests.Responses;
using Microsoft.Extensions.Configuration;
using OpenAI.IntegrationTests.Helpers;

namespace OpenAI.IntegrationTests.Fixtures;

[Trait("Category", "Offline")]
public class StoredResponseScopeTests
{
    [Fact]
    public async Task CreatedResponseIsDeletedEvenWhenAnAssertionPathFails()
    {
        var deleted = new List<string>();
        using var fixture = Fixture(deleted);
        var error = await Assert.ThrowsAsync<InvalidOperationException>(async () =>
        {
            await using var cleanup = new StoredResponseScope(fixture.OpenAI.AsResponses());
            await cleanup.Create(new CreateResponse("offline-model", "hello"));
            throw new InvalidOperationException("simulated assertion failure");
        });
        Assert.Equal("simulated assertion failure", error.Message);
        Assert.Equal(["resp_1"], deleted);
    }

    [Fact]
    public async Task CleanupAttemptsEveryTrackedResourceAndReportsDeletionFailure()
    {
        var deleted = new List<string>();
        using var fixture = Fixture(deleted, failDeletes: true);
        var cleanup = new StoredResponseScope(fixture.OpenAI.AsResponses());
        await cleanup.Create(new CreateResponse("offline-model", "one"));
        await cleanup.Create(new CreateResponse("offline-model", "two"));
        await Assert.ThrowsAsync<AggregateException>(async () => await cleanup.DisposeAsync());
        Assert.Equal(["resp_2", "resp_1"], deleted);
    }

    private static OpenAITestFixture Fixture(List<string> deleted, bool failDeletes = false)
    {
        var configuration = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string?>
        { ["OPENAI_API_KEY"] = "offline-test-key" }).Build();
        return new(configuration, new Handler(deleted, failDeletes));
    }

    private sealed class Handler(List<string> deleted, bool failDeletes) : HttpMessageHandler
    {
        private int _created;
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var delete = request.Method == HttpMethod.Delete;
            if (delete) deleted.Add(request.RequestUri!.Segments.Last());
            var body = delete
                ? failDeletes ? "{\"error\":{\"code\":\"fixture_failure\",\"message\":\"delete failed\"}}" : "{\"id\":\"resp_1\",\"deleted\":true}"
                : "{\"id\":\"resp_" + ++_created + "\",\"status\":\"incomplete\",\"output\":[]}";
            return Task.FromResult(new HttpResponseMessage(delete && failDeletes ? HttpStatusCode.BadRequest : HttpStatusCode.OK)
            { Content = new StringContent(body, Encoding.UTF8, "application/json") });
        }
    }
}
