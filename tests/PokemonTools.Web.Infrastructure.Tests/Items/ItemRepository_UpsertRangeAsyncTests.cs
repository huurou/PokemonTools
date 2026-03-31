using Microsoft.EntityFrameworkCore;
using PokemonTools.Web.Domain.Items;
using PokemonTools.Web.Infrastructure.Items;

namespace PokemonTools.Web.Infrastructure.Tests.Items;

public class ItemRepository_UpsertRangeAsyncTests(PostgreSqlFixture fixture) : IClassFixture<PostgreSqlFixture>
{
    [Fact]
    public async Task 新規道具_DBに挿入される()
    {
        // Arrange
        var ct = TestContext.Current.CancellationToken;
        await using var context = fixture.CreateContext();
        var repository = new ItemRepository(context);
        var items = new List<Item>
        {
            new(new ItemId(1), "マスターボール", null),
            new(new ItemId(233), "こだわりハチマキ", 10),
        };

        // Act
        await repository.UpsertRangeAsync(items, ct);

        // Assert
        await using var verifyContext = fixture.CreateContext();
        var stored = await verifyContext.Items
            .Where(x => x.ItemId == 1 || x.ItemId == 233)
            .OrderBy(x => x.ItemId)
            .ToListAsync(ct);
        Assert.Equal(2, stored.Count);
        Assert.Equal("マスターボール", stored[0].ItemName);
        Assert.Null(stored[0].FlingPower);
        Assert.Equal("こだわりハチマキ", stored[1].ItemName);
        Assert.Equal(10, stored[1].FlingPower);
    }

    [Fact]
    public async Task 既存道具を更新_名前とFlingPowerが上書きされる()
    {
        // Arrange
        var ct = TestContext.Current.CancellationToken;
        await using var setupContext = fixture.CreateContext();
        var setupRepo = new ItemRepository(setupContext);
        await setupRepo.UpsertRangeAsync([new Item(new ItemId(300), "旧名", 50)], ct);

        await using var context = fixture.CreateContext();
        var repository = new ItemRepository(context);

        // Act
        await repository.UpsertRangeAsync([new Item(new ItemId(300), "新名", null)], ct);

        // Assert
        await using var verifyContext = fixture.CreateContext();
        var entity = await verifyContext.Items.SingleAsync(x => x.ItemId == 300, ct);
        Assert.Equal("新名", entity.ItemName);
        Assert.Null(entity.FlingPower);
    }

    [Fact]
    public async Task 空リスト_例外なく完了する()
    {
        // Arrange
        var ct = TestContext.Current.CancellationToken;
        await using var context = fixture.CreateContext();
        var repository = new ItemRepository(context);

        // Act
        await repository.UpsertRangeAsync([], ct);

        // Assert（例外が発生しなければ成功）
    }
}
