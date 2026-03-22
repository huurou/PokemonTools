namespace PokemonTools.Web.Domain.Statistics;

/// <summary>
/// 個体値を表現するクラス
/// </summary>
public record IndividualValues
{
    /// <summary>HP</summary>
    public uint Hp
    {
        get;
        init
        {
            ArgumentOutOfRangeException.ThrowIfGreaterThan(value, 31u);
            field = value;
        }
    }

    /// <summary>攻撃</summary>
    public uint Attack
    {
        get;
        init
        {
            ArgumentOutOfRangeException.ThrowIfGreaterThan(value, 31u);
            field = value;
        }
    }

    /// <summary>防御</summary>
    public uint Defense
    {
        get;
        init
        {
            ArgumentOutOfRangeException.ThrowIfGreaterThan(value, 31u);
            field = value;
        }
    }

    /// <summary>特攻</summary>
    public uint SpecialAttack
    {
        get;
        init
        {
            ArgumentOutOfRangeException.ThrowIfGreaterThan(value, 31u);
            field = value;
        }
    }

    /// <summary>特防</summary>
    public uint SpecialDefense
    {
        get;
        init
        {
            ArgumentOutOfRangeException.ThrowIfGreaterThan(value, 31u);
            field = value;
        }
    }

    /// <summary>素早さ</summary>
    public uint Speed
    {
        get;
        init
        {
            ArgumentOutOfRangeException.ThrowIfGreaterThan(value, 31u);
            field = value;
        }
    }

    public IndividualValues(uint hp, uint attack, uint defense, uint specialAttack, uint specialDefense, uint speed)
    {
        Hp = hp;
        Attack = attack;
        Defense = defense;
        SpecialAttack = specialAttack;
        SpecialDefense = specialDefense;
        Speed = speed;
    }
}
