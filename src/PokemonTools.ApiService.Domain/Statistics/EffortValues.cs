namespace PokemonTools.ApiService.Domain.Statistics;

/// <summary>
/// 努力値を表現するクラス
/// </summary>
public record EffortValues
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

    public EffortValues(uint hp, uint attack, uint defense, uint specialAttack, uint specialDefense, uint speed)
    {
        ArgumentOutOfRangeException.ThrowIfGreaterThan(hp, 252u);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(attack, 252u);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(defense, 252u);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(specialAttack, 252u);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(specialDefense, 252u);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(speed, 252u);

        var total = hp + attack + defense + specialAttack + specialDefense + speed;
        ArgumentOutOfRangeException.ThrowIfGreaterThan(total, 510u);

        Hp = hp;
        Attack = attack;
        Defense = defense;
        SpecialAttack = specialAttack;
        SpecialDefense = specialDefense;
        Speed = speed;
    }
}
