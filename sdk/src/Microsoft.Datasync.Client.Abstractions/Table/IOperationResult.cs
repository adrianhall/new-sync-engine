namespace Microsoft.Datasync.Client.Abstractions;

/// <summary>
/// The <see cref="IOperationResult"/> is returned when an untyped
/// operation completes, and reports the success or failure of the
/// operation.
/// </summary>
public interface IOperationResult
{
    /// <summary>
    /// Shows the success or failure of the request.
    /// </summary>
    /// <value><c>true</c> if the operation was successful, <c>false</c> otherwise.</value>
    bool IsSuccessful { get; }

    /// <summary>
    /// The error message for the error, if the operation was not successful.
    /// </summary>
    string? ErrorMessage { get; }

    /// <summary>
    /// The exception, when one is thrown during the operation.
    /// </summary>
    Exception? Exception { get; }
}

/// <summary>
/// The <see cref="IOperationResult{T}"/> is returned when a typed operation
/// completes and a response is received.
/// </summary>
/// <typeparam name="T">The expected type of the response.</typeparam>
public interface IOperationResult<T>
{
    T? Value { get; }
}
