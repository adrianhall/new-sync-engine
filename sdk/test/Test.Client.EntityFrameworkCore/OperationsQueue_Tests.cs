using Microsoft.Datasync.Client.Abstractions;
using Microsoft.Datasync.Client.EntityFrameworkCore;
using Microsoft.Datasync.Client.EntityFrameworkCore.Models;
using System.Diagnostics.CodeAnalysis;

namespace Test.Client.EntityFrameworkCore;

[ExcludeFromCodeCoverage]
public class OperationsQueue_Tests
{
    #region Setup
    private readonly TestDbContextFactory contextFactory;
    private readonly TestDbContext context;
    private readonly OperationsQueue queue;

    public OperationsQueue_Tests()
    {
        contextFactory = new TestDbContextFactory();
        context = contextFactory.CreateDbContext();
        queue = new OperationsQueue(context);
    }
    #endregion

    [Fact]
    public async Task InitialState_Empty()
    {
        Assert.Equal(0, queue.PendingOperations);

        var count = await queue.CountPendingOperationsAsync();
        Assert.Equal(0, count);

        var elems = await queue.GetPendingOperations().ToListAsync();
        Assert.Empty(elems);
    }

    [Fact]
    public async Task Returns_IDs_InOrder()
    {
        var p1 = new OperationsQueueEntry { Id = Guid.NewGuid(), CreatedAt = DateTimeOffset.UtcNow.AddHours(-2) };
        var p2 = new OperationsQueueEntry { Id = Guid.NewGuid(), CreatedAt = DateTimeOffset.UtcNow };
        var p3 = new OperationsQueueEntry { Id = Guid.NewGuid(), CreatedAt = DateTimeOffset.UtcNow.AddHours(-1) };
        context.OperationsQueue.AddRange(p1, p2, p3);
        await context.SaveChangesAsync();

        var items = await queue.GetPendingOperations().ToListAsync();
        Assert.Equal(p1.Id, items[0].Id);
        Assert.Equal(p3.Id, items[1].Id);
        Assert.Equal(p2.Id, items[2].Id);
    }

    [Fact]
    public async Task Updated_State()
    {
        var entry = new OperationsQueueEntry { Id = Guid.NewGuid(), CreatedAt = DateTimeOffset.UtcNow };
        context.OperationsQueue.Add(entry);
        await context.SaveChangesAsync();

        entry.OperationState = DatasyncOperationState.Attempted;
        var elem = await queue.UpdatePendingOperationAsync(entry);
        Assert.Equal(DatasyncOperationState.Attempted, elem.OperationState);
        Assert.Equal(DatasyncOperationState.Attempted, context.OperationsQueue.SingleOrDefault(x => x.OperationId == entry.OperationId)?.OperationState);
    }

    [Fact]
    public async Task Cannot_Update_ItemId()
    {
        var entry = new OperationsQueueEntry { Id = Guid.NewGuid(), CreatedAt = DateTimeOffset.UtcNow, ItemId = "123" };
        context.OperationsQueue.Add(entry);
        await context.SaveChangesAsync();

        var update = new OperationsQueueEntry { Id = entry.Id, CreatedAt = DateTimeOffset.UtcNow, ItemId = "125" };
        var elem = await queue.UpdatePendingOperationAsync(entry);
        Assert.Equal("123", elem.ItemId);
        Assert.Equal("123", context.OperationsQueue.SingleOrDefault(x => x.OperationId == entry.OperationId)?.ItemId);
    }

    [Fact]
    public async Task Throws_OnInvalidOperationId()
    {
        var entry = new OperationsQueueEntry { Id = Guid.NewGuid(), CreatedAt = DateTimeOffset.UtcNow, ItemId = "123" };
        context.OperationsQueue.Add(entry);
        await context.SaveChangesAsync();

        var p3 = new OperationsQueueEntry { Id = Guid.NewGuid(), CreatedAt = DateTimeOffset.UtcNow.AddHours(-1) };
        var t = await Assert.ThrowsAsync<OperationNotFoundException>(async () => await queue.UpdatePendingOperationAsync(p3));
        Assert.Equal(p3.Id, t.OperationId);
    }
}
