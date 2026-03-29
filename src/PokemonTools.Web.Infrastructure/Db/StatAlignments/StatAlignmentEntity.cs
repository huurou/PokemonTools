using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PokemonTools.Web.Domain.Statistics;

namespace PokemonTools.Web.Infrastructure.Db.StatAlignments;

public class StatAlignmentEntity : IHasUpdatedAt
{
    /// <summary>
    /// 能力補正ID
    /// </summary>
    public required int StatAlignmentId { get; set; }

    /// <summary>
    /// 能力補正名
    /// </summary>
    public required string StatAlignmentName { get; set; }

    /// <summary>
    /// 作成日時
    /// </summary>
    public DateTimeOffset CreatedAt { get; set; }

    /// <summary>
    /// 更新日時
    /// </summary>
    public DateTimeOffset UpdatedAt { get; set; }
}

public class StatAlignmentEntityConfiguration : IEntityTypeConfiguration<StatAlignmentEntity>
{
    public void Configure(EntityTypeBuilder<StatAlignmentEntity> builder)
    {
        builder.ToTable(x => x.HasComment("能力補正"));

        builder.HasKey(x => x.StatAlignmentId);
        builder.Property(x => x.StatAlignmentId)
            .HasComment("能力補正ID PokeAPIでのid")
            .ValueGeneratedNever();

        builder.Property(x => x.StatAlignmentName)
            .HasComment("能力補正名 日本語名")
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .HasComment("作成日時")
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(x => x.UpdatedAt)
            .HasComment("更新日時")
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.HasData(StatAlignment.All
            .OrderBy(x => x.Id.Value)
            .Select(x => new StatAlignmentEntity
            {
                StatAlignmentId = x.Id.Value,
                StatAlignmentName = x.Name,
            })
        );
    }
}
