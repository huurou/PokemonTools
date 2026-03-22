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

    /// <summary>
    /// タイプ1Id
    /// </summary>
    public TypeId Type1Id
    {
        get;
        private init
        {
            ValidateBattleTypeId(value, "種族のタイプ1は18タイプのいずれかを指定してください。", nameof(Type1Id));
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
            ValidateType2Id(value, Type1Id);
            field = value;
        }
    }

    /// <summary>
    /// 特性1Id
    /// </summary>
    public AbilityId Ability1Id { get; init; }
    /// <summary>
    /// 特性2Id
    /// </summary>
    public AbilityId? Ability2Id { get; init; }
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
        Type2Id = type2Id;
        Ability1Id = ability1Id;
        Ability2Id = ability2Id;
        HiddenAbilityId = hiddenAbilityId;
        BaseStats = baseStats;
        Weight = weight;
    }

    public PokemonSpecies SetTypes(TypeId type1Id, TypeId? type2Id) =>
        new(Id, Name, type1Id, type2Id, Ability1Id, Ability2Id, HiddenAbilityId, BaseStats, Weight);

    private static void ValidateBattleTypeId(TypeId value, string message, string paramName)
    {
        if (!PokemonType.BattleTypes.Any(x => x.Id == value))
        {
            throw new ArgumentException(message, paramName);
        }
    }

    private static void ValidateType2Id(TypeId? value, TypeId type1Id)
    {
        if (value is null) return;

        ValidateBattleTypeId(value, "種族のタイプ2は18タイプのいずれかを指定してください。", nameof(Type2Id));

        if (value == type1Id)
        {
            throw new ArgumentException("タイプ2はタイプ1と異なる必要があります。", nameof(Type2Id));
        }
    }
}
