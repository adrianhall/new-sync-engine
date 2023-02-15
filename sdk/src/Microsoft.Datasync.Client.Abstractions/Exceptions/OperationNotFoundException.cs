using System.Runtime.Serialization;

namespace Microsoft.Datasync.Client.Abstractions;

/// <summary>
/// Exception that is thrown when a specific operation is requested and cannot be found.
/// </summary>
public class OperationNotFoundException : OfflineStoreException
{
    public OperationNotFoundException()
    {
    }

    public OperationNotFoundException(string? message) : base(message)
    {
    }

    public OperationNotFoundException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected OperationNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    /// <summary>
    /// The requested operation ID.
    /// </summary>
    /// <value>The Guid of the operation, or <c>Guid.Empty</c> for an invalid GUID.</value>
    public Guid OperationId { get; set; } = Guid.Empty;
}