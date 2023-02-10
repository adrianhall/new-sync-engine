namespace Microsoft.Datasync.Client.Abstractions;

/// <summary>
/// A set of extension methods for the <see cref="IDatasyncTable{T}"/> interface.
/// </summary>
public static class IDatasyncTableExtensions
{
    /// <summary>
    /// Adds a single entity to the datasync table.
    /// </summary>
    /// <typeparam name="T">The type of entity.</typeparam>
    /// <param name="table">The table to add the entity to.</param>
    /// <param name="entity">The entity to be added.</param>
    /// <param name="options">The options to use when adding the entity.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe.</param>
    /// <returns>A task that resolves to the operation result when complete.</returns>
    public static async ValueTask<IOperationResult<T>> AddAsync<T>(this IDatasyncTable<T> table, T entity, DatasyncAddOptions options, CancellationToken cancellationToken = default) where T : IDatasyncEntity
    {
        var results = await table.AddRangeAsync(new T[] { entity }, options, cancellationToken).ConfigureAwait(false);
        return results.Single();
    }

    /// <summary>
    /// Adds a single entity to the datasync table, using default options.
    /// </summary>
    /// <typeparam name="T">The type of entity.</typeparam>
    /// <param name="table">The table to add the entity to.</param>
    /// <param name="entity">The entity to be added.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe.</param>
    /// <returns>A task that resolves to the operation result when complete.</returns>
    public static ValueTask<IOperationResult<T>> AddAsync<T>(this IDatasyncTable<T> table, T entity, CancellationToken cancellationToken = default) where T : IDatasyncEntity
        => table.AddAsync(entity, new DatasyncAddOptions(), cancellationToken);

    /// <summary>
    /// Adds a set of entities into the store, using default options.
    /// </summary>
    /// <remarks>
    /// Each add operation produces an individual operation result.
    /// </remarks>
    /// <param name="entities">The list of entities that need to be added.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe.</param>
    /// <returns>A task that resolves to the result of the add operation on completion.</returns>
    public static ValueTask<IEnumerable<IOperationResult<T>>> AddRangeAsync<T>(this IDatasyncTable<T> table, IEnumerable<T> entities, CancellationToken cancellationToken = default) where T : IDatasyncEntity
        => table.AddRangeAsync(entities, new DatasyncAddOptions(), cancellationToken);

    /// <summary>
    /// Removes an entity from the store; the entity is identified by its ID.
    /// </summary>
    /// <param name="entityIds">The ID of the entity to be removed.</param>
    /// <param name="options">The options for the remove operation.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe.</param>
    /// <returns>A task that resolves to the result of the remove operation on completion.</returns>
    public static async ValueTask<IOperationResult> RemoveAsync<T>(this IDatasyncTable<T> table, string id, DatasyncRemoveOptions options, CancellationToken cancellationToken = default) where T : IDatasyncEntity
    {
        var results = await table.RemoveRangeAsync(new string[] { id }, options, cancellationToken).ConfigureAwait(false);
        return results.Single();
    }

    /// <summary>
    /// Removes an entity from the store; the entity is identified by its ID.  The default remove options are used.
    /// </summary>
    /// <param name="entityIds">The ID of the entity to be removed.</param>
    /// <param name="options">The options for the remove operation.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe.</param>
    /// <returns>A task that resolves to the result of the remove operation on completion.</returns>
    public static ValueTask<IOperationResult> RemoveAsync<T>(this IDatasyncTable<T> table, string id, CancellationToken cancellationToken = default) where T : IDatasyncEntity
        => table.RemoveAsync(id, new DatasyncRemoveOptions(), cancellationToken);

    /// <summary>
    /// Removes an entity from the store.
    /// </summary>
    /// <param name="entity">The entity to be removed.</param>
    /// <param name="options">The options for the remove operation.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe.</param>
    /// <returns>A task that resolves to the result of the remove operation on completion.</returns>
    public static ValueTask<IOperationResult> RemoveAsync<T>(this IDatasyncTable<T> table, T entity, DatasyncRemoveOptions options, CancellationToken cancellationToken = default) where T : IDatasyncEntity
        => table.RemoveAsync(entity.Id, options, cancellationToken);

    /// <summary>
    /// Removes an entity from the store.  The default remove options are used.
    /// </summary>
    /// <param name="entity">The entity to be removed.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe.</param>
    /// <returns>A task that resolves to the result of the remove operation on completion.</returns>
    public static ValueTask<IOperationResult> RemoveAsync<T>(this IDatasyncTable<T> table, T entity, CancellationToken cancellationToken = default) where T : IDatasyncEntity
        => table.RemoveAsync(entity, new DatasyncRemoveOptions(), cancellationToken);

    /// <summary>
    /// Removes a set of entities from the store; each entity is identified by its ID.  The default remove options are used.
    /// </summary>
    /// <param name="entityIds">The set of entity IDs to be removed.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe.</param>
    /// <returns>A task that resolves to the result of the remove operation on completion.</returns>
    public static ValueTask<IEnumerable<IOperationResult>> RemoveRangeAsync<T>(this IDatasyncTable<T> table, IEnumerable<string> entityIds, CancellationToken cancellationToken = default) where T : IDatasyncEntity
        => table.RemoveRangeAsync(entityIds, new DatasyncRemoveOptions(), cancellationToken);

    /// <summary>
    /// Removes a set of entities from the store.  The default remove options are used.
    /// </summary>
    /// <param name="entities">The set of entities to be removed.</param>
    /// <param name="options">The options to use for the remove operation.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe.</param>
    /// <returns>A task that resolves to the result of the remove operation on completion.</returns>
    public static ValueTask<IEnumerable<IOperationResult>> RemoveRangeAsync<T>(this IDatasyncTable<T> table, IEnumerable<T> entities, DatasyncRemoveOptions options, CancellationToken cancellationToken = default) where T : IDatasyncEntity
        => table.RemoveRangeAsync(entities.Select(x => x.Id), options, cancellationToken);

    /// <summary>
    /// Removes a set of entities from the store.  The default remove options are used.
    /// </summary>
    /// <param name="entities">The set of entities to be removed.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe.</param>
    /// <returns>A task that resolves to the result of the remove operation on completion.</returns>
    public static ValueTask<IEnumerable<IOperationResult>> RemoveRangeAsync<T>(this IDatasyncTable<T> table, IEnumerable<T> entities, CancellationToken cancellationToken = default) where T : IDatasyncEntity
        => table.RemoveRangeAsync(entities, new DatasyncRemoveOptions(), cancellationToken);

    /// <summary>
    /// Replaces a single entity to the datasync table.
    /// </summary>
    /// <typeparam name="T">The type of entity.</typeparam>
    /// <param name="table">The table to modify.</param>
    /// <param name="entity">The entity to be replaced.</param>
    /// <param name="options">The options to use when replaciong the entity.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe.</param>
    /// <returns>A task that resolves to the operation result when complete.</returns>
    public static async ValueTask<IOperationResult<T>> ReplaceAsync<T>(this IDatasyncTable<T> table, T entity, DatasyncReplaceOptions options, CancellationToken cancellationToken = default) where T : IDatasyncEntity
    {
        var results = await table.ReplaceRangeAsync(new T[] { entity }, options, cancellationToken).ConfigureAwait(false);
        return results.Single();
    }

    /// <summary>
    /// Replaces a single entity to the datasync table.
    /// </summary>
    /// <typeparam name="T">The type of entity.</typeparam>
    /// <param name="table">The table to modify.</param>
    /// <param name="entity">The entity to be replaced.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe.</param>
    /// <returns>A task that resolves to the operation result when complete.</returns>
    public static ValueTask<IOperationResult<T>> ReplaceAsync<T>(this IDatasyncTable<T> table, T entity, CancellationToken cancellationToken = default) where T : IDatasyncEntity
        => table.ReplaceAsync(entity, new DatasyncReplaceOptions(), cancellationToken);

    /// <summary>
    /// Replaces a set of entities in the store (based on the ID of the entities), using the default replace options.
    /// </summary>
    /// <param name="entities">The entities to be replaced in the store.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe.</param>
    /// <returns>A task that resolves to the result of the replace operation.</returns>
    public static ValueTask<IEnumerable<IOperationResult<T>>> ReplaceRangeAsync<T>(this IDatasyncTable<T> table, IEnumerable<T> entities, CancellationToken cancellationToken = default) where T : IDatasyncEntity
        => table.ReplaceRangeAsync(entities, new DatasyncReplaceOptions(), cancellationToken);
}
