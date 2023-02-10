namespace Microsoft.Datasync.Client.Abstractions;

/// <summary>
/// The base class for operation options on an offline store.
/// </summary>
public abstract class OfflineOperationOptions
{
}

/// <summary>
/// The options for an offline Add operation.
/// </summary>
public class AddOperationOptions : OfflineOperationOptions
{
}

/// <summary>
/// The options for an offline Remove operation.
/// </summary>
public class RemoveOperationOptions : OfflineOperationOptions
{
}

/// <summary>
/// The options for an offline Replace operation.
/// </summary>
public class ReplaceOperationOptions : OfflineOperationOptions
{
}
