using Betalgo.Ranul.OpenAI.Contracts.Enums;
using Betalgo.Ranul.OpenAI.Contracts.Enums.Responses;
using Betalgo.Ranul.OpenAI.Contracts.Requests.Responses;
using Betalgo.Ranul.OpenAI.Contracts.Types.Content;
using Betalgo.Ranul.OpenAI.Contracts.Types.Items;
using OpenAI.IntegrationTests.Fixtures;
using OpenAI.IntegrationTests.Helpers;

namespace OpenAI.IntegrationTests.Responses;

/// <summary>
/// Integration tests for reasoning models (explicitly configured reasoning models) with the Responses API.
/// These tests demonstrate how to use models with advanced reasoning capabilities.
/// </summary>
/// <remarks>
/// Learn more: https://platform.openai.com/docs/guides/reasoning
/// </remarks>
[Collection("OpenAI")]
[Trait("Category", "Integration")]
public class ReasoningModelTests
{
    private readonly OpenAITestFixture _fixture;

    public ReasoningModelTests(OpenAITestFixture fixture)
    {
        _fixture = fixture;
    }

    #region Test 6.1: Reasoning Model Basic

    /// <summary>
    /// Demonstrates using a reasoning model for complex problem-solving tasks.
    /// Reasoning models use extended "thinking" to solve difficult problems.
    /// </summary>
    /// <example>
    /// <code>
    /// var response = await openAI.Responses.CreateResponse(new CreateResponse
    /// {
    ///     Model = reasoningModel, // Explicit reviewed capability model
    ///     Input = "Solve: If 3x + 7 = 22, what is x?",
    ///     Reasoning = new ReasoningConfig
    ///     {
    ///         Effort = ReasoningEffort.Medium,  // low, medium, or high
    ///         Summary = ReasoningSummary.Auto   // auto, concise, or detailed
    ///     }
    /// });
    /// 
    /// // Check reasoning token usage
    /// var reasoningTokens = response.Usage?.OutputTokensDetails?.ReasoningTokens;
    /// Console.WriteLine($"Reasoning tokens used: {reasoningTokens}");
    /// </code>
    /// </example>
    [LiveFact("OPENAI_TEST_REASONING_MODEL")]
    public async Task CreateResponse_WithReasoningModel_ReturnsReasoningTokens()
    {
        // Arrange
        var request = new CreateResponse
        {
            Store = false,
            Model = _fixture.ReasoningModel,
            Input = "A train leaves A at 09:00 at 60 mph toward B, 120 miles away. " +
                    "At 10:00 a train leaves B toward A at 40 mph. Reply only with their meeting time in HH:mm.",
            Reasoning = new ReasoningConfig
            {
                Effort = ReasoningEffort.Low, // Use low to minimize costs
                Summary = ReasoningSummary.Auto
            },
            MaxOutputTokens = _fixture.DefaultMaxOutputTokens // Reasoning can use many tokens
        };

        // Act
        var response = await _fixture.OpenAI.AsResponses().CreateResponse(request);

        // Assert
        AssertExtensions.AssertCompletedStatus(response);
        Assert.NotNull(response.Usage);

        // Reasoning models should report reasoning tokens in the usage details
        // Note: The exact structure depends on the API response format
        Assert.NotNull(response.Usage.OutputTokensDetails);
        Assert.True(response.Usage.OutputTokensDetails.ReasoningTokens > 0, "Expected measured reasoning token usage");

        // Check for output - should contain the answer
        var outputText = GetOutputText(response);
        Assert.False(string.IsNullOrWhiteSpace(outputText), "Expected reasoning output");

        Assert.Contains("10:36", outputText);

    }

    #endregion

    /// <summary>
    /// Helper method to extract text from response output items.
    /// </summary>
    private static string GetOutputText(Betalgo.Ranul.OpenAI.Contracts.Responses.Responses.Response response)
    {
        if (response.Output == null) return string.Empty;

        var textContents = response.Output
            .OfType<OutputMessageItem>()
            .SelectMany(m => m.Content ?? Enumerable.Empty<IContent>())
            .OfType<OutputTextContent>()
            .Select(c => c.Text);

        return string.Join(" ", textContents);
    }
}

