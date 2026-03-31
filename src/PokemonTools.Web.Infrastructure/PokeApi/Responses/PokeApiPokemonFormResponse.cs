namespace PokemonTools.Web.Infrastructure.PokeApi.Responses;

public record PokeApiPokemonFormResponse(int Id, string Name, List<PokeApiLocalizedName> Names);
