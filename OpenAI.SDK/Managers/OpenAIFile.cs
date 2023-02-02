using OpenAI.GPT3.Extensions;
using OpenAI.GPT3.Interfaces;
using OpenAI.GPT3.ObjectModels.ResponseModels.FileResponseModels;
using OpenAI.GPT3.ObjectModels.SharedModels;

namespace OpenAI.GPT3.Managers;

public partial class OpenAIService : IFileService
{
    public async Task<FileListResponse> ListFile(CancellationToken cancellationToken = default)
    {
        return await _httpClient.GetFromJsonAsync<FileListResponse>(_endpointProvider.FilesList(), cancellationToken: cancellationToken);
    }

    public async Task<FileUploadResponse> UploadFile(string purpose, byte[] file, string fileName, CancellationToken cancellationToken = default)
    {
        var multipartContent = new MultipartFormDataContent();
        multipartContent.Add(new StringContent(purpose), "purpose");
        multipartContent.Add(new ByteArrayContent(file), "file", fileName);

        return await _httpClient.PostFileAndReadAsAsync<FileUploadResponse>(_endpointProvider.FilesUpload(), multipartContent, cancellationToken: cancellationToken);
    }

    public async Task<FileDeleteResponse> DeleteFile(string fileId, CancellationToken cancellationToken = default)
    {
        return await _httpClient.DeleteAndReadAsAsync<FileDeleteResponse>(_endpointProvider.FileDelete(fileId), cancellationToken: cancellationToken);
    }

    public async Task<FileResponse> RetrieveFile(string fileId, CancellationToken cancellationToken = default)
    {
        return await _httpClient.GetFromJsonAsync<FileResponse>(_endpointProvider.FileRetrieve(fileId), cancellationToken: cancellationToken);
    }

    //TODO Not tested yet
    //TODO check if there undocumented response object
    // I couldn't figure out how this endpoint works..
    public async Task RetrieveFileContent(string fileId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
        //await _httpClient.GetFromJsonAsync<RetrieveFileResponse>($"/{ApiVersion}/files/{fileId}/content");
    }
}