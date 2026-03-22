namespace PokemonTools.Web.Domain.Statistics;

/// <summary>
/// 種族値を表現するクラス
/// </summary>
/// <param name="Hp">HP</param>
/// <param name="Attack">攻撃</param>
/// <param name="Defense">防御</param>
/// <param name="SpecialAttack">特攻</param>
/// <param name="SpecialDefense">特防</param>
/// <param name="Speed">素早さ</param>
public record BaseStats(uint Hp, uint Attack, uint Defense, uint SpecialAttack, uint SpecialDefense, uint Speed);
