using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PokemonTools.ApiService.Domain.Types;

namespace PokemonTools.ApiService.Infrastructure.Db.Types;

public class TypeEntity : IHasUpdatedAt
{
    /// <summary>
    /// タイプID
    /// </summary>
    public required int TypeId { get; set; }

    /// <summary>
    /// タイプ名
    /// </summary>
    public required string TypeName { get; set; }

    /// <summary>
    /// 作成日時
    /// </summary>
    public DateTimeOffset CreatedAt { get; set; }

    /// <summary>
    /// 更新日時
    /// </summary>
    public DateTimeOffset UpdatedAt { get; set; }
}

public class TypeEntityConfiguration : IEntityTypeConfiguration<TypeEntity>
{
    public void Configure(EntityTypeBuilder<TypeEntity> builder)
    {
        builder.ToTable(x => x.HasComment("ポケモンのタイプ"));

        builder.HasKey(x => x.TypeId);
        builder.Property(x => x.TypeId)
            .HasComment("タイプID PokeAPIでのid")
            .ValueGeneratedNever();

        builder.Property(x => x.TypeName)
            .HasComment("タイプ名 日本語名")
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .HasComment("作成日時")
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(x => x.UpdatedAt)
            .HasComment("更新日時")
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.HasData(PokemonType.All
            .OrderBy(x => x.Id.Value)
            .Select(x => new TypeEntity
            {
                TypeId = x.Id.Value,
                TypeName = x.Name,
            })
        );
    }
}
