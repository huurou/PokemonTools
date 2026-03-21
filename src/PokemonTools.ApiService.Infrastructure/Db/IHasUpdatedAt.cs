namespace PokemonTools.ApiService.Infrastructure.Db;

public interface IHasUpdatedAt
{
    /// <summary>
    /// 更新日時
    /// </summary>
    DateTimeOffset UpdatedAt { get; set; }
}
