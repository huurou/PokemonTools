namespace PokemonTools.ApiService.Domain.Utility;

public static class UintExtensions
{
    /// <summary>
    /// 指定の値を4096で割った後整数になるように四捨五入した値を返す
    /// </summary>
    /// <param name="value">対象の値</param>
    /// <returns>四捨五入された値</returns>
    public static uint Q12Round(this uint value)
    {
        var q = value >> 12;
        var r = value & 0x0FFFu;
        return r < 0x0800u ? q : q + 1;
    }

    /// <summary>
    /// 指定の値を4096で割った後整数になるように五捨五超入した値を返す
    /// </summary>
    /// <param name="value">対象の値</param>
    /// <returns>五捨五超入された値</returns>
    public static uint Q12RoundHalfDown(this uint value)
    {
        var q = value >> 12;
        var r = value & 0x0FFFu;
        return r <= 0x0800u ? q : q + 1;
    }

    /// <summary>
    /// 指定の値を4096で割った後小数点以下を切り捨てた値を返す
    /// </summary>
    /// <param name="value">対象の値</param>
    /// <returns>切り捨てられた値</returns>
    public static uint Q12Floor(this uint value)
    {
        return value >> 12;
    }
}
