using PokemonTools.Web.Domain.Moves;
using PokemonTools.Web.Domain.Types;
using PokemonTools.Web.Infrastructure.PokeApi;
using PokemonTools.Web.Infrastructure.PokeApi.Responses;
using System.Runtime.CompilerServices;

namespace PokemonTools.Web.Infrastructure.Moves;

public class MoveDataFetcher(PokeApiClient pokeApiClient) : IMoveDataFetcher
{
    /// <summary>
    /// PokeAPI の Shadow タイプ ID（コロシアム/XD系）。Pokemon Champions には存在しないためスキップする。
    /// </summary>
    private const int SHADOW_TYPE_ID = 10002;

    public Task<int> GetCountAsync(CancellationToken cancellationToken = default)
    {
        return pokeApiClient.GetResourceCountAsync("move", cancellationToken);
    }

    public async IAsyncEnumerable<Move> GetAllAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        await foreach (var resource in pokeApiClient.GetAllResourcesAsync("move", cancellationToken))
        {
            var response = await pokeApiClient.GetResourceAsync<PokeApiMoveResponse>(resource, cancellationToken);
            var japaneseName = response.Names.GetName(response.Name);
            var typeId = PokeApiUrlHelper.ExtractIdFromUrl(response.Type.Url);
            var damageClassId = PokeApiUrlHelper.ExtractIdFromUrl(response.DamageClass.Url);

            // Shadowタイプの技をスキップ
            if (typeId == SHADOW_TYPE_ID) { continue; }

            yield return new Move(
                new MoveId(response.Id),
                japaneseName,
                new TypeId(typeId),
                new MoveDamageClassId(damageClassId),
                (uint?)response.Power
            );
        }
    }
}
