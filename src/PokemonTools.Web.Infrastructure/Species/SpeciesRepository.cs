using Microsoft.EntityFrameworkCore;
using PokemonTools.Web.Domain.Abilities;
using PokemonTools.Web.Domain.Species;
using PokemonTools.Web.Domain.Statistics;
using PokemonTools.Web.Domain.Types;
using PokemonTools.Web.Infrastructure.Db;
using PokemonTools.Web.Infrastructure.Db.Species;

namespace PokemonTools.Web.Infrastructure.Species;

public class SpeciesRepository(PokemonToolsDbContext context) : ISpeciesRepository
{
    public async Task<List<PokemonSpecies>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var entities = await context.Species
            .OrderBy(x => x.SpeciesId)
            .ToListAsync(cancellationToken);
        return entities.Select(ToDomain).ToList();
    }

    public async Task<PokemonSpecies?> FindByIdAsync(SpeciesId id, CancellationToken cancellationToken = default)
    {
        var entity = await context.Species.FindAsync([id.Value], cancellationToken);
        return entity is not null ? ToDomain(entity) : null;
    }

    public async Task<List<PokemonSpecies>> GetByIdsAsync(IEnumerable<SpeciesId> ids, CancellationToken cancellationToken = default)
    {
        var idValues = ids.Select(x => x.Value).ToList();
        var entities = await context.Species
            .Where(x => idValues.Contains(x.SpeciesId))
            .ToListAsync(cancellationToken);
        return entities.Select(ToDomain).ToList();
    }

    private static PokemonSpecies ToDomain(SpeciesEntity x)
    {
        return new PokemonSpecies(
            new SpeciesId(x.SpeciesId),
            x.SpeciesName,
            new TypeId(x.Type1Id),
            x.Type2Id is not null ? new TypeId(x.Type2Id.Value) : null,
            new AbilityId(x.Ability1Id),
            x.Ability2Id is not null ? new AbilityId(x.Ability2Id.Value) : null,
            x.HiddenAbilityId is not null ? new AbilityId(x.HiddenAbilityId.Value) : null,
            new BaseStats((uint)x.BaseStatHp, (uint)x.BaseStatAttack, (uint)x.BaseStatDefense,
                (uint)x.BaseStatSpecialAttack, (uint)x.BaseStatSpecialDefense, (uint)x.BaseStatSpeed),
            new Weight(x.Weight)
        );
    }

    public async Task UpsertRangeAsync(List<PokemonSpecies> species, CancellationToken cancellationToken = default)
    {
        if (species.Count == 0) { return; }

        var normalized = species
            .GroupBy(x => x.Id.Value)
            .Select(x => x.Last())
            .ToList();

        var ids = normalized.Select(x => x.Id.Value).ToList();
        var existing = await context.Species
            .Where(x => ids.Contains(x.SpeciesId))
            .ToDictionaryAsync(x => x.SpeciesId, cancellationToken);

        foreach (var sp in normalized)
        {
            if (!existing.TryGetValue(sp.Id.Value, out var entity))
            {
                entity = new SpeciesEntity
                {
                    SpeciesId = sp.Id.Value,
                    SpeciesName = sp.Name,
                    Type1Id = sp.Type1Id.Value,
                    Type2Id = sp.Type2Id?.Value,
                    Ability1Id = sp.Ability1Id.Value,
                    Ability2Id = sp.Ability2Id?.Value,
                    HiddenAbilityId = sp.HiddenAbilityId?.Value,
                    BaseStatHp = (int)sp.BaseStats.Hp,
                    BaseStatAttack = (int)sp.BaseStats.Attack,
                    BaseStatDefense = (int)sp.BaseStats.Defense,
                    BaseStatSpecialAttack = (int)sp.BaseStats.SpecialAttack,
                    BaseStatSpecialDefense = (int)sp.BaseStats.SpecialDefense,
                    BaseStatSpeed = (int)sp.BaseStats.Speed,
                    Weight = sp.Weight.Hectograms,
                };
                existing.Add(entity.SpeciesId, entity);
                context.Species.Add(entity);
            }
            else
            {
                entity.SpeciesName = sp.Name;
                entity.Type1Id = sp.Type1Id.Value;
                entity.Type2Id = sp.Type2Id?.Value;
                entity.Ability1Id = sp.Ability1Id.Value;
                entity.Ability2Id = sp.Ability2Id?.Value;
                entity.HiddenAbilityId = sp.HiddenAbilityId?.Value;
                entity.BaseStatHp = (int)sp.BaseStats.Hp;
                entity.BaseStatAttack = (int)sp.BaseStats.Attack;
                entity.BaseStatDefense = (int)sp.BaseStats.Defense;
                entity.BaseStatSpecialAttack = (int)sp.BaseStats.SpecialAttack;
                entity.BaseStatSpecialDefense = (int)sp.BaseStats.SpecialDefense;
                entity.BaseStatSpeed = (int)sp.BaseStats.Speed;
                entity.Weight = sp.Weight.Hectograms;
            }
        }

        await context.SaveChangesAsync(cancellationToken);
        context.ChangeTracker.Clear();
    }
}
