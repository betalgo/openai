namespace OpenAI.IntegrationTests.Helpers;

/// <summary>
/// Shared test data and helper models for integration tests.
/// </summary>
public static class TestModels
{
    /// <summary>
    /// A simple weather function definition for function calling tests.
    /// </summary>
    public static class WeatherFunction
    {
        public const string Name = "get_weather";
        public const string Description = "Get the current weather for a location";

        public static readonly Dictionary<string, object> Parameters = new()
        {
            ["type"] = "object",
            ["properties"] = new Dictionary<string, object>
            {
                ["location"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "The city and state, e.g. San Francisco, CA"
                },
                ["unit"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["enum"] = new[] { "celsius", "fahrenheit" },
                    ["description"] = "The temperature unit to use"
                }
            },
            ["required"] = new[] { "location", "unit" },
            ["additionalProperties"] = false
        };

        /// <summary>
        /// Simulates a weather API response.
        /// </summary>
        public static string GetMockResponse(string location) =>
            $"{{\"location\": \"{location}\", \"temperature\": 22, \"unit\": \"celsius\", \"condition\": \"sunny\"}}";
    }

    /// <summary>
    /// A calculator function for testing multiple functions.
    /// </summary>
    public static class CalculatorFunction
    {
        public const string Name = "calculate";
        public const string Description = "Perform a mathematical calculation";

        public static readonly Dictionary<string, object> Parameters = new()
        {
            ["type"] = "object",
            ["properties"] = new Dictionary<string, object>
            {
                ["expression"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "The mathematical expression to evaluate, e.g. '2 + 2'"
                }
            },
            ["required"] = new[] { "expression" },
            ["additionalProperties"] = false
        };

        public static string GetMockResponse(string expression) =>
            $"{{\"expression\": \"{expression}\", \"result\": 4}}";
    }

    /// <summary>
    /// JSON schema for structured output tests - a simple person object.
    /// </summary>
    public static class PersonSchema
    {
        public const string Name = "person";

        public static readonly Dictionary<string, object> Schema = new()
        {
            ["type"] = "object",
            ["properties"] = new Dictionary<string, object>
            {
                ["name"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "The person's full name"
                },
                ["age"] = new Dictionary<string, object>
                {
                    ["type"] = "integer",
                    ["description"] = "The person's age in years"
                },
                ["occupation"] = new Dictionary<string, object>
                {
                    ["type"] = "string",
                    ["description"] = "The person's job or profession"
                }
            },
            ["required"] = new[] { "name", "age", "occupation" },
            ["additionalProperties"] = false
        };
    }

    /// <summary>
    /// JSON schema for structured output tests - a list of steps (more complex).
    /// </summary>
    public static class StepsSchema
    {
        public const string Name = "steps_response";

        public static readonly Dictionary<string, object> Schema = new()
        {
            ["type"] = "object",
            ["properties"] = new Dictionary<string, object>
            {
                ["steps"] = new Dictionary<string, object>
                {
                    ["type"] = "array",
                    ["items"] = new Dictionary<string, object>
                    {
                        ["type"] = "object",
                        ["properties"] = new Dictionary<string, object>
                        {
                            ["step_number"] = new Dictionary<string, object>
                            {
                                ["type"] = "integer"
                            },
                            ["description"] = new Dictionary<string, object>
                            {
                                ["type"] = "string"
                            }
                        },
                        ["required"] = new[] { "step_number", "description" },
                        ["additionalProperties"] = false
                    }
                },
                ["total_steps"] = new Dictionary<string, object>
                {
                    ["type"] = "integer"
                }
            },
            ["required"] = new[] { "steps", "total_steps" },
            ["additionalProperties"] = false
        };
    }

    /// <summary>
    /// Test prompts for various scenarios.
    /// </summary>
    public static class Prompts
    {
        public const string SimpleGreeting = "Say 'Hello, World!' and nothing else.";
        public const string SimpleQuestion = "What is 2 + 2? Answer with just the number.";
        public const string WeatherQuestion = "What's the weather like in San Francisco?";
        public const string MultiStepTask = "List 3 steps to make a cup of tea.";
        public const string ReasoningTask = "Solve the equation: 3x + 7 = 22. Show your reasoning.";
        public const string PersonDescription = "Create a fictional person named John who is 30 years old and works as a software engineer.";
    }
}

