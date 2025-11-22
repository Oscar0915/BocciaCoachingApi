using BocciaCoaching.Data;
using BocciaCoaching.Models.DTO.General;
using BocciaCoaching.Models.DTO.Statistic;
using BocciaCoaching.Repositories.Statistic.Interfce;
using Microsoft.EntityFrameworkCore;

namespace BocciaCoaching.Repositories.Statistic;

public class StatisticAssessStrength(ApplicationDbContext context) : IStatisticAssessStrength
{

    public async Task<ResponseContract<List<StrengthTestSummaryDto>>> GetRecentStatistics(int coachId, int teamId)
    {
        try
        {
            var responseStatistic = await (
                    from u in context.Users
                    join ae in context.AthletesToEvaluated on u.UserId equals ae.AthleteId
                    join et in context.StrengthStatistics on ae.AssessStrengthId equals et.AssessStrengthId
                    join ast in context.AssessStrengths on et.AssessStrengthId equals ast.AssessStrengthId
                    where ae.CoachId == coachId && ast.TeamId == teamId
                    orderby et.StrengthStatisticsId descending
                    select new StrengthTestSummaryDto
                    {
                        NameAthlete = u.FirstName + " " + u.LastName,
                        EffectivenessPercentage = et.EffectivenessPercentage,
                        AccuracyPercentage = et.AccuracyPercentage,
                        AssessStrengthId = et.AssessStrengthId,
                        StrengthStatisticsId = et.StrengthStatisticsId,
                        AthleteId = u.UserId
                    }
                )
                .Take(4)
                .ToListAsync();


            return ResponseContract<List<StrengthTestSummaryDto>>.Ok(responseStatistic, "Obtenido correctamente");


        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}