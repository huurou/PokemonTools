using Microsoft.EntityFrameworkCore;
using PokemonTools.Web.Domain.Items;
using PokemonTools.Web.Infrastructure.Db;
using PokemonTools.Web.Infrastructure.Db.Items;

namespace PokemonTools.Web.Infrastructure.Items;

public class ItemRepository(PokemonToolsDbContext context) : IItemRepository
{
    public async Task UpsertRangeAsync(List<Item> items, CancellationToken cancellationToken = default)
    {
        if (items.Count == 0) { return; }

        var normalized = items
            .GroupBy(x => x.Id.Value)
            .Select(x => x.Last())
            .ToList();

        var ids = normalized.Select(x => x.Id.Value).ToList();
        var existing = await context.Items
            .Where(x => ids.Contains(x.ItemId))
            .ToDictionaryAsync(x => x.ItemId, cancellationToken);

        foreach (var item in normalized)
        {
            if (!existing.TryGetValue(item.Id.Value, out var entity))
            {
                entity = new ItemEntity
                {
                    ItemId = item.Id.Value,
                    ItemName = item.Name,
                    FlingPower = (int?)item.FlingPower,
                };
                existing.Add(entity.ItemId, entity);
                context.Items.Add(entity);
            }

            entity.ItemName = item.Name;
            entity.FlingPower = (int?)item.FlingPower;
        }

        await context.SaveChangesAsync(cancellationToken);
        context.ChangeTracker.Clear();
    }
}
