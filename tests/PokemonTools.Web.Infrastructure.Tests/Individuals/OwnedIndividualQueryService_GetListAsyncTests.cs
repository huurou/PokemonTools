using PokemonTools.Web.Domain.Individuals;
using PokemonTools.Web.Domain.Statistics;
using PokemonTools.Web.Domain.Types;
using PokemonTools.Web.Infrastructure.Db.Individuals;
using PokemonTools.Web.Infrastructure.Individuals;
using static PokemonTools.Web.Infrastructure.Tests.Individuals.IndividualRepositoryTestHelper;

namespace PokemonTools.Web.Infrastructure.Tests.Individuals;

public class OwnedIndividualQueryService_GetListAsyncTests(PostgreSqlFixture fixture) : IClassFixture<PostgreSqlFixture>
{
    [Fact]
    public async Task 手持ち個体が存在する場合_一覧が返される()
    {
        // Arrange
        var ct = TestContext.Current.CancellationToken;
        await using var seedContext = fixture.CreateContext();
        await SeedMasterDataAsync(seedContext, ct);
        await CleanupIndividualsAsync(seedContext, ct);
        var seedRepo = new IndividualRepository(seedContext);
        await seedRepo.AddAsync(CreateDefaultIndividual(
            id: "ind_qs_list_001",
            name: "テスト個体"
        ), ct);

        // Act
        await using var context = fixture.CreateContext();
        var service = new OwnedIndividualQueryService(context);
        var result = await service.GetListAsync(ct);

        // Assert
        Assert.Contains(result, x => x.Id == "ind_qs_list_001");
        var item = result.First(x => x.Id == "ind_qs_list_001");
        Assert.Equal("テスト個体", item.DisplayName);
        Assert.Equal("ガブリアス", item.SpeciesName);
    }

    [Fact]
    public async Task 個体名がnullの場合_種族名がDisplayNameになる()
    {
        // Arrange
        var ct = TestContext.Current.CancellationToken;
        await using var seedContext = fixture.CreateContext();
        await SeedMasterDataAsync(seedContext, ct);
        await CleanupIndividualsAsync(seedContext, ct);
        var seedRepo = new IndividualRepository(seedContext);
        await seedRepo.AddAsync(CreateDefaultIndividual(
            id: "ind_qs_list_null_name",
            name: null
        ), ct);

        // Act
        await using var context = fixture.CreateContext();
        var service = new OwnedIndividualQueryService(context);
        var result = await service.GetListAsync(ct);

        // Assert
        var item = result.First(x => x.Id == "ind_qs_list_null_name");
        Assert.Equal("ガブリアス", item.DisplayName);
        Assert.Equal("ガブリアス", item.SpeciesName);
    }

    [Fact]
    public async Task プリセット個体のみ存在する場合_空リストが返される()
    {
        // Arrange
        var ct = TestContext.Current.CancellationToken;
        await using var seedContext = fixture.CreateContext();
        await SeedMasterDataAsync(seedContext, ct);
        await CleanupIndividualsAsync(seedContext, ct);
        var seedRepo = new IndividualRepository(seedContext);
        await seedRepo.AddAsync(CreateDefaultIndividual(
            id: "ind_qs_list_preset_only",
            categoryId: IndividualCategory.DamageCalculationPreset.Id
        ), ct);

        // Act
        await using var context = fixture.CreateContext();
        var service = new OwnedIndividualQueryService(context);
        var result = await service.GetListAsync(ct);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task CreatedAt降順で返される()
    {
        // Arrange
        var ct = TestContext.Current.CancellationToken;
        await using var seedContext = fixture.CreateContext();
        await SeedMasterDataAsync(seedContext, ct);
        await CleanupIndividualsAsync(seedContext, ct);

        var now = DateTimeOffset.UtcNow;
        seedContext.Individuals.Add(new IndividualEntity
        {
            IndividualId = "ind_qs_list_order_001",
            IndividualName = "先に作成",
            SpeciesId = SPECIES_ID.Value,
            StatAlignmentId = StatAlignment.Adamant.Id.Value,
            AbilityId = ABILITY_1_ID.Value,
            StatPointHp = 0, StatPointAttack = 32, StatPointDefense = 0,
            StatPointSpecialAttack = 0, StatPointSpecialDefense = 0, StatPointSpeed = 32,
            Move1Id = MOVE_1_ID.Value,
            TeraTypeId = PokemonType.Dragon.Id.Value,
            CategoryId = IndividualCategory.OwnedIndividual.Id.Value,
            CreatedAt = now.AddMinutes(-10),
        });
        seedContext.Individuals.Add(new IndividualEntity
        {
            IndividualId = "ind_qs_list_order_002",
            IndividualName = "後に作成",
            SpeciesId = SPECIES_ID.Value,
            StatAlignmentId = StatAlignment.Adamant.Id.Value,
            AbilityId = ABILITY_1_ID.Value,
            StatPointHp = 0, StatPointAttack = 32, StatPointDefense = 0,
            StatPointSpecialAttack = 0, StatPointSpecialDefense = 0, StatPointSpeed = 32,
            Move1Id = MOVE_1_ID.Value,
            TeraTypeId = PokemonType.Dragon.Id.Value,
            CategoryId = IndividualCategory.OwnedIndividual.Id.Value,
            CreatedAt = now,
        });
        await seedContext.SaveChangesAsync(ct);

        // Act
        await using var context = fixture.CreateContext();
        var service = new OwnedIndividualQueryService(context);
        var result = await service.GetListAsync(ct);

        // Assert
        var idx1 = result.FindIndex(x => x.Id == "ind_qs_list_order_002");
        var idx2 = result.FindIndex(x => x.Id == "ind_qs_list_order_001");
        Assert.True(idx1 < idx2, "後に作成された個体が先に返されるべき（CreatedAt降順）");
    }
}
