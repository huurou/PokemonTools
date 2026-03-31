using PokemonTools.Web.Domain.Abilities;
using PokemonTools.Web.Infrastructure.PokeApi;
using PokemonTools.Web.Infrastructure.PokeApi.Responses;
using System.Runtime.CompilerServices;

namespace PokemonTools.Web.Infrastructure.Abilities;

public class AbilityDataFetcher(PokeApiClient pokeApiClient) : IAbilityDataFetcher
{
    public Task<int> GetCountAsync(CancellationToken cancellationToken = default)
    {
        return pokeApiClient.GetResourceCountAsync("ability", cancellationToken);
    }

    public async IAsyncEnumerable<Ability> GetAllAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        await foreach (var resource in pokeApiClient.GetAllResourcesAsync("ability", cancellationToken))
        {
            var response = await pokeApiClient.GetResourceAsync<PokeApiAbilityResponse>(resource, cancellationToken);
            var japaneseName = response.Names.GetName(response.Name);
            yield return new Ability(new AbilityId(response.Id), japaneseName);
        }
    }
}
