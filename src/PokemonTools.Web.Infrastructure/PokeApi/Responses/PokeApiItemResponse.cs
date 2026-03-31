namespace PokemonTools.Web.Infrastructure.PokeApi.Responses;

/// <summary>
/// PokeAPI item/{id} レスポンス
/// </summary>
public record PokeApiItemResponse(int Id, string Name, List<PokeApiLocalizedName> Names, int? FlingPower);
