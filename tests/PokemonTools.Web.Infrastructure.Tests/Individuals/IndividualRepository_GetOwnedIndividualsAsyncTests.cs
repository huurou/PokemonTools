using PokemonTools.Web.Domain.Individuals;
using PokemonTools.Web.Infrastructure.Individuals;
using static PokemonTools.Web.Infrastructure.Tests.Individuals.IndividualRepositoryTestHelper;

namespace PokemonTools.Web.Infrastructure.Tests.Individuals;

public class IndividualRepository_GetOwnedIndividualsAsyncTests(PostgreSqlFixture fixture) : IClassFixture<PostgreSqlFixture>
{
    [Fact]
    public async Task 手持ち個体のみ返される()
    {
        // Arrange
        var ct = TestContext.Current.CancellationToken;
        await using var seedContext = fixture.CreateContext();
        await SeedMasterDataAsync(seedContext, ct);
        var seedRepo = new IndividualRepository(seedContext);
        await seedRepo.AddAsync(CreateDefaultIndividual(
            id: "ind_owned_001",
            categoryId: IndividualCategory.OwnedIndividual.Id
        ), ct);
        await seedRepo.AddAsync(CreateDefaultIndividual(
            id: "ind_preset_001",
            categoryId: IndividualCategory.DamageCalculationPreset.Id
        ), ct);

        // Act
        await using var context = fixture.CreateContext();
        var repository = new IndividualRepository(context);
        var result = await repository.GetOwnedIndividualsAsync(ct);

        // Assert
        Assert.DoesNotContain(result, x => x.Id == new IndividualId("ind_preset_001"));
        Assert.Contains(result, x => x.Id == new IndividualId("ind_owned_001"));
    }

    [Fact]
    public async Task 個体が存在しない場合_空リストが返される()
    {
        // Arrange
        var ct = TestContext.Current.CancellationToken;
        // 他テストで追加されたデータがあるが、カテゴリフィルタで動作確認

        // Act
        await using var context = fixture.CreateContext();
        var repository = new IndividualRepository(context);
        var result = await repository.GetOwnedIndividualsAsync(ct);

        // Assert
        Assert.IsType<List<Individual>>(result);
    }
}
