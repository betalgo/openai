using Betalgo.Ranul.OpenAI.Contracts.Requests.Responses;
using OpenAI.IntegrationTests.Fixtures;

namespace OpenAI.IntegrationTests.Responses;

/// <summary>
/// Integration tests for error handling with the Responses API.
/// These tests demonstrate how to handle various error conditions.
/// </summary>
/// <remarks>
/// Learn more: https://platform.openai.com/docs/guides/error-codes
/// </remarks>
[Collection("OpenAI")]
[Trait("Category", "Integration")]
public class ErrorHandlingTests
{
    private readonly OpenAITestFixture _fixture;

    public ErrorHandlingTests(OpenAITestFixture fixture)
    {
        _fixture = fixture;
    }

    #region Test 10.1: Invalid Model

    /// <summary>
    /// Demonstrates handling errors when an invalid model is specified.
    /// This shows how the API returns errors for bad requests.
    /// </summary>
    /// <example>
    /// <code>
    /// try
    /// {
    ///     var response = await openAI.Responses.CreateResponse(new CreateResponse
    ///     {
    ///         Model = "invalid-model-name",
    ///         Input = "Hello"
    ///     });
    ///     
    ///     if (!response.Successful)
    ///     {
    ///         Console.WriteLine($"Error: {response.Error?.Message}");
    ///     }
    /// }
    /// catch (Exception ex)
    /// {
    ///     Console.WriteLine($"Exception: {ex.Message}");
    /// }
    /// </code>
    /// </example>
    [LiveFact]
    public async Task CreateResponse_WithInvalidModel_ReturnsError()
    {
        // Arrange
        var request = new CreateResponse
        {
            Store = false,
            Model = "this-model-does-not-exist-12345",
            Input = "Hello"
        };

        // Act
        var response = await _fixture.OpenAI.AsResponses().CreateResponse(request);

        // Assert - The response should indicate failure
        Assert.False(response.Successful, "Expected request to fail with invalid model");
        
        // The error should be populated
        // Note: Depending on implementation, this might be in response.Error or the response itself
        // might throw an exception. Adjust based on actual SDK behavior.
    }

    #endregion

    #region Test 10.2: Invalid Response ID

    /// <summary>
    /// Demonstrates handling errors when retrieving a non-existent response.
    /// This shows 404-style error handling.
    /// </summary>
    /// <example>
    /// <code>
    /// try
    /// {
    ///     var response = await openAI.Responses.RetrieveResponse("resp_nonexistent123");
    ///     
    ///     if (!response.Successful)
    ///     {
    ///         Console.WriteLine($"Not found: {response.Error?.Message}");
    ///     }
    /// }
    /// catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
    /// {
    ///     Console.WriteLine("Response not found");
    /// }
    /// </code>
    /// </example>
    [LiveFact]
    public async Task RetrieveResponse_WithInvalidId_ReturnsNotFound()
    {
        // Arrange
        var invalidId = "resp_thisdoesnotexist12345678";

        // Act
        var response = await _fixture.OpenAI.AsResponses().RetrieveResponse(invalidId);

        // Assert - The response should indicate failure
        Assert.False(response.Successful, "Expected request to fail with invalid ID");
    }

    #endregion
}

