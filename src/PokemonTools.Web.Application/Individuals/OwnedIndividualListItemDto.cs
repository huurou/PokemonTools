using PokemonTools.Web.Domain.Individuals;

namespace PokemonTools.Web.Application.Individuals;

public record OwnedIndividualListItemDto(
    IndividualId Id,
    string DisplayName,
    string SpeciesName
);
