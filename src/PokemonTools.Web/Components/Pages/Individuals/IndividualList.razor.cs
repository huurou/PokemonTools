using Microsoft.AspNetCore.Components;
using PokemonTools.Web.Application.Individuals;

namespace PokemonTools.Web.Components.Pages.Individuals;

public partial class IndividualList(
    IOwnedIndividualQueryService queryService,
    OwnedIndividualCommandUseCase commandUseCase,
    ILogger<IndividualList> logger
) : ComponentBase
{
    private List<OwnedIndividualListItemDto> individuals_ = [];
    private bool isLoading_ = true;
    private string? errorMessage_;

    private bool showDeleteConfirm_;
    private bool deleteIsInParty_;
    private string? deleteTargetId_;

    protected override async Task OnInitializedAsync()
    {
        await LoadListAsync();
    }

    private async Task LoadListAsync()
    {
        isLoading_ = true;
        errorMessage_ = null;
        try
        {
            individuals_ = await queryService.GetListAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "手持ち一覧の取得に失敗しました");
            errorMessage_ = "一覧の取得に失敗しました。しばらくしてから再度お試しください。";
        }
        finally
        {
            isLoading_ = false;
        }
    }

    private async Task OnDeleteClick(string id)
    {
        deleteTargetId_ = id;
        try
        {
            var result = await queryService.CheckDeleteAsync(id);
            deleteIsInParty_ = result.IsInParty;
            showDeleteConfirm_ = true;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "削除チェックに失敗しました");
            errorMessage_ = "削除チェックに失敗しました。しばらくしてから再度お試しください。";
        }
    }

    private void CancelDelete()
    {
        showDeleteConfirm_ = false;
        deleteTargetId_ = null;
    }

    private async Task ConfirmDelete()
    {
        if (deleteTargetId_ is null) { return; }
        showDeleteConfirm_ = false;
        try
        {
            await commandUseCase.DeleteAsync(deleteTargetId_);
            await LoadListAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "個体の削除に失敗しました");
            errorMessage_ = "削除に失敗しました。しばらくしてから再度お試しください。";
        }
        finally
        {
            deleteTargetId_ = null;
        }
    }
}
