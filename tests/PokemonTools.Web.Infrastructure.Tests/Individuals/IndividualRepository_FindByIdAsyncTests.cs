using PokemonTools.Web.Domain.Individuals;
using PokemonTools.Web.Infrastructure.Individuals;
using static PokemonTools.Web.Infrastructure.Tests.Individuals.IndividualRepositoryTestHelper;

namespace PokemonTools.Web.Infrastructure.Tests.Individuals;

public class IndividualRepository_FindByIdAsyncTests(PostgreSqlFixture fixture) : IClassFixture<PostgreSqlFixture>
{
    [Fact]
    public async Task 存在するID_個体が返される()
    {
        // Arrange
        var ct = TestContext.Current.CancellationToken;
        await using var seedContext = fixture.CreateContext();
        await SeedMasterDataAsync(seedContext, ct);
        var seedRepo = new IndividualRepository(seedContext);
        await seedRepo.AddAsync(CreateDefaultIndividual(id: "ind_find_001", name: "テスト個体"), ct);

        // Act
        await using var context = fixture.CreateContext();
        var repository = new IndividualRepository(context);
        var result = await repository.FindByIdAsync(new IndividualId("ind_find_001"), ct);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(new IndividualId("ind_find_001"), result.Id);
        Assert.Equal("テスト個体", result.Name);
        Assert.Equal(SPECIES_ID, result.SpeciesId);
    }

    [Fact]
    public async Task 存在しないID_nullが返される()
    {
        // Arrange
        var ct = TestContext.Current.CancellationToken;

        // Act
        await using var context = fixture.CreateContext();
        var repository = new IndividualRepository(context);
        var result = await repository.FindByIdAsync(new IndividualId("ind_nonexistent"), ct);

        // Assert
        Assert.Null(result);
    }
}
