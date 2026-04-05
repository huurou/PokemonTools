using PokemonTools.Web.Domain.Abilities;
using PokemonTools.Web.Domain.Items;
using PokemonTools.Web.Domain.Moves;
using PokemonTools.Web.Domain.Species;
using PokemonTools.Web.Domain.Statistics;
using PokemonTools.Web.Domain.Types;

namespace PokemonTools.Web.Application.Individuals;

public class OwnedIndividualFormQueryService(
    ISpeciesRepository speciesRepository,
    IAbilityRepository abilityRepository,
    IMoveRepository moveRepository,
    IItemRepository itemRepository
)
{
    public async Task<OwnedIndividualFormDataDto> GetFormDataAsync(CancellationToken cancellationToken = default)
    {
        var speciesList = await speciesRepository.GetAllAsync(cancellationToken);
        var moveList = await moveRepository.GetAllAsync(cancellationToken);
        var itemList = await itemRepository.GetAllAsync(cancellationToken);

        var abilityIds = speciesList
            .SelectMany(x => new[] { (AbilityId?)x.Ability1Id, x.Ability2Id, x.HiddenAbilityId })
            .Where(x => x is not null)
            .Select(x => x!)
            .Distinct()
            .ToList();
        var abilityList = await abilityRepository.GetByIdsAsync(abilityIds, cancellationToken);

        var allSpecies = speciesList.Select(x => new SpeciesOptionDto(
            x.Id.Value, x.Name,
            x.Ability1Id.Value, x.Ability2Id?.Value, x.HiddenAbilityId?.Value,
            x.BaseStats.Hp, x.BaseStats.Attack, x.BaseStats.Defense,
            x.BaseStats.SpecialAttack, x.BaseStats.SpecialDefense, x.BaseStats.Speed
        )).ToList();

        var allMoves = moveList.Select(x => new OptionDto(x.Id.Value, x.Name)).ToList();
        var allItems = itemList.Select(x => new OptionDto(x.Id.Value, x.Name)).ToList();
        var allAbilities = abilityList.Select(x => new OptionDto(x.Id.Value, x.Name)).ToList();

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
