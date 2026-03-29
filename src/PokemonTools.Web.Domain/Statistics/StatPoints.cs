namespace PokemonTools.Web.Domain.Statistics;

/// <summary>
/// 能力ポイントを表現するクラス
/// </summary>
public record StatPoints
{
    // 各能力ポイントは合計66の不変条件を共有するため、with式での個別更新を防ぐ。
    // 値の変更はSetValues経由で行う。

    /// <summary>
    /// HP
    /// </summary>
    public uint Hp
    {
        get;
        private init
        {
            ArgumentOutOfRangeException.ThrowIfGreaterThan(value, 32u);
            field = value;
        }
    }

    /// <summary>
    /// 攻撃
    /// </summary>
    public uint Attack
    {
        get;
        private init
        {
            ArgumentOutOfRangeException.ThrowIfGreaterThan(value, 32u);
            field = value;
        }
    }

    /// <summary>
    /// 防御
    /// </summary>
    public uint Defense
    {
        get;
        private init
        {
            ArgumentOutOfRangeException.ThrowIfGreaterThan(value, 32u);
            field = value;
        }
    }

    /// <summary>
    /// 特攻
    /// </summary>
    public uint SpecialAttack
    {
        get;
        private init
        {
            ArgumentOutOfRangeException.ThrowIfGreaterThan(value, 32u);
            field = value;
        }
    }

    /// <summary>
    /// 特防
    /// </summary>
    public uint SpecialDefense
    {
        get;
        private init
        {
            ArgumentOutOfRangeException.ThrowIfGreaterThan(value, 32u);
            field = value;
        }
    }

    /// <summary>
    /// 素早さ
    /// </summary>
    public uint Speed
    {
        get;
        private init
        {
            ArgumentOutOfRangeException.ThrowIfGreaterThan(value, 32u);
            field = value;
        }
    }

    public StatPoints(uint hp, uint attack, uint defense, uint specialAttack, uint specialDefense, uint speed)
    {
        Hp = hp;
        Attack = attack;
        Defense = defense;
        SpecialAttack = specialAttack;
        SpecialDefense = specialDefense;
        Speed = speed;

        var total = Hp + Attack + Defense + SpecialAttack + SpecialDefense + Speed;
        ArgumentOutOfRangeException.ThrowIfGreaterThan(total, 66u);
    }

    public StatPoints SetValues(
        uint? hp = null,
        uint? attack = null,
        uint? defense = null,
        uint? specialAttack = null,
        uint? specialDefense = null,
        uint? speed = null
    )
    {
        return new StatPoints(
            hp ?? Hp,
            attack ?? Attack,
            defense ?? Defense,
            specialAttack ?? SpecialAttack,
            specialDefense ?? SpecialDefense,
            speed ?? Speed
        );
    }
}
