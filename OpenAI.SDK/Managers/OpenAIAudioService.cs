﻿using System.Globalization;
using Betalgo.Ranul.OpenAI.Contracts.Enums;
using Betalgo.Ranul.OpenAI.Extensions;
using Betalgo.Ranul.OpenAI.Interfaces;
using Betalgo.Ranul.OpenAI.ObjectModels.RequestModels;
using Betalgo.Ranul.OpenAI.ObjectModels.ResponseModels;

namespace Betalgo.Ranul.OpenAI.Managers;

// ReSharper disable once InheritdocInvalidUsage
/// <inheritdoc />
public partial class OpenAIService : IAudioService
{
    /// <inheritdoc />
    public async Task<AudioCreateTranscriptionResponse> CreateTranscription(AudioCreateTranscriptionRequest audioCreateTranscriptionRequest, CancellationToken cancellationToken = default) =>
        await Create(audioCreateTranscriptionRequest, _endpointProvider.AudioCreateTranscription(), cancellationToken);

    /// <inheritdoc />
    public async Task<AudioCreateTranscriptionResponse> CreateTranslation(AudioCreateTranscriptionRequest audioCreateTranscriptionRequest, CancellationToken cancellationToken = default) =>
        await Create(audioCreateTranscriptionRequest, _endpointProvider.AudioCreateTranslation(), cancellationToken);

    public async Task<AudioCreateSpeechResponse<T>> CreateSpeech<T>(AudioCreateSpeechRequest audioCreateSpeechRequest, CancellationToken cancellationToken = default) =>
        await _httpClient.PostAndReadAsDataAsync<AudioCreateSpeechResponse<T>, T>(_endpointProvider.AudioCreateSpeech(), audioCreateSpeechRequest, cancellationToken);

    private async Task<AudioCreateTranscriptionResponse> Create(AudioCreateTranscriptionRequest audioCreateTranscriptionRequest, string uri, CancellationToken cancellationToken = default)
    {
        var multipartContent = new MultipartFormDataContent();

        if (audioCreateTranscriptionRequest is { File: not null, FileStream: not null })
        {
            throw new ArgumentException("Either File or FileStream must be set, but not both.");
        }

        if (audioCreateTranscriptionRequest.File != null)
        {
            multipartContent.Add(new ByteArrayContent(audioCreateTranscriptionRequest.File), "file", audioCreateTranscriptionRequest.FileName);
        }
        else if (audioCreateTranscriptionRequest.FileStream != null)
        {
            multipartContent.Add(new StreamContent(audioCreateTranscriptionRequest.FileStream), "file", audioCreateTranscriptionRequest.FileName);
        }

        multipartContent.Add(new StringContent(audioCreateTranscriptionRequest.Model), "model");

        if (audioCreateTranscriptionRequest.TimestampGranularities != null)
        {
            foreach (var granularity in audioCreateTranscriptionRequest.TimestampGranularities)
            {
                multipartContent.Add(new StringContent(granularity), "timestamp_granularities[]");
            }
        }

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
            multipartContent.Add(new StringContent(audioCreateTranscriptionRequest.Temperature.Value.ToString(CultureInfo.InvariantCulture)), "temperature");
        }


        if (null == audioCreateTranscriptionRequest.ResponseFormat || AudioResponseFormat.Json == audioCreateTranscriptionRequest.ResponseFormat ||
            AudioResponseFormat.VerboseJson == audioCreateTranscriptionRequest.ResponseFormat)
        {
            return await _httpClient.PostFileAndReadAsAsync<AudioCreateTranscriptionResponse>(uri, multipartContent, cancellationToken);
        }

        var response = await _httpClient.PostFileAndReadAsStringAsync<AudioCreateTranscriptionResponse>(uri, multipartContent, cancellationToken);
        if (response.stringResponse != null)
        {
            response.baseResponse.Text = response.stringResponse;
        }

        return response.baseResponse;
    }
}