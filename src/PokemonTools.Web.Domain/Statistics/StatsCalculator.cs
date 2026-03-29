using PokemonTools.Web.Domain.Utility;

namespace PokemonTools.Web.Domain.Statistics;

public static class StatsCalculator
{
    public static Stats Calculate(BaseStats baseStats, StatPoints statPoints, StatAlignment statAlignment)
    {
        var hp = CalculateHp(baseStats.Hp, statPoints.Hp);
        var attack = CalculateOtherStat(baseStats.Attack, statPoints.Attack, statAlignment, StatType.Attack);
        var defense = CalculateOtherStat(baseStats.Defense, statPoints.Defense, statAlignment, StatType.Defense);
        var specialAttack = CalculateOtherStat(baseStats.SpecialAttack, statPoints.SpecialAttack, statAlignment, StatType.SpecialAttack);
        var specialDefense = CalculateOtherStat(baseStats.SpecialDefense, statPoints.SpecialDefense, statAlignment, StatType.SpecialDefense);
        var speed = CalculateOtherStat(baseStats.Speed, statPoints.Speed, statAlignment, StatType.Speed);

        return new Stats(hp, attack, defense, specialAttack, specialDefense, speed);
    }

    // TODO: 種族クラス実装時にヌケニン（baseStat==1）のHP=1特例を対応
    private static uint CalculateHp(uint baseStat, uint statPoint)
    {
        // レベル50 個体値15に固定したため75に固定化
        // 75 = 31/2+50+10
        return baseStat + 75 + statPoint;
    }

    private static uint CalculateOtherStat(uint baseStat, uint statPoint, StatAlignment statAlignment, StatType statType)
    {
        // レベル50 個体値15に固定したため20に固定化
        // 20 = 31/2 + 5
        var value = baseStat + 20 + statPoint;
        var multiplier = statAlignment.GetMultiplier(statType);

        return ((double)value * multiplier).FloorToUint();
    }
}
