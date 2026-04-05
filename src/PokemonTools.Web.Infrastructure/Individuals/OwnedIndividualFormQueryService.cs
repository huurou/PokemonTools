using Microsoft.EntityFrameworkCore;
using PokemonTools.Web.Application.Individuals;
using PokemonTools.Web.Domain.Statistics;
using PokemonTools.Web.Domain.Types;
using PokemonTools.Web.Infrastructure.Db;

namespace PokemonTools.Web.Infrastructure.Individuals;

public class OwnedIndividualFormQueryService(PokemonToolsDbContext context) : IOwnedIndividualFormQueryService
{
    public async Task<OwnedIndividualFormDataDto> GetFormDataAsync(CancellationToken cancellationToken = default)
    {
        var allSpecies = await context.Species.AsNoTracking()
            .OrderBy(x => x.SpeciesId)
            .Select(x => new SpeciesOptionDto(
                x.SpeciesId, x.SpeciesName,
                x.Ability1Id, x.Ability2Id, x.HiddenAbilityId,
                (uint)x.BaseStatHp, (uint)x.BaseStatAttack, (uint)x.BaseStatDefense,
                (uint)x.BaseStatSpecialAttack, (uint)x.BaseStatSpecialDefense, (uint)x.BaseStatSpeed))
            .ToListAsync(cancellationToken);

        var allMoves = await context.Moves.AsNoTracking()
            .OrderBy(x => x.MoveId)
            .Select(x => new OptionDto(x.MoveId, x.MoveName))
            .ToListAsync(cancellationToken);

        var allItems = await context.Items.AsNoTracking()
            .OrderBy(x => x.ItemId)
            .Select(x => new OptionDto(x.ItemId, x.ItemName))
            .ToListAsync(cancellationToken);

        var abilityIds = allSpecies
            .SelectMany(x => new[] { (int?)x.Ability1Id, x.Ability2Id, x.HiddenAbilityId })
            .Where(x => x is not null)
            .Select(x => x!.Value)
            .Distinct()
            .ToList();

        var allAbilities = await context.Abilities.AsNoTracking()
            .Where(x => abilityIds.Contains(x.AbilityId))
            .OrderBy(x => x.AbilityId)
            .Select(x => new OptionDto(x.AbilityId, x.AbilityName))
            .ToListAsync(cancellationToken);

        var allStatAlignments = StatAlignment.All.Select(x =>
        {
            var label = x.IncreasedStat.HasValue
                ? $"{x.Name}（{GetStatName(x.IncreasedStat.Value)}↑ {GetStatName(x.DecreasedStat!.Value)}↓）"
                : $"{x.Name}（補正なし）";
            return new OptionDto(x.Id.Value, label);
        }).ToList();

        var allTeraTypes = PokemonType.All
            .Where(x => x.Id != PokemonType.Unknown.Id)
            .Select(x => new OptionDto(x.Id.Value, x.Name))
            .ToList();

        return new OwnedIndividualFormDataDto(
            allSpecies, allMoves, allItems, allAbilities,
            allStatAlignments, allTeraTypes,
            DefaultStatAlignmentId: StatAlignment.Hardy.Id.Value,
            DefaultTeraTypeId: PokemonType.All.First(x => x.Id != PokemonType.Unknown.Id).Id.Value);
    }

    private static string GetStatName(StatType statType)
    {
        return statType switch
        {
            StatType.Attack => "こうげき",
            StatType.Defense => "ぼうぎょ",
            StatType.SpecialAttack => "とくこう",
            StatType.SpecialDefense => "とくぼう",
            StatType.Speed => "すばやさ",
            _ => statType.ToString(),
        };
    }
}
