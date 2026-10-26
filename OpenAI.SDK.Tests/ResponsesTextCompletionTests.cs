using System.Runtime.CompilerServices;
using System.Text.Json;
using Betalgo.Ranul.OpenAI.Contracts.Responses.Responses;
using Betalgo.Ranul.OpenAI.Contracts.Types.Streaming;
using Betalgo.Ranul.OpenAI.Contracts.Types.Streaming.Events.Lifecycle;
using Betalgo.Ranul.OpenAI.Contracts.Types.Streaming.Events.Text;
using Betalgo.Ranul.OpenAI.Extensions.Streaming;

namespace OpenAI.SDK.Tests;

public class ResponsesTextCompletionTests
{
    [Theory]
    [InlineData("failed")]
    [InlineData("incomplete")]
    [InlineData("cancelled")]
    public async Task TextHelpersRejectUnsuccessfulTerminalAndRetainResponse(string status)
    {
        var response = JsonSerializer.Deserialize<Response>("""
            {"id":"resp_fixture","status":"STATUS","incomplete_details":{"reason":"max_output_tokens"},
             "error":{"code":"fixture_error","message":"fixture failure"}}
            """.Replace("STATUS", status))!;
        IResponseStreamEvent terminal = status switch
        {
            "failed" => new ResponseFailedEvent { Response = response },
            "incomplete" => new ResponseIncompleteEvent { Response = response },
            _ => new ResponseCompletedEvent { Response = response }
        };
        var deltaError = await Assert.ThrowsAsync<ResponseStreamException>(() => Events(terminal).GetTextAsync());
        var doneError = await Assert.ThrowsAsync<ResponseStreamException>(() => Events(terminal).GetFinalTextAsync());
        Assert.Same(response, deltaError.Response);
        Assert.Same(response, doneError.Response);
        Assert.Equal("fixture_error", deltaError.Response!.Error!.Code);
        Assert.Same(response, await Events(terminal).GetFinalResponseAsync());
    }

    [Fact]
    public async Task TextHelpersReturnTextOnlyOnSuccessfulCompletion()
    {
        var terminal = new ResponseCompletedEvent { Response = new Response { Status = "completed" } };
        Assert.Equal("partial", await Events(terminal).GetTextAsync());
        Assert.Equal("final text", await Events(terminal).GetFinalTextAsync());
    }

    [Fact]
    public async Task TextHelpersRejectErrorEventAndRetainDetails()
    {
        var error = new ResponseErrorEvent { Code = "fixture_error", Message = "fixture failure", Param = "input" };
        Assert.Same(error, (await Assert.ThrowsAsync<ResponseStreamException>(() => Events(error).GetTextAsync())).ErrorEvent);
        Assert.Same(error, (await Assert.ThrowsAsync<ResponseStreamException>(() => Events(error).GetFinalTextAsync())).ErrorEvent);
    }

    [Fact]
    public async Task TextHelpersRejectMissingTerminal()
    {
        await Assert.ThrowsAsync<EndOfStreamException>(() => Events().GetTextAsync());
        await Assert.ThrowsAsync<EndOfStreamException>(() => Events().GetFinalTextAsync());
    }

    [Fact]
    public async Task TextHelpersPreserveCancellation()
    {
        using var cts = new CancellationTokenSource();
        cts.Cancel();
        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => Events().GetTextAsync(cts.Token));
        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => Events().GetFinalTextAsync(cts.Token));
    }

    private static async IAsyncEnumerable<IResponseStreamEvent> Events(
        IResponseStreamEvent? terminal = null, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        await Task.Yield();
        cancellationToken.ThrowIfCancellationRequested();
        yield return new ResponseTextDeltaEvent { Delta = "partial" };
        yield return new ResponseTextDoneEvent { Text = "final text" };
        if (terminal != null) yield return terminal;
    }
}
