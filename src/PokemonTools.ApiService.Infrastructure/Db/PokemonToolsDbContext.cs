using Microsoft.EntityFrameworkCore;
using PokemonTools.ApiService.Infrastructure.Db.Abilities;
using PokemonTools.ApiService.Infrastructure.Db.Individuals;
using PokemonTools.ApiService.Infrastructure.Db.Items;
using PokemonTools.ApiService.Infrastructure.Db.Moves;
using PokemonTools.ApiService.Infrastructure.Db.Natures;
using PokemonTools.ApiService.Infrastructure.Db.Parties;
using PokemonTools.ApiService.Infrastructure.Db.Species;
using PokemonTools.ApiService.Infrastructure.Db.Types;

namespace PokemonTools.ApiService.Infrastructure.Db;

public class PokemonToolsDbContext(DbContextOptions<PokemonToolsDbContext> options)
    : DbContext(options)
{
    public DbSet<TypeEntity> Types { get; set; }
    public DbSet<SpeciesEntity> Species { get; set; }
    public DbSet<MoveEntity> Moves { get; set; }
    public DbSet<MoveDamageClassEntity> MoveDamageClasses { get; set; }
    public DbSet<AbilityEntity> Abilities { get; set; }
    public DbSet<ItemEntity> Items { get; set; }
    public DbSet<NatureEntity> Natures { get; set; }
    public DbSet<IndividualEntity> Individuals { get; set; }
    public DbSet<IndividualCategoryEntity> IndividualCategories { get; set; }
    public DbSet<PartyEntity> Parties { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PokemonToolsDbContext).Assembly);
    }
}
