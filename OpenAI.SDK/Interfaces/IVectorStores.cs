using OpenAI.ObjectModels.RequestModels;
using OpenAI.ObjectModels.ResponseModels.VectorStoreResponseModels;

namespace OpenAI.Interfaces;

public interface IVectorStores
{
    /// <summary>
    /// Returns a list of vector stores.
    /// </summary>
    Task<VectorStoreListObjectResponse> ListVectorStores(BaseListRequest request, CancellationToken cancellationToken = default);
    /// <summary>
    /// Create a vector store.
    /// </summary>
    Task<VectorStoreObjectResponse> CreateVectorStore(CreateVectorStoreRequest requestBody, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a vector store.
    /// </summary>
    Task<VectorStoreObjectResponse> GetVectorStore(string vectorStoreId, CancellationToken cancellationToken = default);
    /// <summary>
    /// Modifies a vector store.
    /// </summary>
    Task<VectorStoreObjectResponse> ModifyVectorStore(string vectorStoreId, UpdateVectorStoreRequest requestBody, CancellationToken cancellationToken = default);
    /// <summary>
    /// Delete a vector store.
    /// </summary>
    Task<VectorStoreObjectResponse> DeleteVectorStore(string vectorStoreId, CancellationToken cancellationToken = default);
}