using Betalgo.Ranul.OpenAI.Contracts.Types.ToolChoice;
using Betalgo.Ranul.OpenAI.Contracts.Enums.Responses;
using Betalgo.Ranul.OpenAI.Contracts.Requests.Responses;
using Betalgo.Ranul.OpenAI.Contracts.Types;
using Betalgo.Ranul.OpenAI.Contracts.Types.Items;
using Betalgo.Ranul.OpenAI.Contracts.Types.Tools;
using OpenAI.IntegrationTests.Fixtures;
using OpenAI.IntegrationTests.Helpers;

namespace OpenAI.IntegrationTests.Responses;

/// <summary>
/// Integration tests for function calling with the Responses API.
/// These tests demonstrate how to define and use custom functions that the model can call.
/// </summary>
/// <remarks>
/// Learn more: https://platform.openai.com/docs/guides/function-calling
/// </remarks>
[Collection("OpenAI")]
[Trait("Category", "Integration")]
public class FunctionCallingTests
{
    private readonly OpenAITestFixture _fixture;

    public FunctionCallingTests(OpenAITestFixture fixture)
    {
        _fixture = fixture;
    }

    #region Test 3.1: Single Function Tool

    /// <summary>
    /// Demonstrates defining a function tool and having the model call it.
    /// This is the basic pattern for extending the model with custom capabilities.
    /// </summary>
    /// <example>
    /// <code>
    /// var response = await openAI.Responses.CreateResponse(new CreateResponse
    /// {
    ///     Model = "gpt-6-luna",
    ///     Input = "What's the weather in San Francisco?",
    ///     Tools = new List&lt;ITool&gt;
    ///     {
    ///         new FunctionTool
    ///         {
    ///             Name = "get_weather",
    ///             Description = "Get current weather for a location",
    ///             Parameters = new FunctionParameters(new Dictionary&lt;string, object&gt;
    ///             {
    ///                 ["type"] = "object",
    ///                 ["properties"] = new Dictionary&lt;string, object&gt;
    ///                 {
    ///                     ["location"] = new Dictionary&lt;string, object&gt;
    ///                     {
    ///                         ["type"] = "string",
    ///                         ["description"] = "City name"
    ///                     }
    ///                 },
    ///                 ["required"] = new[] { "location" }
    ///             }),
    ///             Strict = true
    ///         }
    ///     }
    /// });
    /// 
    /// // Check if the model wants to call a function
    /// var functionCalls = response.Output?.OfType&lt;FunctionToolCallItem&gt;();
    /// </code>
    /// </example>
    [LiveFact]
    public async Task CreateResponse_WithFunctionTool_ReturnsToolCall()
    {
        await using var cleanup = new StoredResponseScope(_fixture.OpenAI.AsResponses());
        // Arrange
        var weatherTool = new FunctionTool
        {
            Name = TestModels.WeatherFunction.Name,
            Description = TestModels.WeatherFunction.Description,
            Parameters = TestModels.WeatherFunction.Parameters, // Implicit conversion from Dictionary
            Strict = true
        };

        var request = new CreateResponse
        {
            Model = _fixture.DefaultModel,
            Input = TestModels.Prompts.WeatherQuestion,
            Tools = new List<ITool> { weatherTool },
            ToolChoice = ToolChoice.Required,
            MaxOutputTokens = _fixture.DefaultMaxOutputTokens
        };

        // Act
        var response = await cleanup.Create(request);

        // Assert
        AssertExtensions.AssertSuccessfulResponse(response);
        Assert.NotNull(response.Output);
        Assert.NotEmpty(response.Output);

        // The model should call the weather function
        var functionCalls = response.Output.OfType<FunctionToolCallItem>().ToList();
        Assert.NotEmpty(functionCalls);

        var weatherCall = functionCalls.FirstOrDefault(f => f.Name == TestModels.WeatherFunction.Name);
        Assert.NotNull(weatherCall);
        Assert.NotNull(weatherCall.Arguments);

        // The arguments should include "San Francisco" or similar
        Assert.Contains("San Francisco", weatherCall.Arguments, StringComparison.OrdinalIgnoreCase);
    }

    #endregion

    #region Test 3.2: Function Call with Output

    /// <summary>
    /// Demonstrates the complete function calling cycle:
    /// 1. Model requests a function call
    /// 2. You execute the function
    /// 3. You provide the result back to the model
    /// 4. Model generates a final response
    /// </summary>
    /// <example>
    /// <code>
    /// // Step 1: Initial request with function
    /// var response1 = await openAI.Responses.CreateResponse(new CreateResponse
    /// {
    ///     Model = "gpt-6-luna",
    ///     Input = "What's the weather in Tokyo?",
    ///     Tools = new List&lt;ITool&gt; { weatherTool }
    /// });
    /// 
    /// // Step 2: Get the function call
    /// var functionCall = response1.Output?.OfType&lt;FunctionToolCallItem&gt;().First();
    /// 
    /// // Step 3: Execute your function and provide the result
    /// var response2 = await openAI.Responses.CreateResponse(new CreateResponse
    /// {
    ///     Model = "gpt-6-luna",
    ///     PreviousResponseId = response1.Id,
    ///     Input = new InputParam(new List&lt;IInputItem&gt;
    ///     {
    ///         new FunctionCallOutputItemParam
    ///         {
    ///             CallId = functionCall.CallId,
    ///             Output = "{\"temperature\": 25, \"condition\": \"sunny\"}"
    ///         }
    ///     })
    /// });
    /// // response2 now contains the model's interpretation of the weather data
    /// </code>
    /// </example>
    [LiveFact]
    public async Task CreateResponse_WithFunctionOutput_ProcessesResult()
    {
        await using var cleanup = new StoredResponseScope(_fixture.OpenAI.AsResponses());
        // Arrange - Create the function tool
        var weatherTool = new FunctionTool
        {
            Name = TestModels.WeatherFunction.Name,
            Description = TestModels.WeatherFunction.Description,
            Parameters = TestModels.WeatherFunction.Parameters, // Implicit conversion from Dictionary
            Strict = true
        };

        // Step 1: Initial request that should trigger a function call
        var request1 = new CreateResponse
        {
            Model = _fixture.DefaultModel,
            Input = "What's the weather like in Tokyo right now?",
            Tools = new List<ITool> { weatherTool },
            ToolChoice = ToolChoice.Required,
            MaxOutputTokens = _fixture.DefaultMaxOutputTokens,
            Store = true
        };

        var response1 = await cleanup.Create(request1);
        AssertExtensions.AssertSuccessfulResponse(response1);

        // Get the function call
        var functionCall = response1.Output?.OfType<FunctionToolCallItem>().FirstOrDefault();
        Assert.NotNull(functionCall);
        Assert.NotNull(functionCall.CallId);

        // Step 2: Provide the function output
        var functionOutput = new FunctionCallOutputItemParam
        {
            CallId = functionCall.CallId,
            Output = TestModels.WeatherFunction.GetMockResponse("Tokyo")
        };

        var request2 = new CreateResponse
        {
            Model = _fixture.DefaultModel,
            PreviousResponseId = response1.Id,
            Input = new InputParam(new List<IInputItem> { functionOutput }),
            Tools = new List<ITool> { weatherTool },
            ToolChoice = ToolChoice.None,
            MaxOutputTokens = _fixture.DefaultMaxOutputTokens,
            Store = true
        };

        var response2 = await cleanup.Create(request2);

        // Assert
        AssertExtensions.AssertCompletedStatus(response2);

        // The response should contain information about the weather
        var hasTextOutput = response2.Output?.OfType<OutputMessageItem>().Any() ?? false;
        Assert.True(hasTextOutput, "Expected a text response after function output");

    }

    #endregion
}

