namespace PokemonTools.Web.Application.Individuals;

public record CalculatedStatsDto(
    uint Hp,
    uint Attack,
    uint Defense,
    uint SpecialAttack,
    uint SpecialDefense,
    uint Speed
);
