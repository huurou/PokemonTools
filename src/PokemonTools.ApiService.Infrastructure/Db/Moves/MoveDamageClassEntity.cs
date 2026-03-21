using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PokemonTools.ApiService.Domain.Moves;

namespace PokemonTools.ApiService.Infrastructure.Db.Moves;

public class MoveDamageClassEntity : IHasUpdatedAt
{
    /// <summary>
    /// 技分類ID
    /// </summary>
    public required int MoveDamageClassId { get; set; }

    /// <summary>
    /// 技分類名
    /// </summary>
    public required string MoveDamageClassName { get; set; }

    /// <summary>
    /// 作成日時
    /// </summary>
    public DateTimeOffset CreatedAt { get; set; }

    /// <summary>
    /// 更新日時
    /// </summary>
    public DateTimeOffset UpdatedAt { get; set; }
}

public class MoveDamageClassEntityConfiguration : IEntityTypeConfiguration<MoveDamageClassEntity>
{
    public void Configure(EntityTypeBuilder<MoveDamageClassEntity> builder)
    {
        builder.ToTable(x => x.HasComment("技分類"));

        builder.HasKey(x => x.MoveDamageClassId);
        builder.Property(x => x.MoveDamageClassId)
            .HasComment("技分類ID PokeAPIでのid")
            .ValueGeneratedNever();

        builder.Property(x => x.MoveDamageClassName)
            .HasComment("技分類名 日本語名")
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .HasComment("作成日時")
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(x => x.UpdatedAt)
            .HasComment("更新日時")
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.HasData(MoveDamageClass.All
            .OrderBy(x => x.Id.Value)
            .Select(x => new MoveDamageClassEntity
            {
                MoveDamageClassId = x.Id.Value,
                MoveDamageClassName = x.Name,
            })
        );
    }
}
