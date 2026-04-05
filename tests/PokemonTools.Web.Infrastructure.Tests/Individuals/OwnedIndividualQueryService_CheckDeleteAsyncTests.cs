using PokemonTools.Web.Infrastructure.Db.Parties;
using PokemonTools.Web.Infrastructure.Individuals;
using static PokemonTools.Web.Infrastructure.Tests.Individuals.IndividualRepositoryTestHelper;

namespace PokemonTools.Web.Infrastructure.Tests.Individuals;

public class OwnedIndividualQueryService_CheckDeleteAsyncTests(PostgreSqlFixture fixture) : IClassFixture<PostgreSqlFixture>
{
    [Fact]
    public async Task パーティに所属している場合_IsInPartyがtrueになる()
    {
        // Arrange
        var ct = TestContext.Current.CancellationToken;
        await using var seedContext = fixture.CreateContext();
        await SeedMasterDataAsync(seedContext, ct);
        var seedRepo = new IndividualRepository(seedContext);
        await seedRepo.AddAsync(CreateDefaultIndividual(id: "ind_qs_del_in_party"), ct);
        seedContext.Parties.Add(new PartyEntity
        {
            PartyId = "pty_qs_del_test",
            PartyName = "テストパーティ",
            Individual1Id = "ind_qs_del_in_party",
        });
        await seedContext.SaveChangesAsync(ct);
        seedContext.ChangeTracker.Clear();

        // Act
        await using var context = fixture.CreateContext();
        var service = new OwnedIndividualQueryService(context);
        var result = await service.CheckDeleteAsync("ind_qs_del_in_party", ct);

        // Assert
        Assert.True(result.IsInParty);
    }

    [Fact]
    public async Task どのパーティにも所属していない場合_IsInPartyがfalseになる()
    {
        // Arrange
        var ct = TestContext.Current.CancellationToken;
        await using var seedContext = fixture.CreateContext();
        await SeedMasterDataAsync(seedContext, ct);
        var seedRepo = new IndividualRepository(seedContext);
        await seedRepo.AddAsync(CreateDefaultIndividual(id: "ind_qs_del_no_party"), ct);

        // Act
        await using var context = fixture.CreateContext();
        var service = new OwnedIndividualQueryService(context);
        var result = await service.CheckDeleteAsync("ind_qs_del_no_party", ct);

        // Assert
        Assert.False(result.IsInParty);
    }
}
