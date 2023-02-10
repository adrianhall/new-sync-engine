namespace Microsoft.Datasync.Client.Abstractions;

public static class IOfflineStoreExtensions
{
    /// <summary>
    /// Retrieves an offline table from the store.
    /// </summary>
    /// <typeparam name="T">The type of entity stored in the offline table.</typeparam>
    /// <param name="store">The offline store.</param>
    /// <returns>A reference to the offline table.</returns>
    /// <exception cref="InvalidOperationException">if the table reference cannot be created.</exception>
    public static IOfflineTable<T> GetOfflineTable<T>(this IOfflineStore store) where T : IOfflineEntity
        => store.GetOfflineTable<T>(null);
}
