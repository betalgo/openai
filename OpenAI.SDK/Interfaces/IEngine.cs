using OpenAI.SDK.Models.ResponseModels.EngineResponseModels;

namespace OpenAI.SDK.Interfaces;

/// <summary>
///     Engines describe and provide access to the various models available in the API. You can refer to the Engines
///     documentation to understand what engines are available and the differences between them.
/// </summary>
public interface IEngine
{
    /// <summary>
    ///     Lists the currently available engines, and provides basic information about each one such as the owner and
    ///     availability.
    /// </summary>
    /// <returns></returns>
    Task<EngineListResponse> EngineList();

    /// <summary>
    ///     Retrieves an engine instance, providing basic information about the engine such as the owner and availability.
    /// </summary>
    /// <param name="engineId">The ID of the engine to use for this request</param>
    /// <returns></returns>
    Task<EngineRetrieveResponse> EngineRetrieve(string? engineId = null);
}