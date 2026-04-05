using Microsoft.AspNetCore.Components;
using PokemonTools.Web.Domain.Abilities;
using PokemonTools.Web.Domain.Items;
using PokemonTools.Web.Domain.Moves;
using PokemonTools.Web.Domain.Species;
using PokemonTools.Web.Domain.Statistics;
using PokemonTools.Web.Domain.Types;

namespace PokemonTools.Web.Components.Pages.Individuals;

public partial class IndividualForm : ComponentBase
{
    [Parameter] public string SubmitLabel { get; set; } = "登録";

    [Parameter] public EventCallback OnSubmit { get; set; }

    [Parameter] public string? ErrorMessage { get; set; }

    [Parameter] public bool IsSubmitting { get; set; }

    [Parameter] public List<PokemonSpecies> AllSpecies { get; set; } = [];

    [Parameter] public List<Move> AllMoves { get; set; } = [];

    [Parameter] public List<Item> AllItems { get; set; } = [];

    [Parameter] public List<Ability> AvailableAbilities { get; set; } = [];

    [Parameter] public EventCallback<int> OnSpeciesChanged { get; set; }

    // フォームバインディング用プロパティ
    [Parameter] public string? Name { get; set; }
    [Parameter] public EventCallback<string?> NameChanged { get; set; }

    [Parameter] public int SelectedSpeciesId { get; set; }

    [Parameter] public EventCallback<int> SelectedSpeciesIdChanged { get; set; }

    [Parameter] public int SelectedStatAlignmentId { get; set; } = StatAlignment.Hardy.Id.Value;
    [Parameter] public EventCallback<int> SelectedStatAlignmentIdChanged { get; set; }

    [Parameter] public int SelectedAbilityId { get; set; }
    [Parameter] public EventCallback<int> SelectedAbilityIdChanged { get; set; }

    [Parameter] public uint StatPointHp { get; set; }
    [Parameter] public EventCallback<uint> StatPointHpChanged { get; set; }

    [Parameter] public uint StatPointAttack { get; set; }
    [Parameter] public EventCallback<uint> StatPointAttackChanged { get; set; }

    [Parameter] public uint StatPointDefense { get; set; }
    [Parameter] public EventCallback<uint> StatPointDefenseChanged { get; set; }

    [Parameter] public uint StatPointSpecialAttack { get; set; }
    [Parameter] public EventCallback<uint> StatPointSpecialAttackChanged { get; set; }

    [Parameter] public uint StatPointSpecialDefense { get; set; }
    [Parameter] public EventCallback<uint> StatPointSpecialDefenseChanged { get; set; }

    [Parameter] public uint StatPointSpeed { get; set; }
    [Parameter] public EventCallback<uint> StatPointSpeedChanged { get; set; }

    [Parameter] public int SelectedMove1Id { get; set; }
    [Parameter] public EventCallback<int> SelectedMove1IdChanged { get; set; }

    [Parameter] public int SelectedMove2Id { get; set; }
    [Parameter] public EventCallback<int> SelectedMove2IdChanged { get; set; }

    [Parameter] public int SelectedMove3Id { get; set; }
    [Parameter] public EventCallback<int> SelectedMove3IdChanged { get; set; }

    [Parameter] public int SelectedMove4Id { get; set; }
    [Parameter] public EventCallback<int> SelectedMove4IdChanged { get; set; }

    [Parameter] public int SelectedHeldItemId { get; set; }
    [Parameter] public EventCallback<int> SelectedHeldItemIdChanged { get; set; }

    [Parameter] public int SelectedTeraTypeId { get; set; } = PokemonType.Normal.Id.Value;
    [Parameter] public EventCallback<int> SelectedTeraTypeIdChanged { get; set; }

    [Parameter] public string? Memo { get; set; }
    [Parameter] public EventCallback<string?> MemoChanged { get; set; }

    private string speciesFilter_ = "";
    private string moveFilter1_ = "";
    private string moveFilter2_ = "";
    private string moveFilter3_ = "";
    private string moveFilter4_ = "";
    private string itemFilter_ = "";

    private uint StatPointTotal => StatPointHp + StatPointAttack + StatPointDefense +
        StatPointSpecialAttack + StatPointSpecialDefense + StatPointSpeed;

    private bool IsValid =>
        SelectedSpeciesId > 0 &&
        SelectedAbilityId > 0 &&
        SelectedMove1Id > 0 &&
        SelectedTeraTypeId > 0 &&
        StatPointTotal <= 66;

    private static IReadOnlyList<PokemonType> TeraTypeOptions { get; } =
        PokemonType.All.Where(x => x.Id != PokemonType.Unknown.Id).ToList();

    private IReadOnlyList<PokemonSpecies> FilteredSpecies =>
        string.IsNullOrWhiteSpace(speciesFilter_)
            ? AllSpecies
            : AllSpecies.Where(x => x.Name.Contains(speciesFilter_, StringComparison.OrdinalIgnoreCase)).ToList();

    private IReadOnlyList<Move> FilteredMoves1 => FilterMoves(moveFilter1_);
    private IReadOnlyList<Move> FilteredMoves2 => FilterMoves(moveFilter2_);
    private IReadOnlyList<Move> FilteredMoves3 => FilterMoves(moveFilter3_);
    private IReadOnlyList<Move> FilteredMoves4 => FilterMoves(moveFilter4_);

    private IReadOnlyList<Move> FilterMoves(string filter) =>
        string.IsNullOrWhiteSpace(filter)
            ? AllMoves
            : AllMoves.Where(x => x.Name.Contains(filter, StringComparison.OrdinalIgnoreCase)).ToList();

    private IReadOnlyList<Item> FilteredItems =>
        string.IsNullOrWhiteSpace(itemFilter_)
            ? AllItems
            : AllItems.Where(x => x.Name.Contains(itemFilter_, StringComparison.OrdinalIgnoreCase)).ToList();

    private Stats? CalculatedStats
    {
        get
        {
            var species = AllSpecies.FirstOrDefault(x => x.Id.Value == SelectedSpeciesId);
            if (species is null) { return null; }

            var statAlignment = StatAlignment.All.FirstOrDefault(x => x.Id.Value == SelectedStatAlignmentId);
            if (statAlignment is null) { return null; }

            try
            {
                var statPoints = new StatPoints(StatPointHp, StatPointAttack, StatPointDefense,
                    StatPointSpecialAttack, StatPointSpecialDefense, StatPointSpeed);
                return StatsCalculator.Calculate(species.BaseStats, statPoints, statAlignment);
            }
            catch
            {
                return null;
            }
        }
    }

    private async Task HandleSpeciesChangeAsync()
    {
        await SelectedSpeciesIdChanged.InvokeAsync(SelectedSpeciesId);
        await OnSpeciesChanged.InvokeAsync(SelectedSpeciesId);
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
