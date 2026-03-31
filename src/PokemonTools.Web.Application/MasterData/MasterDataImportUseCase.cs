using PokemonTools.Web.Domain.Abilities;
using PokemonTools.Web.Domain.Items;
using PokemonTools.Web.Domain.Moves;
using PokemonTools.Web.Domain.Species;

namespace PokemonTools.Web.Application.MasterData;

public class MasterDataImportUseCase(
    IAbilityDataFetcher abilityDataFetcher,
    IAbilityRepository abilityRepository,
    IItemDataFetcher itemDataFetcher,
    IItemRepository itemRepository,
    IMoveDataFetcher moveDataFetcher,
    IMoveRepository moveRepository,
    ISpeciesDataFetcher speciesDataFetcher,
    ISpeciesRepository speciesRepository
)
{
    private const int CHUNK_SIZE = 100;

    public async Task ImportAbilitiesAsync(
        IProgress<MasterDataImportProgress>? progress = null,
        CancellationToken cancellationToken = default
    )
    {
        var total = await abilityDataFetcher.GetCountAsync(cancellationToken);
        await ImportAsync(abilityDataFetcher.GetAllAsync(cancellationToken), abilityRepository.UpsertRangeAsync, "特性", total, x => x.Name, progress, cancellationToken);
    }

    public async Task ImportItemsAsync(
        IProgress<MasterDataImportProgress>? progress = null,
        CancellationToken cancellationToken = default
    )
    {
        var total = await itemDataFetcher.GetCountAsync(cancellationToken);
        await ImportAsync(itemDataFetcher.GetAllAsync(cancellationToken), itemRepository.UpsertRangeAsync, "道具", total, x => x.Name, progress, cancellationToken);
    }

    public async Task ImportMovesAsync(
        IProgress<MasterDataImportProgress>? progress = null,
        CancellationToken cancellationToken = default
    )
    {
        var total = await moveDataFetcher.GetCountAsync(cancellationToken);
        await ImportAsync(moveDataFetcher.GetAllAsync(cancellationToken), moveRepository.UpsertRangeAsync, "技", total, x => x.Name, progress, cancellationToken);
    }

    public async Task ImportAbilitiesAndSpeciesAsync(
        IProgress<MasterDataImportProgress>? progress = null,
        CancellationToken cancellationToken = default
    )
    {
        await ImportAbilitiesAsync(progress, cancellationToken);
        await ImportSpeciesOnlyAsync(progress, cancellationToken);
    }

    public async Task ImportAllAsync(
        IProgress<MasterDataImportProgress>? progress = null,
        CancellationToken cancellationToken = default
    )
    {
        await ImportAbilitiesAsync(progress, cancellationToken);
        await ImportItemsAsync(progress, cancellationToken);
        await ImportMovesAsync(progress, cancellationToken);
        await ImportSpeciesOnlyAsync(progress, cancellationToken);
    }

    private async Task ImportSpeciesOnlyAsync(
        IProgress<MasterDataImportProgress>? progress,
        CancellationToken cancellationToken
    )
    {
        var total = await speciesDataFetcher.GetCountAsync(cancellationToken);
        await ImportAsync(speciesDataFetcher.GetAllAsync(cancellationToken), speciesRepository.UpsertRangeAsync, "種族", total, x => x.Name, progress, cancellationToken);
    }

    private static async Task ImportAsync<T>(
        IAsyncEnumerable<T> source,
        Func<List<T>, CancellationToken, Task> upsertAsync,
        string category,
        int total,
        Func<T, string> nameSelector,
        IProgress<MasterDataImportProgress>? progress,
        CancellationToken cancellationToken
    )
    {
        var chunk = new List<T>();
        var current = 0;

        await foreach (var item in source.WithCancellation(cancellationToken))
        {
            chunk.Add(item);
            current++;

            if (chunk.Count >= CHUNK_SIZE)
            {
                await upsertAsync(chunk, cancellationToken);
                progress?.Report(new MasterDataImportProgress(category, current, total, nameSelector(chunk[^1])));
                chunk.Clear();
            }
        }

        if (chunk.Count > 0)
        {
            await upsertAsync(chunk, cancellationToken);
            progress?.Report(new MasterDataImportProgress(category, current, total, nameSelector(chunk[^1])));
        }
    }
}
