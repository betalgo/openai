using System.Text.Json;
using Betalgo.Ranul.OpenAI.Contracts.Requests.Responses;
using Betalgo.Ranul.OpenAI.Contracts.Types.Content;
using Betalgo.Ranul.OpenAI.Contracts.Types.Items;
using Betalgo.Ranul.OpenAI.Contracts.Types.TextFormat;
using OpenAI.IntegrationTests.Fixtures;
using OpenAI.IntegrationTests.Helpers;

namespace OpenAI.IntegrationTests.Responses;

/// <summary>
/// Integration tests for structured outputs (JSON schema) with the Responses API.
/// These tests demonstrate how to get type-safe, structured responses from the model.
/// </summary>
/// <remarks>
/// Learn more: https://platform.openai.com/docs/guides/structured-outputs
/// </remarks>
[Collection("OpenAI")]
[Trait("Category", "Integration")]
public class StructuredOutputTests
{
    private readonly OpenAITestFixture _fixture;

    public StructuredOutputTests(OpenAITestFixture fixture)
    {
        _fixture = fixture;
    }

    #region Test 5.1: JSON Schema Format

    /// <summary>
    /// Demonstrates using JSON schema to get structured, type-safe responses.
    /// This ensures the model's output conforms to a specific schema.
    /// </summary>
    /// <example>
    /// <code>
    /// var response = await openAI.Responses.CreateResponse(new CreateResponse
    /// {
    ///     Model = "gpt-6-luna",
    ///     Input = "Create a person named John, age 30, software engineer.",
    ///     Text = new TextConfig
    ///     {
    ///         Format = TextFormat.JsonSchema("person", new Dictionary&lt;string, object&gt;
    ///         {
    ///             ["type"] = "object",
    ///             ["properties"] = new Dictionary&lt;string, object&gt;
    ///             {
    ///                 ["name"] = new Dictionary&lt;string, object&gt; { ["type"] = "string" },
    ///                 ["age"] = new Dictionary&lt;string, object&gt; { ["type"] = "integer" },
    ///                 ["occupation"] = new Dictionary&lt;string, object&gt; { ["type"] = "string" }
    ///             },
    ///             ["required"] = new[] { "name", "age", "occupation" },
    ///             ["additionalProperties"] = false
    ///         }, strict: true)
    ///     }
    /// });
    /// 
    /// // Parse the structured response
    /// var person = JsonSerializer.Deserialize&lt;Person&gt;(response.OutputText);
    /// </code>
    /// </example>
    [LiveFact]
    public async Task CreateResponse_WithJsonSchemaFormat_ReturnsStructuredJson()
    {
        // Arrange
        var request = new CreateResponse
        {
            Store = false,
            Model = _fixture.DefaultModel,
            Input = TestModels.Prompts.PersonDescription,
            Text = new TextConfig
            {
                Format = TextFormat.JsonSchema(
                    TestModels.PersonSchema.Name,
                    TestModels.PersonSchema.Schema,
                    strict: true)
            },
            MaxOutputTokens = _fixture.DefaultMaxOutputTokens
        };

        // Act
        var response = await _fixture.OpenAI.AsResponses().CreateResponse(request);

        // Assert
        AssertExtensions.AssertSuccessfulResponse(response);

        var outputText = GetOutputText(response);
        Assert.False(string.IsNullOrWhiteSpace(outputText), "Expected JSON output");

        // Parse and validate the JSON structure
        var jsonDoc = JsonDocument.Parse(outputText);
        var root = jsonDoc.RootElement;

        // Verify required fields exist
        Assert.True(root.TryGetProperty("name", out var nameProp), "Expected 'name' property");
        Assert.True(root.TryGetProperty("age", out var ageProp), "Expected 'age' property");
        Assert.True(root.TryGetProperty("occupation", out var occupationProp), "Expected 'occupation' property");

        // Verify types
        Assert.Equal(JsonValueKind.String, nameProp.ValueKind);
        Assert.Equal(JsonValueKind.Number, ageProp.ValueKind);
        Assert.Equal(JsonValueKind.String, occupationProp.ValueKind);

        // Verify content matches the prompt
        Assert.Contains("John", nameProp.GetString(), StringComparison.OrdinalIgnoreCase);
        Assert.Equal(30, ageProp.GetInt32());
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

