using Microsoft.AspNetCore.Components;
using PokemonTools.Web.Application.MasterData;

namespace PokemonTools.Web.Components.Pages.Admin;

public partial class MasterDataImport(
    MasterDataImportUseCase importUseCase,
    ILogger<MasterDataImport> logger
) : ComponentBase, IDisposable
{
    private enum ImportStatus { Idle, Running, Completed, Cancelled, Error }

    private bool isRunning_;
    private ImportStatus status_ = ImportStatus.Idle;
    private MasterDataImportProgress? currentProgress_;
    private string? errorMessage_;
    private CancellationTokenSource? cts_;

    private Task ImportAll()
    {
        return RunImport(importUseCase.ImportAllAsync);
    }

    private Task ImportItems()
    {
        return RunImport(importUseCase.ImportItemsAsync);
    }

    private Task ImportMoves()
    {
        return RunImport(importUseCase.ImportMovesAsync);
    }

    private Task ImportAbilitiesAndSpecies()
    {
        return RunImport(importUseCase.ImportAbilitiesAndSpeciesAsync);
    }

    private void CancelImport()
    {
        cts_?.Cancel();
    }

    private async Task RunImport(Func<IProgress<MasterDataImportProgress>?, CancellationToken, Task> importAction)
    {
        isRunning_ = true;
        status_ = ImportStatus.Running;
        errorMessage_ = null;
        currentProgress_ = null;
        cts_ = new CancellationTokenSource();

        var progress = new Progress<MasterDataImportProgress>(x =>
        {
            currentProgress_ = x;
            InvokeAsync(StateHasChanged);
        });

        try
        {
            await importAction(progress, cts_.Token);
            status_ = ImportStatus.Completed;
        }
        catch (OperationCanceledException)
        {
            status_ = ImportStatus.Cancelled;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "マスターデータインポートに失敗しました");
            errorMessage_ = ex.Message;
            status_ = ImportStatus.Error;
        }
        finally
        {
            isRunning_ = false;
            Interlocked.Exchange(ref cts_, null)?.Dispose();
        }
    }

    public void Dispose()
    {
        var cts = Interlocked.Exchange(ref cts_, null);
        cts?.Cancel();
        cts?.Dispose();
    }
}
