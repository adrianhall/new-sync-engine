using Microsoft.Datasync.Client.Abstractions;
using Microsoft.Datasync.Client.EntityFrameworkCore.Models;
using Microsoft.EntityFrameworkCore;

namespace Microsoft.Datasync.Client.EntityFrameworkCore;

/// <summary>
/// Entity Framework Core edition of the <see cref="IDeltaTokenStore"/>.
/// </summary>
public class DeltaTokenStore : IDeltaTokenStore
{
    private readonly DatasyncDbContext context;

    /// <summary>
    /// Creates a new <see cref="DeltaTokenStore"/> based on a database context.
    /// </summary>
    /// <param name="context">The <see cref="DatasyncDbContext"/> that marshals the delta-token table.</param>
    public DeltaTokenStore(DatasyncDbContext context)
    {
        this.context = context;
    }

    /// <summary>
    /// Clears a delta-token from the store, based on its ID.
    /// </summary>
    /// <param name="tokenId">The ID of the delta-token.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe.</param>
    /// <returns>A task that resolves when the operation is complete.</returns>
    public async ValueTask ClearDeltaTokenAsync(string tokenId, CancellationToken cancellationToken = default)
    {
        var entity = await context.DeltaTokens.SingleOrDefaultAsync(x => x.TokenId == tokenId, cancellationToken).ConfigureAwait(false);
        if (entity != null)
        {
            context.DeltaTokens.Remove(entity);
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }
    }

    /// <summary>
    /// Clears the delta token store.
    /// </summary>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe.</param>
    /// <returns>A task that resolves when the operation is complete.</returns>
    public async ValueTask ClearDeltaTokenStoreAsync(CancellationToken cancellationToken = default)
    {
        if (context.DeltaTokens.Any())
        {
            var entities = await context.DeltaTokens.ToListAsync(cancellationToken).ConfigureAwait(false);
            context.DeltaTokens.RemoveRange(entities);
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }
    }

    /// <summary>
    /// Retrieves a delta token for a specific token ID.
    /// </summary>
    /// <remarks>
    /// The delta-token is null if the token does not exist.
    /// </remarks>
    /// <param name="tokenId">The ID of the delta-token.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe.</param>
    /// <returns>A task that resolves to the value of the delta-token when the operation is complete.</returns>
    public async ValueTask<DateTimeOffset?> GetDeltaTokenAsync(string tokenId, CancellationToken cancellationToken = default)
    {
        var entity = await context.DeltaTokens.SingleOrDefaultAsync(x => x.TokenId == tokenId, cancellationToken).ConfigureAwait(false);
        return (entity != null) ? DateTimeOffset.FromFileTime(entity.Timestamp) : null;
    }

    /// <summary>
    /// Retrieves a list of all the delta tokens available in the store.
    /// </summary>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe.</param>
    /// <returns>A task that resolves to the list of delta-tokens in the delta-token store when complete.</returns>
    public async ValueTask<IEnumerable<string>> GetDeltaTokenIdsAsync(CancellationToken cancellationToken = default)
    {
        var entities = await context.DeltaTokens.ToListAsync(cancellationToken).ConfigureAwait(false);
        return entities.Select(x => x.TokenId);
    }

    /// <summary>
    /// Sets a delta-token  to the specific value, creating it if the token does not exist.
    /// </summary>
    /// <param name="tokenId">The ID of the delta-token to store.</param>
    /// <param name="timestamp">The timestamp of the delta-token.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe.</param>
    /// <returns>A task that resolves when the operation is complete.</returns>
    public async ValueTask SetDeltaTokenAsync(string tokenId, DateTimeOffset timestamp, CancellationToken cancellationToken = default)
    {
        var entity = await context.DeltaTokens.SingleOrDefaultAsync(x => x.TokenId == tokenId, cancellationToken).ConfigureAwait(false);
        if (entity == null)
        {
            var deltaToken = new DeltaToken { TokenId = tokenId, Timestamp = timestamp.ToFileTime() };
            context.DeltaTokens.Add(deltaToken);
        }
        else
        {
            entity.Timestamp = timestamp.ToFileTime();
            context.DeltaTokens.Update(entity);
        }
        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }
}
