using Betalgo.Ranul.OpenAI.Contracts.Requests.Responses;
using Betalgo.Ranul.OpenAI.Contracts.Types;
using Betalgo.Ranul.OpenAI.Contracts.Types.Content;
using Betalgo.Ranul.OpenAI.Contracts.Types.Items;
using Betalgo.Ranul.OpenAI.Contracts.Enums.Responses;
using OpenAI.IntegrationTests.Fixtures;
using OpenAI.IntegrationTests.Helpers;

namespace OpenAI.IntegrationTests.Responses;

/// <summary>
/// Integration tests for multi-turn conversations using the Responses API.
/// These tests demonstrate how to maintain conversation context across multiple requests.
/// </summary>
/// <remarks>
/// Learn more: https://platform.openai.com/docs/guides/conversation-state
/// </remarks>
[Collection("OpenAI")]
[Trait("Category", "Integration")]
public class ConversationTests
{
    private readonly OpenAITestFixture _fixture;

    public ConversationTests(OpenAITestFixture fixture)
    {
        _fixture = fixture;
    }

    #region Test 2.1: Previous Response ID

    /// <summary>
    /// Demonstrates maintaining conversation context using PreviousResponseId.
    /// This is the recommended way to build multi-turn conversations.
    /// </summary>
    /// <example>
    /// <code>
    /// // First turn
    /// var turn1 = await openAI.Responses.CreateResponse(new CreateResponse
    /// {
    ///     Model = "gpt-6-luna",
    ///     Input = "My name is Alice."
    /// });
    /// 
    /// // Second turn - references the first
    /// var turn2 = await openAI.Responses.CreateResponse(new CreateResponse
    /// {
    ///     Model = "gpt-6-luna",
    ///     Input = "What is my name?",
    ///     PreviousResponseId = turn1.Id  // Links to previous conversation
    /// });
    /// // turn2 will know the user's name is Alice
    /// </code>
    /// </example>
    [LiveFact]
    public async Task CreateResponse_WithPreviousResponseId_MaintainsContext()
    {
        await using var cleanup = new StoredResponseScope(_fixture.OpenAI.AsResponses());
        // Arrange - First turn: introduce a fact
        var turn1Request = new CreateResponse
        {
            Model = _fixture.DefaultModel,
            Input = "Remember this number: 42. Just acknowledge that you've noted it.",
            MaxOutputTokens = _fixture.DefaultMaxOutputTokens,
            Store = true
        };

        var turn1 = await cleanup.Create(turn1Request);
        AssertExtensions.AssertCompletedStatus(turn1);

        // Act - Second turn: ask about the fact
        var turn2Request = new CreateResponse
        {
            Model = _fixture.DefaultModel,
            Input = "What number did I ask you to remember? Reply with just the number.",
            PreviousResponseId = turn1.Id,
            MaxOutputTokens = _fixture.DefaultMaxOutputTokens
        };

        var turn2 = await cleanup.Create(turn2Request);

        // Assert
        AssertExtensions.AssertCompletedStatus(turn2);
        Assert.Equal(turn1.Id, turn2.PreviousResponseId);

        // The response should contain "42"
        var outputText = GetOutputText(turn2);
        Assert.Contains("42", outputText);

    }

    #endregion

    #region Test 2.2: Multi-turn Conversation

    /// <summary>
    /// Demonstrates a longer multi-turn conversation with context maintained throughout.
    /// This shows how to build a chatbot-style interaction.
    /// </summary>
    /// <example>
    /// <code>
    /// // Build a conversation chain
    /// var turn1 = await openAI.Responses.CreateResponse(new CreateResponse
    /// {
    ///     Model = "gpt-6-luna",
    ///     Input = "I'm planning a trip to Paris."
    /// });
    /// 
    /// var turn2 = await openAI.Responses.CreateResponse(new CreateResponse
    /// {
    ///     Model = "gpt-6-luna",
    ///     Input = "What's a good time to visit?",
    ///     PreviousResponseId = turn1.Id
    /// });
    /// 
    /// var turn3 = await openAI.Responses.CreateResponse(new CreateResponse
    /// {
    ///     Model = "gpt-6-luna",
    ///     Input = "What should I pack?",
    ///     PreviousResponseId = turn2.Id  // Chain continues
    /// });
    /// </code>
    /// </example>
    [LiveFact]
    public async Task CreateResponse_MultiTurnConversation_RemembersPriorContext()
    {
        await using var cleanup = new StoredResponseScope(_fixture.OpenAI.AsResponses());

        // Turn 1: Establish context
        var turn1 = await cleanup.Create(new CreateResponse
        {
            Model = _fixture.DefaultModel,
            Input = "I have a pet cat named Whiskers. Just acknowledge this.",
            MaxOutputTokens = _fixture.DefaultMaxOutputTokens,
            Store = true
        });
        AssertExtensions.AssertCompletedStatus(turn1);

        // Turn 2: Add more context
        var turn2 = await cleanup.Create(new CreateResponse
        {
            Model = _fixture.DefaultModel,
            Input = "Whiskers is 3 years old. Just acknowledge this.",
            PreviousResponseId = turn1.Id,
            MaxOutputTokens = _fixture.DefaultMaxOutputTokens,
            Store = true
        });
        AssertExtensions.AssertCompletedStatus(turn2);

        // Turn 3: Query the accumulated context
        var turn3 = await cleanup.Create(new CreateResponse
        {
            Model = _fixture.DefaultModel,
            Input = "What is the name and age of my pet? Answer in format: 'Name: X, Age: Y'",
            PreviousResponseId = turn2.Id,
            MaxOutputTokens = _fixture.DefaultMaxOutputTokens,
            Store = true
        });
        AssertExtensions.AssertCompletedStatus(turn3);

        // Assert - Turn 3 should know both facts from earlier turns
        var outputText = GetOutputText(turn3);
        Assert.Contains("Whiskers", outputText, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("3", outputText);

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

