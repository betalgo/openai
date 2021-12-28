using OpenAI.SDK.Extensions;
using OpenAI.SDK.Interfaces;
using OpenAI.SDK.Models.ResponseModels.FileResponseModels;
using OpenAI.SDK.Models.SharedModels;

namespace OpenAI.SDK.Managers;

public partial class OpenAISdk : IFile
{
    public async Task<FileListResponse> FileList()
    {
        return await _httpClient.GetFromJsonAsync<FileListResponse>(_endpointProvider.ListFiles());
    }

    public async Task<FileUploadResponse> FileUpload(string purpose, byte[] file, string fileName)
    {
        var multipartContent = new MultipartFormDataContent();
        multipartContent.Add(new StringContent(purpose), "purpose");
        multipartContent.Add(new ByteArrayContent(file), "file", fileName);

        return await _httpClient.PostFileAndReadAsAsync<FileUploadResponse>(_endpointProvider.UploadFiles(), multipartContent);
    }

    public async Task<FileDeleteResponse> FileDelete(string fileId)
    {
        return await _httpClient.DeleteAndReadAsAsync<FileDeleteResponse>(_endpointProvider.DeleteFile(fileId));
    }

    public async Task<FileResponse> FileRetrieve(string fileId)
    {
        return await _httpClient.GetFromJsonAsync<FileResponse>(_endpointProvider.RetrieveFile(fileId));
    }

    //TODO Not tested yet
    //TODO check if there undocumented response object
    // I couldn't figure out how this endpoint works..
    public async Task FileRetrieveContent(string fileId)
    {
        throw new NotImplementedException();
        //await _httpClient.GetFromJsonAsync<RetrieveFileResponse>($"/{ApiVersion}/files/{fileId}/content");
    }
}