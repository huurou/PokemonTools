using PokemonTools.Web.Domain.Utility;

namespace PokemonTools.Web.Domain.Damages;

public static class DamageCalculator
{
    private static readonly double[] rands_ = [0.85, 0.86, 0.87, 0.88, 0.89, 0.90, 0.91, 0.92, 0.93, 0.94, 0.95, 0.96, 0.97, 0.98, 0.99, 1.00];

    /// <summary>
    /// ダメージを計算します。
    /// </summary>
    /// <param name="power">技の威力</param>
    /// <param name="powerModifiers">威力の補正値のリスト 12bit固定小数点表記</param>
    /// <param name="attackStat">攻撃側のこうげきorとくこう実数値</param>
    /// <param name="attackStage">攻撃側のこうげきorとくこうランク (-6〜+6)</param>
    /// <param name="attackModifiers">攻撃の補正値のリスト 12bit固定小数点表記</param>
    /// <param name="defenseStat">防御側のぼうぎょorとくぼう実数値</param>
    /// <param name="defenseStage">防御側のぼうぎょorとくぼうランク (-6〜+6)</param>
    /// <param name="defenseModifiers">防御の補正値のリスト 12bit固定小数点表記</param>
    /// <param name="damageModifiers">ダメージの補正値のリスト 12bit固定小数点表記</param>
    /// <param name="attackerLevel">攻撃側のレベル</param>
    /// <param name="isCriticalHit">急所に当たったかどうか</param>
    /// <param name="stabType">タイプ一致ボーナスの種類</param>
    /// <param name="typeEffectiveness">タイプ相性補正</param>
    /// <returns>ダメージ</returns>
    public static Damage Calculate(
        uint power, IReadOnlyList<uint> powerModifiers,
        uint attackStat, int attackStage, IReadOnlyList<uint> attackModifiers,
        uint defenseStat, int defenseStage, IReadOnlyList<uint> defenseModifiers,
        IReadOnlyList<uint> damageModifiers,
        uint attackerLevel,
        bool isCriticalHit,
        StabType stabType,
        double typeEffectiveness
    )
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(attackStage, -6);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(attackStage, 6);
        ArgumentOutOfRangeException.ThrowIfLessThan(defenseStage, -6);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(defenseStage, 6);

        // ① 威力の補正値
        var combinedPowerModifier = CalculateCombinedModifier(powerModifiers);
        // ② 最終威力
        var finalPower = CalculateFinalPower(power, powerModifiers, combinedPowerModifier);
        // ③ 攻撃の補正値
        var combinedAttackModifier = CalculateCombinedModifier(attackModifiers);
        // ④ 最終攻撃
        var finalAttack = CalculateFinalAttack(attackStat, attackStage, isCriticalHit, attackModifiers, combinedAttackModifier);
        // ⑤ 防御の補正値
        var combinedDefenseModifier = CalculateCombinedModifier(defenseModifiers);
        // ⑥ 最終防御
        var finalDefense = CalculateFinalDefense(defenseStat, defenseStage, isCriticalHit, defenseModifiers, combinedDefenseModifier);
        // ⑦ ダメージの補正値
        var combinedDamageModifier = CalculateCombinedModifier(damageModifiers);
        // ⑧ 最終ダメージ
        var finalDamages = CalculateFinalDamages(
            damageModifiers,
            attackerLevel,
            isCriticalHit,
            stabType,
            typeEffectiveness,
            finalPower,
            finalAttack,
            finalDefense,
            combinedDamageModifier
        );
        return new Damage([.. finalDamages]);
    }

    private static uint CalculateCombinedModifier(IReadOnlyList<uint> modifiers)
    {
        var combinedModifier = 4096u;
        foreach (var modifier in modifiers)
        {
            combinedModifier = (combinedModifier * modifier).Q12Round();
        }

        return combinedModifier;
    }

    private static (uint StageModifierNumerator, uint StageModifierDenominator) CalculateStageModifier(int stage)
    {
        // ランク倍率の分子
        var stageModifierNumerator = stage switch
        {
            <= 0 => 2u,
            1 => 3u,
            2 => 4u,
            3 => 5u,
            4 => 6u,
            5 => 7u,
            >= 6 => 8u,
        };
        // ランク倍率の分母
        var stageModifierDenominator = stage switch
        {
            <= -6 => 8u,
            -5 => 7u,
            -4 => 6u,
            -3 => 5u,
            -2 => 4u,
            -1 => 3u,
            >= 0 => 2u,
        };
        return (stageModifierNumerator, stageModifierDenominator);
    }

    private static uint CalculateFinalPower(uint power, IReadOnlyList<uint> powerModifiers, uint combinedPowerModifier)
    {
        var tmpPower = power;
        if (powerModifiers.Count > 0)
        {
            tmpPower = (power * combinedPowerModifier).Q12RoundHalfDown();
        }
        var finalPower = Math.Max(tmpPower, 1u);
        // TODO: or テラスタイプと技のタイプが同じで60未満の場合は60にする(一部の技を除く)
        return finalPower;
    }

    private static uint CalculateFinalAttack(uint attackStat, int attackStage, bool isCriticalHit, IReadOnlyList<uint> attackModifiers, uint combinedAttackModifier)
    {
        // 急所時は攻撃側のマイナスランクを無視
        if (isCriticalHit) { attackStage = Math.Max(attackStage, 0); }

        // 攻撃ランク
        var (attackStageModifierNumerator, attackStageModifierDenominator) = CalculateStageModifier(attackStage);

        var tmpAttack = ((double)attackStat * attackStageModifierNumerator / attackStageModifierDenominator).FloorToUint();
        // TODO: はりきり *6144/4096Floor
        if (attackModifiers.Count > 0)
        {
            tmpAttack = (tmpAttack * combinedAttackModifier).Q12RoundHalfDown();
        }
        var finalAttack = Math.Max(tmpAttack, 1u);
        return finalAttack;
    }

    private static uint CalculateFinalDefense(uint defenseStat, int defenseStage, bool isCriticalHit, IReadOnlyList<uint> defenseModifiers, uint combinedDefenseModifier)
    {
        // 急所時は防御側のプラスランクを無視
        if (isCriticalHit) { defenseStage = Math.Min(defenseStage, 0); }

        // 防御ランク
        var (defenseStageModifierNumerator, defenseStageModifierDenominator) = CalculateStageModifier(defenseStage);

        var tmpDefense = ((double)defenseStat * defenseStageModifierNumerator / defenseStageModifierDenominator).FloorToUint();
        // TODO: すなあらし+いわタイプでとくぼう強化 *6144/4096Floor
        // TODO: ゆき+こおりタイプでぼうぎょ強化 *6144/4096Floor
        if (defenseModifiers.Count > 0)
        {
            tmpDefense = (tmpDefense * combinedDefenseModifier).Q12RoundHalfDown();
        }
        var finalDefense = Math.Max(tmpDefense, 1u);
        return finalDefense;
    }

    private static IEnumerable<uint> CalculateFinalDamages(
        IReadOnlyList<uint> damageModifiers,
        uint attackerLevel,
        bool isCriticalHit,
        StabType stabType,
        double typeEffectiveness,
        uint finalPower,
        uint finalAttack,
        uint finalDefense,
        uint combinedDamageModifier
    )
    {
        var tmpDamage = ((double)attackerLevel * 2 / 5 + 2).FloorToUint();
        // doubleの仮数部は52bitのため、実際のポケモンのパラメータ範囲では精度の問題は起きない
        tmpDamage = ((double)tmpDamage * finalPower * finalAttack / finalDefense).FloorToUint();
        tmpDamage = ((double)tmpDamage / 50 + 2).FloorToUint();
        // TODO: 複数対象 *3072/4096RoundHalfDown
        // TODO: おやこあい2発目 *1024/4096RoundHalfDown
        // TODO: 天気弱化 *2048/4096RoundHalfDown
        // TODO: 天気強化 *6144/4096RoundHalfDown
        // TODO: きょけんとつげき *8192/4096RoundHalfDown
        tmpDamage = isCriticalHit ? (tmpDamage * 6144).Q12RoundHalfDown() : tmpDamage;
        var tmpDamages = rands_.Select(x => (tmpDamage * x).FloorToUint());
        tmpDamages = stabType switch
        {
            StabType.None => tmpDamages,
            StabType.Normal => tmpDamages.Select(x => (x * 6144).Q12RoundHalfDown()),
            StabType.Adaptability => tmpDamages.Select(x => (x * 8192).Q12RoundHalfDown()),
            StabType.AdaptabilityTerastal => tmpDamages.Select(x => (x * 9216).Q12RoundHalfDown()),
            _ => throw new ArgumentOutOfRangeException(nameof(stabType)),
        };
        tmpDamages = tmpDamages.Select(x => (x * typeEffectiveness).FloorToUint());
        // TODO: やけど *2048/4096RoundHalfDown
        if (damageModifiers.Count > 0)
        {
            tmpDamages = tmpDamages.Select(x => (x * combinedDamageModifier).Q12RoundHalfDown());
        }
        // TODO: Z技まもる *1024/4096RoundHalfDown
        // TODO: ダイマックス技まもる *1024/4096RoundHalfDown
        if (typeEffectiveness > 0) { tmpDamages = tmpDamages.Select(x => Math.Max(x, 1u)); }
        // 65536で割った余りが最終ダメージになる
        var finalDamages = tmpDamages.Select(x => x & 0xFFFF);
        return finalDamages;
    }
}
