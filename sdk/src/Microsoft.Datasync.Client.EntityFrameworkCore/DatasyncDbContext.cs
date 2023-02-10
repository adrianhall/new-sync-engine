using Microsoft.Datasync.Client.EntityFrameworkCore.Models;
using Microsoft.EntityFrameworkCore;

namespace Microsoft.Datasync.Client.EntityFrameworkCore;

/// <summary>
/// A base line <see cref="DbContext"/> that contains the base system tables.
/// </summary>
public abstract class DatasyncDbContext : DbContext
{
    /// <summary>
    /// Create a new <see cref="DatasyncDbContext"/> object.
    /// </summary>
    /// <param name="options">The <see cref="DbContextOptions"/> for the context object.</param>
    protected DatasyncDbContext(DbContextOptions options) : base(options)
    {
    }
    
    /// <summary>
    /// The EF version of the DeltaToken store.
    /// </summary>
    public DbSet<DeltaToken> DeltaTokens => Set<DeltaToken>();

    /// <summary>
    /// The EF version of the OperationsQueue store.
    /// </summary>
    public DbSet<OperationsQueueEntry> OperationsQueue => Set<OperationsQueueEntry>();
}