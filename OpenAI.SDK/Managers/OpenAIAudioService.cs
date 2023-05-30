using OpenAI.Extensions;
using OpenAI.Interfaces;
using OpenAI.ObjectModels;
using OpenAI.ObjectModels.RequestModels;
using OpenAI.ObjectModels.ResponseModels;

namespace OpenAI.Managers;

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
        var multipartContent = new MultipartFormDataContent();

        if (audioCreateTranscriptionRequest is {File: not null, FileStream: not null})
        {
            throw new ArgumentException("Either File or FileStream must be set, but not both.");
        }

        if (audioCreateTranscriptionRequest.File != null)
        {
            multipartContent.Add(
                new ByteArrayContent(audioCreateTranscriptionRequest.File),
                "file",
                audioCreateTranscriptionRequest.FileName
            );
        }
        else if (audioCreateTranscriptionRequest.FileStream != null)
        {
            multipartContent.Add(
                new StreamContent(audioCreateTranscriptionRequest.FileStream),
                "file",
                audioCreateTranscriptionRequest.FileName
            );
        }

        multipartContent.Add(new StringContent(audioCreateTranscriptionRequest.Model), "model");

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


        if (null == audioCreateTranscriptionRequest.ResponseFormat ||
            StaticValues.AudioStatics.ResponseFormat.Json == audioCreateTranscriptionRequest.ResponseFormat ||
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