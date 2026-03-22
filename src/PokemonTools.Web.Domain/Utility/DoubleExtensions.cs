namespace PokemonTools.Web.Domain.Utility;

public static class DoubleExtensions
{
    public static uint FloorToUint(this double value)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(value);
        return (uint)Math.Floor(value);
    }
}
