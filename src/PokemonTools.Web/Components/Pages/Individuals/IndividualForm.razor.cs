using Microsoft.AspNetCore.Components;
using PokemonTools.Web.Application.Individuals;

namespace PokemonTools.Web.Components.Pages.Individuals;

public partial class IndividualForm : ComponentBase
{
    [Parameter] public string SubmitLabel { get; set; } = "登録";

    [Parameter] public EventCallback OnSubmit { get; set; }

    [Parameter] public string? ErrorMessage { get; set; }

    [Parameter] public bool IsSubmitting { get; set; }

    [Parameter] public OwnedIndividualFormDataDto FormData { get; set; } = default!;

    [Parameter] public OwnedIndividualFormModel Model { get; set; } = default!;

    private string speciesFilter_ = "";
    private string moveFilter1_ = "";
    private string moveFilter2_ = "";
    private string moveFilter3_ = "";
    private string moveFilter4_ = "";
    private string itemFilter_ = "";

    private uint StatPointTotal => Model.StatPointHp + Model.StatPointAttack + Model.StatPointDefense +
        Model.StatPointSpecialAttack + Model.StatPointSpecialDefense + Model.StatPointSpeed;

    private bool IsValid =>
        Model.SelectedSpeciesId > 0 &&
        Model.SelectedAbilityId > 0 &&
        Model.SelectedMove1Id > 0 &&
        Model.SelectedTeraTypeId > 0 &&
        StatPointTotal <= 66;

    private List<OptionDto> AvailableAbilities
    {
        get
        {
            var species = FormData.AllSpecies.FirstOrDefault(x => x.Id == Model.SelectedSpeciesId);
            if (species is null) { return []; }
            var abilityIds = new List<int> { species.Ability1Id };
            if (species.Ability2Id is not null) { abilityIds.Add(species.Ability2Id.Value); }
            if (species.HiddenAbilityId is not null) { abilityIds.Add(species.HiddenAbilityId.Value); }
            return abilityIds
                .Select(x => FormData.AllAbilities.FirstOrDefault(y => y.Id == x))
                .Where(x => x is not null)
                .ToList()!;
        }
    }

    private IReadOnlyList<SpeciesOptionDto> FilteredSpecies =>
        string.IsNullOrWhiteSpace(speciesFilter_)
            ? FormData.AllSpecies
            : FormData.AllSpecies.Where(x => x.Name.Contains(speciesFilter_, StringComparison.OrdinalIgnoreCase)).ToList();

    private IReadOnlyList<OptionDto> FilteredMoves1 => FilterMoves(moveFilter1_);
    private IReadOnlyList<OptionDto> FilteredMoves2 => FilterMoves(moveFilter2_);
    private IReadOnlyList<OptionDto> FilteredMoves3 => FilterMoves(moveFilter3_);
    private IReadOnlyList<OptionDto> FilteredMoves4 => FilterMoves(moveFilter4_);

    private IReadOnlyList<OptionDto> FilterMoves(string filter) =>
        string.IsNullOrWhiteSpace(filter)
            ? FormData.AllMoves
            : FormData.AllMoves.Where(x => x.Name.Contains(filter, StringComparison.OrdinalIgnoreCase)).ToList();

    private IReadOnlyList<OptionDto> FilteredItems =>
        string.IsNullOrWhiteSpace(itemFilter_)
            ? FormData.AllItems
            : FormData.AllItems.Where(x => x.Name.Contains(itemFilter_, StringComparison.OrdinalIgnoreCase)).ToList();

    private CalculatedStatsDto? CalculatedStats
    {
        get
        {
            var species = FormData.AllSpecies.FirstOrDefault(x => x.Id == Model.SelectedSpeciesId);
            return IndividualStatsPreview.Calculate(
                species, Model.SelectedStatAlignmentId,
                Model.StatPointHp, Model.StatPointAttack, Model.StatPointDefense,
                Model.StatPointSpecialAttack, Model.StatPointSpecialDefense, Model.StatPointSpeed);
        }
    }

    private void HandleSpeciesChange()
    {
        var abilities = AvailableAbilities;
        if (abilities.Count > 0 && !abilities.Any(x => x.Id == Model.SelectedAbilityId))
        {
            Model.SelectedAbilityId = abilities[0].Id;
        }
        else if (abilities.Count == 0)
        {
            Model.SelectedAbilityId = 0;
        }
    }
}
