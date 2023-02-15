namespace Microsoft.Datasync.Client.Abstractions;

/// <summary>
/// Description of the operations queue.
/// </summary>
public interface IOperationsQueue
{
    /// <summary>
    /// Gets the number of pending operations.
    /// </summary>
    /// <value>The number of pending operations (or null if the number is not known at this time).</value>
    long? PendingOperations { get; }

    /// <summary>
    /// Counts the number of pending operations.
    /// </summary>
    /// <param name="cancellationToken">A <see cref="CancellationToken"> to observe.</param>
    /// <returns>The number of pending operations in the queue.</returns>
    ValueTask<long> CountPendingOperationsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns the list of pending operations that have not yet been executed.
    /// </summary>
    IAsyncEnumerable<IDatasyncOperation> GetPendingOperations();

    /// <summary>
    /// Updates a pending operation with new information.
    /// </summary>
    /// <remarks>
    /// Only certain fields are updatable by this operation, e.g. the state of the operation.
    /// </remarks>
    /// <param name="operation">The operation to update, with new information.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"> to observe.</param>
    /// <returns>A <see cref="ValueTask"/> that resolves to the updated operation when the update is complete.</returns>
    /// <exception cref="OperationNotFoundException">when the operation specified for update cannot be found.</exception>
    ValueTask<IDatasyncOperation> UpdatePendingOperationAsync(IDatasyncOperation operation, CancellationToken cancellationToken = default);
}
