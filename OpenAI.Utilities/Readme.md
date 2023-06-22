# Dotnet SDK for OpenAI ChatGPT, Whisper, GPT-4 and DALL·E

[![Betalgo.OpenAI.Utilities](https://img.shields.io/nuget/v/Betalgo.OpenAI.Utilities?style=for-the-badge)](https://www.nuget.org/packages/Betalgo.OpenAI.Utilities/)

```
Install-Package Betalgo.OpenAI.Utilities
```

# Utilities for Betalgo.OpenAI

Welcome to add-on for Betalgo.OpenAI. It's filled with lots of cool features that could be really useful. These are a bit experimental, so they might not always work as expected. But don't worry, I'm constantly tweaking and updating them.

Sure, updates might cause a hiccup or two once in a while, but I believe it's less hassle than trying to figure out and write all these tricks yourself. So, if you decide to use this add-on, you'll probably find it saves you some time and helps get things done faster.

## Playground 
Playground project is avaliable OpenAI.UtilitiesPlayground

## Features
### Embedding Tools
In this project, `EmbeddingTools` is used to transform raw text into numerical text embeddings. These embeddings, which represent the semantic content of the text, are a crucial part of many AI systems because they allow AI models to understand and process text. In this case, they help the GPT-4 model understand the context of user queries and generate relevant responses. Without these embeddings, the AI model would have difficulty interpreting and learning from raw text data.

This example demonstrates how to create a conversational AI chatbot using OpenAI SDK and text embeddings. The major steps in this process are:

1. **Embedding Tools Initialization:** The `EmbeddingTools` instance is configured for text embedding tasks using a specified model and dimension size.

2. **Embedding Creation and Loading:** The application reads files from a specific directory, processes their data to create an embedding CSV file, and then loads this embedded data back into the system.

3. **User Interaction:** The application continually prompts the user to input a question.

4. **Context Generation:** For each provided question, the application generates a context using the pre-loaded embedded data.

5. **Chatbot Response:** The user's question, along with its context, are fed to the GPT-4 model through the OpenAI SDK's `ChatCompletion` method. The model generates an appropriate response based on the given context.

6. **Output Display:** The response from the bot is displayed to the user. In case the question can't be answered based on the context or an error is encountered, the program responds with a suitable message or a default "I don't know" response.

This repository offers an example of building a conversational AI application using text embeddings and the OpenAI SDK to deliver context-aware responses to

 user questions. Refer to the code for a more detailed understanding of the implementation.

```csharp
// Instantiate EmbeddingTools for text embedding tasks. 'sdk' is an instance of the SDK, '500' denotes the dimension of the embedding, 
// and 'TextEmbeddingAdaV2' is the model to be used.
IEmbeddingTools embeddingTools = new EmbeddingTools(sdk,500,Models.TextEmbeddingAdaV2);

// Read files from the provided path and create an embedding data CSV file. Awaits the operation to complete before moving forward.
var dataFrame = await embeddingTools.ReadFilesAndCreateEmbeddingDataAsCsv(Path.Combine("Data", "OpenAI"),"processed/scraped.csv"); 

// Load the embedded data from the CSV file into a DataFrame-like data structure. This allows you to easily use the embedded data in the future. Once the CSV file has been created, there is no need to recreate it unless new data has been added.
var dataFrame2 = embeddingTools.LoadEmbeddedDataFromCsv("processed/scraped.csv");

// Enter an infinite loop to interact with the user continually.
do
{
    // Prompt the user to ask a question.
    Console.WriteLine("Ask a question:");

    // Read the user's question.
    var question = Console.ReadLine();

    // If the user's question is not null, proceed.
    if (question != null)
    {
        // Create a context for the question using the loaded embedded data.
        var context = embeddingTools.CreateContext(question, dataFrame);

        // Pass the context and the user's question to the Gpt_4 model via the sdk's ChatCompletion method.
        var completionResponse = await sdk.ChatCompletion.CreateCompletion(new ChatCompletionCreateRequest()
        {
            Model = Models.Gpt_4,
            Messages = new List<ChatMessage>()
            {
                ChatMessage.FromSystem($"Answer the question based on the context below, and if the question can't be answered based on the context, say \"I don't know\".\n\nContext: {context}"),
                ChatMessage.FromUser(question)
            }
        });

        // If the response from the model is successful, print the response. If an error occurs or the question can't be answered based on the context, 
        // handle gracefully by printing an error message or "I don't know".
        Console.WriteLine(completionResponse.Successful ? completionResponse.Choices.First().Message.Content : completionResponse.Error?.Message);
    }
} while (true);
```