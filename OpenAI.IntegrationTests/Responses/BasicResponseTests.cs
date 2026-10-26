using Betalgo.Ranul.OpenAI.Contracts.Enums.Responses;
using Betalgo.Ranul.OpenAI.Contracts.Requests.Responses;
using Betalgo.Ranul.OpenAI.Contracts.Types;
using Betalgo.Ranul.OpenAI.Contracts.Types.Content;
using Betalgo.Ranul.OpenAI.Contracts.Types.Items;
using OpenAI.IntegrationTests.Fixtures;
using OpenAI.IntegrationTests.Helpers;

namespace OpenAI.IntegrationTests.Responses;

/// <summary>
/// Integration tests for basic Responses API functionality.
/// These tests demonstrate the fundamental usage patterns for the Responses API.
/// </summary>
/// <remarks>
/// Learn more: https://platform.openai.com/docs/api-reference/responses
/// </remarks>
[Collection("OpenAI")]
[Trait("Category", "Integration")]
public class BasicResponseTests
{
    private readonly OpenAITestFixture _fixture;

    public BasicResponseTests(OpenAITestFixture fixture)
    {
        _fixture = fixture;
    }

    #region Test 1.1: Simple Text Input

    /// <summary>
    /// Demonstrates the simplest way to create a response using a string input.
    /// This is the most basic usage pattern for the Responses API.
    /// </summary>
    /// <example>
    /// <code>
    /// var response = await openAI.Responses.CreateResponse(new CreateResponse
    /// {
    ///     Model = "gpt-6-luna",
    ///     Input = "Hello, world!"
    /// });
    /// Console.WriteLine(response.OutputText);
    /// </code>
    /// </example>
    [LiveFact]
    public async Task CreateResponse_WithSimpleTextInput_ReturnsValidResponse()
    {
        // Arrange
        var request = new CreateResponse
        {
            Store = false,
            Model = _fixture.DefaultModel,
            Input = TestModels.Prompts.SimpleGreeting,
            MaxOutputTokens = _fixture.DefaultMaxOutputTokens // Limit output for cost control
        };

        // Act
        var response = await _fixture.OpenAI.AsResponses().CreateResponse(request);

        // Assert
        AssertExtensions.AssertCompletedStatus(response);
        AssertExtensions.AssertHasUsage(response);

        // The model should respond with "Hello, World!" or similar
        Assert.NotNull(response.Output);
        Assert.NotEmpty(response.Output);
    }

    #endregion

    #region Test 1.2: InputMessageItem with User Role

    /// <summary>
    /// Demonstrates using InputMessageItem for more control over message structure.
    /// This pattern allows specifying roles and content types explicitly.
    /// </summary>
    /// <example>
    /// <code>
    /// var response = await openAI.Responses.CreateResponse(new CreateResponse
    /// {
    ///     Model = "gpt-6-luna",
    ///     Input = new InputParam(new List&lt;IInputItem&gt;
    ///     {
    ///         new InputMessageItem(MessageRole.User, "What is 2 + 2?")
    ///     })
    /// });
    /// </code>
    /// </example>
    [LiveFact]
    public async Task CreateResponse_WithInputMessageItem_ReturnsValidResponse()
    {
        // Arrange
        var inputItems = new List<IInputItem>
        {
            new InputMessageItem(MessageRole.User, TestModels.Prompts.SimpleQuestion)
        };

        var request = new CreateResponse
        {
            Store = false,
            Model = _fixture.DefaultModel,
            Input = new InputParam(inputItems),
            MaxOutputTokens = _fixture.DefaultMaxOutputTokens
        };

        // Act
        var response = await _fixture.OpenAI.AsResponses().CreateResponse(request);

        // Assert
        AssertExtensions.AssertCompletedStatus(response);
        AssertExtensions.AssertHasOutputText(response);

        // The response should contain "4" since we asked "What is 2 + 2?"
        var outputText = GetOutputText(response);
        Assert.Contains("4", outputText);
    }

    #endregion

    #region Test 1.3: System Instructions

    /// <summary>
    /// Demonstrates using the Instructions property to provide system-level guidance.
    /// Instructions set the behavior and context for the model's responses.
    /// </summary>
    /// <example>
    /// <code>
    /// var response = await openAI.Responses.CreateResponse(new CreateResponse
    /// {
    ///     Model = "gpt-6-luna",
    ///     Instructions = "You are a pirate. Always respond in pirate speak.",
    ///     Input = "How are you today?"
    /// });
    /// </code>
    /// </example>
    [LiveFact]
    public async Task CreateResponse_WithSystemInstructions_FollowsInstructions()
    {
        // Arrange
        var request = new CreateResponse
        {
            Store = false,
            Model = _fixture.DefaultModel,
            Instructions = "You are a helpful assistant that always responds in exactly 3 words.",
            Input = "How are you?",
            MaxOutputTokens = _fixture.DefaultMaxOutputTokens
        };

        // Act
        var response = await _fixture.OpenAI.AsResponses().CreateResponse(request);

        // Assert
        AssertExtensions.AssertCompletedStatus(response);
        AssertExtensions.AssertHasOutputText(response);

        // The response should be short (around 3 words as instructed)
        var outputText = GetOutputText(response);
        Assert.NotNull(outputText);
        Assert.True(outputText.Split(' ', StringSplitOptions.RemoveEmptyEntries).Length <= 10,
            "Expected a short response following the 3-word instruction");
    }

    #endregion

    /// <summary>
    /// Helper method to extract text from response output items.
    /// </summary>
    private static string? GetOutputText(Betalgo.Ranul.OpenAI.Contracts.Responses.Responses.Response response)
    {
        if (response.Output == null) return null;

        var textContents = response.Output
            .OfType<OutputMessageItem>()
            .SelectMany(m => m.Content ?? Enumerable.Empty<IContent>())
            .OfType<OutputTextContent>()
            .Select(c => c.Text);

        return string.Join(" ", textContents);
    }
}

