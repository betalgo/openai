using OpenAI.GPT3.Extensions;
using OpenAI.GPT3.Interfaces;
using OpenAI.GPT3.ObjectModels;
using OpenAI.GPT3.ObjectModels.RequestModels;
using OpenAI.GPT3.ObjectModels.ResponseModels;

namespace OpenAI.GPT3.Managers;

// ReSharper disable once InheritdocInvalidUsage
/// <inheritdoc />
public partial class OpenAIService : IAudioService
{
    /// <inheritdoc />
    public async Task<AudioCreateTranscriptionResponse> CreateTranscription(AudioCreateTranscriptionRequest audioCreateTranscriptionRequest, CancellationToken cancellationToken = default)
    {
        return await Create(audioCreateTranscriptionRequest, _endpointProvider.AudioCreateTranscription(), cancellationToken);
    }

    /// <inheritdoc />
    public async Task<AudioCreateTranscriptionResponse> CreateTranslation(AudioCreateTranscriptionRequest audioCreateTranscriptionRequest, CancellationToken cancellationToken = default)
    {
        return await Create(audioCreateTranscriptionRequest, _endpointProvider.AudioCreateTranslation(), cancellationToken);
    }

    private async Task<AudioCreateTranscriptionResponse> Create(AudioCreateTranscriptionRequest audioCreateTranscriptionRequest, string uri, CancellationToken cancellationToken = default)
    {
        var multipartContent = new MultipartFormDataContent
        {
            {new ByteArrayContent(audioCreateTranscriptionRequest.File), "file", audioCreateTranscriptionRequest.FileName},
            {new StringContent(audioCreateTranscriptionRequest.Model), "model"}
        };
        if (audioCreateTranscriptionRequest.Language != null)
        {
            multipartContent.Add(new StringContent(audioCreateTranscriptionRequest.Language), "language");
        }

        if (audioCreateTranscriptionRequest.Prompt != null)
        {
            multipartContent.Add(new StringContent(audioCreateTranscriptionRequest.Prompt), "prompt");
        }

        if (audioCreateTranscriptionRequest.ResponseFormat != null)
        {
            multipartContent.Add(new StringContent(audioCreateTranscriptionRequest.ResponseFormat), "response_format");
        }

        if (audioCreateTranscriptionRequest.Temperature != null)
        {
            multipartContent.Add(new StringContent(audioCreateTranscriptionRequest.Temperature.ToString()!), "temperature");
        }


        if (StaticValues.AudioStatics.ResponseFormat.Json == audioCreateTranscriptionRequest.ResponseFormat ||
            StaticValues.AudioStatics.ResponseFormat.VerboseJson == audioCreateTranscriptionRequest.ResponseFormat)
        {
            return await _httpClient.PostFileAndReadAsAsync<AudioCreateTranscriptionResponse>(uri, multipartContent, cancellationToken);
        }

        return new AudioCreateTranscriptionResponse
        {
            Text = await _httpClient.PostFileAndReadAsStringAsync(uri, multipartContent, cancellationToken)
        };
    }
}