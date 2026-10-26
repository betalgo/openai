using System.Net;
using System.Text;
using System.Text.Json;
using Betalgo.Ranul.OpenAI;
using Betalgo.Ranul.OpenAI.Contracts.Requests.Responses;
using Betalgo.Ranul.OpenAI.Contracts.Types;
using Betalgo.Ranul.OpenAI.Contracts.Types.Items;
using Betalgo.Ranul.OpenAI.Managers;
using Shouldly;

namespace OpenAI.SDK.Tests;

public class ResponseMessageReplayTests
{
    [Fact]
    public async Task AssistantPhaseIdAndAdditionalMessageFieldsSurviveResponseAndManualReplay()
    {
        const string responseJson = """
            {"id":"resp_1","output":[{"id":"msg_1","type":"message","role":"assistant","phase":"commentary","status":"completed","content":[{"type":"output_text","text":"Checking","annotations":[]}],"future_message":{"flag":false}}]}
            """;
        var requests = new List<string>();
        using var client = new HttpClient(new Handler(async request =>
        {
            requests.Add(await request.Content!.ReadAsStringAsync());
            return new(HttpStatusCode.OK) { Content = new StringContent(responseJson, Encoding.UTF8, "application/json") };
        }));
        using var service = new OpenAIService(new OpenAIOptions { ApiKey = "offline-only" }, client);
        var first = await service.CreateResponse(new CreateResponse("test-model", "hello"));
        var history = JsonSerializer.Deserialize<List<IInputItem>>(JsonSerializer.Serialize(first.Output))!;
        await service.CreateResponse(new CreateResponse("test-model", history));
        using var outbound = JsonDocument.Parse(requests[1]);
        var replay = outbound.RootElement.GetProperty("input")[0];
        replay.GetProperty("phase").GetString().ShouldBe("commentary");
        replay.GetProperty("id").GetString().ShouldBe("msg_1");
        replay.GetProperty("future_message").GetProperty("flag").GetBoolean().ShouldBeFalse();
    }

    [Fact]
    public void OrdinaryUserMessageDoesNotAcquireAnAssistantPhase()
    {
        var item = JsonSerializer.Deserialize<IInputItem>("""{"type":"message","role":"user","content":[{"type":"input_text","text":"hello"}]}""")!;
        using var json = JsonDocument.Parse(JsonSerializer.Serialize(item, new JsonSerializerOptions { DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault }));
        json.RootElement.TryGetProperty("phase", out _).ShouldBeFalse();
    }

    private sealed class Handler(Func<HttpRequestMessage, Task<HttpResponseMessage>> send) : HttpMessageHandler
    { protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken) => send(request); }
}
