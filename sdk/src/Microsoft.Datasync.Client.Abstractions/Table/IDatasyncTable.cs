namespace Microsoft.Datasync.Client.Abstractions;

/// <summary>
/// The interface to a Datasync capable table when used for the basic
/// storage and retrieval of data.
/// </summary>
/// <typeparam name="T">The type of the entity.</typeparam>
public interface IDatasyncTable<T> where T : IDatasyncEntity
{
    /// <summary>
    /// Shows if the table can handle offline synchronization.
    /// </summary>
    /// <remarks><c>true</c> if the table is offline capable, <c>false</c> otherwise.</remarks>
    bool IsOfflineCapable { get; }

    /// <summary>
    /// The last date/time that this table was synchronized.
    /// </summary>
    DateTimeOffset? LastSynchronized { get; }

    /// <summary>
    /// Adds a set of entities into the store.
    /// </summary>
    /// <remarks>
    /// Each add operation produces an individual operation result.
    /// </remarks>
    /// <param name="entities">The list of entities that need to be added.</param>
    /// <param name="options">The options for the add operation.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe.</param>
    /// <returns>A task that resolves to the result of the add operation on completion.</returns>
    ValueTask<IEnumerable<IOperationResult<T>>> AddRangeAsync(IEnumerable<T> entities, DatasyncAddOptions options, CancellationToken cancellationToken = default);

    /// <summary>
    /// Entry-point for searches - allows the user to do LINQ expressions
    /// against this data store.
    /// </summary>
    /// <returns>The <see cref="IQueryable{T}"/> for the entity store.</returns>
    IQueryable<T> AsQueryable();

    /// <summary>
    /// Retrieves an entity from the datasync table by it's ID.
    /// </summary>
    /// <param name="id">The ID of the entity to retrieve.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe.</param>
    /// <returns>A task that resolves to the result of the operation on completion.</returns>
    ValueTask<IOperationResult<T>> GetAsync(string id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Removes a set of entities from the store; each entity is identified by its ID.
    /// </summary>
    /// <param name="entityIds">The set of entity IDs to be removed.</param>
    /// <param name="options">The options for the remove operation.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe.</param>
    /// <returns>A task that resolves to the result of the remove operation on completion.</returns>
    ValueTask<IEnumerable<IOperationResult>> RemoveRangeAsync(IEnumerable<string> entityIds, DatasyncRemoveOptions options, CancellationToken cancellationToken = default);

    /// <summary>
    /// Replaces a set of entities in the store (based on the ID of the entities).
    /// </summary>
    /// <param name="entities">The entities to be replaced in the store.</param>
    /// <param name="options">The options for the replace operation.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe.</param>
    /// <returns>A task that resolves to the result of the replace operation.</returns>
    ValueTask<IEnumerable<IOperationResult<T>>> ReplaceRangeAsync(IEnumerable<T> entities, DatasyncReplaceOptions options, CancellationToken cancellationToken = default);
}
