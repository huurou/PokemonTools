namespace PokemonTools.Web.Application.MasterData;

/// <summary>
/// マスターデータインポートの進捗情報
/// </summary>
/// <param name="Category">処理中のカテゴリ名</param>
/// <param name="Current">現在の処理件数</param>
/// <param name="Total">総件数</param>
/// <param name="CurrentName">現在処理中のアイテム名</param>
public record MasterDataImportProgress(string Category, int Current, int Total, string? CurrentName = null);
