using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PokemonTools.Web.Infrastructure.Db.Types;

namespace PokemonTools.Web.Infrastructure.Db.Moves;

public class MoveEntity : IHasUpdatedAt
{
    /// <summary>
    /// 技ID
    /// </summary>
    public required int MoveId { get; set; }

    /// <summary>
    /// 技名
    /// </summary>
    public required string MoveName { get; set; }

    /// <summary>
    /// タイプID
    /// </summary>
    public required int TypeId { get; set; }

    /// <summary>
    /// 技分類ID
    /// </summary>
    public required int MoveDamageClassId { get; set; }

    /// <summary>
    /// 威力
    /// </summary>
    public int? Power { get; set; }

    /// <summary>
    /// 作成日時
    /// </summary>
    public DateTimeOffset CreatedAt { get; set; }

    /// <summary>
    /// 更新日時
    /// </summary>
    public DateTimeOffset UpdatedAt { get; set; }

    #region ナビゲーションプロパティ

    /// <summary>
    /// タイプ
    /// </summary>
    public TypeEntity Type { get; set; } = null!;

    /// <summary>
    /// 技分類
    /// </summary>
    public MoveDamageClassEntity MoveDamageClass { get; set; } = null!;

    #endregion ナビゲーションプロパティ
}

public class MoveEntityConfiguration : IEntityTypeConfiguration<MoveEntity>
{
    public void Configure(EntityTypeBuilder<MoveEntity> builder)
    {
        builder.ToTable(x => x.HasComment("技"));

        builder.HasKey(x => x.MoveId);
        builder.Property(x => x.MoveId)
            .HasComment("技ID PokeAPIでのid")
            .ValueGeneratedNever();

        builder.Property(x => x.MoveName)
            .HasComment("技名 日本語名")
            .IsRequired();

        builder.Property(x => x.TypeId)
            .HasComment("タイプID");

        builder.Property(x => x.MoveDamageClassId)
            .HasComment("技分類ID");

        builder.Property(x => x.Power)
            .HasComment("威力");

        builder.Property(x => x.CreatedAt)
            .HasComment("作成日時")
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(x => x.UpdatedAt)
            .HasComment("更新日時")
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.HasOne(x => x.Type)
            .WithMany()
            .HasForeignKey(x => x.TypeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.MoveDamageClass)
            .WithMany()
            .HasForeignKey(x => x.MoveDamageClassId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
