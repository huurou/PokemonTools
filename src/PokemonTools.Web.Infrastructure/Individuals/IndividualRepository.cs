using Microsoft.EntityFrameworkCore;
using PokemonTools.Web.Domain.Abilities;
using PokemonTools.Web.Domain.Individuals;
using PokemonTools.Web.Domain.Items;
using PokemonTools.Web.Domain.Moves;
using PokemonTools.Web.Domain.Species;
using PokemonTools.Web.Domain.Statistics;
using PokemonTools.Web.Domain.Types;
using PokemonTools.Web.Infrastructure.Db;
using PokemonTools.Web.Infrastructure.Db.Individuals;

namespace PokemonTools.Web.Infrastructure.Individuals;

public class IndividualRepository(PokemonToolsDbContext context) : IIndividualRepository
{
    public async Task<List<Individual>> GetOwnedIndividualsAsync(CancellationToken cancellationToken = default)
    {
        var entities = await context.Individuals
            .Where(x => x.CategoryId == IndividualCategory.OwnedIndividual.Id.Value)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync(cancellationToken);
        return entities.Select(ToDomain).ToList();
    }

    public async Task<Individual?> FindByIdAsync(IndividualId id, CancellationToken cancellationToken = default)
    {
        var entity = await context.Individuals
            .SingleOrDefaultAsync(x => x.IndividualId == id.Value, cancellationToken);
        return entity is not null ? ToDomain(entity) : null;
    }

    public async Task AddAsync(Individual individual, CancellationToken cancellationToken = default)
    {
        var entity = ToEntity(individual);
        context.Individuals.Add(entity);
        await context.SaveChangesAsync(cancellationToken);
        context.ChangeTracker.Clear();
    }

    public async Task UpdateAsync(Individual individual, CancellationToken cancellationToken = default)
    {
        var entity = await context.Individuals
            .SingleAsync(x => x.IndividualId == individual.Id.Value, cancellationToken);

        entity.IndividualName = individual.Name;
        entity.SpeciesId = individual.SpeciesId.Value;
        entity.StatAlignmentId = individual.StatAlignmentId.Value;
        entity.AbilityId = individual.AbilityId.Value;
        entity.StatPointHp = (int)individual.StatPoints.Hp;
        entity.StatPointAttack = (int)individual.StatPoints.Attack;
        entity.StatPointDefense = (int)individual.StatPoints.Defense;
        entity.StatPointSpecialAttack = (int)individual.StatPoints.SpecialAttack;
        entity.StatPointSpecialDefense = (int)individual.StatPoints.SpecialDefense;
        entity.StatPointSpeed = (int)individual.StatPoints.Speed;
        entity.Move1Id = individual.Move1Id.Value;
        entity.Move2Id = individual.Move2Id?.Value;
        entity.Move3Id = individual.Move3Id?.Value;
        entity.Move4Id = individual.Move4Id?.Value;
        entity.HeldItemId = individual.HeldItemId?.Value;
        entity.TeraTypeId = individual.TeraTypeId.Value;
        entity.Memo = individual.Memo;
        entity.CategoryId = individual.CategoryId.Value;

        await context.SaveChangesAsync(cancellationToken);
        context.ChangeTracker.Clear();
    }

    public async Task DeleteAsync(IndividualId id, CancellationToken cancellationToken = default)
    {
        await context.Individuals
            .Where(x => x.IndividualId == id.Value)
            .ExecuteDeleteAsync(cancellationToken);
        context.ChangeTracker.Clear();
    }

    private static Individual ToDomain(IndividualEntity x)
    {
        return new Individual(
            new IndividualId(x.IndividualId),
            x.IndividualName,
            new SpeciesId(x.SpeciesId),
            new StatAlignmentId(x.StatAlignmentId),
            new AbilityId(x.AbilityId),
            new StatPoints((uint)x.StatPointHp, (uint)x.StatPointAttack, (uint)x.StatPointDefense,
                (uint)x.StatPointSpecialAttack, (uint)x.StatPointSpecialDefense, (uint)x.StatPointSpeed),
            new MoveId(x.Move1Id),
            x.Move2Id is not null ? new MoveId(x.Move2Id.Value) : null,
            x.Move3Id is not null ? new MoveId(x.Move3Id.Value) : null,
            x.Move4Id is not null ? new MoveId(x.Move4Id.Value) : null,
            x.HeldItemId is not null ? new ItemId(x.HeldItemId.Value) : null,
            new TypeId(x.TeraTypeId),
            x.Memo,
            new IndividualCategoryId(x.CategoryId)
        );
    }

    private static IndividualEntity ToEntity(Individual x)
    {
        return new IndividualEntity
        {
            IndividualId = x.Id.Value,
            IndividualName = x.Name,
            SpeciesId = x.SpeciesId.Value,
            StatAlignmentId = x.StatAlignmentId.Value,
            AbilityId = x.AbilityId.Value,
            StatPointHp = (int)x.StatPoints.Hp,
            StatPointAttack = (int)x.StatPoints.Attack,
            StatPointDefense = (int)x.StatPoints.Defense,
            StatPointSpecialAttack = (int)x.StatPoints.SpecialAttack,
            StatPointSpecialDefense = (int)x.StatPoints.SpecialDefense,
            StatPointSpeed = (int)x.StatPoints.Speed,
            Move1Id = x.Move1Id.Value,
            Move2Id = x.Move2Id?.Value,
            Move3Id = x.Move3Id?.Value,
            Move4Id = x.Move4Id?.Value,
            HeldItemId = x.HeldItemId?.Value,
            TeraTypeId = x.TeraTypeId.Value,
            Memo = x.Memo,
            CategoryId = x.CategoryId.Value,
        };
    }
}
