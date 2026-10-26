using Betalgo.Ranul.OpenAI.Contracts.Requests.Responses;
using Betalgo.Ranul.OpenAI.Contracts.Types;
using Betalgo.Ranul.OpenAI.Contracts.Types.Items;
using Betalgo.Ranul.OpenAI.Contracts.Types.Tools;
using Betalgo.Ranul.OpenAI.Contracts.Enums.Responses;
using OpenAI.IntegrationTests.Fixtures;
using OpenAI.IntegrationTests.Helpers;

namespace OpenAI.IntegrationTests.Responses;

/// <summary>
/// Integration tests for token counting with the Responses API.
/// These tests demonstrate how to estimate costs before making API calls.
/// </summary>
/// <remarks>
/// Learn more: https://platform.openai.com/docs/api-reference/responses/count-tokens
/// </remarks>
[Collection("OpenAI")]
[Trait("Category", "Integration")]
public class TokenCountingTests
{
    private readonly OpenAITestFixture _fixture;

    public TokenCountingTests(OpenAITestFixture fixture)
    {
        _fixture = fixture;
    }

    #region Test 8.1: Basic Token Counting

    /// <summary>
    /// Demonstrates counting input tokens before making a response request.
    /// This is useful for estimating costs and ensuring requests fit within limits.
    /// </summary>
    /// <example>
    /// <code>
    /// var tokenCount = await openAI.Responses.CountInputTokens(new TokenCountsRequest
    /// {
    ///     Model = "gpt-6-luna",
    ///     Input = "What is the capital of France?"
    /// });
    /// 
    /// Console.WriteLine($"Input tokens: {tokenCount.InputTokens}");
    /// 
    /// // Now you can decide if the request fits your budget
    /// if (tokenCount.InputTokens &lt; 1000)
    /// {
    ///     var response = await openAI.Responses.CreateResponse(...);
    /// }
    /// </code>
    /// </example>
    [LiveFact]
    public async Task CountInputTokens_WithSimpleText_ReturnsCount()
    {
        // Arrange
        var request = new TokenCountsRequest
        {
            Model = _fixture.DefaultModel,
            Input = "What is the capital of France? Please provide a brief answer."
        };

        // Act
        var tokenCount = await _fixture.OpenAI.AsResponses().CountInputTokens(request);

        // Assert
        Assert.NotNull(tokenCount);
        Assert.True(tokenCount.InputTokens > 0, "Expected positive token count");

        // A simple sentence should be roughly 10-20 tokens
        Assert.True(tokenCount.InputTokens < 100, "Token count seems too high for a simple sentence");
    }

    /// <summary>
    /// Demonstrates that longer inputs result in higher token counts.
    /// </summary>
    [LiveFact]
    public async Task CountInputTokens_WithLongerText_ReturnsHigherCount()
    {
        // Arrange
        var shortRequest = new TokenCountsRequest
        {
            Model = _fixture.DefaultModel,
            Input = "Hello"
        };

        var longRequest = new TokenCountsRequest
        {
            Model = _fixture.DefaultModel,
            Input = "Please write a comprehensive essay about the history of artificial intelligence, " +
                    "covering its origins in the 1950s, major milestones like expert systems and neural networks, " +
                    "and recent advances in large language models and generative AI."
        };

        // Act
        var shortCount = await _fixture.OpenAI.AsResponses().CountInputTokens(shortRequest);
        var longCount = await _fixture.OpenAI.AsResponses().CountInputTokens(longRequest);

        // Assert
        Assert.NotNull(shortCount);
        Assert.NotNull(longCount);
        Assert.True(longCount.InputTokens > shortCount.InputTokens,
            $"Expected long text ({longCount.InputTokens} tokens) to have more tokens than short text ({shortCount.InputTokens} tokens)");
    }

    #endregion
}

