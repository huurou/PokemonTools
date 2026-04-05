using PokemonTools.Web.Domain.Abilities;
using PokemonTools.Web.Domain.Individuals;
using PokemonTools.Web.Domain.Items;
using PokemonTools.Web.Domain.Moves;
using PokemonTools.Web.Domain.Species;
using PokemonTools.Web.Domain.Statistics;
using PokemonTools.Web.Domain.Types;
using PokemonTools.Web.Infrastructure.Abilities;
using PokemonTools.Web.Infrastructure.Db;
using PokemonTools.Web.Infrastructure.Items;
using PokemonTools.Web.Infrastructure.Moves;
using PokemonTools.Web.Infrastructure.Species;

namespace PokemonTools.Web.Infrastructure.Tests.Individuals;

internal static class IndividualRepositoryTestHelper
{
    internal static readonly AbilityId ABILITY_1_ID = new(8);
    internal static readonly AbilityId ABILITY_2_ID = new(9);
    internal static readonly SpeciesId SPECIES_ID = new(445);
    internal static readonly MoveId MOVE_1_ID = new(89);
    internal static readonly MoveId MOVE_2_ID = new(200);
    internal static readonly MoveId MOVE_3_ID = new(337);
    internal static readonly MoveId MOVE_4_ID = new(406);
    internal static readonly ItemId ITEM_ID = new(197);

    internal static async Task SeedMasterDataAsync(PokemonToolsDbContext context, CancellationToken ct)
    {
        var abilityRepo = new AbilityRepository(context);
        await abilityRepo.UpsertRangeAsync(
        [
            new Ability(ABILITY_1_ID, "すなおこし"),
            new Ability(ABILITY_2_ID, "すながくれ"),
        ], ct);

        var moveRepo = new MoveRepository(context);
        await moveRepo.UpsertRangeAsync(
        [
            new Move(MOVE_1_ID, "じしん", PokemonType.Ground.Id, MoveDamageClass.Physical.Id, 100),
            new Move(MOVE_2_ID, "げきりん", PokemonType.Dragon.Id, MoveDamageClass.Physical.Id, 120),
            new Move(MOVE_3_ID, "りゅうせいぐん", PokemonType.Dragon.Id, MoveDamageClass.Special.Id, 130),
            new Move(MOVE_4_ID, "ドラゴンクロー", PokemonType.Dragon.Id, MoveDamageClass.Physical.Id, 80),
        ], ct);

        var itemRepo = new ItemRepository(context);
        await itemRepo.UpsertRangeAsync(
        [
            new Item(ITEM_ID, "こだわりハチマキ", 10),
        ], ct);

        var speciesRepo = new SpeciesRepository(context);
        await speciesRepo.UpsertRangeAsync(
        [
            new PokemonSpecies(
                SPECIES_ID, "ガブリアス",
                PokemonType.Dragon.Id, PokemonType.Ground.Id,
                ABILITY_1_ID, null, ABILITY_2_ID,
                new BaseStats(108, 130, 95, 80, 85, 102),
                new Weight(950)
            ),
        ], ct);
    }

    internal static Individual CreateDefaultIndividual(
        string? id = null,
        string? name = null,
        IndividualCategoryId? categoryId = null,
        MoveId? move2Id = null,
        MoveId? move3Id = null,
        MoveId? move4Id = null,
        ItemId? heldItemId = null,
        string? memo = null
    )
    {
        return new Individual(
            new IndividualId(id ?? "ind_test_001"),
            name,
            SPECIES_ID,
            StatAlignment.Adamant.Id,
            ABILITY_1_ID,
            new StatPoints(0, 32, 0, 0, 0, 32),
            MOVE_1_ID,
            move2Id,
            move3Id,
            move4Id,
            heldItemId,
            PokemonType.Dragon.Id,
            memo,
            categoryId ?? IndividualCategory.OwnedIndividual.Id
        );
    }
}
