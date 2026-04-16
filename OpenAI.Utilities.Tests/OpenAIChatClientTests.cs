using System.Text.Json;
using System.Net;
using System.Net.Http;
using System.Text;
using Betalgo.Ranul.OpenAI;
using Betalgo.Ranul.OpenAI.Managers;
using Betalgo.Ranul.OpenAI.ObjectModels;
using Betalgo.Ranul.OpenAI.Contracts.Enums;
using Microsoft.Extensions.AI;
using Xunit.Abstractions;

namespace OpenAI.Utilities.Tests;

public class OpenAIChatClientTests
{
    private ITestOutputHelper _output;

    public OpenAIChatClientTests(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public void CreateRequest_ShouldConvertVariousContentsToJson()
    {
        // Arrange
        var service = new OpenAIService(new OpenAIOptions { ApiKey = "fake" });

        var messages = new List<ChatMessage>
        {
            new ChatMessage(ChatRole.User, "")
            {
                Contents = new List<AIContent>
                {
                    new TextContent("Text info"),
                }
            },
            new ChatMessage(ChatRole.Assistant, "")
            {
                Contents = new List<AIContent>
                {
                    new FunctionCallContent("call_1", "MyFunction", new Dictionary<string, object?> { { "arg1", "val1" } }),
                    new TextReasoningContent("This is reasoning"),
                }
            },
            new ChatMessage(ChatRole.Tool, "")
            {
                Contents = new List<AIContent>
                {
                    new FunctionResultContent("call_1", "Result from tool")
                }
            }
        };

        // Act
        var request = service.CreateRequest(messages, new ChatOptions { ModelId = Models.Gpt_4_turbo });

        // Assert
        request.Messages.ShouldNotBeEmpty();

        request.Messages.Count.ShouldBe(3);

        request.Messages[0].Role.ShouldBe(ChatCompletionRole.User);
        request.Messages[0].Content.ShouldBe("Text info");

        request.Messages[1].Role.ShouldBe(ChatCompletionRole.Assistant);
        request.Messages[1].ToolCalls.ShouldNotBeNull();
        var toolCalls = request.Messages[1].ToolCalls!;
        toolCalls.Count.ShouldBe(1);
        var functionToolCall = toolCalls[0];
        functionToolCall.Id.ShouldBe("call_1");
        functionToolCall.FunctionCall.ShouldNotBeNull();
        var functionCall = functionToolCall.FunctionCall!;
        functionCall.Name.ShouldBe("MyFunction");
        functionCall.Arguments.ShouldBe("{\"arg1\":\"val1\"}");
        request.Messages[1].ReasoningContent.ShouldBe("This is reasoning");

        request.Messages[2].Role.ShouldBe(ChatCompletionRole.Tool);
        request.Messages[2].ToolCallId.ShouldBe("call_1");
        request.Messages[2].Content.ShouldBe("Result from tool");

        var json = JsonSerializer.Serialize(request, new JsonSerializerOptions { WriteIndented = true });

        _output.WriteLine(json);
    }

    [Fact]
    public void CreateRequest_ShouldSerializeFunctionResultObjectAsJson()
    {
        var service = new OpenAIService(new OpenAIOptions { ApiKey = "fake" });

        var messages = new List<ChatMessage>
        {
            new(ChatRole.Tool, "")
            {
                Contents =
                [
                    new FunctionResultContent("call_42", new { ok = true, value = 123 })
                ]
            }
        };

        var request = service.CreateRequest(messages, new ChatOptions { ModelId = Models.Gpt_4_turbo });

        request.Messages.Count.ShouldBe(1);
        request.Messages[0].Role.ShouldBe(ChatCompletionRole.Tool);
        request.Messages[0].ToolCallId.ShouldBe("call_42");
        request.Messages[0].Content.ShouldBe("{\"ok\":true,\"value\":123}");
    }

    [Fact]
    public void CreateRequest_ShouldIgnoreToolMessageWithoutFunctionResultContent()
    {
        var service = new OpenAIService(new OpenAIOptions { ApiKey = "fake" });

        var messages = new List<ChatMessage>
        {
            new(ChatRole.Tool, "")
            {
                Contents =
                [
                    new TextContent("tool text without call id")
                ]
            }
        };

        var request = service.CreateRequest(messages, new ChatOptions { ModelId = Models.Gpt_4_turbo });

        request.Messages.Count.ShouldBe(0);
    }

    [Fact]
    public void CreateRequest_ShouldIgnoreFunctionCallContentInUserMessage()
    {
        var service = new OpenAIService(new OpenAIOptions { ApiKey = "fake" });

        var messages = new List<ChatMessage>
        {
            new(ChatRole.User, "")
            {
                Contents =
                [
                    new FunctionCallContent("call_x", "DoWork", new Dictionary<string, object?> { ["value"] = 1 })
                ]
            }
        };

        var request = service.CreateRequest(messages, new ChatOptions { ModelId = Models.Gpt_4_turbo });

        request.Messages.Count.ShouldBe(0);
    }

    [Fact]
    public void PopulateContents_ShouldIncludeTextReasoningContent_WhenReasoningContentExists()
    {
        var source = new Betalgo.Ranul.OpenAI.ObjectModels.RequestModels.ChatMessage
        {
            ReasoningContent = "thinking trace",
            Content = "final answer"
        };
        var destination = new List<AIContent>();

        var method = typeof(OpenAIService).GetMethod("PopulateContents", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
        method.ShouldNotBeNull();
        method!.Invoke(null, [source, destination]);

        destination.OfType<TextReasoningContent>().Count().ShouldBe(1);
        destination.OfType<TextReasoningContent>().First().Text.ShouldBe("thinking trace");
        destination.OfType<TextContent>().Count().ShouldBe(1);
        destination.OfType<TextContent>().First().Text.ShouldBe("final answer");
    }

    [Fact]
    public async Task GetStreamingResponseAsync_ShouldEmitUsage_WhenTailChunkHasNoChoices()
    {
        const string streamPayload = """
data: {"id":"chatcmpl-stream-1","object":"chat.completion.chunk","created":1776330793,"model":"gemini-3-flash-preview","choices":[{"index":0,"delta":{"role":"assistant","content":"hello"},"finish_reason":null}]}

data: {"id":"chatcmpl-stream-1","object":"chat.completion.chunk","created":1776330793,"model":"gemini-3-flash-preview","choices":[],"usage":{"prompt_tokens":3,"completion_tokens":156,"total_tokens":159,"prompt_tokens_details":{"cached_tokens":0,"text_tokens":3,"audio_tokens":0,"image_tokens":0},"completion_tokens_details":{"text_tokens":0,"audio_tokens":0,"reasoning_tokens":140}}}

data: [DONE]
""";

        var handler = new StubHttpMessageHandler(_ =>
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(streamPayload, Encoding.UTF8, "text/event-stream")
            };

            return response;
        });

        using var httpClient = new HttpClient(handler)
        {
            BaseAddress = new Uri("https://api.openai.com")
        };
        var service = new OpenAIService(new OpenAIOptions { ApiKey = "fake" }, httpClient);
        var chatClient = (IChatClient)service;

        var updates = new List<ChatResponseUpdate>();
        await foreach (var update in chatClient.GetStreamingResponseAsync([new(ChatRole.User, "hi")], new ChatOptions { ModelId = Models.Gpt_4_turbo }))
        {
            updates.Add(update);
        }

        updates.Count.ShouldBe(2);

        var usageUpdate = updates.Single(u => u.Contents.OfType<UsageContent>().Any());
        var usageContent = usageUpdate.Contents.OfType<UsageContent>().Single();
        usageContent.Details.ShouldNotBeNull();
        usageContent.Details.InputTokenCount.ShouldBe(3);
        usageContent.Details.OutputTokenCount.ShouldBe(156);
        usageContent.Details.TotalTokenCount.ShouldBe(159);
        usageContent.Details.AdditionalCounts.ShouldNotBeNull();
        usageContent.Details.AdditionalCounts[$"PromptTokensDetails.CachedTokens"].ShouldBe(0);
        usageContent.Details.AdditionalCounts[$"CompletionTokensDetails.ReasoningTokens"].ShouldBe(140);
    }

    private sealed class StubHttpMessageHandler(Func<HttpRequestMessage, HttpResponseMessage> handler) : HttpMessageHandler
    {
        protected override HttpResponseMessage Send(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return handler(request);
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task.FromResult(handler(request));
        }
    }
}
