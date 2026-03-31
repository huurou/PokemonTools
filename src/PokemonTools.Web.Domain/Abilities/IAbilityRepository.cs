namespace PokemonTools.Web.Domain.Abilities;

public interface IAbilityRepository
{
    /// <summary>
    /// 特性を一括で更新します。
    /// </summary>
    /// <param name="abilities">対象特性のリスト</param>
    /// <param name="cancellationToken">キャンセルトークン</param>
    /// <returns></returns>
    Task UpsertRangeAsync(List<Ability> abilities, CancellationToken cancellationToken = default);
}
