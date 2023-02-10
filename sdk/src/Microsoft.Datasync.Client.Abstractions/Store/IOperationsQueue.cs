namespace Microsoft.Datasync.Client.Abstractions;

/// <summary>
/// Description of the operations queue.
/// </summary>
public interface IOperationsQueue
{
    /// <summary>
    /// Returns an <see cref="IQueryable{T}"/> of the datasync operations, ordered
    /// by the created data.
    /// </summary>
    IQueryable<IDatasyncOperation> AsQueryable();

    /// <summary>
    /// Returns the next operation that needs to be transmitted to the remote service.
    /// </summary>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe.</param>
    /// <returns>The next operation, or <c>null</c> if nothing is enqueued.</returns>
    ValueTask<IDatasyncOperation?> GetNextOperationAsync(CancellationToken cancellationToken = default);
}
