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

        var speciesIds = individuals.Select(x => x.SpeciesId).Distinct().ToList();
        var speciesList = await speciesRepository.GetByIdsAsync(speciesIds, cancellationToken);
        var speciesDict = speciesList.ToDictionary(x => x.Id);

        return individuals.Select(x =>
        {
            var speciesName = speciesDict.TryGetValue(x.SpeciesId, out var species) ? species.Name : "不明";
            return new OwnedIndividualListItemDto(x.Id.Value, x.Name ?? speciesName, speciesName);
        }).ToList();
    }

    public async Task<OwnedIndividualDetailDto?> GetDetailAsync(
        string id,
        CancellationToken cancellationToken = default)
    {
        var individual = await individualRepository.FindByIdAsync(new IndividualId(id), cancellationToken);
        if (individual is null || individual.CategoryId != IndividualCategory.OwnedIndividual.Id) { return null; }

        var species = await speciesRepository.FindByIdAsync(individual.SpeciesId, cancellationToken);
        var speciesName = species?.Name ?? "不明";

        return new OwnedIndividualDetailDto(
            individual.Id.Value,
            individual.Name,
            speciesName,
            individual.SpeciesId.Value,
            individual.StatAlignmentId.Value,
            individual.AbilityId.Value,
            individual.StatPoints.Hp,
            individual.StatPoints.Attack,
            individual.StatPoints.Defense,
            individual.StatPoints.SpecialAttack,
            individual.StatPoints.SpecialDefense,
            individual.StatPoints.Speed,
            individual.Move1Id.Value,
            individual.Move2Id?.Value,
            individual.Move3Id?.Value,
            individual.Move4Id?.Value,
            individual.HeldItemId?.Value,
            individual.TeraTypeId.Value,
            individual.Memo
        );
    }

    public async Task<DeleteCheckResult> CheckDeleteAsync(
        string id,
        CancellationToken cancellationToken = default)
    {
        var isInParty = await partyRepository.IsIndividualInAnyPartyAsync(new IndividualId(id), cancellationToken);
        return new DeleteCheckResult(isInParty);
    }
}
