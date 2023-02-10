using System.ComponentModel.DataAnnotations;

namespace Microsoft.Datasync.Client.EntityFrameworkCore.Models;

/// <summary>
/// The data storage model for a delta token.
/// </summary>
public class DeltaToken
{
    /// <summary>
    /// The token ID for the delta-token.
    /// </summary>
    [Key]
    public string TokenId { get; set; } = string.Empty;

    /// <summary>
    /// The timestamp, in "ticks".
    /// </summary>
    /// <remarks>
    /// Some databases (e.g. SQLite) do not store DateTimeOffset correctly, so we auto-convert
    /// the required DateTimeOffset into UTC ticks before storage.
    /// </remarks>
    public long Timestamp { get; set; } = 0L;
}
