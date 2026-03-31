namespace PokemonTools.Web.Infrastructure.PokeApi.Responses;

/// <summary>
/// PokeAPI ability/{id} レスポンス
/// </summary>
public record PokeApiAbilityResponse(int Id, string Name, List<PokeApiLocalizedName> Names);
