using PokemonTools.ApiService.Domain.Types;

namespace PokemonTools.ApiService.Domain.Tests.Types;

public class TypeChart_GetEffectivenessTests
{
    // 略記用エイリアス (相性表のコンパクト表記用)
    private static readonly TypeEffectiveness X = TypeEffectiveness.HasNoEffect;
    private static readonly TypeEffectiveness H = TypeEffectiveness.NotVeryEffective;
    private static readonly TypeEffectiveness N = TypeEffectiveness.Neutral;
    private static readonly TypeEffectiveness S = TypeEffectiveness.SuperEffective;

    #region 単タイプ相性テスト（全324組み合わせ）

    [Theory]
    [MemberData(nameof(全単タイプ相性データ))]
    public void 全単タイプ相性の確認_正しい結果が返る(int attackTypeId, int defenseTypeId, TypeEffectiveness expected)
    {
        // Arrange
        var attackType = FindType(attackTypeId);
        var defenseType = FindType(defenseTypeId);

        // Act
        var result = TypeChart.GetEffectiveness(attackType, defenseType);

        // Assert
        Assert.Equal(expected, result);
    }

    // 防御タイプの並び順: ノーマル, かくとう, ひこう, どく, じめん, いわ, むし, ゴースト, はがね, ほのお, みず, くさ, でんき, エスパー, こおり, ドラゴン, あく, フェアリー
    public static TheoryData<int, int, TypeEffectiveness> 全単タイプ相性データ
    {
        get
        {
            var data = new TheoryData<int, int, TypeEffectiveness>();
            var allTypes = PokemonType.All;

            // 相性表
            TypeEffectiveness[][] chart =
            [   //              ノ  か ひ ど  じ い む  ゴ は ほ  み く で  エ こ ド  あ フ
                /* ノーマル   */ [N, N, N, N, N, H, N, X, H, N, N, N, N, N, N, N, N, N],
                /* かくとう   */ [S, N, H, H, N, S, H, X, S, N, N, N, N, H, S, N, S, H],
                /* ひこう    */ [N, S, N, N, N, H, S, N, H, N, N, S, H, N, N, N, N, N],
                /* どく      */ [N, N, N, H, H, H, N, H, X, N, N, S, N, N, N, N, N, S],
                /* じめん    */ [N, N, X, S, N, S, H, N, S, S, N, H, S, N, N, N, N, N],
                /* いわ      */ [N, H, S, N, H, N, S, N, H, S, N, N, N, N, S, N, N, N],
                /* むし      */ [N, H, H, H, N, N, N, H, H, H, N, S, N, S, N, N, S, H],
                /* ゴースト   */ [X, N, N, N, N, N, N, S, N, N, N, N, N, S, N, N, H, N],
                /* はがね    */ [N, N, N, N, N, S, N, N, H, H, H, N, H, N, S, N, N, S],
                /* ほのお    */ [N, N, N, N, N, H, S, N, S, H, H, S, N, N, S, H, N, N],
                /* みず      */ [N, N, N, N, S, S, N, N, N, S, H, H, N, N, N, H, N, N],
                /* くさ      */ [N, N, H, H, S, S, H, N, H, H, S, H, N, N, N, H, N, N],
                /* でんき    */ [N, N, S, N, X, N, N, N, N, N, S, H, H, N, N, H, N, N],
                /* エスパー   */ [N, S, N, S, N, N, N, N, H, N, N, N, N, H, N, N, X, N],
                /* こおり    */ [N, N, S, N, S, N, N, N, H, H, H, S, N, N, H, S, N, N],
                /* ドラゴン   */ [N, N, N, N, N, N, N, N, H, N, N, N, N, N, N, S, N, X],
                /* あく      */ [N, H, N, N, N, N, N, S, N, N, N, N, N, S, N, N, H, H],
                /* フェアリー */ [N, S, N, H, N, N, N, N, H, H, N, N, N, N, N, S, S, N],
            ];

            for (var i = 0; i < allTypes.Count; i++)
            {
                for (var j = 0; j < allTypes.Count; j++)
                {
                    data.Add(allTypes[i].Id.Value, allTypes[j].Id.Value, chart[i][j]);
                }
            }

            return data;
        }
    }

    #endregion 単タイプ相性テスト（全324組み合わせ）

    #region 複合タイプ相性テスト

    [Theory]
    [MemberData(nameof(複合タイプ相性データ))]
    public void 複合タイプ相性の確認_正しい結果が返る(int attackTypeId, int defenseType1Id, int defenseType2Id, TypeEffectiveness expected)
    {
        // Arrange
        var attackType = FindType(attackTypeId);
        var defenseType1 = FindType(defenseType1Id);
        var defenseType2 = FindType(defenseType2Id);

        // Act
        var result = TypeChart.GetEffectiveness(attackType, defenseType1, defenseType2);

        // Assert
        Assert.Equal(expected, result);
    }

    public static TheoryData<int, int, int, TypeEffectiveness> 複合タイプ相性データ => new()
    {
        // --- ExtremelyEffective (4倍) ---
        { 13, 11, 3, TypeEffectiveness.ExtremelyEffective },
        { 5, 10, 9, TypeEffectiveness.ExtremelyEffective },
        { 12, 11, 5, TypeEffectiveness.ExtremelyEffective },
        { 15, 12, 5, TypeEffectiveness.ExtremelyEffective },
        { 2, 1, 6, TypeEffectiveness.ExtremelyEffective },

        // --- SuperEffective (2倍) ---
        { 10, 12, 1, TypeEffectiveness.SuperEffective },
        { 11, 5, 1, TypeEffectiveness.SuperEffective },
        { 15, 3, 1, TypeEffectiveness.SuperEffective },
        { 17, 8, 1, TypeEffectiveness.SuperEffective },
        { 18, 16, 1, TypeEffectiveness.SuperEffective },

        // --- Neutral (等倍) ---
        { 10, 12, 11, TypeEffectiveness.Neutral },
        { 11, 10, 12, TypeEffectiveness.Neutral },
        { 13, 11, 12, TypeEffectiveness.Neutral },
        { 1, 1, 2, TypeEffectiveness.Neutral },
        { 2, 15, 3, TypeEffectiveness.Neutral },

        // --- NotVeryEffective (0.5倍) ---
        { 10, 11, 1, TypeEffectiveness.NotVeryEffective },
        { 12, 10, 1, TypeEffectiveness.NotVeryEffective },
        { 13, 12, 1, TypeEffectiveness.NotVeryEffective },
        { 1, 6, 2, TypeEffectiveness.NotVeryEffective },
        { 16, 9, 1, TypeEffectiveness.NotVeryEffective },

        // --- MostlyIneffective (0.25倍) ---
        { 10, 11, 16, TypeEffectiveness.MostlyIneffective },
        { 12, 10, 3, TypeEffectiveness.MostlyIneffective },
        { 12, 4, 3, TypeEffectiveness.MostlyIneffective },
        { 7, 10, 3, TypeEffectiveness.MostlyIneffective },
        { 13, 12, 16, TypeEffectiveness.MostlyIneffective },

        // --- HasNoEffect (効果なし) ---
        { 1, 8, 17, TypeEffectiveness.HasNoEffect },
        { 13, 11, 5, TypeEffectiveness.HasNoEffect },
        { 2, 8, 17, TypeEffectiveness.HasNoEffect },
        { 5, 3, 9, TypeEffectiveness.HasNoEffect },
        { 16, 18, 16, TypeEffectiveness.HasNoEffect },
        { 14, 17, 2, TypeEffectiveness.HasNoEffect },
        { 4, 9, 18, TypeEffectiveness.HasNoEffect },
    };

    #endregion 複合タイプ相性テスト

    #region 複合タイプ動作テスト

    [Fact]
    public void 防御タイプ2がnull_単タイプとして計算()
    {
        // Act
        var result = TypeChart.GetEffectiveness(PokemonType.Fire, PokemonType.Grass, null);

        // Assert
        Assert.Equal(TypeEffectiveness.SuperEffective, result);
    }

    [Theory]
    [MemberData(nameof(防御タイプ順序テストデータ))]
    public void 防御タイプの順序の入れ替え_結果に影響しない(
        int attackTypeId, int defenseType1Id, int defenseType2Id)
    {
        // Arrange
        var attackType = FindType(attackTypeId);
        var defenseType1 = FindType(defenseType1Id);
        var defenseType2 = FindType(defenseType2Id);

        // Act
        var result1 = TypeChart.GetEffectiveness(attackType, defenseType1, defenseType2);
        var result2 = TypeChart.GetEffectiveness(attackType, defenseType2, defenseType1);

        // Assert
        Assert.Equal(result1, result2);
    }

    public static TheoryData<int, int, int> 防御タイプ順序テストデータ => new()
    {
        { 13, 11, 3 },
        { 10, 12, 11 },
        { 1, 8, 17 },
        { 5, 10, 9 },
        { 15, 12, 5 },
        { 2, 8, 17 },
    };

    #endregion 複合タイプ動作テスト

    private static PokemonType FindType(int typeId)
    {
        return PokemonType.All.Single(x => x.Id.Value == typeId);
    }
}
