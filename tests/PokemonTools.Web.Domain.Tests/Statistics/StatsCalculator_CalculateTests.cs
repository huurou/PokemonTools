using PokemonTools.Web.Domain.Statistics;

namespace PokemonTools.Web.Domain.Tests.Statistics;

public class StatsCalculator_CalculateTests
{
    [Theory]
    [MemberData(nameof(CalculateData))]
    public void 各種パラメータ組み合わせ_期待通りのステータスを計算する(
        uint[] baseStatsValues, uint[] ivValues, uint[] evValues,
        uint? levelValue, int natureId, uint[] expectedValues
    )
    {
        // Arrange
        var baseStats = new BaseStats(baseStatsValues[0], baseStatsValues[1], baseStatsValues[2], baseStatsValues[3], baseStatsValues[4], baseStatsValues[5]);
        var individualValues = new IndividualValues(ivValues[0], ivValues[1], ivValues[2], ivValues[3], ivValues[4], ivValues[5]);
        var effortValues = new EffortValues(evValues[0], evValues[1], evValues[2], evValues[3], evValues[4], evValues[5]);
        var nature = FindNature(natureId);
        var expected = new Stats(expectedValues[0], expectedValues[1], expectedValues[2], expectedValues[3], expectedValues[4], expectedValues[5]);

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
    public void 各性格の補正が10パーセント増減となる_期待通りの補正がかかる(int natureId, string increasedStatName, string decreasedStatName)
    {
        // Arrange
        var baseStats = new BaseStats(80, 80, 80, 80, 80, 80);
        var individualValues = new IndividualValues(0, 0, 0, 0, 0, 0);
        var effortValues = new EffortValues(0, 0, 0, 0, 0, 0);
        var level = new Level(50);
        var nature = FindNature(natureId);

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

    public static TheoryData<uint[], uint[], uint[], uint?, int, uint[]> CalculateData => new()
    {
        // フシギダネ Lv50 がんばりや 個体値MAX 努力値0
        {
            new uint[] { 45, 49, 49, 65, 65, 45 },
            new uint[] { 31, 31, 31, 31, 31, 31 },
            new uint[] { 0, 0, 0, 0, 0, 0 },
            null,
            1,
            new uint[] { 120, 69, 69, 85, 85, 65 }
        },
        // カビゴン Lv50 いじっぱり HP・攻撃極振り 特防4
        {
            new uint[] { 130, 120, 120, 95, 95, 60 },
            new uint[] { 31, 31, 31, 31, 31, 31 },
            new uint[] { 252, 252, 0, 0, 4, 0 },
            50u,
            11,
            new uint[] { 237, 189, 140, 103, 116, 80 }
        },
        // ミロカロス Lv50 さみしがり 個体値MAX 努力値0
        {
            new uint[] { 70, 55, 65, 95, 105, 85 },
            new uint[] { 31, 31, 31, 31, 31, 31 },
            new uint[] { 0, 0, 0, 0, 0, 0 },
            50u,
            6,
            new uint[] { 145, 82, 76, 115, 125, 105 }
        },
        // 種族値オール1 Lv1 おくびょう 個体値0 努力値0
        {
            new uint[] { 1, 1, 1, 1, 1, 1 },
            new uint[] { 0, 0, 0, 0, 0, 0 },
            new uint[] { 0, 0, 0, 0, 0, 0 },
            1u,
            5,
            new uint[] { 11, 4, 5, 5, 5, 5 }
        },
        // ガブリアス Lv78 いじっぱり (Bulbapedia の計算例)
        {
            new uint[] { 108, 130, 95, 80, 85, 102 },
            new uint[] { 24, 12, 30, 16, 23, 5 },
            new uint[] { 74, 190, 91, 48, 84, 23 },
            78u,
            11,
            new uint[] { 289, 278, 193, 135, 171, 171 }
        },
    };

    public static TheoryData<int, string, string> NatureAdjustmentData => new()
    {
        { 6, nameof(Stats.Attack), nameof(Stats.Defense) },
        { 11, nameof(Stats.Attack), nameof(Stats.SpecialAttack) },
        { 17, nameof(Stats.Attack), nameof(Stats.SpecialDefense) },
        { 21, nameof(Stats.Attack), nameof(Stats.Speed) },
        { 2, nameof(Stats.Defense), nameof(Stats.Attack) },
        { 12, nameof(Stats.Defense), nameof(Stats.SpecialAttack) },
        { 18, nameof(Stats.Defense), nameof(Stats.SpecialDefense) },
        { 22, nameof(Stats.Defense), nameof(Stats.Speed) },
        { 3, nameof(Stats.SpecialAttack), nameof(Stats.Attack) },
        { 8, nameof(Stats.SpecialAttack), nameof(Stats.Defense) },
        { 15, nameof(Stats.SpecialAttack), nameof(Stats.SpecialDefense) },
        { 23, nameof(Stats.SpecialAttack), nameof(Stats.Speed) },
        { 4, nameof(Stats.SpecialDefense), nameof(Stats.Attack) },
        { 9, nameof(Stats.SpecialDefense), nameof(Stats.Defense) },
        { 14, nameof(Stats.SpecialDefense), nameof(Stats.SpecialAttack) },
        { 24, nameof(Stats.SpecialDefense), nameof(Stats.Speed) },
        { 5, nameof(Stats.Speed), nameof(Stats.Attack) },
        { 10, nameof(Stats.Speed), nameof(Stats.Defense) },
        { 16, nameof(Stats.Speed), nameof(Stats.SpecialAttack) },
        { 20, nameof(Stats.Speed), nameof(Stats.SpecialDefense) },
    };

    private static Nature FindNature(int natureId)
    {
        return Nature.All.Single(x => x.Id.Value == natureId);
    }

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
