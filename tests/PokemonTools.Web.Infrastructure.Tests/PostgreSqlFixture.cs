using Microsoft.EntityFrameworkCore;
using PokemonTools.Web.Infrastructure.Db;
using Testcontainers.PostgreSql;

namespace PokemonTools.Web.Infrastructure.Tests;

public class PostgreSqlFixture : IAsyncLifetime
{
    private readonly PostgreSqlContainer container_ = new PostgreSqlBuilder("postgres:18").Build();

    public string ConnectionString => container_.GetConnectionString();

    public async ValueTask InitializeAsync()
    {
        await container_.StartAsync();

        await using var context = CreateContext();
        await context.Database.MigrateAsync();
    }

    public PokemonToolsDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<PokemonToolsDbContext>()
            .UseNpgsql(ConnectionString)
            .AddInterceptors(new TimestampSaveChangesInterceptor(TimeProvider.System))
            .Options;
        return new PokemonToolsDbContext(options);
    }

    public async ValueTask DisposeAsync()
    {
        await container_.DisposeAsync();
        GC.SuppressFinalize(this);
    }
}
