namespace Microsoft.Datasync.Client.Abstractions;

/// <summary>
/// The base entity definition for all entities within a datasync service.
/// </summary>
public interface IDatasyncEntity
{
    string Id { get; set; }
}
