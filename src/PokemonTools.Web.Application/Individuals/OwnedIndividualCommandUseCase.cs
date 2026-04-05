using PokemonTools.Web.Domain.Individuals;

namespace PokemonTools.Web.Application.Individuals;

public class OwnedIndividualCommandUseCase(IIndividualRepository individualRepository)
{
    public async Task<IndividualId> RegisterAsync(
        RegisterOwnedIndividualCommand command,
        CancellationToken cancellationToken = default)
    {
        var id = new IndividualId($"ind_{Guid.CreateVersion7()}");
        var individual = new Individual(
            id,
            command.Name,
            command.SpeciesId,
            command.StatAlignmentId,
            command.AbilityId,
            command.StatPoints,
            command.Move1Id,
            command.Move2Id,
            command.Move3Id,
            command.Move4Id,
            command.HeldItemId,
            command.TeraTypeId,
            command.Memo,
            IndividualCategory.OwnedIndividual.Id
        );
        await individualRepository.AddAsync(individual, cancellationToken);
        return id;
    }

    public async Task UpdateAsync(
        UpdateOwnedIndividualCommand command,
        CancellationToken cancellationToken = default)
    {
        var existing = await individualRepository.FindByIdAsync(command.Id, cancellationToken);
        if (existing is null)
        {
            throw new InvalidOperationException($"個体が見つかりません: {command.Id.Value}");
        }

        var updated = new Individual(
            command.Id,
            command.Name,
            command.SpeciesId,
            command.StatAlignmentId,
            command.AbilityId,
            command.StatPoints,
            command.Move1Id,
            command.Move2Id,
            command.Move3Id,
            command.Move4Id,
            command.HeldItemId,
            command.TeraTypeId,
            command.Memo,
            existing.CategoryId
        );
        await individualRepository.UpdateAsync(updated, cancellationToken);
    }

    public async Task DeleteAsync(IndividualId id, CancellationToken cancellationToken = default)
    {
        await individualRepository.DeleteAsync(id, cancellationToken);
    }
}
