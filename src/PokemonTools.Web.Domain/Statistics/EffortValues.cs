namespace PokemonTools.Web.Domain.Statistics;

/// <summary>
/// 努力値を表現するクラス
/// </summary>
public record EffortValues
{
    /// <summary>HP</summary>
    public uint Hp
    {
        get;
        private init
        {
            ArgumentOutOfRangeException.ThrowIfGreaterThan(value, 252u);
            field = value;
        }
    }

    /// <summary>攻撃</summary>
    public uint Attack
    {
        get;
        private init
        {
            ArgumentOutOfRangeException.ThrowIfGreaterThan(value, 252u);
            field = value;
        }
    }

    /// <summary>防御</summary>
    public uint Defense
    {
        get;
        private init
        {
            ArgumentOutOfRangeException.ThrowIfGreaterThan(value, 252u);
            field = value;
        }
    }

    /// <summary>特攻</summary>
    public uint SpecialAttack
    {
        get;
        private init
        {
            ArgumentOutOfRangeException.ThrowIfGreaterThan(value, 252u);
            field = value;
        }
    }

    /// <summary>特防</summary>
    public uint SpecialDefense
    {
        get;
        private init
        {
            ArgumentOutOfRangeException.ThrowIfGreaterThan(value, 252u);
            field = value;
        }
    }

    /// <summary>素早さ</summary>
    public uint Speed
    {
        get;
        private init
        {
            ArgumentOutOfRangeException.ThrowIfGreaterThan(value, 252u);
            field = value;
        }
    }

    public EffortValues(uint hp, uint attack, uint defense, uint specialAttack, uint specialDefense, uint speed)
    {
        Hp = hp;
        Attack = attack;
        Defense = defense;
        SpecialAttack = specialAttack;
        SpecialDefense = specialDefense;
        Speed = speed;

        var total = Hp + Attack + Defense + SpecialAttack + SpecialDefense + Speed;
        ArgumentOutOfRangeException.ThrowIfGreaterThan(total, 510u);
    }

    public EffortValues SetValues(
        uint? hp = null, uint? attack = null, uint? defense = null,
        uint? specialAttack = null, uint? specialDefense = null, uint? speed = null
    ) => new(
        hp ?? Hp, attack ?? Attack, defense ?? Defense,
        specialAttack ?? SpecialAttack, specialDefense ?? SpecialDefense, speed ?? Speed);
}
