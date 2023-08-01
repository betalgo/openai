using LaserCatEyes.HttpClientListener;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenAI.Extensions;
using OpenAI.Interfaces;
using OpenAI.ObjectModels;
using OpenAI.ObjectModels.RequestModels;
using OpenAI.Utilities;

var builder = new ConfigurationBuilder()
    .AddUserSecrets<Program>();

IConfiguration configuration = builder.Build();
var serviceCollection = new ServiceCollection();
serviceCollection.AddScoped(_ => configuration);


// Laser cat eyes is a tool that shows your requests and responses between OpenAI server and your client.
// Get your app key from https://lasercateyes.com for FREE and put it under ApiSettings.json or secrets.json.
// It is in Beta version, if you don't want to use it just comment out below line.
serviceCollection.AddLaserCatEyesHttpClientListener();


serviceCollection.AddOpenAIService();
var serviceProvider = serviceCollection.BuildServiceProvider();
var sdk = serviceProvider.GetRequiredService<IOpenAIService>();


//await ExerciseEmbeddingTools(sdk);
await ExerciseFunctionCalling(sdk);

async Task ExerciseFunctionCalling(IOpenAIService openAIService)
{
	var calculator = new Calculator();
	var req = new ChatCompletionCreateRequest();
	req.Functions = FunctionCallingHelper.GetFunctionDefinitions(calculator);
	req.Messages = new List<ChatMessage>
	{
		ChatMessage.FromSystem("You are a helpful assistant."),
		ChatMessage.FromUser("What is 2 + 2 * 6?"),  // GPT4 is needed for this
		//ChatMessage.FromUser("What is 2 + 6?"),  // GPT3.5 is enough for this
	};

	do
	{
		var reply = await openAIService.ChatCompletion.CreateCompletion(req, Models.Gpt_4_0613);

		if (!reply.Successful)
		{
			Console.WriteLine(reply.Error?.Message);
			break;
		}
		
		var response = reply.Choices.First().Message;

		if (response.FunctionCall != null)
			Console.WriteLine($"Invoking {response.FunctionCall.Name} with params: {response.FunctionCall.Arguments}");
		else
			Console.WriteLine(response.Content);

		req.Messages.Add(response);

		if (response.FunctionCall != null)
		{
			var functionCall = response.FunctionCall;
			var result = FunctionCallingHelper.CallFunction<float>(functionCall, calculator);
			response.Content = result.ToString();
		}

	} while (req.Messages.Last().FunctionCall != null);
}


async Task ExerciseEmbeddingTools(IOpenAIService openAIService)
{
	IEmbeddingTools embeddingTools = new EmbeddingTools(openAIService, 500, Models.TextEmbeddingAdaV2);

	var dataFrame =
		await embeddingTools.ReadFilesAndCreateEmbeddingDataAsCsv(Path.Combine("Data", "OpenAI"),
			"processed/scraped.csv");

	var dataFrame2 = embeddingTools.LoadEmbeddedDataFromCsv("processed/scraped.csv");

	do
	{
		Console.WriteLine("Ask a question:");
		var question = Console.ReadLine();

		if (question != null)
		{
			var context = embeddingTools.CreateContext(question, dataFrame);

			var completionResponse = await openAIService.ChatCompletion.CreateCompletion(
				new ChatCompletionCreateRequest()
				{
					Model = Models.Gpt_4,
					Messages = new List<ChatMessage>()
					{
						ChatMessage.FromSystem(
							$"Answer the question based on the context below, and if the question can't be answered based on the context, say \"I don't know\".\n\nContext: {context}"),
						ChatMessage.FromUser(question)
					}
				});

			Console.WriteLine(completionResponse.Successful
				? completionResponse.Choices.First().Message.Content
				: completionResponse.Error?.Message);
		}
	} while (true);
}

class Calculator
{
	[FunctionDescription("Adds two numbers.")]
	public float Add(float a, float b) => a + b;

	[FunctionDescription("Subtracts two numbers.")]
	public float Subtract(float a, float b) => a - b;

	[FunctionDescription("Performs advanced math operators on two numbers.")]
	public float AdvancedMath(float a, float b, AdvancedOperators advancedOperator)
	{
		return advancedOperator switch
		{
			AdvancedOperators.Multiply => a * b,
			AdvancedOperators.Divide => a / b,
			_ => throw new ArgumentOutOfRangeException(nameof(advancedOperator), advancedOperator, null)
		};
	}

	public enum AdvancedOperators
	{
		Multiply, Divide
	}
}
