namespace PokemonTools.ApiService.Domain.Statistics;

/// <summary>
/// 個体値を表現するクラス
/// </summary>
public record IndividualValues
{
    /// <summary>HP</summary>
    public uint Hp { get; }
    /// <summary>攻撃</summary>
    public uint Attack { get; }
    /// <summary>防御</summary>
    public uint Defense { get; }
    /// <summary>特攻</summary>
    public uint SpecialAttack { get; }
    /// <summary>特防</summary>
    public uint SpecialDefense { get; }
    /// <summary>素早さ</summary>
    public uint Speed { get; }

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
