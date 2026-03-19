using PokemonTools.ApiService.Domain.Statistics;

namespace PokemonTools.ApiService.Domain.Tests.Statistics;

public class StatsCalculator_CalculateTests
{
    [Theory]
    [MemberData(nameof(CalculateData))]
    public void 各種パラメータ組み合わせ_期待通りのステータスを計算する(
        BaseStats baseStats, IndividualValues individualValues, EffortValues effortValues,
        uint? levelValue, Nature nature, Stats expected
    )
    {
        // Act
        var actual = levelValue.HasValue
            ? StatsCalculator.Calculate(baseStats, individualValues, effortValues, nature, new Level(levelValue.Value))
            : StatsCalculator.Calculate(baseStats, individualValues, effortValues, nature);

        // Assert
        Assert.Equal(expected.Hp, actual.Hp);
        Assert.Equal(expected.Attack, actual.Attack);
        Assert.Equal(expected.Defense, actual.Defense);
        Assert.Equal(expected.SpecialAttack, actual.SpecialAttack);
        Assert.Equal(expected.SpecialDefense, actual.SpecialDefense);
        Assert.Equal(expected.Speed, actual.Speed);
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

    public static TheoryData<BaseStats, IndividualValues, EffortValues, uint?, Nature, Stats> CalculateData => new()
    {
        // フシギダネ Lv50 がんばりや 個体値MAX 努力値0
        {
            new BaseStats(45, 49, 49, 65, 65, 45),
            new IndividualValues(31, 31, 31, 31, 31, 31),
            new EffortValues(0, 0, 0, 0, 0, 0),
            null,
            Nature.Hardy,
            new Stats(120, 69, 69, 85, 85, 65)
        },
        // カビゴン Lv50 いじっぱり HP・攻撃極振り 特防4
        {
            new BaseStats(130, 120, 120, 95, 95, 60),
            new IndividualValues(31, 31, 31, 31, 31, 31),
            new EffortValues(252, 252, 0, 0, 4, 0),
            50u,
            Nature.Adamant,
            new Stats(237, 189, 140, 103, 116, 80)
        },
        // ミロカロス Lv50 さみしがり 個体値MAX 努力値0
        {
            new BaseStats(70, 55, 65, 95, 105, 85),
            new IndividualValues(31, 31, 31, 31, 31, 31),
            new EffortValues(0, 0, 0, 0, 0, 0),
            50u,
            Nature.Lonely,
            new Stats(145, 82, 76, 115, 125, 105)
        },
        // 種族値オール1 Lv1 おくびょう 個体値0 努力値0
        {
            new BaseStats(1, 1, 1, 1, 1, 1),
            new IndividualValues(0, 0, 0, 0, 0, 0),
            new EffortValues(0, 0, 0, 0, 0, 0),
            1u,
            Nature.Timid,
            new Stats(11, 4, 5, 5, 5, 5)
        },
        // ガブリアス Lv78 いじっぱり (Bulbapedia の計算例)
        {
            new BaseStats(108, 130, 95, 80, 85, 102),
            new IndividualValues(24, 12, 30, 16, 23, 5),
            new EffortValues(74, 190, 91, 48, 84, 23),
            78u,
            Nature.Adamant,
            new Stats(289, 278, 193, 135, 171, 171)
        },
    };

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
