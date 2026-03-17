using PokemonTools.ApiService.Domain.Statistics;

namespace PokemonTools.ApiService.Domain.Tests.Statistics;

public class StatsCalculator_CalculateTests
{
    [Theory]
    [InlineData(
        45u, 49u, 49u, 65u, 65u, 45u,
        31u, 31u, 31u, 31u, 31u, 31u,
        0u, 0u, 0u, 0u, 0u, 0u,
        null,
        Nature.Hardy,
        120u, 69u, 69u, 85u, 85u, 65u
    )]
    [InlineData(
        130u, 120u, 120u, 95u, 95u, 60u,
        31u, 31u, 31u, 31u, 31u, 31u,
        252u, 252u, 0u, 0u, 4u, 0u,
        50u,
        Nature.Adamant,
        237u, 189u, 140u, 103u, 116u, 80u
    )]
    [InlineData(
        70u, 55u, 65u, 95u, 105u, 85u,
        31u, 31u, 31u, 31u, 31u, 31u,
        0u, 0u, 0u, 0u, 0u, 0u,
        50u,
        Nature.Lonely,
        145u, 82u, 76u, 115u, 125u, 105u
    )]
    [InlineData(
        1u, 1u, 1u, 1u, 1u, 1u,
        0u, 0u, 0u, 0u, 0u, 0u,
        0u, 0u, 0u, 0u, 0u, 0u,
        1u,
        Nature.Timid,
        11u, 4u, 5u, 5u, 5u, 5u
    )]
    // レベル78 いじっぱり ガブリアス https://bulbapedia.bulbagarden.net/wiki/Stat にあった例
    [InlineData(
        108u, 130u, 95u, 80u, 85u, 102u,
        24u, 12u, 30u, 16u, 23u, 5u,
        74u, 190u, 91u, 48u, 84u, 23u,
        78u,
        Nature.Adamant,
        289u, 278u, 193u, 135u, 171u, 171u
    )]
    public void 各種パラメータ組み合わせ_期待通りのステータスを計算する(
        uint baseHp, uint baseAttack, uint baseDefense, uint baseSpecialAttack, uint baseSpecialDefense, uint baseSpeed,
        uint individualHp, uint individualAttack, uint individualDefense, uint individualSpecialAttack, uint individualSpecialDefense, uint individualSpeed,
        uint effortHp, uint effortAttack, uint effortDefense, uint effortSpecialAttack, uint effortSpecialDefense, uint effortSpeed,
        uint? levelValue,
        Nature nature,
        uint expectedHp, uint expectedAttack, uint expectedDefense, uint expectedSpecialAttack, uint expectedSpecialDefense, uint expectedSpeed
    )
    {
        // Arrange
        var baseStats = new BaseStats(baseHp, baseAttack, baseDefense, baseSpecialAttack, baseSpecialDefense, baseSpeed);
        var individualValues = new IndividualValues(individualHp, individualAttack, individualDefense, individualSpecialAttack, individualSpecialDefense, individualSpeed);
        var effortValues = new EffortValues(effortHp, effortAttack, effortDefense, effortSpecialAttack, effortSpecialDefense, effortSpeed);

        // Act
        var actual = levelValue.HasValue
            ? StatsCalculator.Calculate(baseStats, individualValues, effortValues, nature, new Level(levelValue.Value))
            : StatsCalculator.Calculate(baseStats, individualValues, effortValues, nature);

        // Assert
        Assert.Equal(expectedHp, actual.Hp);
        Assert.Equal(expectedAttack, actual.Attack);
        Assert.Equal(expectedDefense, actual.Defense);
        Assert.Equal(expectedSpecialAttack, actual.SpecialAttack);
        Assert.Equal(expectedSpecialDefense, actual.SpecialDefense);
        Assert.Equal(expectedSpeed, actual.Speed);
    }

    [Theory]
    [MemberData(nameof(NatureAdjustmentData))]
    public void 各性格の補正が10パーセント増減となる_期待通りの補正がかかる(Nature nature, string increasedStatName, string decreasedStatName)
    {
        // Arrange
        var baseStats = new BaseStats(80, 80, 80, 80, 80, 80);
        var individualValues = new IndividualValues(0, 0, 0, 0, 0, 0);
        var effortValues = new EffortValues(0, 0, 0, 0, 0, 0);
        var level = new Level(50);

        var neutral = StatsCalculator.Calculate(baseStats, individualValues, effortValues, Nature.Serious, level);

        // Act
        var actual = StatsCalculator.Calculate(baseStats, individualValues, effortValues, nature, level);

        // Assert
        var neutralIncrease = GetStatValue(neutral, increasedStatName);
        var neutralDecrease = GetStatValue(neutral, decreasedStatName);
        var expectedIncreased = (uint)Math.Floor(neutralIncrease * 1.1);
        var expectedDecreased = (uint)Math.Floor(neutralDecrease * 0.9);

        Assert.Equal(expectedIncreased, GetStatValue(actual, increasedStatName));
        Assert.Equal(expectedDecreased, GetStatValue(actual, decreasedStatName));
    }

    public static TheoryData<Nature, string, string> NatureAdjustmentData => new()
    {
        { Nature.Lonely, nameof(Stats.Attack), nameof(Stats.Defense) },
        { Nature.Adamant, nameof(Stats.Attack), nameof(Stats.SpecialAttack) },
        { Nature.Naughty, nameof(Stats.Attack), nameof(Stats.SpecialDefense) },
        { Nature.Brave, nameof(Stats.Attack), nameof(Stats.Speed) },
        { Nature.Bold, nameof(Stats.Defense), nameof(Stats.Attack) },
        { Nature.Impish, nameof(Stats.Defense), nameof(Stats.SpecialAttack) },
        { Nature.Lax, nameof(Stats.Defense), nameof(Stats.SpecialDefense) },
        { Nature.Relaxed, nameof(Stats.Defense), nameof(Stats.Speed) },
        { Nature.Modest, nameof(Stats.SpecialAttack), nameof(Stats.Attack) },
        { Nature.Mild, nameof(Stats.SpecialAttack), nameof(Stats.Defense) },
        { Nature.Rash, nameof(Stats.SpecialAttack), nameof(Stats.SpecialDefense) },
        { Nature.Quiet, nameof(Stats.SpecialAttack), nameof(Stats.Speed) },
        { Nature.Calm, nameof(Stats.SpecialDefense), nameof(Stats.Attack) },
        { Nature.Gentle, nameof(Stats.SpecialDefense), nameof(Stats.Defense) },
        { Nature.Careful, nameof(Stats.SpecialDefense), nameof(Stats.SpecialAttack) },
        { Nature.Sassy, nameof(Stats.SpecialDefense), nameof(Stats.Speed) },
        { Nature.Timid, nameof(Stats.Speed), nameof(Stats.Attack) },
        { Nature.Hasty, nameof(Stats.Speed), nameof(Stats.Defense) },
        { Nature.Jolly, nameof(Stats.Speed), nameof(Stats.SpecialAttack) },
        { Nature.Naive, nameof(Stats.Speed), nameof(Stats.SpecialDefense) },
    };

    private static uint GetStatValue(Stats stats, string statName)
    {
        return statName switch
        {
            nameof(Stats.Hp) => stats.Hp,
            nameof(Stats.Attack) => stats.Attack,
            nameof(Stats.Defense) => stats.Defense,
            nameof(Stats.SpecialAttack) => stats.SpecialAttack,
            nameof(Stats.SpecialDefense) => stats.SpecialDefense,
            nameof(Stats.Speed) => stats.Speed,
            _ => throw new ArgumentOutOfRangeException(nameof(statName), statName, "存在しない能力値です"),
        };
    }
}
