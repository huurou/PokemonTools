using Microsoft.EntityFrameworkCore;
using PokemonTools.Web.Infrastructure.Db.Abilities;
using PokemonTools.Web.Infrastructure.Db.Individuals;
using PokemonTools.Web.Infrastructure.Db.Items;
using PokemonTools.Web.Infrastructure.Db.Moves;
using PokemonTools.Web.Infrastructure.Db.Parties;
using PokemonTools.Web.Infrastructure.Db.Species;
using PokemonTools.Web.Infrastructure.Db.StatAlignments;
using PokemonTools.Web.Infrastructure.Db.Types;

namespace PokemonTools.Web.Infrastructure.Db;

public class PokemonToolsDbContext(DbContextOptions<PokemonToolsDbContext> options)
    : DbContext(options)
{
    public DbSet<TypeEntity> Types { get; set; }
    public DbSet<SpeciesEntity> Species { get; set; }
    public DbSet<MoveEntity> Moves { get; set; }
    public DbSet<MoveDamageClassEntity> MoveDamageClasses { get; set; }
    public DbSet<AbilityEntity> Abilities { get; set; }
    public DbSet<ItemEntity> Items { get; set; }
    public DbSet<StatAlignmentEntity> StatAlignments { get; set; }
    public DbSet<IndividualEntity> Individuals { get; set; }
    public DbSet<IndividualCategoryEntity> IndividualCategories { get; set; }
    public DbSet<PartyEntity> Parties { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PokemonToolsDbContext).Assembly);
    }
}
