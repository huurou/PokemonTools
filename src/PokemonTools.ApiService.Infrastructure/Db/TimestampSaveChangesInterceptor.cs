using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace PokemonTools.ApiService.Infrastructure.Db;

public sealed class TimestampSaveChangesInterceptor : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData,
        InterceptionResult<int> result
    )
    {
        Apply(eventData.Context);
        return result;
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default
    )
    {
        Apply(eventData.Context);
        return ValueTask.FromResult(result);
    }

    private static void Apply(DbContext? context)
    {
        if (context is null) { return; }

        var now = DateTimeOffset.UtcNow;
        foreach (var entry in context.ChangeTracker.Entries<IHasUpdatedAt>())
        {
            if (entry.State is EntityState.Added or EntityState.Modified)
            {
                entry.Entity.UpdatedAt = now;
            }
        }
    }
}
