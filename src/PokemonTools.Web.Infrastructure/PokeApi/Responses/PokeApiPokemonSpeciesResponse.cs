namespace PokemonTools.Web.Infrastructure.PokeApi.Responses;

/// <summary>
/// PokeAPI pokemon-species/{id} レスポンス
/// </summary>
public record PokeApiPokemonSpeciesResponse(int Id, string Name, List<PokeApiLocalizedName> Names);
