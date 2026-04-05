using PokemonTools.Web.Domain.Individuals;

namespace PokemonTools.Web.Application.Individuals;

public record OwnedIndividualDetailDto(
    Individual Individual,
    string SpeciesName
);
