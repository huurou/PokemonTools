using Microsoft.EntityFrameworkCore;
using PokemonTools.Web.Domain.Individuals;
using PokemonTools.Web.Infrastructure.Individuals;
using static PokemonTools.Web.Infrastructure.Tests.Individuals.IndividualRepositoryTestHelper;

namespace PokemonTools.Web.Infrastructure.Tests.Individuals;

public class IndividualRepository_DeleteAsyncTests(PostgreSqlFixture fixture) : IClassFixture<PostgreSqlFixture>
{
    [Fact]
    public async Task 存在しないID_例外が発生しない()
    {
        // Arrange
        var ct = TestContext.Current.CancellationToken;
        await using var context = fixture.CreateContext();
        var repository = new IndividualRepository(context);

        // Act
        var exception = await Record.ExceptionAsync(() =>
            repository.DeleteAsync(new IndividualId("ind_nonexistent_delete"), ct));

        // Assert
        Assert.Null(exception);
    }

    [Fact]
    public async Task 存在するID_削除される()
    {
        // Arrange
        var ct = TestContext.Current.CancellationToken;
        await using var seedContext = fixture.CreateContext();
        await SeedMasterDataAsync(seedContext, ct);
        var seedRepo = new IndividualRepository(seedContext);
        await seedRepo.AddAsync(CreateDefaultIndividual(id: "ind_delete_001"), ct);

        // Act
        await using var context = fixture.CreateContext();
        var repository = new IndividualRepository(context);
        await repository.DeleteAsync(new IndividualId("ind_delete_001"), ct);

        // Assert
        await using var verifyContext = fixture.CreateContext();
        var exists = await verifyContext.Individuals.AnyAsync(x => x.IndividualId == "ind_delete_001", ct);
        Assert.False(exists);
    }
}
