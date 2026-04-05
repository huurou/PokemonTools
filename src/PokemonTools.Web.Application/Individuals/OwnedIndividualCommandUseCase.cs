using PokemonTools.Web.Domain.Abilities;
using PokemonTools.Web.Domain.Individuals;
using PokemonTools.Web.Domain.Items;
using PokemonTools.Web.Domain.Moves;
using PokemonTools.Web.Domain.Species;
using PokemonTools.Web.Domain.Statistics;
using PokemonTools.Web.Domain.Types;

namespace PokemonTools.Web.Application.Individuals;

public class OwnedIndividualCommandUseCase(IIndividualRepository individualRepository)
{
    public async Task<string> RegisterAsync(
        RegisterOwnedIndividualCommand command,
        CancellationToken cancellationToken = default)
    {
        var id = new IndividualId($"ind_{Guid.CreateVersion7()}");
        var individual = new Individual(
            id,
            command.Name,
            new SpeciesId(command.SpeciesId),
            new StatAlignmentId(command.StatAlignmentId),
            new AbilityId(command.AbilityId),
            new StatPoints(command.StatPointHp, command.StatPointAttack, command.StatPointDefense,
                command.StatPointSpecialAttack, command.StatPointSpecialDefense, command.StatPointSpeed),
            new MoveId(command.Move1Id),
            command.Move2Id is not null ? new MoveId(command.Move2Id.Value) : null,
            command.Move3Id is not null ? new MoveId(command.Move3Id.Value) : null,
            command.Move4Id is not null ? new MoveId(command.Move4Id.Value) : null,
            command.HeldItemId is not null ? new ItemId(command.HeldItemId.Value) : null,
            new TypeId(command.TeraTypeId),
            command.Memo,
            IndividualCategory.OwnedIndividual.Id);
        await individualRepository.AddAsync(individual, cancellationToken);
        return id.Value;
    }

    public async Task UpdateAsync(
        UpdateOwnedIndividualCommand command,
        CancellationToken cancellationToken = default)
    {
        var individualId = new IndividualId(command.Id);
        var existing = await individualRepository.FindByIdAsync(individualId, cancellationToken);
        if (existing is null)
        {
            throw new InvalidOperationException($"個体が見つかりません: {command.Id}");
        }
        if (existing.CategoryId != IndividualCategory.OwnedIndividual.Id)
        {
            throw new InvalidOperationException($"手持ち個体以外は更新できません: {command.Id}");
        }

        var updated = existing.Update(
            command.Name,
            new SpeciesId(command.SpeciesId),
            new StatAlignmentId(command.StatAlignmentId),
            new AbilityId(command.AbilityId),
            new StatPoints(command.StatPointHp, command.StatPointAttack, command.StatPointDefense,
                command.StatPointSpecialAttack, command.StatPointSpecialDefense, command.StatPointSpeed),
            new MoveId(command.Move1Id),
            command.Move2Id is not null ? new MoveId(command.Move2Id.Value) : null,
            command.Move3Id is not null ? new MoveId(command.Move3Id.Value) : null,
            command.Move4Id is not null ? new MoveId(command.Move4Id.Value) : null,
            command.HeldItemId is not null ? new ItemId(command.HeldItemId.Value) : null,
            new TypeId(command.TeraTypeId),
            command.Memo);
        await individualRepository.UpdateAsync(updated, cancellationToken);
    }

    public async Task DeleteAsync(string id, CancellationToken cancellationToken = default)
    {
        var individualId = new IndividualId(id);
        var existing = await individualRepository.FindByIdAsync(individualId, cancellationToken)
            ?? throw new InvalidOperationException($"個体が見つかりません: {id}");
        if (existing.CategoryId != IndividualCategory.OwnedIndividual.Id)
        {
            throw new InvalidOperationException($"手持ち個体以外は削除できません: {id}");
        }
        await individualRepository.DeleteAsync(individualId, cancellationToken);
    }
}
