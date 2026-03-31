using Microsoft.EntityFrameworkCore;
using PokemonTools.Web.Domain.Abilities;
using PokemonTools.Web.Infrastructure.Db;
using PokemonTools.Web.Infrastructure.Db.Abilities;

namespace PokemonTools.Web.Infrastructure.Abilities;

public class AbilityRepository(PokemonToolsDbContext context) : IAbilityRepository
{
    public async Task UpsertRangeAsync(List<Ability> abilities, CancellationToken cancellationToken = default)
    {
        if (abilities.Count == 0) { return; }

        var normalized = abilities
            .GroupBy(x => x.Id.Value)
            .Select(x => x.Last())
            .ToList();

        var ids = normalized.Select(x => x.Id.Value).ToList();
        var existing = await context.Abilities
            .Where(x => ids.Contains(x.AbilityId))
            .ToDictionaryAsync(x => x.AbilityId, cancellationToken);

        foreach (var ability in normalized)
        {
            if (!existing.TryGetValue(ability.Id.Value, out var entity))
            {
                entity = new AbilityEntity
                {
                    AbilityId = ability.Id.Value,
                    AbilityName = ability.Name,
                };
                existing.Add(entity.AbilityId, entity);
                context.Abilities.Add(entity);
            }

            entity.AbilityName = ability.Name;
        }

        await context.SaveChangesAsync(cancellationToken);
        context.ChangeTracker.Clear();
    }
}
