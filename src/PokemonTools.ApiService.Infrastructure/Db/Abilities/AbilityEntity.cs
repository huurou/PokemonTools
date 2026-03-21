using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PokemonTools.ApiService.Infrastructure.Db.Abilities;

public class AbilityEntity : IHasUpdatedAt
{
    /// <summary>
    /// 特性ID
    /// </summary>
    public required int AbilityId { get; set; }

    /// <summary>
    /// 特性名
    /// </summary>
    public required string AbilityName { get; set; }

    /// <summary>
    /// 作成日時
    /// </summary>
    public DateTimeOffset CreatedAt { get; set; }

    /// <summary>
    /// 更新日時
    /// </summary>
    public DateTimeOffset UpdatedAt { get; set; }
}

public class AbilityEntityConfiguration : IEntityTypeConfiguration<AbilityEntity>
{
    public void Configure(EntityTypeBuilder<AbilityEntity> builder)
    {
        builder.ToTable(x => x.HasComment("特性"));

        builder.HasKey(x => x.AbilityId);
        builder.Property(x => x.AbilityId)
            .HasComment("特性ID PokeAPIでのid")
            .ValueGeneratedNever();

        builder.Property(x => x.AbilityName)
            .HasComment("特性名 日本語名")
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .HasComment("作成日時")
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(x => x.UpdatedAt)
            .HasComment("更新日時")
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
    }
}
