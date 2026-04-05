using PokemonTools.Web.Domain.Individuals;
using PokemonTools.Web.Domain.Statistics;
using PokemonTools.Web.Domain.Types;
using PokemonTools.Web.Infrastructure.Individuals;
using static PokemonTools.Web.Infrastructure.Tests.Individuals.IndividualRepositoryTestHelper;

namespace PokemonTools.Web.Infrastructure.Tests.Individuals;

public class OwnedIndividualQueryService_GetDetailAsyncTests(PostgreSqlFixture fixture) : IClassFixture<PostgreSqlFixture>
{
    [Fact]
    public async Task 存在する手持ち個体_詳細DTOが返される()
    {
        // Arrange
        var ct = TestContext.Current.CancellationToken;
        await using var seedContext = fixture.CreateContext();
        await SeedMasterDataAsync(seedContext, ct);
        await CleanupIndividualsAsync(seedContext, ct);
        var seedRepo = new IndividualRepository(seedContext);
        await seedRepo.AddAsync(CreateDefaultIndividual(
            id: "ind_qs_detail_001",
            name: "ガブ詳細テスト",
            move2Id: MOVE_2_ID,
            move3Id: MOVE_3_ID,
            move4Id: MOVE_4_ID,
            heldItemId: ITEM_ID,
            memo: "詳細テスト用"
        ), ct);

        // Act
        await using var context = fixture.CreateContext();
        var service = new OwnedIndividualQueryService(context);
        var result = await service.GetDetailAsync("ind_qs_detail_001", ct);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("ind_qs_detail_001", result.Id);
        Assert.Equal("ガブ詳細テスト", result.Name);
        Assert.Equal("ガブリアス", result.SpeciesName);
        Assert.Equal(SPECIES_ID.Value, result.SpeciesId);
        Assert.Equal(StatAlignment.Adamant.Id.Value, result.StatAlignmentId);
        Assert.Equal(ABILITY_1_ID.Value, result.AbilityId);
        Assert.Equal(0u, result.StatPointHp);
        Assert.Equal(32u, result.StatPointAttack);
        Assert.Equal(0u, result.StatPointDefense);
        Assert.Equal(0u, result.StatPointSpecialAttack);
        Assert.Equal(0u, result.StatPointSpecialDefense);
        Assert.Equal(32u, result.StatPointSpeed);
        Assert.Equal(MOVE_1_ID.Value, result.Move1Id);
        Assert.Equal(MOVE_2_ID.Value, result.Move2Id);
        Assert.Equal(MOVE_3_ID.Value, result.Move3Id);
        Assert.Equal(MOVE_4_ID.Value, result.Move4Id);
        Assert.Equal(ITEM_ID.Value, result.HeldItemId);
        Assert.Equal(PokemonType.Dragon.Id.Value, result.TeraTypeId);
        Assert.Equal("詳細テスト用", result.Memo);
    }

    [Fact]
    public async Task 存在しないID_nullが返される()
    {
        // Arrange
        var ct = TestContext.Current.CancellationToken;
        await using var seedContext = fixture.CreateContext();
        await CleanupIndividualsAsync(seedContext, ct);

        // Act
        await using var context = fixture.CreateContext();
        var service = new OwnedIndividualQueryService(context);
        var result = await service.GetDetailAsync("ind_qs_detail_nonexistent", ct);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task プリセット個体のID_nullが返される()
    {
        // Arrange
        var ct = TestContext.Current.CancellationToken;
        await using var seedContext = fixture.CreateContext();
        await SeedMasterDataAsync(seedContext, ct);
        await CleanupIndividualsAsync(seedContext, ct);
        var seedRepo = new IndividualRepository(seedContext);
        await seedRepo.AddAsync(CreateDefaultIndividual(
            id: "ind_qs_detail_preset",
            categoryId: IndividualCategory.DamageCalculationPreset.Id
        ), ct);

        // Act
        await using var context = fixture.CreateContext();
        var service = new OwnedIndividualQueryService(context);
        var result = await service.GetDetailAsync("ind_qs_detail_preset", ct);

        // Assert
        Assert.Null(result);
    }
}
