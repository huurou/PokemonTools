namespace PokemonTools.Web.Infrastructure.PokeApi.Responses;

/// <summary>
/// PokeAPI move/{id} レスポンス
/// </summary>
public record PokeApiMoveResponse(
    int Id, string Name,
    List<PokeApiLocalizedName> Names,
    NamedApiResource Type,
    NamedApiResource DamageClass,
    int? Power
);
