using System.Net;
using System.Text;
using System.Text.Json;
using Betalgo.Ranul.OpenAI;
using Betalgo.Ranul.OpenAI.Contracts.Requests.Responses;
using Betalgo.Ranul.OpenAI.Contracts.Types;
using Betalgo.Ranul.OpenAI.Contracts.Types.TextFormat;
using Betalgo.Ranul.OpenAI.Managers;
using Shouldly;

namespace OpenAI.SDK.Tests;

public class ResponsesStructuredOutputTests
{
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task SchemaFieldsAreFlatOnActualRequest(bool strict)
    {
        using var handler = new Handler();
        using var service = new OpenAIService(new OpenAIOptions { ApiKey = "fixture-only" }, new HttpClient(handler));
        await service.CreateResponse(new CreateResponse("fixture-model", "Return JSON")
        {
            Text = new TextConfig { Format = TextFormat.JsonSchema("answer", Schema(), strict) }
        });
        using var json = JsonDocument.Parse(handler.Body!);
        var format = json.RootElement.GetProperty("text").GetProperty("format");
        format.GetProperty("type").GetString().ShouldBe("json_schema");
        format.GetProperty("name").GetString().ShouldBe("answer");
        format.GetProperty("strict").GetBoolean().ShouldBe(strict);
        format.GetProperty("schema").GetProperty("additionalProperties").GetBoolean().ShouldBeFalse();
        format.TryGetProperty("json_schema", out _).ShouldBeFalse();
    }

    [Fact]
    public void FlatSchemaResponseDeserializesAndRoundTrips()
    {
        const string json = """{"type":"json_schema","name":"answer","description":"A result","schema":{"type":"object","additionalProperties":false},"strict":false}""";
        var format = JsonSerializer.Deserialize<TextFormat>(json).ShouldBeOfType<JsonSchemaFormat>();
        format.JsonSchema!.Name.ShouldBe("answer");
        format.JsonSchema.Description.ShouldBe("A result");
        format.JsonSchema.Strict.ShouldBe(false);
        using var output = JsonDocument.Parse(JsonSerializer.Serialize<TextFormat>(format));
        output.RootElement.GetProperty("name").GetString().ShouldBe("answer");
    }

    [Theory]
    [InlineData("text")]
    [InlineData("json_object")]
    public void SimpleFormatsRoundTripWithoutRecursiveSerialization(string type)
    {
        var format = new TextFormat(type);
        var json = JsonSerializer.Serialize(format);
        JsonSerializer.Deserialize<TextFormat>(json)!.Type.ShouldBe(type);
    }

    private static Dictionary<string, object> Schema() => new()
    {
        ["type"] = "object", ["properties"] = new Dictionary<string, object>(),
        ["additionalProperties"] = false, ["required"] = Array.Empty<string>()
    };

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
