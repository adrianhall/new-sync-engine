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

#region IOperationsQueue
    /// <summary>
    /// Gets the number of pending operations.
    /// </summary>
    /// <value>The number of pending operations (or null if the number is not known at this time).</value>
    public long? PendingOperations
    {
        get => context.OperationsQueue.Count(x => x.State != DatasyncOperationState.Completed);
    }

    /// <summary>
    /// Counts the number of pending operations.
    /// </summary>
    /// <param name="cancellationToken">A <see cref="CancellationToken"> to observe.</param>
    /// <returns>The number of pending operations in the queue.</returns>
    public ValueTask<long> CountPendingOperationsAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Returns the list of pending operations that have not yet been executed.
    /// </summary>
    public IAsyncEnumerable<IDatasyncOperation> GetPendingOperations()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Updates a pending operation with new information.
    /// </summary>
    /// <remarks>
    /// Only certain fields are updatable by this operation, e.g. the state of the operation.
    /// </remarks>
    /// <param name="operation">The operation to update, with new information.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"> to observe.</param>
    /// <returns>A <see cref="ValueTask"/> that resolves when the update is complete.</returns>
    public ValueTask<IDatasyncOperation> UpdatePendingOperationAsync(IDatasyncOperation operation, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
#endregion
}
