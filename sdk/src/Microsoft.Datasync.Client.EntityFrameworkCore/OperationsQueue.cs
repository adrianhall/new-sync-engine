using Microsoft.Datasync.Client.Abstractions;
using Microsoft.EntityFrameworkCore;

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

#region IOperationsQueue
    /// <summary>
    /// Gets the number of pending operations.
    /// </summary>
    /// <value>The number of pending operations (or null if the number is not known at this time).</value>
    public long? PendingOperations
    {
        get => context.OperationsQueue.Count(x => x.OperationState != DatasyncOperationState.Completed);
    }

    /// <summary>
    /// Counts the number of pending operations.
    /// </summary>
    /// <param name="cancellationToken">A <see cref="CancellationToken"> to observe.</param>
    /// <returns>A <see cref="ValueTask{T}"/> that resolves to the number of pending operations in the queue when complete.</returns>
    public async ValueTask<long> CountPendingOperationsAsync(CancellationToken cancellationToken = default)
        => await context.OperationsQueue.CountAsync(x => x.OperationState != DatasyncOperationState.Completed, cancellationToken).ConfigureAwait(false);

    /// <summary>
    /// Returns the list of pending operations that have not yet been executed.
    /// </summary>
    public IAsyncEnumerable<IDatasyncOperation> GetPendingOperations()
        => context.OperationsQueue.Where(x => x.OperationState != DatasyncOperationState.Completed).OrderBy(x => x.Sequence).AsAsyncEnumerable();

    /// <summary>
    /// Updates a pending operation with new information.
    /// </summary>
    /// <remarks>
    /// Only certain fields are updatable by this operation, e.g. the state of the operation.
    /// </remarks>
    /// <param name="operation">The operation to update, with new information.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"> to observe.</param>
    /// <returns>A <see cref="ValueTask"/> that resolves when the update is complete.</returns>
    /// <exception cref="OperationNotFoundException">when the operation specified for update cannot be found.</exception>
    public async ValueTask<IDatasyncOperation> UpdatePendingOperationAsync(IDatasyncOperation operation, CancellationToken cancellationToken = default)
    {
        var op = await context.OperationsQueue.SingleOrDefaultAsync(x => x.OperationId == operation.Id.ToString("N"), cancellationToken).ConfigureAwait(false);
        if (op == null)
        {
            throw new OperationNotFoundException() { OperationId = operation.Id };

        }
        op.OperationState = operation.OperationState;
        context.OperationsQueue.Update(op);
        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        return op;
    }
#endregion
}
