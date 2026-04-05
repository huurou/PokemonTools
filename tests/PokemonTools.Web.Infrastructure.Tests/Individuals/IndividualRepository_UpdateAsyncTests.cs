using Microsoft.EntityFrameworkCore;
using PokemonTools.Web.Domain.Individuals;
using PokemonTools.Web.Domain.Statistics;
using PokemonTools.Web.Domain.Types;
using PokemonTools.Web.Infrastructure.Individuals;
using static PokemonTools.Web.Infrastructure.Tests.Individuals.IndividualRepositoryTestHelper;

namespace PokemonTools.Web.Infrastructure.Tests.Individuals;

public class IndividualRepository_UpdateAsyncTests(PostgreSqlFixture fixture) : IClassFixture<PostgreSqlFixture>
{
    [Fact]
    public async Task 存在しないID_例外が発生する()
    {
        // Arrange
        var ct = TestContext.Current.CancellationToken;
        await using var seedContext = fixture.CreateContext();
        await SeedMasterDataAsync(seedContext, ct);

        await using var context = fixture.CreateContext();
        var repository = new IndividualRepository(context);
        var nonExistent = CreateDefaultIndividual(id: "ind_nonexistent_update");

        // Act
        var exception = await Record.ExceptionAsync(() =>
            repository.UpdateAsync(nonExistent, ct));

        // Assert
        Assert.IsType<InvalidOperationException>(exception);
    }

    [Fact]
    public async Task 既存個体のフィールドを更新_DBに反映される()
    {
        // Arrange
        var ct = TestContext.Current.CancellationToken;
        await using var seedContext = fixture.CreateContext();
        await SeedMasterDataAsync(seedContext, ct);
        var seedRepo = new IndividualRepository(seedContext);
        await seedRepo.AddAsync(CreateDefaultIndividual(id: "ind_update_001", name: "更新前"), ct);

        await using var context = fixture.CreateContext();
        var repository = new IndividualRepository(context);
        var updated = new Individual(
            new IndividualId("ind_update_001"),
            "更新後",
            SPECIES_ID,
            StatAlignment.Bold.Id,
            ABILITY_2_ID,
            new StatPoints(32, 0, 32, 0, 0, 0),
            MOVE_2_ID,
            MOVE_1_ID,
            null,
            null,
            null,
            PokemonType.Fire.Id,
            "更新メモ",
            IndividualCategory.OwnedIndividual.Id
        );

        // Act
        await repository.UpdateAsync(updated, ct);

        // Assert
        await using var verifyContext = fixture.CreateContext();
        var entity = await verifyContext.Individuals.SingleAsync(x => x.IndividualId == "ind_update_001", ct);
        Assert.Equal("更新後", entity.IndividualName);
        Assert.Equal(StatAlignment.Bold.Id.Value, entity.StatAlignmentId);
        Assert.Equal(ABILITY_2_ID.Value, entity.AbilityId);
        Assert.Equal(32, entity.StatPointHp);
        Assert.Equal(0, entity.StatPointAttack);
        Assert.Equal(32, entity.StatPointDefense);
        Assert.Equal(MOVE_2_ID.Value, entity.Move1Id);
        Assert.Equal(MOVE_1_ID.Value, entity.Move2Id);
        Assert.Null(entity.Move3Id);
        Assert.Null(entity.HeldItemId);
        Assert.Equal(PokemonType.Fire.Id.Value, entity.TeraTypeId);
        Assert.Equal("更新メモ", entity.Memo);
    }
}
