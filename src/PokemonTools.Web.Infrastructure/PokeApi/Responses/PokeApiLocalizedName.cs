namespace PokemonTools.Web.Infrastructure.PokeApi.Responses;

/// <summary>
/// PokeAPIの多言語名エントリ
/// </summary>
public record PokeApiLocalizedName(string Name, NamedApiResource Language);
