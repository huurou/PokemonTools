using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PokemonTools.Web.Infrastructure.Db.Items;

public class ItemEntity : IHasUpdatedAt
{
    /// <summary>
    /// 道具ID
    /// </summary>
    public required int ItemId { get; set; }

    /// <summary>
    /// 道具名
    /// </summary>
    public required string ItemName { get; set; }

    /// <summary>
    /// 投げつけるの威力 投げつけるが失敗する道具はnull
    /// </summary>
    public int? FlingPower { get; set; }

    /// <summary>
    /// 作成日時
    /// </summary>
    public DateTimeOffset CreatedAt { get; set; }

    /// <summary>
    /// 更新日時
    /// </summary>
    public DateTimeOffset UpdatedAt { get; set; }
}

public class ItemEntityConfiguration : IEntityTypeConfiguration<ItemEntity>
{
    public void Configure(EntityTypeBuilder<ItemEntity> builder)
    {
        builder.ToTable(x => x.HasComment("道具"));

        builder.HasKey(x => x.ItemId);
        builder.Property(x => x.ItemId)
            .HasComment("道具ID PokeAPIでのid")
            .ValueGeneratedNever();

        builder.Property(x => x.ItemName)
            .HasComment("道具名 日本語名")
            .IsRequired();

        builder.Property(x => x.FlingPower)
            .HasComment("投げつけるの威力 投げつけるが失敗する道具はNULL");

        builder.Property(x => x.CreatedAt)
            .HasComment("作成日時")
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(x => x.UpdatedAt)
            .HasComment("更新日時")
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
    }
}
