using PokemonTools.Web.Domain.Utility;

namespace PokemonTools.Web.Domain.Statistics;

public static class StatsCalculator
{
    public static Stats Calculate(BaseStats baseStats, IndividualValues individualValues, EffortValues effortValues, Nature nature)
    {
        return Calculate(baseStats, individualValues, effortValues, nature, new Level(50));
    }

    public static Stats Calculate(BaseStats baseStats, IndividualValues individualValues, EffortValues effortValues, Nature nature, Level level)
    {
        var hp = CalculateHp(baseStats.Hp, individualValues.Hp, effortValues.Hp, level.Value);
        var attack = CalculateOtherStat(baseStats.Attack, individualValues.Attack, effortValues.Attack, nature, StatType.Attack, level.Value);
        var defense = CalculateOtherStat(baseStats.Defense, individualValues.Defense, effortValues.Defense, nature, StatType.Defense, level.Value);
        var specialAttack = CalculateOtherStat(baseStats.SpecialAttack, individualValues.SpecialAttack, effortValues.SpecialAttack, nature, StatType.SpecialAttack, level.Value);
        var specialDefense = CalculateOtherStat(baseStats.SpecialDefense, individualValues.SpecialDefense, effortValues.SpecialDefense, nature, StatType.SpecialDefense, level.Value);
        var speed = CalculateOtherStat(baseStats.Speed, individualValues.Speed, effortValues.Speed, nature, StatType.Speed, level.Value);

        return new Stats(hp, attack, defense, specialAttack, specialDefense, speed);
    }

    // TODO: 種族クラス実装時にヌケニン（baseStat==1）のHP=1特例を対応
    private static uint CalculateHp(uint baseStat, uint individualValue, uint effortValue, uint level)
    {
        return (baseStat * 2 + individualValue + effortValue / 4) * level / 100 + level + 10;
    }

    private static uint CalculateOtherStat(uint baseStat, uint individualValue, uint effortValue, Nature nature, StatType statType, uint level)
    {
        var value = (baseStat * 2 + individualValue + effortValue / 4) * level / 100 + 5;
        var natureMultiplier = nature.GetMultiplier(statType);

        return (value * natureMultiplier).FloorToUint();
    }
}
