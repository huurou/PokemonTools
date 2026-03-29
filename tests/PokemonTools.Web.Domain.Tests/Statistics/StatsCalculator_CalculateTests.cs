using PokemonTools.Web.Domain.Statistics;

namespace PokemonTools.Web.Domain.Tests.Statistics;

public class StatsCalculator_CalculateTests
{
    [Theory]
    [MemberData(nameof(CalculateData))]
    public void 各種パラメータ組み合わせ_期待通りのステータスを計算する(
        uint[] baseStatsValues, uint[] statPointValues,
        int statAlignmentId, uint[] expectedValues
    )
    {
        // Arrange
        var baseStats = new BaseStats(baseStatsValues[0], baseStatsValues[1], baseStatsValues[2], baseStatsValues[3], baseStatsValues[4], baseStatsValues[5]);
        var statPoints = new StatPoints(statPointValues[0], statPointValues[1], statPointValues[2], statPointValues[3], statPointValues[4], statPointValues[5]);
        var statAlignment = FindStatAlignment(statAlignmentId);
        var expected = new Stats(expectedValues[0], expectedValues[1], expectedValues[2], expectedValues[3], expectedValues[4], expectedValues[5]);

        // Act
        var actual = StatsCalculator.Calculate(baseStats, statPoints, statAlignment);

        // Assert
        Assert.Equal(expected.Hp, actual.Hp);
        Assert.Equal(expected.Attack, actual.Attack);
        Assert.Equal(expected.Defense, actual.Defense);
        Assert.Equal(expected.SpecialAttack, actual.SpecialAttack);
        Assert.Equal(expected.SpecialDefense, actual.SpecialDefense);
        Assert.Equal(expected.Speed, actual.Speed);
    }

    [Theory]
    [MemberData(nameof(StatAlignmentAdjustmentData))]
    public void 各能力補正の補正が10パーセント増減となる_期待通りの補正がかかる(int statAlignmentId, string increasedStatName, string decreasedStatName)
    {
        // Arrange
        var baseStats = new BaseStats(80, 80, 80, 80, 80, 80);
        var statPoints = new StatPoints(0, 0, 0, 0, 0, 0);
        var statAlignment = FindStatAlignment(statAlignmentId);

        var neutral = StatsCalculator.Calculate(baseStats, statPoints, StatAlignment.Serious);

        // Act
        var actual = StatsCalculator.Calculate(baseStats, statPoints, statAlignment);

        // Assert
        var neutralIncrease = GetStatValue(neutral, increasedStatName);
        var neutralDecrease = GetStatValue(neutral, decreasedStatName);
        var expectedIncreased = (uint)Math.Floor(neutralIncrease * 1.1);
        var expectedDecreased = (uint)Math.Floor(neutralDecrease * 0.9);

        Assert.Equal(expectedIncreased, GetStatValue(actual, increasedStatName));
        Assert.Equal(expectedDecreased, GetStatValue(actual, decreasedStatName));
    }

    public static TheoryData<uint[], uint[], int, uint[]> CalculateData => new()
    {
        // フシギダネ がんばりや 能力ポイント0
        {
            new uint[] { 45, 49, 49, 65, 65, 45 },
            new uint[] { 0, 0, 0, 0, 0, 0 },
            1,
            new uint[] { 120, 69, 69, 85, 85, 65 }
        },
        // カビゴン いじっぱり HP32 攻撃32 特防1
        {
            new uint[] { 130, 120, 120, 95, 95, 60 },
            new uint[] { 32, 32, 0, 0, 1, 0 },
            11,
            new uint[] { 237, 189, 140, 103, 116, 80 }
        },
        // ミロカロス さみしがり 能力ポイント0
        {
            new uint[] { 70, 55, 65, 95, 105, 85 },
            new uint[] { 0, 0, 0, 0, 0, 0 },
            6,
            new uint[] { 145, 82, 76, 115, 125, 105 }
        },
    };

    public static TheoryData<int, string, string> StatAlignmentAdjustmentData => new()
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

    private static StatAlignment FindStatAlignment(int statAlignmentId)
    {
        return StatAlignment.All.Single(x => x.Id.Value == statAlignmentId);
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
