using Betalgo.Ranul.OpenAI.Contracts.Responses.Responses;
using Betalgo.Ranul.OpenAI.Contracts.Types.Streaming;
using Betalgo.Ranul.OpenAI.Contracts.Types.Streaming.Events.Lifecycle;
using Betalgo.Ranul.OpenAI.Contracts.Types.Streaming.Events.Tools;
using Betalgo.Ranul.OpenAI.Extensions.Streaming;
using Shouldly;

namespace OpenAI.SDK.Tests;

public class ResponsesStreamHandlerTests
{
    [Fact]
    public async Task ToolCallbacksOnlyReceiveToolLifecycleEvents()
    {
        var inProgressCalls = 0;
        var completedCalls = 0;

        await new ResponsesStreamHandler(GetEvents(
                new ResponseInProgressEvent
                {
                    SequenceNumber = 1,
                    Response = new Response { Id = "resp_1" }
                },
                new ResponseWebSearchCallInProgressEvent
                {
                    SequenceNumber = 2,
                    OutputIndex = 0,
                    ItemId = "item_1"
                },
                new ResponseCompletedEvent
                {
                    SequenceNumber = 3,
                    Response = new Response { Id = "resp_1" }
                },
                new ResponseWebSearchCallCompletedEvent
                {
                    SequenceNumber = 4,
                    OutputIndex = 0,
                    ItemId = "item_1"
                }))
            .OnToolCallInProgress(_ => inProgressCalls++)
            .OnToolCallCompleted(_ => completedCalls++)
            .ExecuteAsync();

        inProgressCalls.ShouldBe(1);
        completedCalls.ShouldBe(1);
    }

    private static async IAsyncEnumerable<IResponseStreamEvent> GetEvents(params IResponseStreamEvent[] events)
    {
        foreach (var evt in events)
        {
            yield return evt;
            await Task.Yield();
        }
    }
}
