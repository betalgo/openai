using Betalgo.Ranul.OpenAI.Contracts.Enums.Responses;
using Betalgo.Ranul.OpenAI.Contracts.Responses.Responses;

namespace OpenAI.IntegrationTests.Helpers;

/// <summary>
/// Custom assertion helpers for Responses API tests.
/// </summary>
public static class AssertExtensions
{
    /// <summary>
    /// Asserts that a response is successful and has expected basic properties.
    /// </summary>
    public static void AssertSuccessfulResponse(Response response)
    {
        Assert.NotNull(response);
        Assert.True(response.Successful, $"Response failed: {response.Error?.Message ?? "Unknown error"}");
        Assert.NotNull(response.Id);
        Assert.NotEmpty(response.Id);
        Assert.NotNull(response.Model);
    }

    /// <summary>
    /// Asserts that a response has completed status.
    /// </summary>
    public static void AssertCompletedStatus(Response response)
    {
        AssertSuccessfulResponse(response);
        Assert.Equal(ResponsesStatus.Completed, response.Status);
    }

    /// <summary>
    /// Asserts that a response has output text.
    /// </summary>
    public static void AssertHasOutputText(Response response)
    {
        AssertSuccessfulResponse(response);
        Assert.NotNull(response.Output);
        Assert.NotEmpty(response.Output);
    }

    /// <summary>
    /// Asserts that a response has usage information.
    /// </summary>
    public static void AssertHasUsage(Response response)
    {
        AssertSuccessfulResponse(response);
        Assert.NotNull(response.Usage);
        Assert.True(response.Usage.InputTokens > 0, "Expected input tokens > 0");
        Assert.True(response.Usage.OutputTokens > 0, "Expected output tokens > 0");
        Assert.True(response.Usage.TotalTokens > 0, "Expected total tokens > 0");
    }

    /// <summary>
    /// Asserts that a deleted response indicates successful deletion.
    /// </summary>
    public static void AssertDeleted(DeletedResponse response)
    {
        Assert.NotNull(response);
        Assert.True(response.Deleted, "Expected deletion to be successful");
        Assert.NotNull(response.Id);
        Assert.NotEmpty(response.Id);
    }
}

