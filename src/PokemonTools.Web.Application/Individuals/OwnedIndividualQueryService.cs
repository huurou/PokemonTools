using PokemonTools.Web.Domain.Individuals;
using PokemonTools.Web.Domain.Parties;
using PokemonTools.Web.Domain.Species;

namespace PokemonTools.Web.Application.Individuals;

public class OwnedIndividualQueryService(
    IIndividualRepository individualRepository,
    ISpeciesRepository speciesRepository,
    IPartyRepository partyRepository
)
{
    public async Task<List<OwnedIndividualListItemDto>> GetListAsync(CancellationToken cancellationToken = default)
    {
        var individuals = await individualRepository.GetOwnedIndividualsAsync(cancellationToken);
        if (individuals.Count == 0) { return []; }

        var speciesList = await speciesRepository.GetAllAsync(cancellationToken);
        var speciesDict = speciesList.ToDictionary(x => x.Id);

        return individuals.Select(x =>
        {
            var speciesName = speciesDict.TryGetValue(x.SpeciesId, out var species) ? species.Name : "不明";
            return new OwnedIndividualListItemDto(x.Id, x.Name ?? speciesName, speciesName);
        }).ToList();
    }

    public async Task<OwnedIndividualDetailDto?> GetDetailAsync(
        IndividualId id,
        CancellationToken cancellationToken = default)
    {
        var individual = await individualRepository.FindByIdAsync(id, cancellationToken);
        if (individual is null || individual.CategoryId != IndividualCategory.OwnedIndividual.Id) { return null; }

        var species = await speciesRepository.FindByIdAsync(individual.SpeciesId, cancellationToken);
        var speciesName = species?.Name ?? "不明";
        return new OwnedIndividualDetailDto(individual, speciesName);
    }

    public async Task<DeleteCheckResult> CheckDeleteAsync(
        IndividualId id,
        CancellationToken cancellationToken = default)
    {
        var isInParty = await partyRepository.IsIndividualInAnyPartyAsync(id, cancellationToken);
        return new DeleteCheckResult(isInParty);
    }
}
