using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PokemonTools.Web.Infrastructure.Db.Abilities;
using PokemonTools.Web.Infrastructure.Db.Items;
using PokemonTools.Web.Infrastructure.Db.Moves;
using PokemonTools.Web.Infrastructure.Db.Natures;
using PokemonTools.Web.Infrastructure.Db.Species;
using PokemonTools.Web.Infrastructure.Db.Types;

namespace PokemonTools.Web.Infrastructure.Db.Individuals;

public class IndividualEntity : IHasUpdatedAt
{
    /// <summary>
    /// 個体ID
    /// </summary>
    public required string IndividualId { get; set; }

    /// <summary>
    /// 個体名 NULLだったら表示時に種族名を表示
    /// </summary>
    public string? IndividualName { get; set; }

    /// <summary>
    /// 種族ID
    /// </summary>
    public required int SpeciesId { get; set; }

    /// <summary>
    /// 性格ID
    /// </summary>
    public required int NatureId { get; set; }

    /// <summary>
    /// 特性ID
    /// </summary>
    public required int AbilityId { get; set; }

    /// <summary>
    /// 個体値::HP
    /// </summary>
    public required int IndividualValueHp { get; set; }
    /// <summary>
    /// 個体値::こうげき
    /// </summary>
    public required int IndividualValueAttack { get; set; }
    /// <summary>
    /// 個体値::ぼうぎょ
    /// </summary>
    public required int IndividualValueDefense { get; set; }
    /// <summary>
    /// 個体値::とくこう
    /// </summary>
    public required int IndividualValueSpecialAttack { get; set; }
    /// <summary>
    /// 個体値::とくぼう
    /// </summary>
    public required int IndividualValueSpecialDefense { get; set; }
    /// <summary>
    /// 個体値::すばやさ
    /// </summary>
    public required int IndividualValueSpeed { get; set; }

    /// <summary>
    /// 努力値::HP
    /// </summary>
    public required int EffortValueHp { get; set; }
    /// <summary>
    /// 努力値::こうげき
    /// </summary>
    public required int EffortValueAttack { get; set; }
    /// <summary>
    /// 努力値::ぼうぎょ
    /// </summary>
    public required int EffortValueDefense { get; set; }
    /// <summary>
    /// 努力値::とくこう
    /// </summary>
    public required int EffortValueSpecialAttack { get; set; }
    /// <summary>
    /// 努力値::とくぼう
    /// </summary>
    public required int EffortValueSpecialDefense { get; set; }
    /// <summary>
    /// 努力値::すばやさ
    /// </summary>
    public required int EffortValueSpeed { get; set; }

    /// <summary>
    /// 技1ID
    /// </summary>
    public required int Move1Id { get; set; }
    /// <summary>
    /// 技2ID
    /// </summary>
    public int? Move2Id { get; set; }
    /// <summary>
    /// 技3ID
    /// </summary>
    public int? Move3Id { get; set; }
    /// <summary>
    /// 技4ID
    /// </summary>
    public int? Move4Id { get; set; }

    /// <summary>
    /// 持ち物 道具ID
    /// </summary>
    public int? HeldItemId { get; set; }

    /// <summary>
    /// テラスタイプID
    /// </summary>
    public required int TeraTypeId { get; set; }

    /// <summary>
    /// 備考
    /// </summary>
    public string? Memo { get; set; }

    /// <summary>
    /// カテゴリID
    /// </summary>
    public required int CategoryId { get; set; }

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
    /// 種族
    /// </summary>
    public SpeciesEntity Species { get; set; } = null!;

    /// <summary>
    /// 性格
    /// </summary>
    public NatureEntity Nature { get; set; } = null!;

    /// <summary>
    /// 特性
    /// </summary>
    public AbilityEntity Ability { get; set; } = null!;

    /// <summary>
    /// 技1
    /// </summary>
    public MoveEntity Move1 { get; set; } = null!;
    /// <summary>
    /// 技2
    /// </summary>
    public MoveEntity? Move2 { get; set; }
    /// <summary>
    /// 技3
    /// </summary>
    public MoveEntity? Move3 { get; set; }
    /// <summary>
    /// 技4
    /// </summary>
    public MoveEntity? Move4 { get; set; }

    /// <summary>
    /// 持ち物
    /// </summary>
    public ItemEntity? HeldItem { get; set; }

    /// <summary>
    /// テラスタイプ
    /// </summary>
    public TypeEntity TeraType { get; set; } = null!;

    /// <summary>
    /// カテゴリ
    /// </summary>
    public IndividualCategoryEntity Category { get; set; } = null!;

    #endregion ナビゲーションプロパティ
}

public class IndividualEntityConfiguration : IEntityTypeConfiguration<IndividualEntity>
{
    public void Configure(EntityTypeBuilder<IndividualEntity> builder)
    {
        builder.ToTable(x => x.HasComment("個体（育成済ポケモン）"));

        builder.HasKey(x => x.IndividualId);
        builder.Property(x => x.IndividualId)
            .HasComment("個体ID prefix_uuidv7");

        builder.Property(x => x.IndividualName)
            .HasComment("個体名 NULLだったら表示時に種族名を表示");

        builder.Property(x => x.SpeciesId)
            .HasComment("種族ID");

        builder.Property(x => x.NatureId)
            .HasComment("性格ID");

        builder.Property(x => x.AbilityId)
            .HasComment("特性ID");

        builder.Property(x => x.IndividualValueHp)
            .HasComment("個体値::HP");

        builder.Property(x => x.IndividualValueAttack)
            .HasComment("個体値::こうげき");

        builder.Property(x => x.IndividualValueDefense)
            .HasComment("個体値::ぼうぎょ");

        builder.Property(x => x.IndividualValueSpecialAttack)
            .HasComment("個体値::とくこう");

        builder.Property(x => x.IndividualValueSpecialDefense)
            .HasComment("個体値::とくぼう");

        builder.Property(x => x.IndividualValueSpeed)
            .HasComment("個体値::すばやさ");

        builder.Property(x => x.EffortValueHp)
            .HasComment("努力値::HP");

        builder.Property(x => x.EffortValueAttack)
            .HasComment("努力値::こうげき");

        builder.Property(x => x.EffortValueDefense)
            .HasComment("努力値::ぼうぎょ");

        builder.Property(x => x.EffortValueSpecialAttack)
            .HasComment("努力値::とくこう");

        builder.Property(x => x.EffortValueSpecialDefense)
            .HasComment("努力値::とくぼう");

        builder.Property(x => x.EffortValueSpeed)
            .HasComment("努力値::すばやさ");

        builder.Property(x => x.Move1Id)
            .HasComment("技1ID");

        builder.Property(x => x.Move2Id)
            .HasComment("技2ID");

        builder.Property(x => x.Move3Id)
            .HasComment("技3ID");

        builder.Property(x => x.Move4Id)
            .HasComment("技4ID");

        builder.Property(x => x.HeldItemId)
            .HasComment("持ち物 道具ID");

        builder.Property(x => x.TeraTypeId)
            .HasComment("テラスタイプID");

        builder.Property(x => x.Memo)
            .HasComment("備考");

        builder.Property(x => x.CategoryId)
            .HasComment("カテゴリID");

        builder.Property(x => x.CreatedAt)
            .HasComment("作成日時")
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
        builder.HasIndex(x => x.CreatedAt);

        builder.Property(x => x.UpdatedAt)
            .HasComment("更新日時")
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
        builder.HasIndex(x => x.UpdatedAt);

        builder.HasOne(x => x.Species)
            .WithMany()
            .HasForeignKey(x => x.SpeciesId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Nature)
            .WithMany()
            .HasForeignKey(x => x.NatureId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Ability)
            .WithMany()
            .HasForeignKey(x => x.AbilityId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Move1)
            .WithMany()
            .HasForeignKey(x => x.Move1Id)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Move2)
            .WithMany()
            .HasForeignKey(x => x.Move2Id)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Move3)
            .WithMany()
            .HasForeignKey(x => x.Move3Id)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Move4)
            .WithMany()
            .HasForeignKey(x => x.Move4Id)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.HeldItem)
            .WithMany()
            .HasForeignKey(x => x.HeldItemId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.TeraType)
            .WithMany()
            .HasForeignKey(x => x.TeraTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Category)
            .WithMany()
            .HasForeignKey(x => x.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
