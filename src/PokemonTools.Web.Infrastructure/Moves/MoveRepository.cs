using Microsoft.EntityFrameworkCore;
using PokemonTools.Web.Domain.Moves;
using PokemonTools.Web.Infrastructure.Db;
using PokemonTools.Web.Infrastructure.Db.Moves;

namespace PokemonTools.Web.Infrastructure.Moves;

public class MoveRepository(PokemonToolsDbContext context) : IMoveRepository
{
    public async Task UpsertRangeAsync(List<Move> moves, CancellationToken cancellationToken = default)
    {
        if (moves.Count == 0) { return; }

        var normalized = moves
            .GroupBy(x => x.Id.Value)
            .Select(x => x.Last())
            .ToList();

        var ids = normalized.Select(x => x.Id.Value).ToList();
        var existing = await context.Moves
            .Where(x => ids.Contains(x.MoveId))
            .ToDictionaryAsync(x => x.MoveId, cancellationToken);

        foreach (var move in normalized)
        {
            if (!existing.TryGetValue(move.Id.Value, out var entity))
            {
                entity = new MoveEntity
                {
                    MoveId = move.Id.Value,
                    MoveName = move.Name,
                    TypeId = move.TypeId.Value,
                    MoveDamageClassId = move.DamageClassId.Value,
                    Power = (int?)move.Power,
                };
                existing.Add(entity.MoveId, entity);
                context.Moves.Add(entity);
            }
            else
            {
                entity.MoveName = move.Name;
                entity.TypeId = move.TypeId.Value;
                entity.MoveDamageClassId = move.DamageClassId.Value;
                entity.Power = (int?)move.Power;
            }
        }

        await context.SaveChangesAsync(cancellationToken);
        context.ChangeTracker.Clear();
    }
}
