using PokemonTools.Web.Domain.Statistics;

namespace PokemonTools.Web.Application.Individuals;

public static class IndividualStatsPreview
{
    public static CalculatedStatsDto? Calculate(
        SpeciesOptionDto? species,
        int statAlignmentId,
        uint statPointHp,
        uint statPointAttack,
        uint statPointDefense,
        uint statPointSpecialAttack,
        uint statPointSpecialDefense,
        uint statPointSpeed)
    {
        if (species is null) { return null; }

        var statAlignment = StatAlignment.All.FirstOrDefault(x => x.Id.Value == statAlignmentId);
        if (statAlignment is null) { return null; }

        try
        {
            var baseStats = new BaseStats(
                species.BaseStatHp, species.BaseStatAttack, species.BaseStatDefense,
                species.BaseStatSpecialAttack, species.BaseStatSpecialDefense, species.BaseStatSpeed);
            var statPoints = new StatPoints(
                statPointHp, statPointAttack, statPointDefense,
                statPointSpecialAttack, statPointSpecialDefense, statPointSpeed);
            var stats = StatsCalculator.Calculate(baseStats, statPoints, statAlignment);
            return new CalculatedStatsDto(
                stats.Hp, stats.Attack, stats.Defense,
                stats.SpecialAttack, stats.SpecialDefense, stats.Speed);
        }
        catch (ArgumentOutOfRangeException)
        {
            return null;
        }
    }
}
