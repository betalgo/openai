using OpenAI.ObjectModels.RequestModels;
using OpenAI.ObjectModels.ResponseModels.VectorStoreResponseModels;
using OpenAI.ObjectModels.SharedModels;

namespace OpenAI.Interfaces;

public interface IVectorStoreFiles
{
    /// <summary>
    /// Returns a list of vector store files.
    /// </summary>
    Task<VectorStoreFileListObject> ListVectorStoreFiles(string vectorStoreId, VectorStoreFileListRequest baseListRequest, CancellationToken cancellationToken = default);
    /// <summary>
    /// Create a vector store file by attaching a [File](/docs/api-reference/files) to a [vector store](/docs/api-reference/vector-stores/object).
    /// </summary>
    Task<VectorStoreFileObject> CreateVectorStoreFile(string vectorStoreId, CreateVectorStoreFileRequest requestBody, CancellationToken cancellationToken = default);
    /// <summary>
    /// Retrieves a vector store file.
    /// </summary>
    Task<VectorStoreFileObject> GetVectorStoreFile(string vectorStoreId, string fileId, CancellationToken cancellationToken = default);
    /// <summary>
    /// Delete a vector store file. This will remove the file from the vector store but the file itself will not be deleted. To delete the file, use the [delete file](/docs/api-reference/files/delete) endpoint.
    /// </summary>
    Task<DeletionStatusResponse> DeleteVectorStoreFile(string vectorStoreId, string fileId, CancellationToken cancellationToken = default);
    /// <summary>
    /// Create a vector store file batch.
    /// </summary>
    Task<VectorStoreFileBatchObject> CreateVectorStoreFileBatch(string vectorStoreId, CreateVectorStoreFileBatchRequest requestBody, CancellationToken cancellationToken = default);
    /// <summary>
    /// Retrieves a vector store file batch.
    /// </summary>
    Task<VectorStoreFileBatchObject> GetVectorStoreFileBatch(string vectorStoreId, string batchId, CancellationToken cancellationToken = default);
    /// <summary>
    /// Cancel a vector store file batch. This attempts to cancel the processing of files in this batch as soon as possible.
    /// </summary>
    Task<VectorStoreFileBatchObject> CancelVectorStoreFileBatch(string vectorStoreId, string batchId, CancellationToken cancellationToken = default);
    /// <summary>
    /// Returns a list of vector store files in a batch.
    /// </summary>
    Task<VectorStoreFileBatchListObjectResponse> ListFilesInVectorStoreBatch(string vectorStoreId, string batchId, PaginationRequest baseListRequest, CancellationToken cancellationToken = default);
}