using Microsoft.AspNetCore.Components;
using PokemonTools.Web.Application.Individuals;
using PokemonTools.Web.Domain.Abilities;
using PokemonTools.Web.Domain.Individuals;
using PokemonTools.Web.Domain.Items;
using PokemonTools.Web.Domain.Moves;
using PokemonTools.Web.Domain.Species;
using PokemonTools.Web.Domain.Statistics;
using PokemonTools.Web.Domain.Types;

namespace PokemonTools.Web.Components.Pages.Individuals;

public partial class IndividualEdit(
    OwnedIndividualCommandUseCase commandUseCase,
    OwnedIndividualQueryService queryService,
    ISpeciesRepository speciesRepository,
    IAbilityRepository abilityRepository,
    IMoveRepository moveRepository,
    IItemRepository itemRepository,
    NavigationManager navigationManager,
    ILogger<IndividualEdit> logger
) : ComponentBase
{
    [Parameter] public string Id { get; set; } = "";

    private List<PokemonSpecies> allSpecies_ = [];
    private List<Move> allMoves_ = [];
    private List<Item> allItems_ = [];
    private List<Ability> availableAbilities_ = [];
    private bool isLoading_ = true;
    private bool notFound_;
    private bool isSubmitting_;
    private string? errorMessage_;

    private string? name_;
    private int selectedSpeciesId_;
    private int selectedStatAlignmentId_;
    private int selectedAbilityId_;
    private uint statPointHp_;
    private uint statPointAttack_;
    private uint statPointDefense_;
    private uint statPointSpecialAttack_;
    private uint statPointSpecialDefense_;
    private uint statPointSpeed_;
    private int selectedMove1Id_;
    private int selectedMove2Id_;
    private int selectedMove3Id_;
    private int selectedMove4Id_;
    private int selectedHeldItemId_;
    private int selectedTeraTypeId_;
    private string? memo_;

    protected override async Task OnInitializedAsync()
    {
        allSpecies_ = await speciesRepository.GetAllAsync();
        allMoves_ = await moveRepository.GetAllAsync();
        allItems_ = await itemRepository.GetAllAsync();

        var detail = await queryService.GetDetailAsync(new IndividualId(Id));
        if (detail is null)
        {
            notFound_ = true;
            isLoading_ = false;
            return;
        }

        var individual = detail.Individual;
        name_ = individual.Name;
        selectedSpeciesId_ = individual.SpeciesId.Value;
        selectedStatAlignmentId_ = individual.StatAlignmentId.Value;
        selectedAbilityId_ = individual.AbilityId.Value;
        statPointHp_ = individual.StatPoints.Hp;
        statPointAttack_ = individual.StatPoints.Attack;
        statPointDefense_ = individual.StatPoints.Defense;
        statPointSpecialAttack_ = individual.StatPoints.SpecialAttack;
        statPointSpecialDefense_ = individual.StatPoints.SpecialDefense;
        statPointSpeed_ = individual.StatPoints.Speed;
        selectedMove1Id_ = individual.Move1Id.Value;
        selectedMove2Id_ = individual.Move2Id?.Value ?? 0;
        selectedMove3Id_ = individual.Move3Id?.Value ?? 0;
        selectedMove4Id_ = individual.Move4Id?.Value ?? 0;
        selectedHeldItemId_ = individual.HeldItemId?.Value ?? 0;
        selectedTeraTypeId_ = individual.TeraTypeId.Value;
        memo_ = individual.Memo;

        await UpdateAvailableAbilitiesAsync();
        isLoading_ = false;
    }

    private async Task HandleSpeciesChanged(int speciesId)
    {
        selectedSpeciesId_ = speciesId;
        await UpdateAvailableAbilitiesAsync();
    }

    private async Task UpdateAvailableAbilitiesAsync()
    {
        var species = allSpecies_.FirstOrDefault(x => x.Id.Value == selectedSpeciesId_);
        if (species is null)
        {
            availableAbilities_ = [];
            selectedAbilityId_ = 0;
            return;
        }

        var abilityIds = new List<AbilityId> { species.Ability1Id };
        if (species.Ability2Id is not null) { abilityIds.Add(species.Ability2Id); }
        if (species.HiddenAbilityId is not null) { abilityIds.Add(species.HiddenAbilityId); }

        availableAbilities_ = await abilityRepository.GetByIdsAsync(abilityIds);
        if (availableAbilities_.Count > 0 && !availableAbilities_.Any(x => x.Id.Value == selectedAbilityId_))
        {
            selectedAbilityId_ = availableAbilities_[0].Id.Value;
        }
    }

    private async Task HandleSubmit()
    {
        isSubmitting_ = true;
        errorMessage_ = null;
        try
        {
            var command = new UpdateOwnedIndividualCommand(
                new IndividualId(Id),
                string.IsNullOrWhiteSpace(name_) ? null : name_,
                new SpeciesId(selectedSpeciesId_),
                new StatAlignmentId(selectedStatAlignmentId_),
                new AbilityId(selectedAbilityId_),
                new StatPoints(statPointHp_, statPointAttack_, statPointDefense_,
                    statPointSpecialAttack_, statPointSpecialDefense_, statPointSpeed_),
                new MoveId(selectedMove1Id_),
                selectedMove2Id_ > 0 ? new MoveId(selectedMove2Id_) : null,
                selectedMove3Id_ > 0 ? new MoveId(selectedMove3Id_) : null,
                selectedMove4Id_ > 0 ? new MoveId(selectedMove4Id_) : null,
                selectedHeldItemId_ > 0 ? new ItemId(selectedHeldItemId_) : null,
                new TypeId(selectedTeraTypeId_),
                string.IsNullOrWhiteSpace(memo_) ? null : memo_
            );
            await commandUseCase.UpdateAsync(command);
            navigationManager.NavigateTo("individuals");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "手持ち個体の更新に失敗しました");
            errorMessage_ = ex.Message;
        }
        finally
        {
            isSubmitting_ = false;
        }
    }
}
