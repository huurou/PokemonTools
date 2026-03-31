using Microsoft.EntityFrameworkCore;
using PokemonTools.Web.Domain.Abilities;
using PokemonTools.Web.Infrastructure.Abilities;

namespace PokemonTools.Web.Infrastructure.Tests.Abilities;

public class AbilityRepository_UpsertRangeAsyncTests(PostgreSqlFixture fixture) : IClassFixture<PostgreSqlFixture>
{
    [Fact]
    public async Task 新規特性_DBに挿入される()
    {
        // Arrange
        var ct = TestContext.Current.CancellationToken;
        await using var context = fixture.CreateContext();
        var repository = new AbilityRepository(context);
        var abilities = new List<Ability>
        {
            new(new AbilityId(65), "しんりょく"),
            new(new AbilityId(66), "もうか"),
        };

        // Act
        await repository.UpsertRangeAsync(abilities, ct);

        // Assert
        await using var verifyContext = fixture.CreateContext();
        var stored = await verifyContext.Abilities
            .Where(x => x.AbilityId == 65 || x.AbilityId == 66)
            .OrderBy(x => x.AbilityId)
            .ToListAsync(ct);
        Assert.Equal(2, stored.Count);
        Assert.Equal("しんりょく", stored[0].AbilityName);
        Assert.Equal("もうか", stored[1].AbilityName);
    }

    [Fact]
    public async Task 既存特性を更新_名前が上書きされる()
    {
        // Arrange
        var ct = TestContext.Current.CancellationToken;
        await using var setupContext = fixture.CreateContext();
        var setupRepo = new AbilityRepository(setupContext);
        await setupRepo.UpsertRangeAsync([new Ability(new AbilityId(100), "旧名")], ct);

        await using var context = fixture.CreateContext();
        var repository = new AbilityRepository(context);

        // Act
        await repository.UpsertRangeAsync([new Ability(new AbilityId(100), "新名")], ct);

        // Assert
        await using var verifyContext = fixture.CreateContext();
        var entity = await verifyContext.Abilities.SingleAsync(x => x.AbilityId == 100, ct);
        Assert.Equal("新名", entity.AbilityName);
    }

    [Fact]
    public async Task 空リスト_例外なく完了する()
    {
        // Arrange
        var ct = TestContext.Current.CancellationToken;
        await using var context = fixture.CreateContext();
        var repository = new AbilityRepository(context);

        // Act
        await repository.UpsertRangeAsync([], ct);

        // Assert（例外が発生しなければ成功）
    }

    [Fact]
    public async Task 重複IDを含むリスト_最後の値が採用される()
    {
        // Arrange
        var ct = TestContext.Current.CancellationToken;
        await using var context = fixture.CreateContext();
        var repository = new AbilityRepository(context);
        var abilities = new List<Ability>
        {
            new(new AbilityId(200), "最初"),
            new(new AbilityId(200), "最後"),
        };

        // Act
        await repository.UpsertRangeAsync(abilities, ct);

        // Assert
        await using var verifyContext = fixture.CreateContext();
        var entity = await verifyContext.Abilities.SingleAsync(x => x.AbilityId == 200, ct);
        Assert.Equal("最後", entity.AbilityName);
    }
}
