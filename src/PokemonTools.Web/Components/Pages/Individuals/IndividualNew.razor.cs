using Microsoft.AspNetCore.Components;
using PokemonTools.Web.Application.Individuals;

namespace PokemonTools.Web.Components.Pages.Individuals;

public partial class IndividualNew(
    OwnedIndividualCommandUseCase commandUseCase,
    OwnedIndividualFormQueryService formQueryService,
    NavigationManager navigationManager,
    ILogger<IndividualNew> logger
) : ComponentBase
{
    private OwnedIndividualFormDataDto formData_ = default!;
    private OwnedIndividualFormModel model_ = new();
    private bool isLoading_ = true;
    private bool isSubmitting_;
    private string? errorMessage_;

    protected override async Task OnInitializedAsync()
    {
        formData_ = await formQueryService.GetFormDataAsync();
        model_.ApplyDefaults(formData_);
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

            var command = model_.ToRegisterCommand();
            await commandUseCase.RegisterAsync(command);
            navigationManager.NavigateTo("individuals");
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "手持ち個体の登録でバリデーションエラーが発生しました");
            errorMessage_ = ex.Message;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "手持ち個体の登録に失敗しました");
            errorMessage_ = "登録に失敗しました。しばらくしてから再度お試しください。";
        }
        finally
        {
            isSubmitting_ = false;
        }
    }
}
