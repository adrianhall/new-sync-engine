namespace Microsoft.Datasync.Client.Abstractions;

public interface IDatasyncStore
{
    /// <summary>
    /// Shows if the store is an offline store.
    /// </summary>
    /// <remarks><c>true</c> if the store is offline capable, <c>false</c> otherwise.</remarks>
    bool IsOfflineCapable { get; }

    /// <summary>
    /// Retrieves a table reference for a datasync table.
    /// </summary>
    /// <typeparam name="T">The type of data stored in the table.</typeparam>
    /// <param name="tableName">The name of the table.</param>
    /// <returns>A reference to the datasync table.</returns>
    /// <exception cref="InvalidOperationException">if the table reference cannot be created.</exception>
    IDatasyncTable<T> GetDatasyncTable<T>(string? tableName) where T : IDatasyncEntity;
}
