using System.Net;
using System.Text;
using System.Text.Json;
using Betalgo.OpenAI.Utilities.FunctionCalling;
using Betalgo.Ranul.OpenAI;
using Betalgo.Ranul.OpenAI.Contracts.Requests.Responses;
using Betalgo.Ranul.OpenAI.Contracts.Types.Tools;
using Betalgo.Ranul.OpenAI.Managers;

namespace OpenAI.Utilities.Tests;

public class ResponsesGeneratedSchemaTests
{
    [Fact]
    public async Task Net9DefaultOptionsGenerateAUsableStrictToolThroughHttp()
    {
        var tool = FunctionCallingHelper.CreateFunctionToolFromTypeNet9<Parameters>("lookup");
        var root = await CaptureParameters(tool);
        AssertStrictObject(root);
        root.GetProperty("properties").GetProperty("Count").GetProperty("type").GetString().ShouldBe("integer");
    }

    [Fact]
    public async Task TypedGeneratorDoesNotEmitNullSchemaKeywordsAndRequiresValueProperties()
    {
        var root = await CaptureParameters(FunctionCallingHelper.CreateFunctionToolFromType<Parameters>("lookup"));
        AssertStrictObject(root);
        var city = root.GetProperty("properties").GetProperty("City");
        city.TryGetProperty("const", out _).ShouldBeFalse();
        city.TryGetProperty("properties", out _).ShouldBeFalse();
        city.TryGetProperty("required", out _).ShouldBeFalse();
        root.GetProperty("properties").GetProperty("Children").GetProperty("items").GetProperty("required")
            .EnumerateArray().Select(x => x.GetString()).ShouldContain("Count");
    }

    [Fact]
    public async Task Net9CallerOptionsAreRespectedWithoutBeingFrozenAndNestedObjectsAreStrict()
    {
        var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower };
        var root = await CaptureParameters(FunctionCallingHelper.CreateFunctionToolFromTypeNet9<Parameters>("lookup", options: options));
        options.IsReadOnly.ShouldBeFalse();
        root.GetProperty("properties").TryGetProperty("city", out _).ShouldBeTrue();
        AssertStrictObject(root);
    }

    [Fact]
    public async Task ExplicitNonStrictAndRawSchemasKeepTheirRequiredPolicy()
    {
        var generated = await CaptureParameters(FunctionCallingHelper.CreateFunctionToolFromType<Parameters>("lookup", strict: false));
        generated.GetProperty("required").EnumerateArray().Select(x => x.GetString()).ShouldNotContain("Count");
        const string raw = """{"type":"object","properties":{"city":{"type":"string"}},"required":[],"additionalProperties":true}""";
        var supplied = await CaptureParameters(FunctionCallingHelper.CreateFunctionTool("lookup", null, raw, strict: false));
        using var expected = JsonDocument.Parse(raw);
        JsonElement.DeepEquals(expected.RootElement, supplied).ShouldBeTrue();
    }

    [Fact]
    public async Task EmptyNet9ParameterObjectIsStillClosedInStrictMode()
    {
        var schema = await CaptureParameters(FunctionCallingHelper.CreateFunctionToolFromTypeNet9<EmptyParameters>("ping"));
        schema.GetProperty("type").GetString().ShouldBe("object");
        schema.GetProperty("additionalProperties").GetBoolean().ShouldBeFalse();
        schema.GetProperty("required").GetArrayLength().ShouldBe(0);
    }

    private static void AssertStrictObject(JsonElement schema)
    {
        if (schema.ValueKind == JsonValueKind.Array) { foreach (var node in schema.EnumerateArray()) AssertStrictObject(node); return; }
        if (schema.ValueKind != JsonValueKind.Object) return;
        if (schema.TryGetProperty("properties", out var properties) && properties.ValueKind == JsonValueKind.Object)
        {
            schema.GetProperty("additionalProperties").GetBoolean().ShouldBeFalse();
            var required = schema.GetProperty("required").EnumerateArray().Select(x => x.GetString()).ToArray();
            required.OrderBy(x => x).ShouldBe(properties.EnumerateObject().Select(x => x.Name).OrderBy(x => x));
        }
        foreach (var field in schema.EnumerateObject()) AssertStrictObject(field.Value);
    }

    private static async Task<JsonElement> CaptureParameters(FunctionTool tool)
    {
        string? body = null;
        using var client = new HttpClient(new Handler(async request => { body = await request.Content!.ReadAsStringAsync(); return new(HttpStatusCode.OK) { Content = new StringContent("{\"id\":\"resp_test\",\"output\":[]}", Encoding.UTF8, "application/json") }; }));
        using var service = new OpenAIService(new OpenAIOptions { ApiKey = "offline-only" }, client);
        await service.CreateResponse(new CreateResponse("test-model", "hello") { Tools = [tool] });
        using var json = JsonDocument.Parse(body!);
        return json.RootElement.GetProperty("tools")[0].GetProperty("parameters").Clone();
    }
    private sealed class Handler(Func<HttpRequestMessage, Task<HttpResponseMessage>> send) : HttpMessageHandler
    { protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken) => send(request); }
    public class EmptyParameters { }
    public class Parameters { public string City { get; set; } = ""; public int Count { get; set; } public List<Child> Children { get; set; } = []; }
    public class Child { public string Label { get; set; } = ""; public int Count { get; set; } }
}
