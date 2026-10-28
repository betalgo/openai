using System.Text.Json;
using Betalgo.Ranul.OpenAI.Contracts.Enums.Responses;
using Betalgo.Ranul.OpenAI.Contracts.Responses.Responses;

namespace OpenAI.Utilities.Tests;

public class ResponsesStatusUsageTests
{
    [Theory]
    [InlineData("completed")]
    [InlineData("failed")]
    [InlineData("in_progress")]
    [InlineData("cancelled")]
    [InlineData("queued")]
    [InlineData("incomplete")]
    [InlineData("future_status")]
    public void StatusRetainsKnownAndUnknownWireValues(string wireValue)
    {
        var json = JsonSerializer.Serialize(wireValue);
        var status = JsonSerializer.Deserialize<ResponsesStatus>(json);
        status.Value.ShouldBe(wireValue);
        JsonSerializer.Serialize(status).ShouldBe(json);
    }

    [Fact]
    public void UsageReadsNestedCountsFromApiJsonAndWritesTheSameWireShape()
    {
        const string json = """
            {"input_tokens":12,"input_tokens_details":{"cached_tokens":0},
             "output_tokens":5,"output_tokens_details":{"reasoning_tokens":2},"total_tokens":17}
            """;
        var usage = JsonSerializer.Deserialize<ResponsesUsage>(json)!;
        usage.InputTokens.ShouldBe(12);
        usage.InputTokensDetails!.CachedTokens.ShouldBe(0);
        usage.OutputTokens.ShouldBe(5);
        usage.OutputTokensDetails!.ReasoningTokens.ShouldBe(2);
        usage.TotalTokens.ShouldBe(17);
        using var written = JsonDocument.Parse(JsonSerializer.Serialize(usage));
        var root = written.RootElement;
        root.GetProperty("input_tokens").GetInt32().ShouldBe(12);
        root.GetProperty("input_tokens_details").GetProperty("cached_tokens").GetInt32().ShouldBe(0);
        root.GetProperty("output_tokens").GetInt32().ShouldBe(5);
        root.GetProperty("output_tokens_details").GetProperty("reasoning_tokens").GetInt32().ShouldBe(2);
        root.GetProperty("total_tokens").GetInt32().ShouldBe(17);
    }

    [Fact]
    public void MissingOrNullOptionalUsageDetailsRemainAbsent()
    {
        var omitted = JsonSerializer.Deserialize<ResponsesUsage>("{\"input_tokens\":0,\"output_tokens\":0,\"total_tokens\":0}")!;
        omitted.InputTokensDetails.ShouldBeNull();
        omitted.OutputTokensDetails.ShouldBeNull();
        var explicitNull = JsonSerializer.Deserialize<ResponsesUsage>("{\"input_tokens_details\":null,\"output_tokens_details\":null}")!;
        explicitNull.InputTokensDetails.ShouldBeNull();
        explicitNull.OutputTokensDetails.ShouldBeNull();
    }

    [Theory]
    [InlineData("max_output_tokens")]
    [InlineData("content_filter")]
    [InlineData("future_reason")]
    public void IncompleteReasonRetainsTheServerValue(string reason)
    {
        var json = JsonSerializer.Serialize(new { reason });
        var details = JsonSerializer.Deserialize<IncompleteDetails>(json)!;
        details.Reason.ShouldBe(reason);
        JsonSerializer.Serialize(details).ShouldBe(json);
    }
}
