using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace PokemonTools.Web.Infrastructure.Db;

public class DesignTimePokemonToolsDbContextFactory : IDesignTimeDbContextFactory<PokemonToolsDbContext>
{
    public PokemonToolsDbContext CreateDbContext(string[] args)
    {
        // dotnet ef の実行ディレクトリ（repo root または src/PokemonTools.Web.Infrastructure）のどちらからでも動作するようにする
        var currentDir = Directory.GetCurrentDirectory();
        var appHostDir = Path.Combine(currentDir, "src", "PokemonTools.AppHost");
        if (!Directory.Exists(appHostDir))
        {
            appHostDir = Path.Combine(currentDir, "../PokemonTools.AppHost");
        }

        var configuration = new ConfigurationBuilder()
            .SetBasePath(appHostDir)
            .AddJsonFile("appsettings.Development.json")
            .Build();

        var connectionString = configuration.GetConnectionString("pokemon-tools-db");

        var optionsBuilder = new DbContextOptionsBuilder<PokemonToolsDbContext>();
        optionsBuilder.UseNpgsql(connectionString);

        return new PokemonToolsDbContext(optionsBuilder.Options);
    }
}
