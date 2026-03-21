using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PokemonTools.ApiService.Infrastructure.Db.Individuals;

namespace PokemonTools.ApiService.Infrastructure.Db.Parties;

public class PartyEntity : IHasUpdatedAt
{
    /// <summary>
    /// パーティID
    /// </summary>
    public required string PartyId { get; set; }

    /// <summary>
    /// パーティ名
    /// </summary>
    public required string PartyName { get; set; }

    /// <summary>
    /// 個体1ID
    /// </summary>
    public string? Individual1Id { get; set; }
    /// <summary>
    /// 個体2ID
    /// </summary>
    public string? Individual2Id { get; set; }
    /// <summary>
    /// 個体3ID
    /// </summary>
    public string? Individual3Id { get; set; }
    /// <summary>
    /// 個体4ID
    /// </summary>
    public string? Individual4Id { get; set; }
    /// <summary>
    /// 個体5ID
    /// </summary>
    public string? Individual5Id { get; set; }
    /// <summary>
    /// 個体6ID
    /// </summary>
    public string? Individual6Id { get; set; }

    /// <summary>
    /// 備考
    /// </summary>
    public string? Memo { get; set; }

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
    /// 個体1
    /// </summary>
    public IndividualEntity? Individual1 { get; set; }
    /// <summary>
    /// 個体2
    /// </summary>
    public IndividualEntity? Individual2 { get; set; }
    /// <summary>
    /// 個体3
    /// </summary>
    public IndividualEntity? Individual3 { get; set; }
    /// <summary>
    /// 個体4
    /// </summary>
    public IndividualEntity? Individual4 { get; set; }
    /// <summary>
    /// 個体5
    /// </summary>
    public IndividualEntity? Individual5 { get; set; }
    /// <summary>
    /// 個体6
    /// </summary>
    public IndividualEntity? Individual6 { get; set; }

    #endregion ナビゲーションプロパティ
}

public class PartyEntityConfiguration : IEntityTypeConfiguration<PartyEntity>
{
    public void Configure(EntityTypeBuilder<PartyEntity> builder)
    {
        builder.ToTable(x => x.HasComment("パーティ"));

        builder.HasKey(x => x.PartyId);
        builder.Property(x => x.PartyId)
            .HasComment("パーティID prefix_uuidv7");

        builder.Property(x => x.PartyName)
            .HasComment("パーティ名")
            .IsRequired();

        builder.Property(x => x.Individual1Id)
            .HasComment("個体1ID");

        builder.Property(x => x.Individual2Id)
            .HasComment("個体2ID");

        builder.Property(x => x.Individual3Id)
            .HasComment("個体3ID");

        builder.Property(x => x.Individual4Id)
            .HasComment("個体4ID");

        builder.Property(x => x.Individual5Id)
            .HasComment("個体5ID");

        builder.Property(x => x.Individual6Id)
            .HasComment("個体6ID");

        builder.Property(x => x.Memo)
            .HasComment("備考");

        builder.Property(x => x.CreatedAt)
            .HasComment("作成日時")
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
        builder.HasIndex(x => x.CreatedAt);

        builder.Property(x => x.UpdatedAt)
            .HasComment("更新日時")
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
        builder.HasIndex(x => x.UpdatedAt);

        builder.HasOne(x => x.Individual1)
            .WithMany()
            .HasForeignKey(x => x.Individual1Id)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(x => x.Individual2)
            .WithMany()
            .HasForeignKey(x => x.Individual2Id)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(x => x.Individual3)
            .WithMany()
            .HasForeignKey(x => x.Individual3Id)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(x => x.Individual4)
            .WithMany()
            .HasForeignKey(x => x.Individual4Id)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(x => x.Individual5)
            .WithMany()
            .HasForeignKey(x => x.Individual5Id)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(x => x.Individual6)
            .WithMany()
            .HasForeignKey(x => x.Individual6Id)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
