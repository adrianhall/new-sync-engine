namespace Microsoft.Datasync.Client.Abstractions;

/// <summary>
/// The types of operation that will fit into the <see cref="DatasyncOperation"/>.
/// </summary>
public enum DatasyncOperationKind
{
    Unknown,
    Add,
    Delete,
    Replace
}

/// <summary>
/// The current state of the operation.
/// </summary>
public enum DatasyncOperationState
{
    Pending,
    Attempted,
    Completed,
    Failed
}

/// <summary>
/// A description of a single operation in the operations queue.
/// </summary>
public interface IDatasyncOperation
{
    /// <summary>
    /// The globally unique ID for the operation.
    /// </summary>
    Guid Id { get; set; }

    /// <summary>
    /// The date and time that the record was created.
    /// </summary>
    DateTimeOffset CreatedAt { get; set; }

    /// <summary>
    /// The type of operation.
    /// </summary>
    DatasyncOperationKind OperationKind { get; set; }

    /// <summary>
    /// The current state of this operation.
    /// </summary>
    DatasyncOperationState OperationState { get; set; }

    /// <summary>
    /// The ID of the item that the operation is for.
    /// </summary>
    string ItemId { get; set; }

    /// <summary>
    /// The name of the table where the item is located.
    /// </summary>
    string TableName { get; set; }

    /// <summary>
    /// The serialized content of the original entity (if replace/delete)
    /// </summary>
    string SerializedItem { get; set; }

    /// <summary>
    /// The version ID of the operation.
    /// </summary>
    long Version { get; set; }
}
