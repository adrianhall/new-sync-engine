using Microsoft.Datasync.Client.Abstractions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Microsoft.Datasync.Client.EntityFrameworkCore.Models;

/// <summary>
/// The operations queue entity, stored in the database.
/// </summary>
public class OperationsQueueEntry : IDatasyncOperation
{
    /// <summary>
    /// The globally unique ID for the operation.
    /// </summary>
    [Key]
    public string OperationId { get; set; } = Guid.Empty.ToString("N");

    /// <summary>
    /// The exposed globally unique ID for the operation.
    /// </summary>
    [NotMapped]
    public Guid Id
    {
        get => Guid.Parse(OperationId);
        set => OperationId = value.ToString("N");
    }

    /// <summary>
    /// The date and time that the record was created.
    /// </summary>
    public long Sequence { get; set; } = 0L;

    /// <summary>
    /// The date/timestamp that the operation was created.
    /// </summary>
    [NotMapped]
    public DateTimeOffset CreatedAt
    {
        get => DateTimeOffset.FromFileTime(Sequence);
        set => Sequence = value.ToFileTime();
    }

    /// <summary>
    /// The type of operation.
    /// </summary>
    public DatasyncOperationKind OperationKind { get; set; } = DatasyncOperationKind.Unknown;

    /// <summary>
    /// The current state of this operation.
    /// </summary>
    public DatasyncOperationState OperationState { get; set; } = DatasyncOperationState.Pending;

    /// <summary>
    /// The ID of the item that the operation is for.
    /// </summary>
    public string ItemId { get; set; } = string.Empty;

    /// <summary>
    /// The name of the table where the item is located.
    /// </summary>
    public string TableName { get; set; } = string.Empty;

    /// <summary>
    /// The serialized content of the original entity (if replace/delete)
    /// </summary>
    public string SerializedItem { get; set; } = string.Empty;

    /// <summary>
    /// The version ID of the operation.
    /// </summary>
    public long Version { get; set; } = 0;
}
