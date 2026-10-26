using System.Net;
using System.Text;
using System.Text.Json;
using Betalgo.Ranul.OpenAI;
using Betalgo.Ranul.OpenAI.Contracts.Requests.Responses;
using Betalgo.Ranul.OpenAI.Contracts.Responses.Responses;
using Betalgo.Ranul.OpenAI.Contracts.Types.Streaming;
using Betalgo.Ranul.OpenAI.Interfaces;
using Betalgo.Ranul.OpenAI.Managers;
using Betalgo.Ranul.OpenAI.ObjectModels;
using Shouldly;

namespace OpenAI.SDK.Tests;

public class OpenAIResponsesServiceTests
{
    [Fact]
    public async Task CreateResponseUsesConfiguredDefaultModelWhenRequestModelIsMissing()
    {
        string? requestBody = null;
        var service = CreateService(async (request, cancellationToken) =>
        {
            requestBody = await request.Content!.ReadAsStringAsync(cancellationToken);
            return JsonResponse(HttpStatusCode.OK, """{"id":"resp_123","status":"completed","model":"gpt-4o","output":[]}""");
        }, defaultModelId: Models.Gpt_4o);

        var response = await service.CreateResponse(new CreateResponse
        {
            Input = "hello"
        });

        response.Model.ShouldBe(Models.Gpt_4o);
        GetJsonProperty(requestBody!, "model").ShouldBe(Models.Gpt_4o);
    }

    [Fact]
    public async Task CreateExtensionUsesExplicitTypedModelOverride()
    {
        string? requestBody = null;
        IResponsesService service = CreateService(async (request, cancellationToken) =>
        {
            requestBody = await request.Content!.ReadAsStringAsync(cancellationToken);
            return JsonResponse(HttpStatusCode.OK, """{"id":"resp_456","status":"completed","model":"gpt-4.1","output":[]}""");
        }, defaultModelId: Models.Gpt_4o);

        var response = await service.Create(new CreateResponse
        {
            Input = "hello"
        }, Models.Model.Gpt_4_1);

        response.Model.ShouldBe(Models.Gpt_4_1);
        GetJsonProperty(requestBody!, "model").ShouldBe(Models.Gpt_4_1);
    }

    [Fact]
    public async Task CountInputTokensUsesConfiguredDefaultModelWhenRequestModelIsMissing()
    {
        string? requestBody = null;
        var service = CreateService(async (request, cancellationToken) =>
        {
            requestBody = await request.Content!.ReadAsStringAsync(cancellationToken);
            return JsonResponse(HttpStatusCode.OK, """{"input_tokens":12}""");
        }, defaultModelId: Models.Gpt_4o_mini);

        var response = await service.CountInputTokens(new TokenCountsRequest
        {
            Input = "hello"
        });

        response.InputTokens.ShouldBe(12);
        GetJsonProperty(requestBody!, "model").ShouldBe(Models.Gpt_4o_mini);
    }

    [Fact]
    public async Task CreateAsStreamAsyncUsesConfiguredDefaultModelWhenRequestModelIsMissing()
    {
        string? requestBody = null;
        var service = CreateService(async (request, cancellationToken) =>
        {
            requestBody = await request.Content!.ReadAsStringAsync(cancellationToken);
            return EventStreamResponse("""
data: {"type":"response.output_text.delta","sequence_number":1,"item_id":"item_1","output_index":0,"content_index":0,"delta":"hello"}

data: {"type":"response.completed","sequence_number":2,"response":{"id":"resp_123","status":"completed"}}

data: [DONE]

""");
        }, defaultModelId: Models.Gpt_4o);

        var events = new List<IResponseStreamEvent>();
        await foreach (var evt in service.CreateAsStreamAsync(new CreateResponse
        {
            Input = "hello"
        }))
        {
            events.Add(evt);
        }

        events.Count.ShouldBe(2);
        GetJsonProperty(requestBody!, "model").ShouldBe(Models.Gpt_4o);
        GetJsonProperty(requestBody!, "stream").ShouldBe("True");
    }

    [Fact]
    public async Task RetrieveAsStreamAsyncAppendsSingleCanonicalStreamQueryParameter()
    {
        Uri? requestUri = null;
        var service = CreateService((request, _) =>
        {
            requestUri = request.RequestUri;
            return Task.FromResult(EventStreamResponse("data: [DONE]\n\n"));
        });

        await foreach (var _ in service.RetrieveAsStreamAsync("resp_123", new RetrieveResponseRequest
        {
            Stream = false,
            StartingAfter = 42
        }))
        {
        }

        requestUri.ShouldNotBeNull();
        requestUri.Query.ShouldContain("starting_after=42");
        requestUri.Query.ShouldContain("stream=true");
        requestUri.Query.Split("stream=").Length.ShouldBe(2);
    }

    [Fact]
    public async Task CreateResponseUsesUnifiedErrorContractOnFailure()
    {
        var service = CreateService((_, _) => Task.FromResult(JsonResponse(HttpStatusCode.BadRequest,
            """{"error":{"message":"The model does not exist","type":"invalid_request_error","param":"model","code":"model_not_found"}}""")));

        var response = await service.CreateResponse(new CreateResponse
        {
            Model = "missing-model",
            Input = "hello"
        });

        response.Successful.ShouldBeFalse();
        response.Error.ShouldNotBeNull();
        response.Error.Code.ShouldBe("model_not_found");
        response.Error.Message.ShouldBe("The model does not exist");
    }

    private static OpenAIService CreateService(
        Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> handler,
        string? defaultModelId = null)
    {
        var httpClient = new HttpClient(new StubHttpMessageHandler(handler));
        return new OpenAIService(new OpenAIOptions
        {
            ApiKey = "test-key",
            DefaultModelId = defaultModelId
        }, httpClient);
    }

    private static HttpResponseMessage JsonResponse(HttpStatusCode statusCode, string json)
    {
        return new HttpResponseMessage(statusCode)
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        };
    }

    private static HttpResponseMessage EventStreamResponse(string payload)
    {
        return new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(payload, Encoding.UTF8, "text/event-stream")
        };
    }

    private static string? GetJsonProperty(string json, string propertyName)
    {
        using var document = JsonDocument.Parse(json);
        var value = document.RootElement.GetProperty(propertyName);

        return value.ValueKind switch
        {
            JsonValueKind.String => value.GetString(),
            JsonValueKind.True => bool.TrueString,
            JsonValueKind.False => bool.FalseString,
            _ => value.GetRawText()
        };
    }

    private sealed class StubHttpMessageHandler : HttpMessageHandler
    {
        private readonly Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> _handler;

        public StubHttpMessageHandler(Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> handler)
        {
            _handler = handler;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return _handler(request, cancellationToken);
        }
    }
}
