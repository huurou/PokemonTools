using Microsoft.EntityFrameworkCore;
using PokemonTools.Web.Domain.Individuals;
using PokemonTools.Web.Domain.Statistics;
using PokemonTools.Web.Domain.Types;
using PokemonTools.Web.Infrastructure.Individuals;
using static PokemonTools.Web.Infrastructure.Tests.Individuals.IndividualRepositoryTestHelper;

namespace PokemonTools.Web.Infrastructure.Tests.Individuals;

public class IndividualRepository_AddAsyncTests(PostgreSqlFixture fixture) : IClassFixture<PostgreSqlFixture>
{
    [Fact]
    public async Task 正常な個体_DBに挿入される()
    {
        // Arrange
        var ct = TestContext.Current.CancellationToken;
        await using var seedContext = fixture.CreateContext();
        await SeedMasterDataAsync(seedContext, ct);

        await using var context = fixture.CreateContext();
        var repository = new IndividualRepository(context);
        var individual = CreateDefaultIndividual(
            id: "ind_add_001",
            name: "ガブ1号",
            move2Id: MOVE_2_ID,
            heldItemId: ITEM_ID,
            memo: "テスト用"
        );

        // Act
        await repository.AddAsync(individual, ct);

        // Assert
        await using var verifyContext = fixture.CreateContext();
        var entity = await verifyContext.Individuals.SingleAsync(x => x.IndividualId == "ind_add_001", ct);
        Assert.Equal("ガブ1号", entity.IndividualName);
        Assert.Equal(SPECIES_ID.Value, entity.SpeciesId);
        Assert.Equal(StatAlignment.Adamant.Id.Value, entity.StatAlignmentId);
        Assert.Equal(ABILITY_1_ID.Value, entity.AbilityId);
        Assert.Equal(MOVE_1_ID.Value, entity.Move1Id);
        Assert.Equal(MOVE_2_ID.Value, entity.Move2Id);
        Assert.Null(entity.Move3Id);
        Assert.Null(entity.Move4Id);
        Assert.Equal(ITEM_ID.Value, entity.HeldItemId);
        Assert.Equal(PokemonType.Dragon.Id.Value, entity.TeraTypeId);
        Assert.Equal("テスト用", entity.Memo);
        Assert.Equal(IndividualCategory.OwnedIndividual.Id.Value, entity.CategoryId);
    }

    [Fact]
    public async Task StatPointsが正しく保存される()
    {
        // Arrange
        var ct = TestContext.Current.CancellationToken;
        await using var seedContext = fixture.CreateContext();
        await SeedMasterDataAsync(seedContext, ct);

        await using var context = fixture.CreateContext();
        var repository = new IndividualRepository(context);
        var individual = CreateDefaultIndividual(id: "ind_add_sp_001");

        // Act
        await repository.AddAsync(individual, ct);

        // Assert
        await using var verifyContext = fixture.CreateContext();
        var entity = await verifyContext.Individuals.SingleAsync(x => x.IndividualId == "ind_add_sp_001", ct);
        Assert.Equal(0, entity.StatPointHp);
        Assert.Equal(32, entity.StatPointAttack);
        Assert.Equal(0, entity.StatPointDefense);
        Assert.Equal(0, entity.StatPointSpecialAttack);
        Assert.Equal(0, entity.StatPointSpecialDefense);
        Assert.Equal(32, entity.StatPointSpeed);
    }
}
