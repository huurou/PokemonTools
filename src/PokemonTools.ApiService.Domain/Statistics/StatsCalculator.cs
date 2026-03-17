using PokemonTools.ApiService.Domain.Utility;

namespace PokemonTools.ApiService.Domain.Statistics;

public static class StatsCalculator
{
    public static Stats Calculate(BaseStats baseStats, IndividualValues individualValues, EffortValues effortValues, Nature nature, Level? level = default)
    {
        level ??= new Level(50);

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
        var natureMultiplier = GetNatureMultiplier(nature, statType);

        return (value * natureMultiplier).FloorToUint();
    }

    private static double GetNatureMultiplier(Nature nature, StatType statType)
    {
        // 性格補正の上昇/下降を1.1/0.9の倍率で反映する
        (StatType? Increased, StatType? Decreased) adjustment = nature switch
        {
            Nature.Lonely => (StatType.Attack, StatType.Defense),
            Nature.Adamant => (StatType.Attack, StatType.SpecialAttack),
            Nature.Naughty => (StatType.Attack, StatType.SpecialDefense),
            Nature.Brave => (StatType.Attack, StatType.Speed),
            Nature.Bold => (StatType.Defense, StatType.Attack),
            Nature.Impish => (StatType.Defense, StatType.SpecialAttack),
            Nature.Lax => (StatType.Defense, StatType.SpecialDefense),
            Nature.Relaxed => (StatType.Defense, StatType.Speed),
            Nature.Modest => (StatType.SpecialAttack, StatType.Attack),
            Nature.Mild => (StatType.SpecialAttack, StatType.Defense),
            Nature.Rash => (StatType.SpecialAttack, StatType.SpecialDefense),
            Nature.Quiet => (StatType.SpecialAttack, StatType.Speed),
            Nature.Calm => (StatType.SpecialDefense, StatType.Attack),
            Nature.Gentle => (StatType.SpecialDefense, StatType.Defense),
            Nature.Careful => (StatType.SpecialDefense, StatType.SpecialAttack),
            Nature.Sassy => (StatType.SpecialDefense, StatType.Speed),
            Nature.Timid => (StatType.Speed, StatType.Attack),
            Nature.Hasty => (StatType.Speed, StatType.Defense),
            Nature.Jolly => (StatType.Speed, StatType.SpecialAttack),
            Nature.Naive => (StatType.Speed, StatType.SpecialDefense),
            _ => (null, null),
        };

        return
            adjustment.Increased.HasValue && statType == adjustment.Increased.Value ? 1.1
            : adjustment.Decreased.HasValue && statType == adjustment.Decreased.Value ? 0.9
            : 1.0;
    }

    private enum StatType
    {
        Attack,
        Defense,
        SpecialAttack,
        SpecialDefense,
        Speed,
    }
}
