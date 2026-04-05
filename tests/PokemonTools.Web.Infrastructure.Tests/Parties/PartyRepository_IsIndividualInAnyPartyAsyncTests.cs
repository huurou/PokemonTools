using PokemonTools.Web.Domain.Individuals;
using PokemonTools.Web.Infrastructure.Db.Parties;
using PokemonTools.Web.Infrastructure.Individuals;
using PokemonTools.Web.Infrastructure.Parties;
using static PokemonTools.Web.Infrastructure.Tests.Individuals.IndividualRepositoryTestHelper;

namespace PokemonTools.Web.Infrastructure.Tests.Parties;

public class PartyRepository_IsIndividualInAnyPartyAsyncTests(PostgreSqlFixture fixture) : IClassFixture<PostgreSqlFixture>
{
    [Fact]
    public async Task パーティのスロット1に所属_trueが返される()
    {
        // Arrange
        var ct = TestContext.Current.CancellationToken;
        await using var seedContext = fixture.CreateContext();
        await SeedMasterDataAsync(seedContext, ct);
        var indRepo = new IndividualRepository(seedContext);
        await indRepo.AddAsync(CreateDefaultIndividual(id: "ind_party_slot1"), ct);
        seedContext.Parties.Add(new PartyEntity
        {
            PartyId = "pty_slot1_test",
            PartyName = "テストPT",
            Individual1Id = "ind_party_slot1",
        });
        await seedContext.SaveChangesAsync(ct);
        seedContext.ChangeTracker.Clear();

        // Act
        await using var context = fixture.CreateContext();
        var repository = new PartyRepository(context);
        var result = await repository.IsIndividualInAnyPartyAsync(new IndividualId("ind_party_slot1"), ct);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task パーティのスロット6に所属_trueが返される()
    {
        // Arrange
        var ct = TestContext.Current.CancellationToken;
        await using var seedContext = fixture.CreateContext();
        await SeedMasterDataAsync(seedContext, ct);
        var indRepo = new IndividualRepository(seedContext);
        await indRepo.AddAsync(CreateDefaultIndividual(id: "ind_party_slot6"), ct);
        seedContext.Parties.Add(new PartyEntity
        {
            PartyId = "pty_slot6_test",
            PartyName = "テストPT6",
            Individual6Id = "ind_party_slot6",
        });
        await seedContext.SaveChangesAsync(ct);
        seedContext.ChangeTracker.Clear();

        // Act
        await using var context = fixture.CreateContext();
        var repository = new PartyRepository(context);
        var result = await repository.IsIndividualInAnyPartyAsync(new IndividualId("ind_party_slot6"), ct);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task どのパーティにも所属しない_falseが返される()
    {
        // Arrange
        var ct = TestContext.Current.CancellationToken;

        // Act
        await using var context = fixture.CreateContext();
        var repository = new PartyRepository(context);
        var result = await repository.IsIndividualInAnyPartyAsync(new IndividualId("ind_not_in_party"), ct);

        // Assert
        Assert.False(result);
    }
}
