namespace PokemonTools.Web.Domain.Abilities;

/// <summary>
/// 特性を表現するクラス
/// </summary>
/// <param name="Id">特性Id</param>
/// <param name="Name">特性名</param>
public record Ability(AbilityId Id, string Name);
