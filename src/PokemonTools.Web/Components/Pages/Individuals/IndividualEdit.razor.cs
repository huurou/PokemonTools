using Microsoft.AspNetCore.Components;
using PokemonTools.Web.Application.Individuals;

namespace PokemonTools.Web.Components.Pages.Individuals;

public partial class IndividualEdit(
    OwnedIndividualCommandUseCase commandUseCase,
    OwnedIndividualQueryService queryService,
    OwnedIndividualFormQueryService formQueryService,
    NavigationManager navigationManager,
    ILogger<IndividualEdit> logger
) : ComponentBase
{
    [Parameter] public string Id { get; set; } = "";

    private OwnedIndividualFormDataDto formData_ = default!;
    private OwnedIndividualFormModel model_ = new();
    private bool isLoading_ = true;
    private bool notFound_;
    private bool isSubmitting_;
    private string? errorMessage_;

    protected override async Task OnInitializedAsync()
    {
        formData_ = await formQueryService.GetFormDataAsync();

        var detail = await queryService.GetDetailAsync(Id);
        if (detail is null)
        {
            notFound_ = true;
            isLoading_ = false;
            return;
        }

        model_.Name = detail.Name;
        model_.SelectedSpeciesId = detail.SpeciesId;
        model_.SelectedStatAlignmentId = detail.StatAlignmentId;
        model_.SelectedAbilityId = detail.AbilityId;
        model_.StatPointHp = detail.StatPointHp;
        model_.StatPointAttack = detail.StatPointAttack;
        model_.StatPointDefense = detail.StatPointDefense;
        model_.StatPointSpecialAttack = detail.StatPointSpecialAttack;
        model_.StatPointSpecialDefense = detail.StatPointSpecialDefense;
        model_.StatPointSpeed = detail.StatPointSpeed;
        model_.SelectedMove1Id = detail.Move1Id;
        model_.SelectedMove2Id = detail.Move2Id ?? 0;
        model_.SelectedMove3Id = detail.Move3Id ?? 0;
        model_.SelectedMove4Id = detail.Move4Id ?? 0;
        model_.SelectedHeldItemId = detail.HeldItemId ?? 0;
        model_.SelectedTeraTypeId = detail.TeraTypeId;
        model_.Memo = detail.Memo;

        isLoading_ = false;
    }

    private async Task HandleSubmit()
    {
        isSubmitting_ = true;
        errorMessage_ = null;
        try
        {
            if (model_.SelectedSpeciesId <= 0 || model_.SelectedAbilityId <= 0 ||
                model_.SelectedMove1Id <= 0 || model_.SelectedTeraTypeId <= 0)
            {
                errorMessage_ = "必須項目を入力してください";
                return;
            }

            var command = new UpdateOwnedIndividualCommand(
                Id,
                string.IsNullOrWhiteSpace(model_.Name) ? null : model_.Name,
                model_.SelectedSpeciesId,
                model_.SelectedStatAlignmentId,
                model_.SelectedAbilityId,
                model_.StatPointHp, model_.StatPointAttack, model_.StatPointDefense,
                model_.StatPointSpecialAttack, model_.StatPointSpecialDefense, model_.StatPointSpeed,
                model_.SelectedMove1Id,
                model_.SelectedMove2Id > 0 ? model_.SelectedMove2Id : null,
                model_.SelectedMove3Id > 0 ? model_.SelectedMove3Id : null,
                model_.SelectedMove4Id > 0 ? model_.SelectedMove4Id : null,
                model_.SelectedHeldItemId > 0 ? model_.SelectedHeldItemId : null,
                model_.SelectedTeraTypeId,
                string.IsNullOrWhiteSpace(model_.Memo) ? null : model_.Memo
            );
            await commandUseCase.UpdateAsync(command);
            navigationManager.NavigateTo("individuals");
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "手持ち個体の更新でバリデーションエラーが発生しました");
            errorMessage_ = ex.Message;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "手持ち個体の更新に失敗しました");
            errorMessage_ = "更新に失敗しました。しばらくしてから再度お試しください。";
        }
        finally
        {
            isSubmitting_ = false;
        }
    }
}
