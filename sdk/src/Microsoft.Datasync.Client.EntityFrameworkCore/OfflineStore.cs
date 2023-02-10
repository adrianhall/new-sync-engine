using Microsoft.Datasync.Client.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Microsoft.Datasync.Client.EntityFrameworkCore;

public class OfflineStore<TContext> : IOfflineStore where TContext : DatasyncDbContext
{
    private IDbContextFactory<TContext> contextFactory;

    /// <summary>
    /// Creates a new <see cref="OfflineStore"/> using the specified context factory.
    /// </summary>
    /// <param name="contextFactory"></param>
    public OfflineStore(IDbContextFactory<TContext> contextFactory)
    {
        this.contextFactory = contextFactory;
    }

    /// <summary>
    /// Retrieves a reference to the delta token store.
    /// </summary>
    /// <returns>A reference to the <see cref="IDeltaTokenStore"/> for storing delta-tokens.</returns>
    public IDeltaTokenStore GetDeltaTokenStore()
    {
        var context = contextFactory.CreateDbContext();
        return new DeltaTokenStore(context);
    }

    /// <summary>
    /// Retrieves a table reference for a datasync table.
    /// </summary>
    /// <typeparam name="T">The type of data stored in the table.</typeparam>
    /// <param name="tableName">The name of the table.</param>
    /// <returns>A reference to the datasync table.</returns>
    /// <exception cref="InvalidOperationException">if the table reference cannot be created.</exception>
    public IOfflineTable<T> GetOfflineTable<T>(string? tableName) where T : IOfflineEntity
    {
        throw new NotImplementedException();
    }
}
