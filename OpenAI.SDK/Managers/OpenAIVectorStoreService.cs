using OpenAI.Extensions;
using OpenAI.Interfaces;
using OpenAI.ObjectModels.RequestModels;
using OpenAI.ObjectModels.ResponseModels.VectorStoreResponseModels;

namespace OpenAI.Managers;
public partial class OpenAIService : IVectorStores
{
    /// <inheritdoc/>
    public async Task<VectorStoreListObjectResponse> ListVectorStores(BaseListRequest request, CancellationToken cancellationToken = default)
    {
        return await _httpClient.GetReadAsAsync<VectorStoreListObjectResponse>(_endpointProvider.VectorStoreList(request), cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<VectorStoreObjectResponse> CreateVectorStore(CreateVectorStoreRequest requestBody, CancellationToken cancellationToken = default)
    {
        return await _httpClient.PostAndReadAsAsync<VectorStoreObjectResponse>(_endpointProvider.VectorStoreCreate(), requestBody, cancellationToken);
    }

    public async Task<VectorStoreObjectResponse> GetVectorStore(string vectorStoreId, CancellationToken cancellationToken = default)
    {
        return await _httpClient.GetReadAsAsync<VectorStoreObjectResponse>(_endpointProvider.VectorStoreRetrieve(vectorStoreId), cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<VectorStoreObjectResponse> ModifyVectorStore(string vectorStoreId, UpdateVectorStoreRequest requestBody, CancellationToken cancellationToken = default)
    {
        return await _httpClient.PostAndReadAsAsync<VectorStoreObjectResponse>(_endpointProvider.VectorStoreModify(vectorStoreId), requestBody, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<VectorStoreObjectResponse> DeleteVectorStore(string vectorStoreId, CancellationToken cancellationToken = default)
    {
        return await _httpClient.DeleteAndReadAsAsync<VectorStoreObjectResponse>(_endpointProvider.VectorStoreDelete(vectorStoreId), cancellationToken);
    }
}
public partial class OpenAIService : IVectorStoreFiles
{
    public async Task<VectorStoreFileListObject> ListVectorStoreFiles(string vectorStoreId, BaseListRequest baseListRequest, CancellationToken cancellationToken = default)
    {
        return await _httpClient.GetReadAsAsync<VectorStoreFileListObject>(_endpointProvider.VectorStoreFileList(vectorStoreId, baseListRequest), cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<VectorStoreFileObject> CreateVectorStoreFile(string vectorStoreId, CreateVectorStoreFileRequest requestBody, CancellationToken cancellationToken = default)
    {
        return await _httpClient.PostAndReadAsAsync<VectorStoreFileObject>(_endpointProvider.VectorStoreFileCreate(vectorStoreId), requestBody, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<VectorStoreFileObject> GetVectorStoreFile(string vectorStoreId, string fileId, CancellationToken cancellationToken = default)
    {
        return await _httpClient.GetReadAsAsync<VectorStoreFileObject>(_endpointProvider.VectorStoreFileRetrieve(vectorStoreId, fileId), cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<VectorStoreFileObject> DeleteVectorStoreFile(string vectorStoreId, string fileId, CancellationToken cancellationToken = default)
    {
        return await _httpClient.DeleteAndReadAsAsync<VectorStoreFileObject>(_endpointProvider.VectorStoreFileDelete(vectorStoreId, fileId), cancellationToken);
    }

    public async Task<VectorStoreFileBatchObject> CreateVectorStoreFileBatch(string vectorStoreId, CreateVectorStoreFileBatchRequest requestBody, CancellationToken cancellationToken = default)
    {
        return await _httpClient.PostAndReadAsAsync<VectorStoreFileBatchObject>(_endpointProvider.VectorStoreFileBatchCreate(vectorStoreId), requestBody, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<VectorStoreFileBatchObject> GetVectorStoreFileBatch(string vectorStoreId, string batchId, CancellationToken cancellationToken = default)
    {
        return await _httpClient.GetReadAsAsync<VectorStoreFileBatchObject>(_endpointProvider.VectorStoreFileBatchRetrieve(vectorStoreId, batchId), cancellationToken);
    }


    /// <inheritdoc/>
    public async Task<VectorStoreFileBatchObject> CancelVectorStoreFileBatch(string vectorStoreId, string batchId, CancellationToken cancellationToken = default)
    {
        return await _httpClient.PostAndReadAsAsync<VectorStoreFileBatchObject>(_endpointProvider.VectorStoreFileBatchCancel(vectorStoreId, batchId), null,cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<VectorStoreFileBatchListObjectResponse> ListFilesInVectorStoreBatch(string vectorStoreId, string batchId, BaseListRequest baseListRequest, CancellationToken cancellationToken = default)
    {
        return await _httpClient.GetReadAsAsync<VectorStoreFileBatchListObjectResponse>(_endpointProvider.VectorStoreFileBatchList(vectorStoreId, batchId, baseListRequest), cancellationToken);
    }
}
