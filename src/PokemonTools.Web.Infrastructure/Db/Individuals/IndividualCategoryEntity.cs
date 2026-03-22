using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PokemonTools.Web.Domain.Individuals;

namespace PokemonTools.Web.Infrastructure.Db.Individuals;

public class IndividualCategoryEntity : IHasUpdatedAt
{
    /// <summary>
    /// 個体カテゴリID
    /// </summary>
    public required int IndividualCategoryId { get; set; }

    /// <summary>
    /// 個体カテゴリ名
    /// </summary>
    public required string IndividualCategoryName { get; set; }

    /// <summary>
    /// 作成日時
    /// </summary>
    public DateTimeOffset CreatedAt { get; set; }

    /// <summary>
    /// 更新日時
    /// </summary>
    public DateTimeOffset UpdatedAt { get; set; }
}

public class IndividualCategoryEntityConfiguration : IEntityTypeConfiguration<IndividualCategoryEntity>
{
    public void Configure(EntityTypeBuilder<IndividualCategoryEntity> builder)
    {
        builder.ToTable(x => x.HasComment("個体カテゴリ"));

        builder.HasKey(x => x.IndividualCategoryId);
        builder.Property(x => x.IndividualCategoryId)
            .HasComment("個体カテゴリID 連番")
            .ValueGeneratedNever();

        builder.Property(x => x.IndividualCategoryName)
            .HasComment("個体カテゴリ名")
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .HasComment("作成日時")
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(x => x.UpdatedAt)
            .HasComment("更新日時")
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.HasData(IndividualCategory.All
            .OrderBy(x => x.Id.Value)
            .Select(x => new IndividualCategoryEntity
            {
                IndividualCategoryId = x.Id.Value,
                IndividualCategoryName = x.Name,
            })
        );
    }
}
