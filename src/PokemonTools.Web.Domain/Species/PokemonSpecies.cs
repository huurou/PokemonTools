using PokemonTools.Web.Domain.Abilities;
using PokemonTools.Web.Domain.Statistics;
using PokemonTools.Web.Domain.Types;

namespace PokemonTools.Web.Domain.Species;

/// <summary>
/// ポケモンの種族を表現するクラス
/// </summary>
public record PokemonSpecies
{
    /// <summary>
    /// 種族Id
    /// </summary>
    public SpeciesId Id { get; init; }

    /// <summary>
    /// 種族名
    /// </summary>
    public string Name { get; init; }

    // Type1IdとType2Idは組み合わせで不変条件を持つため、with式での部分更新を防ぐ。
    // タイプ変更はSetTypes経由で行う。

    /// <summary>
    /// タイプ1Id
    /// </summary>
    public TypeId Type1Id
    {
        get;
        private init
        {
            if (!PokemonType.SpeciesTypes.Any(x => x.Id == value))
            {
                throw new ArgumentException("種族のタイプ1は有効なタイプを指定してください。", nameof(Type1Id));
            }
            field = value;
        }
    }
    /// <summary>
    /// タイプ2Id
    /// </summary>
    public TypeId? Type2Id
    {
        get;
        private init
        {
            if (value is not null && !PokemonType.SpeciesTypes.Any(x => x.Id == value))
            {
                throw new ArgumentException("種族のタイプ2は有効なタイプを指定してください。", nameof(Type2Id));
            }
            field = value;
        }
    }

    // Ability1IdとAbility2Idは組み合わせで不変条件を持つため、with式での部分更新を防ぐ。
    // 特性変更はSetAbilities経由で行う。

    /// <summary>
    /// 特性1Id
    /// </summary>
    public AbilityId Ability1Id { get; private init; }
    /// <summary>
    /// 特性2Id
    /// </summary>
    public AbilityId? Ability2Id { get; private init; }
    /// <summary>
    /// 隠れ特性Id
    /// </summary>
    public AbilityId? HiddenAbilityId { get; init; }

    /// <summary>
    /// 種族値
    /// </summary>
    public BaseStats BaseStats { get; init; }

    /// <summary>
    /// 体重
    /// </summary>
    public Weight Weight { get; init; }

    public PokemonSpecies(
        SpeciesId id,
        string name,
        TypeId type1Id,
        TypeId? type2Id,
        AbilityId ability1Id,
        AbilityId? ability2Id,
        AbilityId? hiddenAbilityId,
        BaseStats baseStats,
        Weight weight
    )
    {
        Id = id;
        Name = name;
        Type1Id = type1Id;
        if (type2Id is not null && type2Id == type1Id)
        {
            throw new ArgumentException("タイプ2はタイプ1と異なる必要があります。", nameof(type2Id));
        }
        Type2Id = type2Id;
        Ability1Id = ability1Id;
        if (ability2Id is not null && ability2Id == ability1Id)
        {
            throw new ArgumentException("特性2は特性1と異なる必要があります。", nameof(ability2Id));
        }
        Ability2Id = ability2Id;
        HiddenAbilityId = hiddenAbilityId;
        BaseStats = baseStats;
        Weight = weight;
    }

    public PokemonSpecies SetTypes(TypeId type1Id, TypeId? type2Id)
    {
        return new PokemonSpecies(Id, Name, type1Id, type2Id, Ability1Id, Ability2Id, HiddenAbilityId, BaseStats, Weight);
    }

    public PokemonSpecies SetAbilities(AbilityId ability1Id, AbilityId? ability2Id)
    {
        return new PokemonSpecies(Id, Name, Type1Id, Type2Id, ability1Id, ability2Id, HiddenAbilityId, BaseStats, Weight);
    }
}
