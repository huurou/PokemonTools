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
        var individual = CreateIndividual(
            id, command.Name,
            command.SpeciesId, command.StatAlignmentId, command.AbilityId,
            command.StatPointHp, command.StatPointAttack, command.StatPointDefense,
            command.StatPointSpecialAttack, command.StatPointSpecialDefense, command.StatPointSpeed,
            command.Move1Id, command.Move2Id, command.Move3Id, command.Move4Id,
            command.HeldItemId, command.TeraTypeId, command.Memo,
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

        var updated = CreateIndividual(
            individualId, command.Name,
            command.SpeciesId, command.StatAlignmentId, command.AbilityId,
            command.StatPointHp, command.StatPointAttack, command.StatPointDefense,
            command.StatPointSpecialAttack, command.StatPointSpecialDefense, command.StatPointSpeed,
            command.Move1Id, command.Move2Id, command.Move3Id, command.Move4Id,
            command.HeldItemId, command.TeraTypeId, command.Memo,
            existing.CategoryId);
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

    private static Individual CreateIndividual(
        IndividualId id, string? name,
        int speciesId, int statAlignmentId, int abilityId,
        uint statPointHp, uint statPointAttack, uint statPointDefense,
        uint statPointSpecialAttack, uint statPointSpecialDefense, uint statPointSpeed,
        int move1Id, int? move2Id, int? move3Id, int? move4Id,
        int? heldItemId, int teraTypeId, string? memo,
        IndividualCategoryId categoryId)
    {
        return new Individual(
            id, name,
            new SpeciesId(speciesId),
            new StatAlignmentId(statAlignmentId),
            new AbilityId(abilityId),
            new StatPoints(statPointHp, statPointAttack, statPointDefense,
                statPointSpecialAttack, statPointSpecialDefense, statPointSpeed),
            new MoveId(move1Id),
            move2Id is not null ? new MoveId(move2Id.Value) : null,
            move3Id is not null ? new MoveId(move3Id.Value) : null,
            move4Id is not null ? new MoveId(move4Id.Value) : null,
            heldItemId is not null ? new ItemId(heldItemId.Value) : null,
            new TypeId(teraTypeId),
            memo, categoryId);
    }
}
