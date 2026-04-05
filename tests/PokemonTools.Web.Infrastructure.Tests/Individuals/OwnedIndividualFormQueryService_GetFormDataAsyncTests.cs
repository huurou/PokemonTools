using PokemonTools.Web.Domain.Statistics;
using PokemonTools.Web.Domain.Types;
using PokemonTools.Web.Infrastructure.Individuals;
using static PokemonTools.Web.Infrastructure.Tests.Individuals.IndividualRepositoryTestHelper;

namespace PokemonTools.Web.Infrastructure.Tests.Individuals;

public class OwnedIndividualFormQueryService_GetFormDataAsyncTests(PostgreSqlFixture fixture) : IClassFixture<PostgreSqlFixture>
{
    [Fact]
    public async Task マスターデータが存在する場合_全コレクションが返される()
    {
        // Arrange
        var ct = TestContext.Current.CancellationToken;
        await using var seedContext = fixture.CreateContext();
        await SeedMasterDataAsync(seedContext, ct);

        // Act
        await using var context = fixture.CreateContext();
        var service = new OwnedIndividualFormQueryService(context);
        var result = await service.GetFormDataAsync(ct);

        // Assert
        Assert.NotEmpty(result.AllSpecies);
        Assert.NotEmpty(result.AllMoves);
        Assert.NotEmpty(result.AllItems);
        Assert.NotEmpty(result.AllAbilities);
        Assert.NotEmpty(result.AllStatAlignments);
        Assert.NotEmpty(result.AllTeraTypes);
    }

    [Fact]
    public async Task 種族が参照する特性のみAbilitiesに含まれる()
    {
        // Arrange
        var ct = TestContext.Current.CancellationToken;
        await using var seedContext = fixture.CreateContext();
        await SeedMasterDataAsync(seedContext, ct);

        // Act
        await using var context = fixture.CreateContext();
        var service = new OwnedIndividualFormQueryService(context);
        var result = await service.GetFormDataAsync(ct);

        // Assert
        Assert.Contains(result.AllAbilities, x => x.Id == ABILITY_1_ID.Value);
        Assert.Contains(result.AllAbilities, x => x.Id == ABILITY_2_ID.Value);
    }

    [Fact]
    public async Task StatAlignmentsがドメインモデルから生成される()
    {
        // Arrange
        var ct = TestContext.Current.CancellationToken;
        await using var seedContext = fixture.CreateContext();
        await SeedMasterDataAsync(seedContext, ct);

        // Act
        await using var context = fixture.CreateContext();
        var service = new OwnedIndividualFormQueryService(context);
        var result = await service.GetFormDataAsync(ct);

        // Assert
        Assert.Equal(StatAlignment.All.Length, result.AllStatAlignments.Count);
        Assert.Contains(result.AllStatAlignments, x => x.Id == StatAlignment.Hardy.Id.Value);
    }

    [Fact]
    public async Task TeraTypesにUnknownが含まれない()
    {
        // Arrange
        var ct = TestContext.Current.CancellationToken;
        await using var seedContext = fixture.CreateContext();
        await SeedMasterDataAsync(seedContext, ct);

        // Act
        await using var context = fixture.CreateContext();
        var service = new OwnedIndividualFormQueryService(context);
        var result = await service.GetFormDataAsync(ct);

        // Assert
        Assert.DoesNotContain(result.AllTeraTypes, x => x.Id == PokemonType.Unknown.Id.Value);
        Assert.NotEmpty(result.AllTeraTypes);
    }

    [Fact]
    public async Task DefaultStatAlignmentIdとDefaultTeraTypeIdが正しい()
    {
        // Arrange
        var ct = TestContext.Current.CancellationToken;
        await using var seedContext = fixture.CreateContext();
        await SeedMasterDataAsync(seedContext, ct);

        // Act
        await using var context = fixture.CreateContext();
        var service = new OwnedIndividualFormQueryService(context);
        var result = await service.GetFormDataAsync(ct);

        // Assert
        Assert.Equal(StatAlignment.Hardy.Id.Value, result.DefaultStatAlignmentId);
        var expectedTeraTypeId = PokemonType.All.First(x => x.Id != PokemonType.Unknown.Id).Id.Value;
        Assert.Equal(expectedTeraTypeId, result.DefaultTeraTypeId);
    }
}
