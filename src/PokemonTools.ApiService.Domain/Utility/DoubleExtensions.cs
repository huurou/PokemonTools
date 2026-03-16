namespace PokemonTools.ApiService.Domain.Utility;

public static class DoubleExtensions
{
    public static uint FloorToUint(this double value)
    {
        return (uint)Math.Floor(value);
    }
}
