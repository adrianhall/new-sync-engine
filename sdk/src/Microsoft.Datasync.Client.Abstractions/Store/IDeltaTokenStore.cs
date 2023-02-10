namespace Microsoft.Datasync.Client.Abstractions;

/// <summary>
/// The delta-token store holds the "last-updated" timestamp for a table/query combination.
/// </summary>
public interface IDeltaTokenStore
{
    /// <summary>
    /// Clears the delta token store.
    /// </summary>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe.</param>
    /// <returns>A task that resolves when the operation is complete.</returns>
    ValueTask ClearDeltaTokenStoreAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Clears a delta-token from the store, based on its ID.
    /// </summary>
    /// <param name="tokenId">The ID of the delta-token.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe.</param>
    /// <returns>A task that resolves when the operation is complete.</returns>
    ValueTask ClearDeltaTokenAsync(string tokenId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a delta token for a specific token ID.
    /// </summary>
    /// <remarks>
    /// The delta-token is null if the token does not exist.
    /// </remarks>
    /// <param name="tokenId">The ID of the delta-token.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe.</param>
    /// <returns>A task that resolves to the value of the delta-token when the operation is complete.</returns>
    ValueTask<DateTimeOffset?> GetDeltaTokenAsync(string tokenId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a list of all the delta tokens available in the store.
    /// </summary>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe.</param>
    /// <returns>A task that resolves to the list of delta-tokens in the delta-token store when complete.</returns>
    ValueTask<IEnumerable<string>> GetDeltaTokenIdsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Sets a delta-token  to the specific value, creating it if the token does not exist.
    /// </summary>
    /// <param name="tokenId">The ID of the delta-token to store.</param>
    /// <param name="timestamp">The timestamp of the delta-token.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe.</param>
    /// <returns>A task that resolves when the operation is complete.</returns>
    ValueTask SetDeltaTokenAsync(string tokenId, DateTimeOffset timestamp, CancellationToken cancellationToken = default);
}
