using Microsoft.Data.Sqlite;
using Microsoft.Datasync.Client.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace Test.Client.EntityFrameworkCore.TestHelpers;

[ExcludeFromCodeCoverage]
class TestDbContextFactory : IDbContextFactory<TestDbContext>, IDisposable
{
    private readonly SqliteConnection connection;
    private readonly DbContextOptions<TestDbContext> options;

    public TestDbContextFactory()
    {
        connection = new SqliteConnection("Filename=:memory:");
        connection.Open();

        options = new DbContextOptionsBuilder<TestDbContext>().UseSqlite(connection).Options;
    }

    public TestDbContext CreateDbContext()
    {
        var context = new TestDbContext(options);
        context.Database.EnsureCreated();
        return context;
    }

    public void Dispose()
    {
        connection.Dispose();
    }
}

[ExcludeFromCodeCoverage]
class TestDbContext : DatasyncDbContext
{
    public TestDbContext(DbContextOptions<TestDbContext> options) : base(options)
    {
    }
}
