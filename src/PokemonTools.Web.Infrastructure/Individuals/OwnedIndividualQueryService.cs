using Microsoft.EntityFrameworkCore;
using PokemonTools.Web.Application.Individuals;
using PokemonTools.Web.Domain.Individuals;
using PokemonTools.Web.Infrastructure.Db;

namespace PokemonTools.Web.Infrastructure.Individuals;

public class OwnedIndividualQueryService(PokemonToolsDbContext context) : IOwnedIndividualQueryService
{
    public async Task<List<OwnedIndividualListItemDto>> GetListAsync(CancellationToken cancellationToken = default)
    {
        return await context.Individuals.AsNoTracking()
            .Where(x => x.CategoryId == IndividualCategory.OwnedIndividual.Id.Value)
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => new OwnedIndividualListItemDto(
                x.IndividualId,
                x.IndividualName ?? x.Species.SpeciesName,
                x.Species.SpeciesName))
            .ToListAsync(cancellationToken);
    }

    public async Task<OwnedIndividualDetailDto?> GetDetailAsync(
        string id,
        CancellationToken cancellationToken = default)
    {
        var row = await context.Individuals.AsNoTracking()
            .Where(x => x.IndividualId == id && x.CategoryId == IndividualCategory.OwnedIndividual.Id.Value)
            .Select(x => new
            {
                x.IndividualId,
                x.IndividualName,
                x.Species.SpeciesName,
                x.SpeciesId,
                x.StatAlignmentId,
                x.AbilityId,
                x.StatPointHp,
                x.StatPointAttack,
                x.StatPointDefense,
                x.StatPointSpecialAttack,
                x.StatPointSpecialDefense,
                x.StatPointSpeed,
                x.Move1Id,
                x.Move2Id,
                x.Move3Id,
                x.Move4Id,
                x.HeldItemId,
                x.TeraTypeId,
                x.Memo,
            })
            .SingleOrDefaultAsync(cancellationToken);

        if (row is null) { return null; }

        return new OwnedIndividualDetailDto(
            row.IndividualId,
            row.IndividualName,
            row.SpeciesName,
            row.SpeciesId,
            row.StatAlignmentId,
            row.AbilityId,
            (uint)row.StatPointHp,
            (uint)row.StatPointAttack,
            (uint)row.StatPointDefense,
            (uint)row.StatPointSpecialAttack,
            (uint)row.StatPointSpecialDefense,
            (uint)row.StatPointSpeed,
            row.Move1Id,
            row.Move2Id,
            row.Move3Id,
            row.Move4Id,
            row.HeldItemId,
            row.TeraTypeId,
            row.Memo);
    }

    public async Task<DeleteCheckResult> CheckDeleteAsync(
        string id,
        CancellationToken cancellationToken = default)
    {
        var isInParty = await context.Parties.AnyAsync(x =>
            x.Individual1Id == id || x.Individual2Id == id || x.Individual3Id == id ||
            x.Individual4Id == id || x.Individual5Id == id || x.Individual6Id == id,
            cancellationToken);
        return new DeleteCheckResult(isInParty);
    }
}
