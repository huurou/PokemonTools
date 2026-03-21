using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PokemonTools.ApiService.Domain.Statistics;

namespace PokemonTools.ApiService.Infrastructure.Db.Natures;

public class NatureEntity : IHasUpdatedAt
{
    /// <summary>
    /// 性格ID
    /// </summary>
    public required int NatureId { get; set; }

    /// <summary>
    /// 性格名
    /// </summary>
    public required string NatureName { get; set; }

    /// <summary>
    /// 作成日時
    /// </summary>
    public DateTimeOffset CreatedAt { get; set; }

    /// <summary>
    /// 更新日時
    /// </summary>
    public DateTimeOffset UpdatedAt { get; set; }
}

public class NatureEntityConfiguration : IEntityTypeConfiguration<NatureEntity>
{
    public void Configure(EntityTypeBuilder<NatureEntity> builder)
    {
        builder.ToTable(x => x.HasComment("性格"));

        builder.HasKey(x => x.NatureId);
        builder.Property(x => x.NatureId)
            .HasComment("性格ID PokeAPIでのid")
            .ValueGeneratedNever();

        builder.Property(x => x.NatureName)
            .HasComment("性格名 日本語名")
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .HasComment("作成日時")
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(x => x.UpdatedAt)
            .HasComment("更新日時")
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.HasData(Nature.All
            .OrderBy(x => x.Id.Value)
            .Select(x => new NatureEntity
            {
                NatureId = x.Id.Value,
                NatureName = x.Name,
            })
        );
    }
}
