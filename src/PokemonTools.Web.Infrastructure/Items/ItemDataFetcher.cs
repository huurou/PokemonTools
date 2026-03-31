using PokemonTools.Web.Domain.Items;
using PokemonTools.Web.Infrastructure.PokeApi;
using PokemonTools.Web.Infrastructure.PokeApi.Responses;
using System.Runtime.CompilerServices;

namespace PokemonTools.Web.Infrastructure.Items;

public class ItemDataFetcher(PokeApiClient pokeApiClient) : IItemDataFetcher
{
    public Task<int> GetCountAsync(CancellationToken cancellationToken = default)
    {
        return pokeApiClient.GetResourceCountAsync("item", cancellationToken);
    }

    public async IAsyncEnumerable<Item> GetAllAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        await foreach (var resource in pokeApiClient.GetAllResourcesAsync("item", cancellationToken))
        {
            var response = await pokeApiClient.GetResourceAsync<PokeApiItemResponse>(resource, cancellationToken);
            var japaneseName = response.Names.GetName(response.Name);
            yield return new Item(new ItemId(response.Id), japaneseName, (uint?)response.FlingPower);
        }
    }
}
