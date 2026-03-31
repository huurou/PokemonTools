using Microsoft.Extensions.Logging;
using PokemonTools.Web.Domain.Abilities;
using PokemonTools.Web.Domain.Species;
using PokemonTools.Web.Domain.Statistics;
using PokemonTools.Web.Domain.Types;
using PokemonTools.Web.Infrastructure.PokeApi;
using PokemonTools.Web.Infrastructure.PokeApi.Responses;
using System.Runtime.CompilerServices;

namespace PokemonTools.Web.Infrastructure.Species;

public class SpeciesDataFetcher(PokeApiClient pokeApiClient, ILogger<SpeciesDataFetcher> logger) : ISpeciesDataFetcher
{
    public Task<int> GetCountAsync(CancellationToken cancellationToken = default)
    {
        return pokeApiClient.GetResourceCountAsync("pokemon", cancellationToken);
    }

    public async IAsyncEnumerable<PokemonSpecies> GetAllAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        await foreach (var resource in pokeApiClient.GetAllResourcesAsync("pokemon", cancellationToken))
        {
            var pokemon = await pokeApiClient.GetResourceAsync<PokeApiPokemonResponse>(resource, cancellationToken);

            if (pokemon.Weight == 0)
            {
                logger.LogWarning("体重が0のためスキップしました: id={Id}, name={Name}", pokemon.Id, pokemon.Name);
                continue;
            }

            var speciesResponse = await pokeApiClient.GetResourceAsync<PokeApiPokemonSpeciesResponse>(pokemon.Species, cancellationToken);

            // フォルム名の取得: 別フォルムの場合はpokemon-formから日本語名を取得
            string japaneseName;
            if (pokemon.Name == speciesResponse.Name)
            {
                japaneseName = speciesResponse.Names.GetName(speciesResponse.Name);
            }
            else
            {
                var formResource = pokemon.Forms.FirstOrDefault(x => x.Name == pokemon.Name);
                if (formResource is null)
                {
                    logger.LogWarning("フォルムリソースが見つからないためスキップしました: id={Id}, name={Name}", pokemon.Id, pokemon.Name);
                    continue;
                }
                var formResponse = await pokeApiClient.GetResourceAsync<PokeApiPokemonFormResponse>(formResource, cancellationToken);
                japaneseName = formResponse.Names.Count > 0
                    ? formResponse.Names.GetName(formResponse.Name)
                    : speciesResponse.Names.GetName(speciesResponse.Name);
            }

            var type1 = pokemon.Types.FirstOrDefault(x => x.Slot == 1);
            if (type1 is null)
            {
                logger.LogWarning("タイプ1が見つからないためスキップしました: id={Id}, name={Name}", pokemon.Id, pokemon.Name);
                continue;
            }
            var type2 = pokemon.Types.FirstOrDefault(x => x.Slot == 2);
            var type1Id = new TypeId(PokeApiUrlHelper.ExtractIdFromUrl(type1.Type.Url));
            var type2Id = type2 is not null ? new TypeId(PokeApiUrlHelper.ExtractIdFromUrl(type2.Type.Url)) : null;

            var normalAbilities = pokemon.Abilities
                .Where(x => !x.IsHidden)
                .OrderBy(x => x.Slot)
                .ToList();
            var hiddenAbility = pokemon.Abilities.FirstOrDefault(x => x.IsHidden);

            if (normalAbilities.Count == 0)
            {
                logger.LogWarning("通常特性がないためスキップしました: id={Id}, name={Name}", pokemon.Id, pokemon.Name);
                continue;
            }

            var ability1Id = new AbilityId(PokeApiUrlHelper.ExtractIdFromUrl(normalAbilities[0].Ability.Url));
            var ability2Id = normalAbilities.Count > 1
                ? new AbilityId(PokeApiUrlHelper.ExtractIdFromUrl(normalAbilities[1].Ability.Url))
                : null;
            var hiddenAbilityId = hiddenAbility is not null
                ? new AbilityId(PokeApiUrlHelper.ExtractIdFromUrl(hiddenAbility.Ability.Url))
                : null;

            var stats = pokemon.Stats.ToDictionary(x => x.Stat.Name, x => (uint)x.BaseStat);
            if (!stats.ContainsKey("hp") || !stats.ContainsKey("attack") || !stats.ContainsKey("defense")
                || !stats.ContainsKey("special-attack") || !stats.ContainsKey("special-defense") || !stats.ContainsKey("speed"))
            {
                logger.LogWarning("種族値が不完全なためスキップしました: id={Id}, name={Name}", pokemon.Id, pokemon.Name);
                continue;
            }
            var baseStats = new BaseStats(
                stats["hp"], stats["attack"], stats["defense"],
                stats["special-attack"], stats["special-defense"], stats["speed"]
            );

            yield return new PokemonSpecies(
                new SpeciesId(pokemon.Id),
                japaneseName,
                type1Id, type2Id,
                ability1Id, ability2Id, hiddenAbilityId,
                baseStats,
                new Weight(pokemon.Weight)
            );
        }
    }
}
