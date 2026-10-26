using System.Net;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Betalgo.OpenAI.Utilities.FunctionCalling;
using Betalgo.Ranul.OpenAI;
using Betalgo.Ranul.OpenAI.Contracts.Requests.Responses;
using Betalgo.Ranul.OpenAI.Contracts.Types;
using Betalgo.Ranul.OpenAI.Contracts.Types.Tools;
using Betalgo.Ranul.OpenAI.Contracts.Types.TextFormat;
using Betalgo.Ranul.OpenAI.Contracts.Types.Content;
using Betalgo.Ranul.OpenAI.Contracts.Types.Items;
using Betalgo.Ranul.OpenAI.Contracts.Types.Streaming.Events.Lifecycle;
using Betalgo.Ranul.OpenAI.Contracts.Types.Streaming.Events.Text;
using Betalgo.Ranul.OpenAI.Managers;

var packageVersion = typeof(OpenAIService).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;
var candidateCommit = packageVersion?.Split('+').LastOrDefault();
var mode = args.Length == 0 ? "--preflight" : args[0];
var scenario = args.Length < 2 ? "basic" : args[1];
var key = mode == "--offline" ? null : Environment.GetEnvironmentVariable("OPENAI_API_KEY");
var model = mode == "--offline" ? null : Environment.GetEnvironmentVariable("OPENAI_TEST_MODEL");
var report = new Dictionary<string, object?>
{
    ["timestamp_utc"] = DateTimeOffset.UtcNow,
    ["candidate_commit"] = candidateCommit,
    ["candidate_package"] = packageVersion,
    ["utilities_package"] = typeof(FunctionCallingHelper).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion,
    ["runtime"] = RuntimeInformation.FrameworkDescription,
    ["os"] = RuntimeInformation.OSDescription,
    ["architecture"] = RuntimeInformation.ProcessArchitecture.ToString(),
    ["mode"] = mode,
    ["scenario"] = scenario
};

void WriteReport() => Console.WriteLine(JsonSerializer.Serialize(report, new JsonSerializerOptions { WriteIndented = true }));
void Require([DoesNotReturnIf(false)] bool condition, string message)
{
    if (!condition)
    {
        report["failed_check"] = message;
        throw new InvalidOperationException(message);
    }
}

if (mode == "--preflight")
{
    report["result"] = "PREFLIGHT_ONLY";
    report["api_key_configured"] = !string.IsNullOrWhiteSpace(key);
    report["test_model_configured"] = !string.IsNullOrWhiteSpace(model);
    report["sdk_assembly_version"] = typeof(OpenAIService).Assembly.GetName().Version?.ToString();
    report["sdk_informational_version"] = typeof(OpenAIService).Assembly
        .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;
    report["live_requests"] = 0;
    WriteReport();
    return 0;
}

if ((mode != "--offline" && mode != "--live") || !new[] { "basic", "stream", "structured", "function", "context", "function-generated", "function-generated-net9", "manual-context" }.Contains(scenario))
{
    report["result"] = "INVALID_ARGUMENTS";
    report["usage"] = "ResponsesSmoke [--preflight | --offline basic|stream|structured|function|context|function-generated|function-generated-net9|manual-context | --live basic|stream|structured|function|context|function-generated|function-generated-net9|manual-context]";
    WriteReport();
    return 2;
}

if (mode == "--live" && (string.IsNullOrWhiteSpace(key) || string.IsNullOrWhiteSpace(model)))
{
    report["result"] = "NOT_RUN_MISSING_CONFIGURATION";
    report["requires"] = "OPENAI_API_KEY and OPENAI_TEST_MODEL configured locally";
    report["live_requests"] = 0;
    WriteReport();
    return 2;
}

var offline = mode == "--offline";
var functionScenario = scenario is "function" or "function-generated" or "function-generated-net9";
var storedScenario = functionScenario || scenario == "context";
var twoRequests = storedScenario || scenario == "manual-context";
var metadata = new MetadataHandler(offline ? new FixtureHandler(scenario) : new HttpClientHandler(), storedScenario ? 4 : twoRequests ? 2 : 1);
using var http = new HttpClient(metadata) { Timeout = TimeSpan.FromSeconds(60) };
using var service = new OpenAIService(new OpenAIOptions { ApiKey = offline ? "fixture-only" : key! }, http);
using var deadline = new CancellationTokenSource(TimeSpan.FromSeconds(60));
var request = new CreateResponse
{
    Model = offline ? "fixture-model" : model!,
    Input = "Reply with the single word READY.",
    Store = false,
    MaxOutputTokens = 256,
    Reasoning = new() { Effort = "none" },
    ServiceTier = "default"
};
report["requested_model"] = request.Model;
report["max_output_tokens"] = request.MaxOutputTokens;

var storedIds = new List<string>();
var usages = new List<object>();
void CheckResponse(Betalgo.Ranul.OpenAI.Contracts.Responses.Responses.Response response)
{
    Require(response.Successful, "API returned an error response");
    Require(response.Status?.ToString() == "completed", "Response did not complete");
    Require(response.Usage is { TotalTokens: > 0 }, "Missing token usage");
    usages.Add(new { response.Id, response.Model, response.Usage });
}
try
{
    if (scenario == "basic")
    {
        var response = await service.CreateResponse(request, deadline.Token);
        report["http_status"] = metadata.StatusCode;
        report["response_id"] = response.Id;
        report["response_status"] = response.Status?.ToString();
        report["returned_model"] = response.Model;
        report["usage"] = response.Usage;
        report["error_code"] = response.Error?.Code;
        report["error_type"] = response.Error?.Type;
        report["output_text_property_present"] = !string.IsNullOrWhiteSpace(response.OutputText);
        Require(response.Successful, "API returned an error response");
        Require(response.Status?.ToString() == "completed", "Response did not complete");
        var text = string.Concat((response.Output ?? []).OfType<OutputMessageItem>()
            .SelectMany(message => message.Content).OfType<OutputTextContent>().Select(content => content.Text));
        Require(!string.IsNullOrWhiteSpace(text), "No parsed text output");
        Require(response.Usage is { TotalTokens: > 0 }, "No parsed token usage");
        report["parsed_text_characters"] = text.Length;
    }
    else if (scenario == "stream")
    {
        var count = 0;
        var text = new StringBuilder();
        ResponseCompletedEvent? completed = null;
        await foreach (var evt in service.CreateAsStreamAsync(request, deadline.Token))
        {
            count++;
            if (evt is ResponseTextDeltaEvent delta) text.Append(delta.Delta);
            if (evt is ResponseCompletedEvent terminal) completed = terminal;
        }
        Require(completed is not null, "No parsed response.completed event");
        Require(text.Length > 0, "No parsed text deltas");
        Require(completed.Response.Status?.ToString() == "completed", "Streamed response did not complete");
        Require(completed.Response.Usage is { TotalTokens: > 0 }, "No parsed streaming token usage");
        report["response_id"] = completed.Response.Id;
        report["returned_model"] = completed.Response.Model;
        report["usage"] = completed.Response.Usage;
        report["event_count"] = count;
        report["parsed_text_characters"] = text.Length;
    }
    else if (scenario == "structured")
    {
        request.Input = "Return the integer 7 in the answer field.";
        request.Text = new TextConfig { Format = TextFormat.JsonSchema("answer", new()
        {
            ["type"] = "object", ["additionalProperties"] = false,
            ["properties"] = new Dictionary<string, object>
            {
                ["answer"] = new Dictionary<string, object> { ["type"] = "integer" }
            },
            ["required"] = new[] { "answer" }
        }, strict: true) };
        var response = await service.CreateResponse(request, deadline.Token);
        CheckResponse(response);
        using var json = JsonDocument.Parse(response.OutputText!);
        Require(json.RootElement.GetProperty("answer").GetInt32() == 7, "Structured value mismatch");
        Require(json.RootElement.EnumerateObject().Count() == 1, "Unexpected structured properties");
        report["validated_schema"] = "one required integer answer; no additional properties";
    }
    else if (storedScenario)
    {
        request.Store = true;
        if (functionScenario)
        {
            request.Input = "Call get_test_value with key alpha. After receiving its result, reply with exactly that result.";
            request.Tools = [new FunctionTool
            {
                Name = "get_test_value", Strict = true,
                Parameters = """{"type":"object","properties":{"key":{"type":"string","enum":["alpha"]}},"required":["key"],"additionalProperties":false}"""
            }];
            if (scenario != "function")
            {
                request.Input = "Call get_test_value with key alpha, details label fixture, and samples containing only 7. After receiving its result, reply with exactly that result.";
                request.Tools = [scenario == "function-generated"
                    ? FunctionCallingHelper.CreateFunctionToolFromType<GeneratedTestValueParameters>("get_test_value", "Read a fixed synthetic test value")
                    : FunctionCallingHelper.CreateFunctionToolFromTypeNet9<GeneratedTestValueParameters>("get_test_value", "Read a fixed synthetic test value")];
                report["schema_generator"] = scenario;
            }
            request.ToolChoice = "required";
        }
        else request.Input = "Remember this code for my next question: BLUE_7319. Reply OK.";
        var first = await service.CreateResponse(request, deadline.Token);
        if (!string.IsNullOrEmpty(first.Id)) storedIds.Add(first.Id);
        CheckResponse(first);
        request.PreviousResponseId = first.Id;
        request.Store = true;
        if (functionScenario)
        {
            var call = first.Output!.OfType<FunctionToolCallItem>().Single();
            Require(call.Name == "get_test_value", "Unexpected function name");
            using var arguments = JsonDocument.Parse(call.Arguments);
            Require(arguments.RootElement.GetProperty("key").GetString() == "alpha", "Unexpected tool arguments");
            if (scenario != "function")
            {
                Require(arguments.RootElement.GetProperty("details").GetProperty("label").GetString() == "fixture", "Generated tool nested arguments mismatch");
                Require(arguments.RootElement.GetProperty("samples").EnumerateArray().Select(item => item.GetInt32()).SequenceEqual(new[] { 7 }), "Generated tool array arguments mismatch");
            }
            Require(!string.IsNullOrEmpty(call.CallId), "Missing call correlation ID");
            request.Input = new InputParam([new FunctionCallOutputItemParam(call.CallId, "VALUE_7841")]);
            request.ToolChoice = "none";
            report["tool_arguments_validated"] = true;
        }
        else request.Input = "What code did I ask you to remember? Reply with only the code.";
        var second = await service.CreateResponse(request, deadline.Token);
        if (!string.IsNullOrEmpty(second.Id)) storedIds.Add(second.Id);
        CheckResponse(second);
        Require(second.PreviousResponseId == first.Id, "Missing response lineage");
        Require(second.OutputText?.Trim() == (functionScenario ? "VALUE_7841" : "BLUE_7319"), "Follow-up result mismatch");
        report["follow_up_validated"] = true;
    }
    else if (scenario == "manual-context")
    {
        const string originalPrompt = "Remember this code for my next question: BLUE_7319. Reply OK.";
        request.Input = originalPrompt;
        var first = await service.CreateResponse(request, deadline.Token);
        CheckResponse(first);
        var history = new List<IInputItem> { new InputMessageItem("user", originalPrompt) };
        history.AddRange(first.ToInputItems());
        history.Add(new InputMessageItem("user", "What code did I ask you to remember? Reply with only the code."));
        request.Input = history;
        var second = await service.CreateResponse(request, deadline.Token);
        CheckResponse(second);
        Require(second.OutputText?.Trim() == "BLUE_7319", "Manual history follow-up mismatch");
        Require(request.Store == false && request.PreviousResponseId == null, "Manual history must be stateless");
        report["manual_replay_items"] = history.Count;
        report["follow_up_validated"] = true;
    }
    Require(metadata.RequestCount == (twoRequests ? 2 : 1), "Unexpected number of requests");
    report["result"] = offline ? "OFFLINE_PASS" : "LIVE_PASS";
}
catch (Exception error)
{
    report["result"] = offline ? "OFFLINE_FAIL" : "LIVE_FAIL";
    report["exception_type"] = error.GetType().Name;
    // Provider exception messages may contain response bodies; do not persist them here.
}
finally
{
    // Delete every stored synthetic response, including when an assertion failed.
    using var cleanupDeadline = new CancellationTokenSource(TimeSpan.FromSeconds(20));
    var deleted = 0;
    foreach (var id in storedIds)
    {
        try
        {
            var result = await service.DeleteResponse(id, cleanupDeadline.Token);
            if (result.Deleted) deleted++;
        }
        catch { /* Record failure without persisting provider payloads. */ }
    }
    report["stored_responses"] = storedIds.Count;
    report["deleted_responses"] = deleted;
    if (deleted != storedIds.Count)
    {
        report["result"] = offline ? "OFFLINE_FAIL" : "LIVE_FAIL";
        report["cleanup_incomplete"] = true;
    }
}
report["workflow_usage"] = usages;
report["http_status"] = metadata.StatusCode;
report["request_ids"] = metadata.RequestIds;
report["request_count"] = metadata.RequestCount;
report["live_requests"] = offline ? 0 : metadata.RequestCount;
WriteReport();
return report["result"]?.ToString()?.EndsWith("_PASS", StringComparison.Ordinal) is true ? 0 : 1;

sealed class MetadataHandler(HttpMessageHandler inner, int maxRequests) : DelegatingHandler(inner)
{
    public int RequestCount { get; private set; }
    public int? StatusCode { get; private set; }
    public List<string> RequestIds { get; } = [];
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (RequestCount >= maxRequests) throw new InvalidOperationException("Smoke request limit exceeded");
        RequestCount++;
        var response = await base.SendAsync(request, cancellationToken);
        StatusCode = (int)response.StatusCode;
        if (response.Headers.TryGetValues("x-request-id", out var ids)) RequestIds.AddRange(ids);
        return response;
    }
}

sealed class FixtureHandler(string scenario) : HttpMessageHandler
{
    const string completed = """{"id":"resp_fixture","object":"response","status":"completed","model":"fixture-model","output":[{"type":"message","id":"msg_fixture","status":"completed","role":"assistant","content":[{"type":"output_text","text":"READY","annotations":[]}]}],"usage":{"input_tokens":5,"output_tokens":1,"total_tokens":6}}""";
    private int posts;
    private bool FunctionScenario => scenario is "function" or "function-generated" or "function-generated-net9";
    private bool StoredScenario => FunctionScenario || scenario == "context";
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (request.Method == HttpMethod.Delete)
            return new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent("{\"id\":\"resp_fixture\",\"deleted\":true}", Encoding.UTF8, "application/json") };
        posts++;
        if (request.Method != HttpMethod.Post || request.RequestUri?.AbsolutePath != "/v1/responses")
            throw new InvalidOperationException("Unexpected endpoint");
        using var body = JsonDocument.Parse(await request.Content!.ReadAsStringAsync(cancellationToken));
        if (body.RootElement.GetProperty("store").GetBoolean() != StoredScenario
            || body.RootElement.GetProperty("max_output_tokens").GetInt32() != 256
            || body.RootElement.GetProperty("service_tier").GetString() != "default"
            || body.RootElement.GetProperty("reasoning").GetProperty("effort").GetString() != "none")
            throw new InvalidOperationException("Request limits were not serialized");
        var streaming = body.RootElement.TryGetProperty("stream", out var flag) && flag.GetBoolean();
        var fixture = completed;
        if (scenario == "structured")
        {
            var format = body.RootElement.GetProperty("text").GetProperty("format");
            if (format.GetProperty("name").GetString() != "answer" || !format.GetProperty("strict").GetBoolean()
                || format.TryGetProperty("json_schema", out _)) throw new InvalidOperationException("Invalid schema wire format");
            fixture = completed.Replace("READY", JsonSerializer.Serialize("{\"answer\":7}")[1..^1]);
        }
        if (FunctionScenario && posts == 1)
            fixture = """{"id":"resp_fixture","status":"completed","model":"fixture-model","output":[{"type":"function_call","name":"get_test_value","call_id":"call_fixture","arguments":"{\"key\":\"alpha\"}"}],"usage":{"input_tokens":5,"output_tokens":1,"total_tokens":6}}""";
        if (FunctionScenario && scenario != "function")
        {
            ValidateGeneratedTool(body.RootElement);
            if (posts == 1) fixture = JsonSerializer.Serialize(new { id = "resp_fixture", status = "completed", model = "fixture-model", output = new[] { new { type = "function_call", name = "get_test_value", call_id = "call_fixture", arguments = JsonSerializer.Serialize(new { key = "alpha", details = new { label = "fixture" }, samples = new[] { 7 } }) } }, usage = new { input_tokens = 5, output_tokens = 1, total_tokens = 6 } });
        }
        if (StoredScenario && posts == 2)
        {
            if (body.RootElement.GetProperty("previous_response_id").GetString() != "resp_fixture") throw new InvalidOperationException("Missing lineage");
            if (FunctionScenario && body.RootElement.GetProperty("input")[0].GetProperty("call_id").GetString() != "call_fixture") throw new InvalidOperationException("Missing call correlation");
            fixture = completed.Replace("READY", FunctionScenario ? "VALUE_7841" : "BLUE_7319").Replace("\"status\":", "\"previous_response_id\":\"resp_fixture\",\"status\":");
        }
        if (scenario == "manual-context")
        {
            if (body.RootElement.TryGetProperty("previous_response_id", out _)) throw new InvalidOperationException("Unexpected server-side lineage");
            if (posts == 1) fixture = completed.Replace("READY", "OK").Replace("\"role\":\"assistant\"", "\"role\":\"assistant\",\"phase\":\"final_answer\",\"future_message\":{\"flag\":true}");
            else
            {
                var input = body.RootElement.GetProperty("input");
                if (input.GetArrayLength() != 3 || input[0].GetProperty("role").GetString() != "user"
                    || input[1].GetProperty("id").GetString() != "msg_fixture" || input[1].GetProperty("phase").GetString() != "final_answer"
                    || !input[1].GetProperty("future_message").GetProperty("flag").GetBoolean()
                    || input[1].GetProperty("content")[0].GetProperty("text").GetString() != "OK"
                    || input[2].GetProperty("role").GetString() != "user") throw new InvalidOperationException("Manual replay metadata or order lost");
                fixture = completed.Replace("READY", "BLUE_7319");
            }
        }
        var payload = streaming
            ? "data: {\"type\":\"response.output_text.delta\",\"sequence_number\":1,\"item_id\":\"msg_fixture\",\"output_index\":0,\"content_index\":0,\"delta\":\"READY\"}\n\n"
                + "data: {\"type\":\"response.completed\",\"sequence_number\":2,\"response\":" + completed + "}\n\n"
            : fixture;
        var response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(payload, Encoding.UTF8, streaming ? "text/event-stream" : "application/json")
        };
        response.Headers.Add("x-request-id", "fixture-request");
        return response;
    }
    private static void ValidateGeneratedTool(JsonElement request)
    {
        var tool = request.GetProperty("tools")[0];
        if (tool.GetProperty("name").GetString() != "get_test_value" || !tool.GetProperty("strict").GetBoolean()
            || tool.TryGetProperty("function", out _)) throw new InvalidOperationException("Generated function wire shape invalid");
        var schema = tool.GetProperty("parameters");
        ValidateStrictSchema(schema);
        var properties = schema.GetProperty("properties");
        if (schema.GetProperty("type").GetString() != "object" || properties.GetProperty("key").GetProperty("type").GetString() != "string"
            || properties.GetProperty("samples").GetProperty("items").GetProperty("type").GetString() != "integer")
            throw new InvalidOperationException("Generated schema types mismatch");
    }

    private static void ValidateStrictSchema(JsonElement schema)
    {
        if (schema.ValueKind != JsonValueKind.Object) return;
        if (schema.TryGetProperty("const", out var constant) && constant.ValueKind == JsonValueKind.Null)
            throw new InvalidOperationException("Spurious const:null schema constraint");
        if (schema.TryGetProperty("properties", out var properties))
        {
            if (schema.GetProperty("additionalProperties").GetBoolean()
                || !schema.GetProperty("required").EnumerateArray().Select(value => value.GetString()).Order().SequenceEqual(properties.EnumerateObject().Select(property => property.Name).Order()))
                throw new InvalidOperationException("Generated strict schema object is not closed/fully required");
        }
        foreach (var property in schema.EnumerateObject())
        {
            if (property.Value.ValueKind == JsonValueKind.Object) ValidateStrictSchema(property.Value);
            if (property.Value.ValueKind == JsonValueKind.Array)
                foreach (var child in property.Value.EnumerateArray()) ValidateStrictSchema(child);
        }
    }

}


sealed class GeneratedTestValueParameters
{
    [JsonPropertyName("key")]
    public string Key { get; set; } = "";
    [JsonPropertyName("details")]
    public GeneratedTestValueDetails Details { get; set; } = new();
    [JsonPropertyName("samples")]
    public int[] Samples { get; set; } = [];
}

sealed class GeneratedTestValueDetails
{
    [JsonPropertyName("label")]
    public string Label { get; set; } = "";
}
