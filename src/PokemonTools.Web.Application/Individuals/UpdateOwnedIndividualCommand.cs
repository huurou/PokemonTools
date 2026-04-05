namespace PokemonTools.Web.Application.Individuals;

public record UpdateOwnedIndividualCommand(
    string Id,
    string? Name,
    int SpeciesId,
    int StatAlignmentId,
    int AbilityId,
    uint StatPointHp,
    uint StatPointAttack,
    uint StatPointDefense,
    uint StatPointSpecialAttack,
    uint StatPointSpecialDefense,
    uint StatPointSpeed,
    int Move1Id,
    int? Move2Id,
    int? Move3Id,
    int? Move4Id,
    int? HeldItemId,
    int TeraTypeId,
    string? Memo
);
