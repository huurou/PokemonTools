namespace PokemonTools.Web.Domain.Statistics;

/// <summary>
/// 個体値を表現するクラス
/// </summary>
public record IndividualValues
{
    /// <summary>HP</summary>
    public uint Hp { get; init; }

    /// <summary>攻撃</summary>
    public uint Attack { get; init; }

    /// <summary>防御</summary>
    public uint Defense { get; init; }

    /// <summary>特攻</summary>
    public uint SpecialAttack { get; init; }

    /// <summary>特防</summary>
    public uint SpecialDefense { get; init; }

    /// <summary>素早さ</summary>
    public uint Speed { get; init; }

    public IndividualValues(uint hp, uint attack, uint defense, uint specialAttack, uint specialDefense, uint speed)
    {
        ArgumentOutOfRangeException.ThrowIfGreaterThan(hp, 31u);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(attack, 31u);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(defense, 31u);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(specialAttack, 31u);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(specialDefense, 31u);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(speed, 31u);

        Hp = hp;
        Attack = attack;
        Defense = defense;
        SpecialAttack = specialAttack;
        SpecialDefense = specialDefense;
        Speed = speed;
    }
}
