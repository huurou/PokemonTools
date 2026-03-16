using System.Collections.Frozen;

namespace PokemonTools.ApiService.Domain.Types;

/// <summary>
/// タイプ相性表を保持し、相性を算出するクラス
/// </summary>
public static class TypeChart
{
    private static readonly FrozenDictionary<(TypeId, TypeId), TypeEffectiveness> chart_ = BuildChart();

    /// <summary>
    /// 攻撃タイプと防御タイプ1つから相性を返す
    /// </summary>
    public static TypeEffectiveness GetEffectiveness(PokemonType attackType, PokemonType defenseType)
    {
        return chart_.GetValueOrDefault((attackType.Id, defenseType.Id), TypeEffectiveness.Neutral);
    }

    /// <summary>
    /// 攻撃タイプと防御タイプ1〜2つから最終的な相性を返す
    /// </summary>
    public static TypeEffectiveness GetEffectiveness(
        PokemonType attackType, PokemonType defenseType1, PokemonType? defenseType2 = null)
    {
        if (defenseType2 is null)
        {
            return GetEffectiveness(attackType, defenseType1);
        }

        var e1 = GetEffectiveness(attackType, defenseType1);
        var e2 = GetEffectiveness(attackType, defenseType2);
        return Combine(e1, e2);
    }

    private static TypeEffectiveness Combine(TypeEffectiveness e1, TypeEffectiveness e2)
    {
        return (e1, e2) switch
        {
            _ when e1 == TypeEffectiveness.HasNoEffect || e2 == TypeEffectiveness.HasNoEffect
                => TypeEffectiveness.HasNoEffect,
            (TypeEffectiveness.NotVeryEffective, TypeEffectiveness.NotVeryEffective) => TypeEffectiveness.MostlyIneffective,
            (TypeEffectiveness.NotVeryEffective, TypeEffectiveness.Neutral) or
            (TypeEffectiveness.Neutral, TypeEffectiveness.NotVeryEffective) => TypeEffectiveness.NotVeryEffective,
            (TypeEffectiveness.NotVeryEffective, TypeEffectiveness.SuperEffective) or
            (TypeEffectiveness.SuperEffective, TypeEffectiveness.NotVeryEffective) => TypeEffectiveness.Neutral,
            (TypeEffectiveness.Neutral, TypeEffectiveness.Neutral) => TypeEffectiveness.Neutral,
            (TypeEffectiveness.Neutral, TypeEffectiveness.SuperEffective) or
            (TypeEffectiveness.SuperEffective, TypeEffectiveness.Neutral) => TypeEffectiveness.SuperEffective,
            (TypeEffectiveness.SuperEffective, TypeEffectiveness.SuperEffective) => TypeEffectiveness.ExtremelyEffective,
            _ => throw new InvalidOperationException($"予期しないタイプ相性の組み合わせです: {e1}, {e2}"),
        };
    }

    private static FrozenDictionary<(TypeId, TypeId), TypeEffectiveness> BuildChart()
    {
        var dict = new Dictionary<(TypeId, TypeId), TypeEffectiveness>();

        // ノーマル
        Set(dict, PokemonType.Normal,
            superEffective: [],
            notVeryEffective: [PokemonType.Rock, PokemonType.Steel],
            noEffect: [PokemonType.Ghost]);

        // かくとう
        Set(dict, PokemonType.Fighting,
            superEffective: [PokemonType.Normal, PokemonType.Rock, PokemonType.Steel, PokemonType.Ice, PokemonType.Dark],
            notVeryEffective: [PokemonType.Flying, PokemonType.Poison, PokemonType.Bug, PokemonType.Psychic, PokemonType.Fairy],
            noEffect: [PokemonType.Ghost]);

        // ひこう
        Set(dict, PokemonType.Flying,
            superEffective: [PokemonType.Fighting, PokemonType.Bug, PokemonType.Grass],
            notVeryEffective: [PokemonType.Rock, PokemonType.Steel, PokemonType.Electric],
            noEffect: []);

        // どく
        Set(dict, PokemonType.Poison,
            superEffective: [PokemonType.Grass, PokemonType.Fairy],
            notVeryEffective: [PokemonType.Poison, PokemonType.Ground, PokemonType.Rock, PokemonType.Ghost],
            noEffect: [PokemonType.Steel]);

        // じめん
        Set(dict, PokemonType.Ground,
            superEffective: [PokemonType.Poison, PokemonType.Rock, PokemonType.Steel, PokemonType.Fire, PokemonType.Electric],
            notVeryEffective: [PokemonType.Bug, PokemonType.Grass],
            noEffect: [PokemonType.Flying]);

        // いわ
        Set(dict, PokemonType.Rock,
            superEffective: [PokemonType.Flying, PokemonType.Bug, PokemonType.Fire, PokemonType.Ice],
            notVeryEffective: [PokemonType.Fighting, PokemonType.Ground, PokemonType.Steel],
            noEffect: []);

        // むし
        Set(dict, PokemonType.Bug,
            superEffective: [PokemonType.Grass, PokemonType.Psychic, PokemonType.Dark],
            notVeryEffective: [PokemonType.Fighting, PokemonType.Flying, PokemonType.Poison, PokemonType.Ghost, PokemonType.Steel, PokemonType.Fire, PokemonType.Fairy],
            noEffect: []);

        // ゴースト
        Set(dict, PokemonType.Ghost,
            superEffective: [PokemonType.Ghost, PokemonType.Psychic],
            notVeryEffective: [PokemonType.Dark],
            noEffect: [PokemonType.Normal]);

        // はがね
        Set(dict, PokemonType.Steel,
            superEffective: [PokemonType.Rock, PokemonType.Ice, PokemonType.Fairy],
            notVeryEffective: [PokemonType.Steel, PokemonType.Fire, PokemonType.Water, PokemonType.Electric],
            noEffect: []);

        // ほのお
        Set(dict, PokemonType.Fire,
            superEffective: [PokemonType.Bug, PokemonType.Steel, PokemonType.Grass, PokemonType.Ice],
            notVeryEffective: [PokemonType.Rock, PokemonType.Fire, PokemonType.Water, PokemonType.Dragon],
            noEffect: []);

        // みず
        Set(dict, PokemonType.Water,
            superEffective: [PokemonType.Ground, PokemonType.Rock, PokemonType.Fire],
            notVeryEffective: [PokemonType.Water, PokemonType.Grass, PokemonType.Dragon],
            noEffect: []);

        // くさ
        Set(dict, PokemonType.Grass,
            superEffective: [PokemonType.Ground, PokemonType.Rock, PokemonType.Water],
            notVeryEffective: [PokemonType.Flying, PokemonType.Poison, PokemonType.Bug, PokemonType.Steel, PokemonType.Fire, PokemonType.Grass, PokemonType.Dragon],
            noEffect: []);

        // でんき
        Set(dict, PokemonType.Electric,
            superEffective: [PokemonType.Flying, PokemonType.Water],
            notVeryEffective: [PokemonType.Grass, PokemonType.Electric, PokemonType.Dragon],
            noEffect: [PokemonType.Ground]);

        // エスパー
        Set(dict, PokemonType.Psychic,
            superEffective: [PokemonType.Fighting, PokemonType.Poison],
            notVeryEffective: [PokemonType.Steel, PokemonType.Psychic],
            noEffect: [PokemonType.Dark]);

        // こおり
        Set(dict, PokemonType.Ice,
            superEffective: [PokemonType.Flying, PokemonType.Ground, PokemonType.Grass, PokemonType.Dragon],
            notVeryEffective: [PokemonType.Steel, PokemonType.Fire, PokemonType.Water, PokemonType.Ice],
            noEffect: []);

        // ドラゴン
        Set(dict, PokemonType.Dragon,
            superEffective: [PokemonType.Dragon],
            notVeryEffective: [PokemonType.Steel],
            noEffect: [PokemonType.Fairy]);

        // あく
        Set(dict, PokemonType.Dark,
            superEffective: [PokemonType.Ghost, PokemonType.Psychic],
            notVeryEffective: [PokemonType.Fighting, PokemonType.Dark, PokemonType.Fairy],
            noEffect: []);

        // フェアリー
        Set(dict, PokemonType.Fairy,
            superEffective: [PokemonType.Fighting, PokemonType.Dragon, PokemonType.Dark],
            notVeryEffective: [PokemonType.Poison, PokemonType.Steel, PokemonType.Fire],
            noEffect: []);

        return dict.ToFrozenDictionary();
    }

    private static void Set(
        Dictionary<(TypeId, TypeId), TypeEffectiveness> dict,
        PokemonType attackType,
        PokemonType[] superEffective,
        PokemonType[] notVeryEffective,
        PokemonType[] noEffect)
    {
        foreach (var def in superEffective)
        {
            dict[(attackType.Id, def.Id)] = TypeEffectiveness.SuperEffective;
        }

        foreach (var def in notVeryEffective)
        {
            dict[(attackType.Id, def.Id)] = TypeEffectiveness.NotVeryEffective;
        }

        foreach (var def in noEffect)
        {
            dict[(attackType.Id, def.Id)] = TypeEffectiveness.HasNoEffect;
        }
    }
}
