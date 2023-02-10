namespace Microsoft.Datasync.Client.Abstractions;

public interface IOfflineStore
{
    /// <summary>
    /// Retrieves a table reference for a datasync table.
    /// </summary>
    /// <typeparam name="T">The type of data stored in the table.</typeparam>
    /// <param name="tableName">The name of the table.</param>
    /// <returns>A reference to the datasync table.</returns>
    /// <exception cref="InvalidOperationException">if the table reference cannot be created.</exception>
    IOfflineTable<T> GetOfflineTable<T>(string? tableName) where T : IOfflineEntity;
}
