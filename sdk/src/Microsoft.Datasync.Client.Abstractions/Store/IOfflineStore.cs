namespace Microsoft.Datasync.Client.Abstractions;

/// <summary>
/// A set of extension methods for the <see cref="IOfflineStore"/> interface.
/// </summary>
public interface IOfflineStore
{
    /// <summary>
    /// Retrieves a reference to the delta token store.
    /// </summary>
    /// <returns>A reference to the <see cref="IDeltaTokenStore"/> for storing delta-tokens.</returns>
    IDeltaTokenStore GetDeltaTokenStore();

    /// <summary>
    /// Retrieves a table reference for a datasync table.
    /// </summary>
    /// <typeparam name="T">The type of data stored in the table.</typeparam>
    /// <param name="tableName">The name of the table.</param>
    /// <returns>A reference to the datasync table.</returns>
    /// <exception cref="InvalidOperationException">if the table reference cannot be created.</exception>
    IOfflineTable<T> GetOfflineTable<T>(string? tableName) where T : IOfflineEntity;

    /// <summary>
    /// Retrieves a reference to the operations queue.
    /// </summary>
    /// <returns>A reference to the <see cref="IOperationsQueue"/> for storing pending operations.</returns>
    IOperationsQueue GetOperationsQueue();
}
