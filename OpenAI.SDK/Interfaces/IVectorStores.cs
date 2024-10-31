using Betalgo.Ranul.OpenAI.ObjectModels.RequestModels;
using Betalgo.Ranul.OpenAI.ObjectModels.ResponseModels.VectorStoreResponseModels;
using Betalgo.Ranul.OpenAI.ObjectModels.SharedModels;

namespace Betalgo.Ranul.OpenAI.Interfaces;

public interface IVectorStores
{
    /// <summary>
    ///     Returns a list of vector stores.
    /// </summary>
    Task<VectorStoreListObjectResponse> ListVectorStores(PaginationRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Create a vector store.
    /// </summary>
    Task<VectorStoreObjectResponse> CreateVectorStore(CreateVectorStoreRequest requestBody, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Retrieves a vector store.
    /// </summary>
    Task<VectorStoreObjectResponse> RetrieveVectorStore(string vectorStoreId, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Modifies a vector store.
    /// </summary>
    Task<VectorStoreObjectResponse> ModifyVectorStore(string vectorStoreId, UpdateVectorStoreRequest requestBody, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Delete a vector store.
    /// </summary>
    Task<DeletionStatusResponse> DeleteVectorStore(string vectorStoreId, CancellationToken cancellationToken = default);
}