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
    public void 全単タイプ相性の確認_正しい結果が返る(string attackTypeId, string defenseTypeId, TypeEffectiveness expected)
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
    public static TheoryData<string, string, TypeEffectiveness> 全単タイプ相性データ
    {
        get
        {
            var data = new TheoryData<string, string, TypeEffectiveness>();
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
    public void 複合タイプ相性の確認_正しい結果が返る(string attackTypeId, string defenseType1Id, string defenseType2Id, TypeEffectiveness expected)
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

    public static TheoryData<string, string, string, TypeEffectiveness> 複合タイプ相性データ => new()
    {
        // --- ExtremelyEffective (4倍) ---
        { "electric", "water", "flying", TypeEffectiveness.ExtremelyEffective },
        { "ground", "fire", "steel", TypeEffectiveness.ExtremelyEffective },
        { "grass", "water", "ground", TypeEffectiveness.ExtremelyEffective },
        { "ice", "grass", "ground", TypeEffectiveness.ExtremelyEffective },
        { "fighting", "normal", "rock", TypeEffectiveness.ExtremelyEffective },

        // --- SuperEffective (2倍) ---
        { "fire", "grass", "normal", TypeEffectiveness.SuperEffective },
        { "water", "ground", "normal", TypeEffectiveness.SuperEffective },
        { "ice", "flying", "normal", TypeEffectiveness.SuperEffective },
        { "dark", "ghost", "normal", TypeEffectiveness.SuperEffective },
        { "fairy", "dragon", "normal", TypeEffectiveness.SuperEffective },

        // --- Neutral (等倍) ---
        { "fire", "grass", "water", TypeEffectiveness.Neutral },
        { "water", "fire", "grass", TypeEffectiveness.Neutral },
        { "electric", "water", "grass", TypeEffectiveness.Neutral },
        { "normal", "normal", "fighting", TypeEffectiveness.Neutral },
        { "fighting", "ice", "flying", TypeEffectiveness.Neutral },

        // --- NotVeryEffective (0.5倍) ---
        { "fire", "water", "normal", TypeEffectiveness.NotVeryEffective },
        { "grass", "fire", "normal", TypeEffectiveness.NotVeryEffective },
        { "electric", "grass", "normal", TypeEffectiveness.NotVeryEffective },
        { "normal", "rock", "fighting", TypeEffectiveness.NotVeryEffective },
        { "dragon", "steel", "normal", TypeEffectiveness.NotVeryEffective },

        // --- MostlyIneffective (0.25倍) ---
        { "fire", "water", "dragon", TypeEffectiveness.MostlyIneffective },
        { "grass", "fire", "flying", TypeEffectiveness.MostlyIneffective },
        { "grass", "poison", "flying", TypeEffectiveness.MostlyIneffective },
        { "bug", "fire", "flying", TypeEffectiveness.MostlyIneffective },
        { "electric", "grass", "dragon", TypeEffectiveness.MostlyIneffective },

        // --- HasNoEffect (効果なし) ---
        { "normal", "ghost", "dark", TypeEffectiveness.HasNoEffect },
        { "electric", "water", "ground", TypeEffectiveness.HasNoEffect },
        { "fighting", "ghost", "dark", TypeEffectiveness.HasNoEffect },
        { "ground", "flying", "steel", TypeEffectiveness.HasNoEffect },
        { "dragon", "fairy", "dragon", TypeEffectiveness.HasNoEffect },
        { "psychic", "dark", "fighting", TypeEffectiveness.HasNoEffect },
        { "poison", "steel", "fairy", TypeEffectiveness.HasNoEffect },
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
        string attackTypeId, string defenseType1Id, string defenseType2Id)
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

    public static TheoryData<string, string, string> 防御タイプ順序テストデータ => new()
    {
        { "electric", "water", "flying" },
        { "fire", "grass", "water" },
        { "normal", "ghost", "dark" },
        { "ground", "fire", "steel" },
        { "ice", "grass", "ground" },
        { "fighting", "ghost", "dark" },
    };

    #endregion 複合タイプ動作テスト

    private static PokemonType FindType(string typeId)
    {
        return PokemonType.All.Single(x => x.Id.Value == typeId);
    }
}
