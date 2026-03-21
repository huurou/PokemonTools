using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PokemonTools.ApiService.Infrastructure.Db.Abilities;
using PokemonTools.ApiService.Infrastructure.Db.Types;

namespace PokemonTools.ApiService.Infrastructure.Db.Species;

public class SpeciesEntity : IHasUpdatedAt
{
    /// <summary>
    /// 種族ID
    /// </summary>
    public required int SpeciesId { get; set; }

    /// <summary>
    /// 種族名
    /// </summary>
    public required string SpeciesName { get; set; }

    /// <summary>
    /// タイプ1ID
    /// </summary>
    public required int Type1Id { get; set; }
    /// <summary>
    /// タイプ2ID
    /// </summary>
    public int? Type2Id { get; set; }

    /// <summary>
    /// 特性1ID
    /// </summary>
    public required int Ability1Id { get; set; }
    /// <summary>
    /// 特性2ID
    /// </summary>
    public int? Ability2Id { get; set; }
    /// <summary>
    /// 隠れ特性ID
    /// </summary>
    public int? HiddenAbilityId { get; set; }

    /// <summary>
    /// 種族値::HP
    /// </summary>
    public required int BaseStatHp { get; set; }
    /// <summary>
    /// 種族値::こうげき
    /// </summary>
    public required int BaseStatAttack { get; set; }
    /// <summary>
    /// 種族値::ぼうぎょ
    /// </summary>
    public required int BaseStatDefense { get; set; }
    /// <summary>
    /// 種族値::とくこう
    /// </summary>
    public required int BaseStatSpecialAttack { get; set; }
    /// <summary>
    /// 種族値::とくぼう
    /// </summary>
    public required int BaseStatSpecialDefense { get; set; }
    /// <summary>
    /// 種族値::すばやさ
    /// </summary>
    public required int BaseStatSpeed { get; set; }

    /// <summary>
    /// 体重
    /// </summary>
    public required int Weight { get; set; }

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
    /// タイプ1
    /// </summary>
    public TypeEntity Type1 { get; set; } = null!;
    /// <summary>
    /// タイプ2
    /// </summary>
    public TypeEntity? Type2 { get; set; }

    /// <summary>
    /// 特性1
    /// </summary>
    public AbilityEntity Ability1 { get; set; } = null!;
    /// <summary>
    /// 特性2
    /// </summary>
    public AbilityEntity? Ability2 { get; set; }
    /// <summary>
    /// 隠れ特性
    /// </summary>
    public AbilityEntity? HiddenAbility { get; set; }

    #endregion ナビゲーションプロパティ
}

public class SpeciesEntityConfiguration : IEntityTypeConfiguration<SpeciesEntity>
{
    public void Configure(EntityTypeBuilder<SpeciesEntity> builder)
    {
        builder.ToTable(x => x.HasComment("ポケモンの種族"));

        builder.HasKey(x => x.SpeciesId);
        builder.Property(x => x.SpeciesId)
            .HasComment("種族ID PokeAPIでのpokemonエンドポイントのid")
            .ValueGeneratedNever();

        builder.Property(x => x.SpeciesName)
            .HasComment("種族名 日本語名")
            .IsRequired();

        builder.Property(x => x.Type1Id)
            .HasComment("タイプ1ID");

        builder.Property(x => x.Type2Id)
            .HasComment("タイプ2ID");

        builder.Property(x => x.Ability1Id)
            .HasComment("特性1ID");

        builder.Property(x => x.Ability2Id)
            .HasComment("特性2ID");

        builder.Property(x => x.HiddenAbilityId)
            .HasComment("隠れ特性ID");

        builder.Property(x => x.BaseStatHp)
            .HasComment("種族値::HP");

        builder.Property(x => x.BaseStatAttack)
            .HasComment("種族値::こうげき");

        builder.Property(x => x.BaseStatDefense)
            .HasComment("種族値::ぼうぎょ");

        builder.Property(x => x.BaseStatSpecialAttack)
            .HasComment("種族値::とくこう");

        builder.Property(x => x.BaseStatSpecialDefense)
            .HasComment("種族値::とくぼう");

        builder.Property(x => x.BaseStatSpeed)
            .HasComment("種族値::すばやさ");

        builder.Property(x => x.Weight)
            .HasComment("体重 PokeAPIでの値そのまま");

        builder.Property(x => x.CreatedAt)
            .HasComment("作成日時")
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(x => x.UpdatedAt)
            .HasComment("更新日時")
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.HasOne(x => x.Type1)
            .WithMany()
            .HasForeignKey(x => x.Type1Id)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Type2)
            .WithMany()
            .HasForeignKey(x => x.Type2Id)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Ability1)
            .WithMany()
            .HasForeignKey(x => x.Ability1Id)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Ability2)
            .WithMany()
            .HasForeignKey(x => x.Ability2Id)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.HiddenAbility)
            .WithMany()
            .HasForeignKey(x => x.HiddenAbilityId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
