using Microsoft.EntityFrameworkCore;
using PokemonTools.Web.Domain.Abilities;
using PokemonTools.Web.Domain.Species;
using PokemonTools.Web.Domain.Statistics;
using PokemonTools.Web.Domain.Types;
using PokemonTools.Web.Infrastructure.Abilities;
using PokemonTools.Web.Infrastructure.Species;

namespace PokemonTools.Web.Infrastructure.Tests.Species;

public class SpeciesRepository_UpsertRangeAsyncTests(PostgreSqlFixture fixture) : IClassFixture<PostgreSqlFixture>
{
    [Fact]
    public async Task 新規種族_DBに挿入される()
    {
        // Arrange
        var ct = TestContext.Current.CancellationToken;
        await InsertAbilitiesAsync(ct, 65, 34);
        await using var context = fixture.CreateContext();
        var repository = new SpeciesRepository(context);
        var species = new List<PokemonSpecies>
        {
            new(
                id: new SpeciesId(1),
                name: "フシギダネ",
                type1Id: PokemonType.Grass.Id,
                type2Id: PokemonType.Poison.Id,
                ability1Id: new AbilityId(65),
                ability2Id: null,
                hiddenAbilityId: new AbilityId(34),
                baseStats: new BaseStats(45, 49, 49, 65, 65, 45),
                weight: new Weight(69)
            ),
        };

        // Act
        await repository.UpsertRangeAsync(species, ct);

        // Assert
        await using var verifyContext = fixture.CreateContext();
        var entity = await verifyContext.Species.SingleAsync(x => x.SpeciesId == 1, ct);
        Assert.Equal("フシギダネ", entity.SpeciesName);
        Assert.Equal(PokemonType.Grass.Id.Value, entity.Type1Id);
        Assert.Equal(PokemonType.Poison.Id.Value, entity.Type2Id);
        Assert.Equal(65, entity.Ability1Id);
        Assert.Null(entity.Ability2Id);
        Assert.Equal(34, entity.HiddenAbilityId);
        Assert.Equal(45, entity.BaseStatHp);
        Assert.Equal(49, entity.BaseStatAttack);
        Assert.Equal(49, entity.BaseStatDefense);
        Assert.Equal(65, entity.BaseStatSpecialAttack);
        Assert.Equal(65, entity.BaseStatSpecialDefense);
        Assert.Equal(45, entity.BaseStatSpeed);
        Assert.Equal(69, entity.Weight);
    }

    [Fact]
    public async Task 既存種族を更新_各プロパティが上書きされる()
    {
        // Arrange
        var ct = TestContext.Current.CancellationToken;
        await InsertAbilitiesAsync(ct, 500, 501);
        await using var setupContext = fixture.CreateContext();
        var setupRepo = new SpeciesRepository(setupContext);
        await setupRepo.UpsertRangeAsync(
        [
            new PokemonSpecies(
                id: new SpeciesId(500),
                name: "旧名",
                type1Id: PokemonType.Normal.Id,
                type2Id: null,
                ability1Id: new AbilityId(500),
                ability2Id: null,
                hiddenAbilityId: null,
                baseStats: new BaseStats(50, 50, 50, 50, 50, 50),
                weight: new Weight(100)
            ),
        ], ct);

        await using var context = fixture.CreateContext();
        var repository = new SpeciesRepository(context);

        // Act
        await repository.UpsertRangeAsync(
        [
            new PokemonSpecies(
                id: new SpeciesId(500),
                name: "新名",
                type1Id: PokemonType.Fire.Id,
                type2Id: PokemonType.Flying.Id,
                ability1Id: new AbilityId(500),
                ability2Id: new AbilityId(501),
                hiddenAbilityId: null,
                baseStats: new BaseStats(78, 84, 78, 109, 85, 100),
                weight: new Weight(905)
            ),
        ], ct);

        // Assert
        await using var verifyContext = fixture.CreateContext();
        var entity = await verifyContext.Species.SingleAsync(x => x.SpeciesId == 500, ct);
        Assert.Equal("新名", entity.SpeciesName);
        Assert.Equal(PokemonType.Fire.Id.Value, entity.Type1Id);
        Assert.Equal(PokemonType.Flying.Id.Value, entity.Type2Id);
        Assert.Equal(501, entity.Ability2Id);
        Assert.Equal(78, entity.BaseStatHp);
        Assert.Equal(905, entity.Weight);
    }

    [Fact]
    public async Task 空リスト_例外なく完了する()
    {
        // Arrange
        var ct = TestContext.Current.CancellationToken;
        await using var context = fixture.CreateContext();
        var repository = new SpeciesRepository(context);

        // Act
        await repository.UpsertRangeAsync([], ct);

        // Assert（例外が発生しなければ成功）
    }

    private async Task InsertAbilitiesAsync(CancellationToken ct, params int[] ids)
    {
        await using var context = fixture.CreateContext();
        var repository = new AbilityRepository(context);
        var abilities = ids.Select(x => new Ability(new AbilityId(x), $"特性{x}")).ToList();
        await repository.UpsertRangeAsync(abilities, ct);
    }
}
