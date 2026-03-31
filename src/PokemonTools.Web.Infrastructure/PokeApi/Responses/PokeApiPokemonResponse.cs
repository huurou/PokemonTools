namespace PokemonTools.Web.Infrastructure.PokeApi.Responses;

/// <summary>
/// PokeAPI pokemon/{id} レスポンス
/// </summary>
public record PokeApiPokemonResponse(
    int Id, string Name,
    List<PokeApiPokemonTypeEntry> Types,
    List<PokeApiPokemonAbilityEntry> Abilities,
    List<PokeApiPokemonStatEntry> Stats,
    List<NamedApiResource> Forms,
    int Weight, NamedApiResource Species
);

public record PokeApiPokemonTypeEntry(int Slot, NamedApiResource Type);

public record PokeApiPokemonAbilityEntry(int Slot, bool IsHidden, NamedApiResource Ability);

public record PokeApiPokemonStatEntry(int BaseStat, NamedApiResource Stat);
