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
    public TypeId Type1Id { get; init; }
    /// <summary>
    /// タイプ2Id
    /// </summary>
    public TypeId? Type2Id { get; init; }

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
        if (!PokemonType.BattleTypes.Any(x => x.Id == type1Id))
        {
            throw new ArgumentException("種族のタイプ1は18タイプのいずれかを指定してください。", nameof(type1Id));
        }

        if (type2Id is not null)
        {
            if (!PokemonType.BattleTypes.Any(x => x.Id == type2Id))
            {
                throw new ArgumentException("種族のタイプ2は18タイプのいずれかを指定してください。", nameof(type2Id));
            }

            if (type2Id == type1Id)
            {
                throw new ArgumentException("タイプ2はタイプ1と異なる必要があります。", nameof(type2Id));
            }
        }

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
}
