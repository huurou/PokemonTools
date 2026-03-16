using PokemonTools.ApiService.Domain.Damages;

namespace PokemonTools.ApiService.Domain.Tests.Damages;

public class DamageCalculator_CalculateTests
{
    private static Damage CalculateWithDefaults(
        uint power = 100u,
        IReadOnlyList<uint>? powerModifiers = null,
        uint attackStat = 182u,
        int attackStage = 0,
        IReadOnlyList<uint>? attackModifiers = null,
        uint defenseStat = 178u,
        int defenseStage = 0,
        IReadOnlyList<uint>? defenseModifiers = null,
        IReadOnlyList<uint>? damageModifiers = null,
        uint attackerLevel = 50u,
        bool isCriticalHit = false,
        StabType stabType = StabType.None,
        double typeEffectiveness = 1.0)
    {
        return DamageCalculator.Calculate(
            power, powerModifiers ?? [],
            attackStat, attackStage, attackModifiers ?? [],
            defenseStat, defenseStage, defenseModifiers ?? [],
            damageModifiers ?? [],
            attackerLevel, isCriticalHit, stabType, typeEffectiveness);
    }

    [Fact]
    // https://latest.pokewiki.net/%E3%83%80%E3%83%A1%E3%83%BC%E3%82%B8%E8%A8%88%E7%AE%97%E5%BC%8F に書いてあった例
    public void Lv50A182ガブリアスストーンエッジvsB178クレセリア_期待されたダメージが返る()
    {
        // Act
        var damage = CalculateWithDefaults();

        // Assert
        List<uint> expected = [39u, 39u, 40u, 40u, 40u, 41u, 41u, 42u, 42u, 43u, 43u, 44u, 44u, 45u, 45u, 46u];
        Assert.Equal(expected, damage.Values);
    }

    [Theory]
    [InlineData(-6, new uint[] { 11u, 11u, 11u, 11u, 11u, 11u, 11u, 11u, 12u, 12u, 12u, 12u, 12u, 12u, 12u, 13u })]
    [InlineData(-5, new uint[] { 11u, 12u, 12u, 12u, 12u, 12u, 12u, 12u, 13u, 13u, 13u, 13u, 13u, 13u, 13u, 14u })]
    [InlineData(-4, new uint[] { 13u, 13u, 13u, 14u, 14u, 14u, 14u, 14u, 14u, 15u, 15u, 15u, 15u, 15u, 15u, 16u })]
    [InlineData(-3, new uint[] { 16u, 16u, 16u, 16u, 16u, 17u, 17u, 17u, 17u, 17u, 18u, 18u, 18u, 18u, 18u, 19u })]
    [InlineData(-2, new uint[] { 20u, 20u, 20u, 21u, 21u, 21u, 21u, 22u, 22u, 22u, 22u, 23u, 23u, 23u, 23u, 24u })]
    [InlineData(-1, new uint[] { 26u, 26u, 26u, 27u, 27u, 27u, 28u, 28u, 28u, 29u, 29u, 29u, 30u, 30u, 30u, 31u })]
    [InlineData(0, new uint[] { 39u, 39u, 40u, 40u, 40u, 41u, 41u, 42u, 42u, 43u, 43u, 44u, 44u, 45u, 45u, 46u })]
    [InlineData(1, new uint[] { 58u, 59u, 60u, 60u, 61u, 62u, 62u, 63u, 64u, 64u, 65u, 66u, 66u, 67u, 68u, 69u })]
    [InlineData(2, new uint[] { 77u, 78u, 79u, 80u, 80u, 81u, 82u, 83u, 84u, 85u, 86u, 87u, 88u, 89u, 90u, 91u })]
    [InlineData(3, new uint[] { 96u, 98u, 99u, 100u, 101u, 102u, 103u, 104u, 106u, 107u, 108u, 109u, 110u, 111u, 112u, 114u })]
    [InlineData(4, new uint[] { 115u, 116u, 118u, 119u, 121u, 122u, 123u, 125u, 126u, 127u, 129u, 130u, 131u, 133u, 134u, 136u })]
    [InlineData(5, new uint[] { 135u, 136u, 138u, 139u, 141u, 143u, 144u, 146u, 147u, 149u, 151u, 152u, 154u, 155u, 157u, 159u })]
    [InlineData(6, new uint[] { 153u, 155u, 157u, 159u, 161u, 162u, 164u, 166u, 168u, 170u, 171u, 173u, 175u, 177u, 179u, 181u })]
    public void 攻撃ランクを変化させる_攻撃ランク別の期待されたダメージが返る(int attackStage, uint[] expected)
    {
        // Act
        var damage = CalculateWithDefaults(attackStage: attackStage);

        // Assert
        Assert.Equal(expected, damage.Values);
    }

    [Theory]
    [InlineData(-6, new uint[] { 156u, 158u, 160u, 161u, 163u, 165u, 167u, 169u, 171u, 172u, 174u, 176u, 178u, 180u, 182u, 184u })]
    [InlineData(-5, new uint[] { 137u, 139u, 140u, 142u, 144u, 145u, 147u, 149u, 150u, 152u, 153u, 155u, 157u, 158u, 160u, 162u })]
    [InlineData(-4, new uint[] { 116u, 117u, 119u, 120u, 121u, 123u, 124u, 126u, 127u, 128u, 130u, 131u, 132u, 134u, 135u, 137u })]
    [InlineData(-3, new uint[] { 96u, 98u, 99u, 100u, 101u, 102u, 103u, 104u, 106u, 107u, 108u, 109u, 110u, 111u, 112u, 114u })]
    [InlineData(-2, new uint[] { 77u, 78u, 79u, 80u, 80u, 81u, 82u, 83u, 84u, 85u, 86u, 87u, 88u, 89u, 90u, 91u })]
    [InlineData(-1, new uint[] { 58u, 59u, 60u, 60u, 61u, 62u, 62u, 63u, 64u, 64u, 65u, 66u, 66u, 67u, 68u, 69u })]
    [InlineData(0, new uint[] { 39u, 39u, 40u, 40u, 40u, 41u, 41u, 42u, 42u, 43u, 43u, 44u, 44u, 45u, 45u, 46u })]
    [InlineData(1, new uint[] { 26u, 26u, 26u, 27u, 27u, 27u, 28u, 28u, 28u, 29u, 29u, 29u, 30u, 30u, 30u, 31u })]
    [InlineData(2, new uint[] { 20u, 20u, 20u, 21u, 21u, 21u, 21u, 22u, 22u, 22u, 22u, 23u, 23u, 23u, 23u, 24u })]
    [InlineData(3, new uint[] { 16u, 16u, 16u, 16u, 16u, 17u, 17u, 17u, 17u, 17u, 18u, 18u, 18u, 18u, 18u, 19u })]
    [InlineData(4, new uint[] { 13u, 13u, 13u, 14u, 14u, 14u, 14u, 14u, 14u, 15u, 15u, 15u, 15u, 15u, 15u, 16u })]
    [InlineData(5, new uint[] { 11u, 12u, 12u, 12u, 12u, 12u, 12u, 12u, 13u, 13u, 13u, 13u, 13u, 13u, 13u, 14u })]
    [InlineData(6, new uint[] { 11u, 11u, 11u, 11u, 11u, 11u, 11u, 11u, 12u, 12u, 12u, 12u, 12u, 12u, 12u, 13u })]
    public void 防御ランクを変化させる_防御ランク別の期待されたダメージが返る(int defenseStage, uint[] expected)
    {
        // Act
        var damage = CalculateWithDefaults(defenseStage: defenseStage);

        // Assert
        Assert.Equal(expected, damage.Values);
    }

    [Fact]
    public void 威力補正値を追加する_威力補正値を反映した期待されたダメージが返る()
    {
        // Act
        // すなのちから、ちからのハチマキ
        var damage = CalculateWithDefaults(powerModifiers: [5325u, 4505u]);

        // Assert
        List<uint> expected = [56u, 56u, 57u, 58u, 58u, 59u, 60u, 60u, 61u, 62u, 62u, 63u, 64u, 64u, 65u, 66u];
        Assert.Equal(expected, damage.Values);
    }

    [Fact]
    public void 攻撃補正値を追加する_攻撃補正値を反映した期待されたダメージが返る()
    {
        // Act
        // スロースタート、こだわりハチマキ
        var damage = CalculateWithDefaults(attackModifiers: [2048u, 6144u]);

        // Assert
        List<uint> expected = [29u, 30u, 30u, 30u, 31u, 31u, 31u, 32u, 32u, 32u, 33u, 33u, 33u, 34u, 34u, 35u];
        Assert.Equal(expected, damage.Values);
    }

    [Fact]
    public void 防御補正値を追加する_防御補正値を反映した期待されたダメージが返る()
    {
        // Act
        // ファーコート、しんかのきせき
        var damage = CalculateWithDefaults(defenseModifiers: [8192u, 6144u]);

        // Assert
        List<uint> expected = [13u, 13u, 13u, 14u, 14u, 14u, 14u, 14u, 14u, 15u, 15u, 15u, 15u, 15u, 15u, 16u];
        Assert.Equal(expected, damage.Values);
    }

    [Fact]
    public void ダメージ補正値を追加する_ダメージ補正値を反映した期待されたダメージが返る()
    {
        // Act
        // リフレクター、いのちのたま
        var damage = CalculateWithDefaults(damageModifiers: [2048u, 5324u]);

        // Assert
        List<uint> expected = [25u, 25u, 26u, 26u, 26u, 27u, 27u, 27u, 27u, 28u, 28u, 29u, 29u, 29u, 29u, 30u];
        Assert.Equal(expected, damage.Values);
    }

    [Fact]
    public void タイプ一致_期待されたダメージが返る()
    {
        // Act
        var damage = CalculateWithDefaults(stabType: StabType.Normal);

        // Assert
        List<uint> expected = [58u, 58u, 60u, 60u, 60u, 61u, 61u, 63u, 63u, 64u, 64u, 66u, 66u, 67u, 67u, 69u];
        Assert.Equal(expected, damage.Values);
    }

    [Fact]
    public void 適応力が有効_期待されたダメージが返る()
    {
        // Act
        var damage = CalculateWithDefaults(stabType: StabType.Adaptability);

        // Assert
        List<uint> expected = [78u, 78u, 80u, 80u, 80u, 82u, 82u, 84u, 84u, 86u, 86u, 88u, 88u, 90u, 90u, 92u];
        Assert.Equal(expected, damage.Values);
    }

    [Fact]
    public void 適応力かつテラスタルが有効_期待されたダメージが返る()
    {
        // Act
        var damage = CalculateWithDefaults(stabType: StabType.AdaptabilityTerastal);

        // Assert
        List<uint> expected = [88u, 88u, 90u, 90u, 90u, 92u, 92u, 94u, 94u, 97u, 97u, 99u, 99u, 101u, 101u, 103u];
        Assert.Equal(expected, damage.Values);
    }

    [Fact]
    public void 急所かつ攻撃ランクがマイナス_マイナスランクが無視されランク0相当のダメージが返る()
    {
        // Act
        var damage = CalculateWithDefaults(attackStage: -6, isCriticalHit: true);

        // Assert
        // 急所はマイナス攻撃ランクを無視するので、ランク0+急所1.5倍と同じ結果
        var damageAtStage0 = CalculateWithDefaults(attackStage: 0, isCriticalHit: true);
        Assert.Equal(damageAtStage0.Values, damage.Values);
    }

    [Fact]
    public void 急所かつ防御ランクがプラス_プラスランクが無視されランク0相当のダメージが返る()
    {
        // Act
        var damage = CalculateWithDefaults(defenseStage: 6, isCriticalHit: true);

        // Assert
        // 急所はプラス防御ランクを無視するので、ランク0+急所1.5倍と同じ結果
        var damageAtStage0 = CalculateWithDefaults(defenseStage: 0, isCriticalHit: true);
        Assert.Equal(damageAtStage0.Values, damage.Values);
    }

    [Fact]
    public void 急所かつ攻撃ランクがプラス_プラスランクはそのまま適用される()
    {
        // Act
        var damage = CalculateWithDefaults(attackStage: 6, isCriticalHit: true);

        // Assert
        // 急所でもプラス攻撃ランクは無視しない
        List<uint> expected = [230u, 233u, 235u, 238u, 241u, 243u, 246u, 249u, 252u, 254u, 257u, 260u, 262u, 265u, 268u, 271u];
        Assert.Equal(expected, damage.Values);
    }


    [Fact]
    public void 急所かつ防御ランクがマイナス_マイナスランクはそのまま適用される()
    {
        // Act
        var damage = CalculateWithDefaults(defenseStage: -6, isCriticalHit: true);

        // Assert
        // 急所でもマイナス防御ランクは無視しない
        List<uint> expected = [234u, 237u, 240u, 242u, 245u, 248u, 251u, 253u, 256u, 259u, 262u, 264u, 267u, 270u, 273u, 276u];
        Assert.Equal(expected, damage.Values);
    }

    [Fact]
    public void タイプ相性が効果なし_ダメージが全て0になる()
    {
        // Act
        var damage = CalculateWithDefaults(typeEffectiveness: 0.0);

        // Assert
        List<uint> expected = [0u, 0u, 0u, 0u, 0u, 0u, 0u, 0u, 0u, 0u, 0u, 0u, 0u, 0u, 0u, 0u];
        Assert.Equal(expected, damage.Values);
    }

    [Theory]
    [InlineData(-7)]
    [InlineData(7)]
    public void 攻撃ランクが範囲外_ArgumentOutOfRangeExceptionがスローされる(int attackStage)
    {
        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => CalculateWithDefaults(attackStage: attackStage));
    }

    [Theory]
    [InlineData(-7)]
    [InlineData(7)]
    public void 防御ランクが範囲外_ArgumentOutOfRangeExceptionがスローされる(int defenseStage)
    {
        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => CalculateWithDefaults(defenseStage: defenseStage));
    }

    [Fact]
    public void ダメージの16bitオーバーフロー_オーバーフローした値が返る()
    {
        /*
         * https://tetspond.hatenablog.com/entry/2020/12/05/122654
         * 攻撃役　エースバーン
         * ・レベル99
         * ・特攻実数値222
         * ・特攻ランク3段階上昇
         * ・ほのおタイプ
         * ・特性「てきおうりょく」
         * ・持ち物「ピントレンズ」
         * ・きゅうしょアップ状態
         *
         * 防御役　ゴース
         * ・むしはがねタイプ（タイプを変更している）
         * ・もりののろい状態（上記のタイプにくさタイプを追加している）
         * ・タールショット状態
         * ・レベル1
         * ・特防実数値4
         * ・特防ランク6段階下降
         *
         * 使用技　ブラストバーン
         * ・特殊技
         * ・ほのおタイプ
         * ・威力150
         */
        // Act
        var damage = CalculateWithDefaults(
            power: 150u,
            attackStat: 222u,
            attackStage: 3,
            defenseStat: 4u,
            defenseStage: -6,
            attackerLevel: 99u,
            isCriticalHit: true,
            stabType: StabType.Adaptability,
            typeEffectiveness: 16.0
        );

        // Assert
        // 補正値がない場合は補正値の初期値4096もかけられないと仮定すると途中で32bitオーバーフローが起きず、1より小さい場合0になることもなくこうなる
        // 補正値がなくても初期値4096がかけられ、1より小さい場合0になる処理がオーバーフロー後起きるなら、0ではなく1になる箇所が生まれるはず 真相は不明
        // とりあえず前者を採用する
        List<uint> expected = [32768u, 0u, 32768u, 0u, 32768u, 0u, 32768u, 0u, 32768u, 0u, 32768u, 0u, 32768u, 0u, 32768u, 0u];
        Assert.Equal(expected, damage.Values);
    }
}
