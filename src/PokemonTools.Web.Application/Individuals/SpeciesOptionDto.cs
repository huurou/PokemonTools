namespace PokemonTools.Web.Application.Individuals;

public record SpeciesOptionDto(
    int Id,
    string Name,
    int Ability1Id,
    int? Ability2Id,
    int? HiddenAbilityId,
    uint BaseStatHp,
    uint BaseStatAttack,
    uint BaseStatDefense,
    uint BaseStatSpecialAttack,
    uint BaseStatSpecialDefense,
    uint BaseStatSpeed
);
