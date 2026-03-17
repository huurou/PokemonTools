namespace PokemonTools.ApiService.Domain.Statistics;

/// <summary>
/// 性格Idを表現するクラス
/// </summary>
public record NatureId
{
    /// <summary>
    /// 性格Idの値
    /// </summary>
    public string Value { get; }

    public NatureId(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        Value = value;
    }
}
