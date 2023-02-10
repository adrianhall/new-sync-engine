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
    public void InitialState_Empty()
    {
        Assert.False(queue.AsQueryable().Any());
    }

    [Fact]
    public async Task Returns_IDs_InOrder()
    {
        var p1 = new OperationsQueueEntry { Id = Guid.NewGuid(), CreatedAt = DateTimeOffset.UtcNow.AddHours(-2) };
        var p2 = new OperationsQueueEntry { Id = Guid.NewGuid(), CreatedAt = DateTimeOffset.UtcNow };
        var p3 = new OperationsQueueEntry { Id = Guid.NewGuid(), CreatedAt = DateTimeOffset.UtcNow.AddHours(-1) };
        context.OperationsQueue.AddRange(p1, p2, p3);
        await context.SaveChangesAsync();

        var items = queue.AsQueryable().ToList();
        Assert.Equal(p1.Id, items[0].Id);
        Assert.Equal(p3.Id, items[1].Id);
        Assert.Equal(p2.Id, items[2].Id);
    }

    [Fact]
    public async Task Returns_WithNextInOrder()
    {
        var p1 = new OperationsQueueEntry { Id = Guid.NewGuid(), CreatedAt = DateTimeOffset.UtcNow.AddHours(-2) };
        var p2 = new OperationsQueueEntry { Id = Guid.NewGuid(), CreatedAt = DateTimeOffset.UtcNow };
        var p3 = new OperationsQueueEntry { Id = Guid.NewGuid(), CreatedAt = DateTimeOffset.UtcNow.AddHours(-1) };
        context.OperationsQueue.AddRange(p1, p2, p3);
        await context.SaveChangesAsync();

        var op = await queue.GetNextOperationAsync();
        Assert.NotNull(op);
        Assert.Equal(p1.Id, op!.Id);

        op = await queue.GetNextOperationAsync();
        Assert.NotNull(op);
        Assert.Equal(p3.Id, op!.Id);

        op = await queue.GetNextOperationAsync();
        Assert.NotNull(op);
        Assert.Equal(p2.Id, op!.Id);

        op = await queue.GetNextOperationAsync();
        Assert.Null(op);
    }
}
