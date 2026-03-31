namespace PokemonTools.Web.Domain.Moves;

public interface IMoveRepository
{
    /// <summary>
    /// 技を一括で更新します。
    /// </summary>
    /// <param name="moves">対象技のリスト</param>
    /// <param name="cancellationToken">キャンセルトークン</param>
    /// <returns></returns>
    Task UpsertRangeAsync(List<Move> moves, CancellationToken cancellationToken = default);
}
