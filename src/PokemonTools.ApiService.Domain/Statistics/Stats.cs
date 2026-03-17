namespace PokemonTools.ApiService.Domain.Statistics;

/// <summary>
/// 実数値を表現するクラス
/// </summary>
/// <param name="Hp">HP</param>
/// <param name="Attack">攻撃</param>
/// <param name="Defense">防御</param>
/// <param name="SpecialAttack">特攻</param>
/// <param name="SpecialDefense">特防</param>
/// <param name="Speed">素早さ</param>
public record Stats(uint Hp, uint Attack, uint Defense, uint SpecialAttack, uint SpecialDefense, uint Speed);
