using System.Runtime.Serialization;

namespace Microsoft.Datasync.Client.Abstractions;

/// <summary>
/// A generic exception that can be generated within the offline store.
/// </summary>
public class OfflineStoreException : Exception
{
    public OfflineStoreException()
    {
    }

    public OfflineStoreException(string? message) : base(message)
    {
    }

    public OfflineStoreException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected OfflineStoreException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}