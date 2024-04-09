using System.Collections;
using System.Globalization;
using System.Text;
using CsvHelper;
using MathNet.Numerics;
using Microsoft.Data.Analysis;
using OpenAI.Interfaces;
using OpenAI.ObjectModels.RequestModels;
using OpenAI.ObjectModels.ResponseModels;
using OpenAI.Tokenizer.GPT3;

namespace OpenAI.Utilities.Embedding;

// ReSharper disable MemberCanBePrivate.Global
public interface IEmbeddingTools
{
    /// <summary>
    ///     Reads a file or a directory and creates embedding data as CSV.
    /// </summary>
    /// <param name="pathToDirectoryOrFile">The path to the file or directory.</param>
    /// <param name="outputFileName">The name of the output file.</param>
    /// <returns>The DataFrame containing the embedding data.</returns>
    Task<DataFrame> ReadFilesAndCreateEmbeddingDataAsCsv(string pathToDirectoryOrFile, string outputFileName);

    /// <summary>
    ///     Reads multiple files or directories and creates embedding data as CSV.
    /// </summary>
    /// <param name="pathsToDirectoriesOrFiles">The paths to the files or directories.</param>
    /// <param name="outputFileName">The name of the output file.</param>
    /// <returns>The DataFrame containing the embedding data.</returns>
    /// <exception cref="Exception">Thrown when no files are found in the provided paths.</exception>
    Task<DataFrame> ReadFilesAndCreateEmbeddingDataAsCsv(IEnumerable<string> pathsToDirectoriesOrFiles, string outputFileName);

    string CreateContext(string question, DataFrame df, int maxLen = 1800);

    DataFrame LoadEmbeddedDataFromCsv(string path);
}

public interface IEmbeddingToolsAdvanced
{
    /// <returns></returns>
    /// <summary>
    ///     Loads all files from a directory and returns a list of <see cref="TextEmbeddingData" />.
    /// </summary>
    /// <param name="pathToDirectory"></param>
    /// <returns></returns>
    IEnumerable<TextEmbeddingData> LoadFilesFromDirectory(string pathToDirectory);

    /// <summary>
    ///     Writes the provided data to a CSV file at the specified file path.
    /// </summary>
    Task WriteToTempCsv(IEnumerable<TextEmbeddingData> textEmbeddingData, string outputFilePath);

    void ClearTempCsv(string outputFilePath);

    /// <summary>
    ///     Reads data from a CSV file and splits the text based on specific conditions.
    /// </summary>
    DataFrame ReadAndSplitData(string outputFilePath);

    /// <summary>
    ///     Returns a list of EmbeddingResults by running each text through an EmbeddingCreateRequest.
    /// </summary>
    Task<List<EmbeddingCreateResponse>> GetEmbeddings(IEnumerable<string> texts);

    /// <summary>
    ///     Adds embeddings to the DataFrame and handles any unsuccessful embedding results.
    /// </summary>
    DataFrame AddEmbeddingsToDf(DataFrame df, List<EmbeddingCreateResponse> embeddingResults);

    /// <summary>
    ///     Writes the DataFrame to a CSV file at the specified file path.
    /// </summary>
    void SaveDfToCsv(DataFrame df, string outputFilePath);

    /// <summary>
    ///     Performs text embedding on input data, then saves and returns the resulting DataFrame.
    /// </summary>
    Task<DataFrame> PerformTextEmbedding(IEnumerable<TextEmbeddingData> textEmbeddingData, string outputFilePath);

    /// <summary>
    ///     Splits a given text into chunks, ensuring that no chunk exceeds a maximum token count.
    /// </summary>
    /// <param name="text">The text to be split.</param>
    /// <returns>A list of chunks split from the input text.</returns>
    IEnumerable<string> SplitIntoMany(string text);

    PrimitiveDataFrameColumn<double> DistancesFromEmbeddings(List<double> qEmbeddings, DataFrameColumn embeddingsColumn);

    /// <summary>
    ///     Reads a file and returns a <see cref="TextEmbeddingData" /> object.
    /// </summary>
    /// <param name="file"></param>
    TextEmbeddingData? LoadFile(string file);
}

public class EmbeddingTools : IEmbeddingTools, IEmbeddingToolsAdvanced
{
    private readonly string _embeddingModel;
    private readonly int _maxToken;
    private readonly IOpenAIService _sdk;

    public EmbeddingTools(IOpenAIService sdk, int maxToken, string embeddingModel)
    {
        _sdk = sdk;
        _maxToken = maxToken;
        _embeddingModel = embeddingModel;
    }

    public async Task<DataFrame> ReadFilesAndCreateEmbeddingDataAsCsv(string pathToDirectoryOrFile, string outputFileName)
    {
        return await ReadFilesAndCreateEmbeddingDataAsCsv(new[] { pathToDirectoryOrFile }, outputFileName);
    }

    public async Task<DataFrame> ReadFilesAndCreateEmbeddingDataAsCsv(IEnumerable<string> pathsToDirectoriesOrFiles, string outputFileName)
    {
        var files = new List<TextEmbeddingData>();

        foreach (var path in pathsToDirectoriesOrFiles)
        {
            if (File.Exists(path))
            {
                var file = LoadFile(path);
                if (file != null)
                {
                    files.Add(file);
                }
            }
            else if (Directory.Exists(path))
            {
                files.AddRange(LoadFilesFromDirectory(path));
            }
        }

        if (!files.Any())
        {
            throw new Exception("No files found.");
        }

        return await PerformTextEmbedding(files, outputFileName);
    }

    public string CreateContext(string question, DataFrame df, int maxLen = 1800)
    {
        // Get the embeddings for the question
        var questionEmbeddingResult = _sdk.Embeddings.CreateEmbedding(new EmbeddingCreateRequest
        {
            Input = question,
            Model = _embeddingModel
        }).Result;

        if (!questionEmbeddingResult.Successful)
        {
            throw new Exception($"Error creating question embedding: {questionEmbeddingResult.Error?.Code}: {questionEmbeddingResult.Error?.Message}");
        }

        var qEmbeddings = questionEmbeddingResult.Data[0].Embedding;

        // Get the distances from the embeddings
        df.Columns.Remove(EmbedStaticValues.Distances);
        df.Columns.Add(DistancesFromEmbeddings(qEmbeddings, df[EmbedStaticValues.Embeddings]));

        // Sort by distance and add the text to the context until the context is too long
        df = df.OrderBy(EmbedStaticValues.Distances);
        var returns = new List<string>();
        var curLen = 0;

        var nTokensIndex = df.Columns.IndexOf(EmbedStaticValues.NTokens);
        var textIndex = df.Columns.IndexOf(EmbedStaticValues.Text);

        foreach (var row in df.Rows)
        {
            // Add the length of the text to the current length
            curLen += Convert.ToInt32(row[nTokensIndex]) + 4;

            // If the context is too long, break
            if (curLen > maxLen)
            {
                break;
            }

            // Else add it to the text that is being returned
            returns.Add((string)row[textIndex]);
        }

        // Return the context
        return string.Join("\n\n###\n\n", returns);
    }

    public DataFrame LoadEmbeddedDataFromCsv(string path)
    {
        return DataFrame.LoadCsv(path);
    }

    /// <returns></returns>
    /// <summary>
    ///     Loads all files from a directory and returns a list of <see cref="TextEmbeddingData" />.
    /// </summary>
    /// <param name="pathToDirectory"></param>
    /// <returns></returns>
    public IEnumerable<TextEmbeddingData> LoadFilesFromDirectory(string pathToDirectory)
    {
        return !Path.Exists(pathToDirectory)
            ? new List<TextEmbeddingData>()
            : Directory.EnumerateFiles(pathToDirectory).Select(LoadFile).Where(r => r != null).ToList()!;
    }

    /// <summary>
    ///     Writes the provided data to a CSV file at the specified file path.
    /// </summary>
    public async Task WriteToTempCsv(IEnumerable<TextEmbeddingData> textEmbeddingData, string outputFilePath)
    {
        outputFilePath = Path.Combine(Path.GetTempPath(), outputFilePath);
        Directory.CreateDirectory(Path.GetDirectoryName(outputFilePath) ?? string.Empty);
        await using var writer = new StreamWriter(outputFilePath);
        await using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
        await csv.WriteRecordsAsync((IEnumerable)textEmbeddingData);
        await csv.DisposeAsync();
    }

    public void ClearTempCsv(string outputFilePath)
    {
        outputFilePath = Path.Combine(Path.GetTempPath(), outputFilePath);
        //delete file
        File.Delete(outputFilePath);
    }

    /// <summary>
    ///     Reads data from a CSV file and splits the text based on specific conditions.
    /// </summary>
    public DataFrame ReadAndSplitData(string outputFilePath)
    {
        var shortened = new List<string>();
        var df = DataFrame.LoadCsv(outputFilePath);

        for (long i = 0; i < df.Rows.Count; i++)
        {
            var textValue = df[EmbedStaticValues.Text][i];
            var nTokensValue = df[EmbedStaticValues.NTokens][i];

            if (textValue is null or DBNull)
            {
                continue;
            }

            var text = textValue.ToString()!;
            var nTokens = Convert.ToInt32(nTokensValue);

            if (nTokens > _maxToken)
            {
                shortened.AddRange(SplitIntoMany(text));
            }
            else
            {
                shortened.Add(text);
            }
        }

        var shortenedDf = new DataFrame();
        shortenedDf.Columns.Add(new StringDataFrameColumn(EmbedStaticValues.Text, shortened));
        shortenedDf.Columns.Add(new PrimitiveDataFrameColumn<int>(EmbedStaticValues.NTokens, shortened.Select(x => TokenizerGpt3.TokenCount(x))));

        return shortenedDf;
    }

    /// <summary>
    ///     Returns a list of EmbeddingResults by running each text through an EmbeddingCreateRequest.
    /// </summary>
    public async Task<List<EmbeddingCreateResponse>> GetEmbeddings(IEnumerable<string> texts)
    {
        var embeddingTasks = texts.Select(async text => await _sdk.Embeddings.CreateEmbedding(new EmbeddingCreateRequest
        {
            Input = text,
            Model = _embeddingModel
        })).ToList();

        return (await Task.WhenAll(embeddingTasks)).ToList();
    }

    /// <summary>
    ///     Adds embeddings to the DataFrame and handles any unsuccessful embedding results.
    /// </summary>
    public DataFrame AddEmbeddingsToDf(DataFrame df, List<EmbeddingCreateResponse> embeddingResults)
    {
        if (embeddingResults.All(result => result.Successful))
        {
            var embeddings = embeddingResults.Select(result => string.Join(",", result.Data.FirstOrDefault()!.Embedding)).ToList();
            df.Columns.Add(new StringDataFrameColumn(EmbedStaticValues.Embeddings, embeddings));
        }
        else
        {
            var failedResult = embeddingResults.FirstOrDefault(result => !result.Successful);
            if (failedResult?.Error == null)
            {
                throw new Exception("Unknown Error");
            }

            Console.WriteLine($"{failedResult.Error.Code}: {failedResult.Error.Message}");
        }

        return df;
    }

    /// <summary>
    ///     Writes the DataFrame to a CSV file at the specified file path.
    /// </summary>
    public void SaveDfToCsv(DataFrame df, string outputFilePath)
    {
        DataFrame.SaveCsv(df, outputFilePath);
    }

    /// <summary>
    ///     Performs text embedding on input data, then saves and returns the resulting DataFrame.
    /// </summary>
    public async Task<DataFrame> PerformTextEmbedding(IEnumerable<TextEmbeddingData> textEmbeddingData, string outputFilePath)
    {
        await WriteToTempCsv(textEmbeddingData, outputFilePath);

        var df = ReadAndSplitData(outputFilePath);
        var dfTextList = new List<string>();
        for (var i = 0; i < df.Rows.Count; i++)
        {
            dfTextList.Add(df[EmbedStaticValues.Text][i].ToString() ?? string.Empty);
        }

        var embeddingResults = await GetEmbeddings(dfTextList);
        var newDf = AddEmbeddingsToDf(df, embeddingResults);
        SaveDfToCsv(newDf, outputFilePath);
        ClearTempCsv(outputFilePath);
        return newDf;
    }

    /// <summary>
    ///     Splits a given text into chunks, ensuring that no chunk exceeds a maximum token count.
    /// </summary>
    /// <param name="text">The text to be split.</param>
    /// <returns>A list of chunks split from the input text.</returns>
    public IEnumerable<string> SplitIntoMany(string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            throw new ArgumentException("Input text cannot be null or empty.", nameof(text));
        }

        var sentences = text.Split(". ", StringSplitOptions.RemoveEmptyEntries);

        var chunks = new List<StringBuilder>();
        var currentChunk = new StringBuilder();
        var tokensInCurrentChunk = 0;

        foreach (var sentence in sentences)
        {
            var tokensInSentence = TokenizerGpt3.TokenCount(" " + sentence);

            // If adding the current sentence exceeds the maximum token count, add the current chunk to the list
            if (tokensInCurrentChunk + tokensInSentence > _maxToken)
            {
                if (currentChunk.Length > 0)
                {
                    chunks.Add(currentChunk);
                    currentChunk = new StringBuilder();
                    tokensInCurrentChunk = 0;
                }

                // If a single sentence has more tokens than allowed, it cannot be included
                if (tokensInSentence > _maxToken)
                {
                    continue;
                }
            }

            if (currentChunk.Length > 0)
            {
                currentChunk.Append(". ");
                tokensInCurrentChunk++;
            }

            currentChunk.Append(sentence);
            tokensInCurrentChunk += tokensInSentence;
        }

        // Add the final chunk if it is not empty
        if (currentChunk.Length > 0)
        {
            chunks.Add(currentChunk);
        }

        // Convert StringBuilder list to string list
        return chunks.Select(c => c.ToString()).ToList();
    }

    public PrimitiveDataFrameColumn<double> DistancesFromEmbeddings(List<double> qEmbeddings, DataFrameColumn embeddingsColumn)
    {
        var distances = new PrimitiveDataFrameColumn<double>(EmbedStaticValues.Distances, embeddingsColumn.Length);
        for (var i = 0; i < embeddingsColumn.Length; i++)
        {
            var rowEmbeddings = embeddingsColumn[i].ToString()!.Split(',').Select(Convert.ToDouble);
            distances[i] = Distance.Cosine(qEmbeddings.Select(x => x).ToArray(), Array.ConvertAll(rowEmbeddings.ToArray(), x => x));
        }

        return distances;
    }

    /// <summary>
    ///     Reads a file and returns a <see cref="TextEmbeddingData" /> object.
    /// </summary>
    /// <param name="file"></param>
    public TextEmbeddingData? LoadFile(string file)
    {
        if (!File.Exists(file))
        {
            return null;
        }

        var text = File.ReadAllText(file, Encoding.UTF8);

        var fileName = Path.GetFileNameWithoutExtension(file)
            .Replace('-', ' ')
            .Replace('_', ' ');

        var textFile = new TextEmbeddingData
        {
            FileName = fileName,
            Text = $"{fileName}. {text.RemoveNewlines()}"
        };

        textFile.NToken = TokenizerGpt3.TokenCount(textFile.Text);

        return textFile;
    }

    /// <summary>
    /// </summary>
    /// <param name="pathToDirectory"></param>
    /// <param name="outputFileName"></param>
    /// <returns></returns>
    public async Task<DataFrame> ReadAllDataInFolderAndCreateEmbeddingData(string pathToDirectory, string outputFileName)
    {
        var files = LoadFilesFromDirectory(pathToDirectory);
        return await PerformTextEmbedding(files, outputFileName);
    }
}
