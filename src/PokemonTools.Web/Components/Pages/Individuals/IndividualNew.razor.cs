using Microsoft.AspNetCore.Components;
using PokemonTools.Web.Application.Individuals;
using PokemonTools.Web.Domain.Abilities;
using PokemonTools.Web.Domain.Items;
using PokemonTools.Web.Domain.Moves;
using PokemonTools.Web.Domain.Species;
using PokemonTools.Web.Domain.Statistics;
using PokemonTools.Web.Domain.Types;

namespace PokemonTools.Web.Components.Pages.Individuals;

public partial class IndividualNew(
    OwnedIndividualCommandUseCase commandUseCase,
    ISpeciesRepository speciesRepository,
    IAbilityRepository abilityRepository,
    IMoveRepository moveRepository,
    IItemRepository itemRepository,
    NavigationManager navigationManager,
    ILogger<IndividualNew> logger
) : ComponentBase
{
    private List<PokemonSpecies> allSpecies_ = [];
    private List<Move> allMoves_ = [];
    private List<Item> allItems_ = [];
    private List<Ability> availableAbilities_ = [];
    private bool isLoading_ = true;
    private bool isSubmitting_;
    private string? errorMessage_;

    private string? name_;
    private int selectedSpeciesId_;
    private int selectedStatAlignmentId_ = StatAlignment.Hardy.Id.Value;
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
    private int selectedTeraTypeId_ = PokemonType.Normal.Id.Value;
    private string? memo_;

    protected override async Task OnInitializedAsync()
    {
        allSpecies_ = await speciesRepository.GetAllAsync();
        allMoves_ = await moveRepository.GetAllAsync();
        allItems_ = await itemRepository.GetAllAsync();
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
            if (selectedSpeciesId_ <= 0 || selectedAbilityId_ <= 0 ||
                selectedMove1Id_ <= 0 || selectedTeraTypeId_ <= 0)
            {
                errorMessage_ = "必須項目を入力してください";
                return;
            }

            var command = new RegisterOwnedIndividualCommand(
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
            await commandUseCase.RegisterAsync(command);
            navigationManager.NavigateTo("individuals");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "手持ち個体の登録に失敗しました");
            errorMessage_ = ex.Message;
        }
        finally
        {
            isSubmitting_ = false;
        }
    }
}
