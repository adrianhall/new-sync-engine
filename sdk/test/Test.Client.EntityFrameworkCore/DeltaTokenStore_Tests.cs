using Microsoft.Datasync.Client.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace Test.Client.EntityFrameworkCore;

[ExcludeFromCodeCoverage]
public class DeltaTokenStore_Tests
{
    #region Setup
    private readonly TestDbContextFactory contextFactory;
    private readonly TestDbContext context;
    private readonly DeltaTokenStore store;

    public DeltaTokenStore_Tests()
    {
        contextFactory = new TestDbContextFactory();
        context = contextFactory.CreateDbContext();
        store = new DeltaTokenStore(context);
    }
    #endregion

    [Theory]
    [InlineData("2022-12-25T03:00:00.000Z")]
    [InlineData("1970-03-29T09:15:30.123Z")]
    [InlineData("2022-12-25T03:00:00.000+08:00")]
    [InlineData("1970-03-29T09:15:30.123+08:00")]
    [InlineData("2022-12-25T03:00:00.000-04:00")]
    [InlineData("1970-03-29T09:15:30.123-04:00")]
    public async Task Can_RoundTrip(string value)
    {
        var expected = DateTimeOffset.Parse(value);

        await store.SetDeltaTokenAsync("roundtrip", expected);
        var actual = await store.GetDeltaTokenAsync("roundtrip");
        Assert.Equal(expected, actual);

        var entity = await context.DeltaTokens.SingleOrDefaultAsync(x => x.TokenId == "roundtrip");
        Assert.NotNull(entity);
        Assert.Equal(expected.ToFileTime(), entity!.Timestamp);
    }

    [Theory]
    [InlineData("2022-12-25T03:00:00.000Z")]
    [InlineData("1970-03-29T09:15:30.123Z")]
    [InlineData("2022-12-25T03:00:00.000+08:00")]
    [InlineData("1970-03-29T09:15:30.123+08:00")]
    [InlineData("2022-12-25T03:00:00.000-04:00")]
    [InlineData("1970-03-29T09:15:30.123-04:00")]
    public async Task Can_BeReset(string value)
    {
        var expected = DateTimeOffset.Parse(value);

        await store.SetDeltaTokenAsync("roundtrip", DateTimeOffset.UtcNow);
        await store.SetDeltaTokenAsync("roundtrip", expected);

        var actual = await store.GetDeltaTokenAsync("roundtrip");
        Assert.Equal(expected, actual);

        var entity = await context.DeltaTokens.SingleOrDefaultAsync(x => x.TokenId == "roundtrip");
        Assert.NotNull(entity);
        Assert.Equal(expected.ToFileTime(), entity!.Timestamp);
    }

    [Theory]
    [InlineData("2022-12-25T03:00:00.000Z")]
    [InlineData("1970-03-29T09:15:30.123Z")]
    [InlineData("2022-12-25T03:00:00.000+08:00")]
    [InlineData("1970-03-29T09:15:30.123+08:00")]
    [InlineData("2022-12-25T03:00:00.000-04:00")]
    [InlineData("1970-03-29T09:15:30.123-04:00")]
    public async Task Can_BeCleared(string value)
    {
        var expected = DateTimeOffset.Parse(value);

        await store.SetDeltaTokenAsync("roundtrip", expected);
        await store.ClearDeltaTokenAsync("roundtrip");

        var actual = await store.GetDeltaTokenAsync("roundtrip");
        Assert.Null(actual);

        var entity = await context.DeltaTokens.SingleOrDefaultAsync(x => x.TokenId == "roundtrip");
        Assert.Null(entity);
    }

    [Fact]
    public async Task Can_IterateAndClear()
    {
        const int count = 50;
        DateTimeOffset timestamp = DateTimeOffset.Parse("1970-03-29T09:15:30.123Z");
        for (int i = 0; i < count; i++)
        {
            await store.SetDeltaTokenAsync($"token-{i}", timestamp);
        }

        var tokenIds = (await store.GetDeltaTokenIdsAsync()).ToList();
        Assert.Equal(count, tokenIds.Count);
        Assert.Equal(count, context.DeltaTokens.Count());

        foreach (var tokenId in tokenIds)
        {
            Assert.StartsWith("token-", tokenId);
        }

        await store.ClearDeltaTokenStoreAsync();
        tokenIds = (await store.GetDeltaTokenIdsAsync()).ToList();
        Assert.Empty(tokenIds);

        Assert.False(context.DeltaTokens.Any());
    }
}