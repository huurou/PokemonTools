using PokemonTools.Web.Domain.Abilities;
using PokemonTools.Web.Domain.Items;
using PokemonTools.Web.Domain.Moves;
using PokemonTools.Web.Domain.Species;
using PokemonTools.Web.Domain.Statistics;
using PokemonTools.Web.Domain.Types;

namespace PokemonTools.Web.Application.Individuals;

public record RegisterOwnedIndividualCommand(
    string? Name,
    SpeciesId SpeciesId,
    StatAlignmentId StatAlignmentId,
    AbilityId AbilityId,
    StatPoints StatPoints,
    MoveId Move1Id,
    MoveId? Move2Id,
    MoveId? Move3Id,
    MoveId? Move4Id,
    ItemId? HeldItemId,
    TypeId TeraTypeId,
    string? Memo
);
