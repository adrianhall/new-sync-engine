using Microsoft.Datasync.Client.Abstractions;

namespace Microsoft.Datasync.Client.EntityFrameworkCore;

/// <summary>
/// Entity Framework Core edition of the <see cref="IOperationsQueue"/>
/// </summary>
public class OperationsQueue : IOperationsQueue
{
    private readonly DatasyncDbContext context;

    /// <summary>
    /// Creates a new <see cref="OperationsQueue"/> based on the database context.
    /// </summary>
    /// <param name="context">The database context.</param>
    public OperationsQueue(DatasyncDbContext context)
    {
        this.context = context;
    }

    /// <summary>
    /// Returns an <see cref="IQueryable{T}"/> of the datasync operations, ordered
    /// by the created data.
    /// </summary>
    public IQueryable<IDatasyncOperation> AsQueryable()
        => context.OperationsQueue.OrderBy(x => x.Sequence).AsQueryable<IDatasyncOperation>();
}
