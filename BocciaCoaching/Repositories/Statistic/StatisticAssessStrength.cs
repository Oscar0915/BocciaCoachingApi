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

    public async Task<ResponseContract<TeamStrengthStatisticsDto>> GetTeamStrengthStatistics(int teamId)
    {
        try
        {
            // Obtener información del equipo
            var team = await context.Teams
                .Where(t => t.TeamId == teamId)
                .Select(t => new { t.TeamId, t.NameTeam })
                .FirstOrDefaultAsync();

            if (team == null)
            {
                return ResponseContract<TeamStrengthStatisticsDto>.Fail("Equipo no encontrado");
            }

            // Obtener todas las estadísticas de los atletas del equipo
            var athleteStatistics = await (
                from u in context.Users
                join ae in context.AthletesToEvaluated on u.UserId equals ae.AthleteId
                join ss in context.StrengthStatistics on ae.AssessStrengthId equals ss.AssessStrengthId
                join ast in context.AssessStrengths on ss.AssessStrengthId equals ast.AssessStrengthId
                where ast.TeamId == teamId && ast.State == "FINALIZADA"
                orderby ast.EvaluationDate descending
                select new AthleteStatisticsDto
                {
                    AthleteId = u.UserId,
                    AthleteName = u.FirstName + " " + u.LastName,
                    AssessStrengthId = ss.AssessStrengthId,
                    EvaluationDate = ast.EvaluationDate,
                    EffectivenessPercentage = ss.EffectivenessPercentage,
                    AccuracyPercentage = ss.AccuracyPercentage,
                    EffectiveThrow = ss.EffectiveThrow,
                    FailedThrow = ss.FailedThrow,
                    ShortThrow = ss.ShortThrow,
                    MediumThrow = ss.MediumThrow,
                    LongThrow = ss.LongThrow,
                    ShortEffectivenessPercentage = ss.ShortEffectivenessPercentage,
                    MediumEffectivenessPercentage = ss.MediumEffectivenessPercentage,
                    LongEffectivenessPercentage = ss.LongEffectivenessPercentage,
                    ShortThrowAccuracy = ss.ShortThrowAccuracy,
                    MediumThrowAccuracy = ss.MediumThrowAccuracy,
                    LongThrowAccuracy = ss.LongThrowAccuracy,
                    ShortAccuracyPercentage = ss.ShortAccuracyPercentage,
                    MediumAccuracyPercentage = ss.MediumAccuracyPercentage,
                    LongAccuracyPercentage = ss.LongAccuracyPercentage
                }
            ).ToListAsync();

            // Calcular promedios del equipo
            var teamAverages = new TeamAverageStatisticsDto();
            if (athleteStatistics.Any())
            {
                teamAverages.AverageEffectivenessPercentage = athleteStatistics.Average(a => a.EffectivenessPercentage);
                teamAverages.AverageAccuracyPercentage = athleteStatistics.Average(a => a.AccuracyPercentage);
                teamAverages.AverageEffectiveThrow = athleteStatistics.Average(a => a.EffectiveThrow);
                teamAverages.AverageFailedThrow = athleteStatistics.Average(a => a.FailedThrow);
                teamAverages.AverageShortThrow = athleteStatistics.Average(a => a.ShortThrow);
                teamAverages.AverageMediumThrow = athleteStatistics.Average(a => a.MediumThrow);
                teamAverages.AverageLongThrow = athleteStatistics.Average(a => a.LongThrow);
                teamAverages.AverageShortEffectivenessPercentage = athleteStatistics.Average(a => a.ShortEffectivenessPercentage);
                teamAverages.AverageMediumEffectivenessPercentage = athleteStatistics.Average(a => a.MediumEffectivenessPercentage);
                teamAverages.AverageLongEffectivenessPercentage = athleteStatistics.Average(a => a.LongEffectivenessPercentage);
                teamAverages.AverageShortAccuracyPercentage = athleteStatistics.Average(a => a.ShortAccuracyPercentage);
                teamAverages.AverageMediumAccuracyPercentage = athleteStatistics.Average(a => a.MediumAccuracyPercentage);
                teamAverages.AverageLongAccuracyPercentage = athleteStatistics.Average(a => a.LongAccuracyPercentage);
                teamAverages.TotalAthletes = athleteStatistics.GroupBy(a => a.AthleteId).Count();
            }

            var result = new TeamStrengthStatisticsDto
            {
                TeamId = team.TeamId,
                TeamName = team.NameTeam ?? "Sin nombre",
                Athletes = athleteStatistics,
                TeamAverages = teamAverages
            };

            return ResponseContract<TeamStrengthStatisticsDto>.Ok(result, "Estadísticas obtenidas correctamente");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return ResponseContract<TeamStrengthStatisticsDto>.Fail($"Error al obtener las estadísticas: {e.Message}");
        }
    }

    public async Task<ResponseContract<object>> GetTeamEvaluationsDebug(int teamId)
    {
        try
        {
            // 1. Verificar el equipo
            var team = await context.Teams
                .Where(t => t.TeamId == teamId)
                .Select(t => new { 
                    t.TeamId, 
                    t.NameTeam, 
                    t.Status,
                    t.CoachId 
                })
                .FirstOrDefaultAsync();

            // 2. Obtener todas las evaluaciones del equipo
            var evaluations = await context.AssessStrengths
                .Where(a => a.TeamId == teamId)
                .Select(a => new {
                    a.AssessStrengthId,
                    a.EvaluationDate,
                    a.Description,
                    a.State,
                    a.TeamId
                })
                .OrderBy(a => a.EvaluationDate)
                .ToListAsync();

            // 3. Obtener atletas evaluados
            var athletesToEvaluated = await (
                from ae in context.AthletesToEvaluated
                join a in context.AssessStrengths on ae.AssessStrengthId equals a.AssessStrengthId
                join u in context.Users on ae.AthleteId equals u.UserId
                where a.TeamId == teamId
                select new {
                    ae.AssessStrengthId,
                    ae.AthleteId,
                    ae.CoachId,
                    AthleteName = u.FirstName + " " + u.LastName,
                    a.State
                }
            ).ToListAsync();

            // 4. Obtener estadísticas generadas
            var statistics = await (
                from ss in context.StrengthStatistics
                join a in context.AssessStrengths on ss.AssessStrengthId equals a.AssessStrengthId
                join u in context.Users on ss.AthleteId equals u.UserId
                where a.TeamId == teamId
                select new {
                    ss.StrengthStatisticsId,
                    ss.AssessStrengthId,
                    ss.AthleteId,
                    AthleteName = u.FirstName + " " + u.LastName,
                    ss.EffectivenessPercentage,
                    ss.AccuracyPercentage,
                    a.State
                }
            ).ToListAsync();

            // 5. Obtener detalles de evaluación
            var evaluationDetails = await (
                from ed in context.EvaluationDetailStrengths
                join a in context.AssessStrengths on ed.AssessStrengthId equals a.AssessStrengthId
                where a.TeamId == teamId
                select new {
                    ed.EvaluationDetailStrengthId,
                    ed.AssessStrengthId,
                    ed.AthleteId,
                    ed.ThrowOrder,
                    ed.ScoreObtained,
                    ed.TargetDistance,
                    a.State
                }
            ).ToListAsync();

            var debugInfo = new {
                Team = team,
                EvaluationsCount = evaluations.Count,
                Evaluations = evaluations,
                AthletesToEvaluatedCount = athletesToEvaluated.Count,
                AthletesToEvaluated = athletesToEvaluated,
                StatisticsCount = statistics.Count,
                Statistics = statistics,
                EvaluationDetailsCount = evaluationDetails.Count,
                EvaluationDetailsSample = evaluationDetails.Take(10), // Solo una muestra
                Summary = new {
                    TotalEvaluations = evaluations.Count,
                    FinishedEvaluations = evaluations.Count(e => e.State == "FINALIZADA"),
                    ActiveEvaluations = evaluations.Count(e => e.State == "ACTIVA"),
                    AthletesWithStatistics = statistics.Select(s => s.AthleteId).Distinct().Count(),
                    TotalEvaluationDetails = evaluationDetails.Count
                }
            };

            return ResponseContract<object>.Ok(debugInfo, "Información de depuración obtenida correctamente");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return ResponseContract<object>.Fail($"Error al obtener información de depuración: {e.Message}");
        }
    }

    public async Task<ResponseContract<TeamStrengthStatisticsDto>> GetTeamStrengthStatisticsIndividualized(int teamId)
    {
        try
        {
            // Obtener información del equipo
            var team = await context.Teams
                .Where(t => t.TeamId == teamId)
                .Select(t => new { t.TeamId, t.NameTeam })
                .FirstOrDefaultAsync();

            if (team == null)
            {
                return ResponseContract<TeamStrengthStatisticsDto>.Fail("Equipo no encontrado");
            }

            var athleteStatistics = new List<AthleteStatisticsDto>();

            // Obtener TODAS las evaluaciones del equipo (no solo finalizadas)
            var evaluations = await context.AssessStrengths
                .Where(a => a.TeamId == teamId)
                .OrderByDescending(a => a.EvaluationDate)
                .ToListAsync();

            foreach (var evaluation in evaluations)
            {
                // Obtener atletas de esta evaluación específica
                var athletesInEvaluation = await (
                    from ae in context.AthletesToEvaluated
                    join u in context.Users on ae.AthleteId equals u.UserId
                    where ae.AssessStrengthId == evaluation.AssessStrengthId
                    select new {
                        ae.AthleteId,
                        AthleteName = u.FirstName + " " + u.LastName,
                        ae.AssessStrengthId
                    }
                ).ToListAsync();

                foreach (var athlete in athletesInEvaluation)
                {
                    // Verificar si existe estadística generada
                    var statistics = await context.StrengthStatistics
                        .Where(ss => ss.AssessStrengthId == athlete.AssessStrengthId && 
                                   ss.AthleteId == athlete.AthleteId)
                        .FirstOrDefaultAsync();

                    if (statistics != null)
                    {
                        // Si hay estadísticas, usar esos datos
                        athleteStatistics.Add(new AthleteStatisticsDto
                        {
                            AthleteId = athlete.AthleteId,
                            AthleteName = athlete.AthleteName,
                            AssessStrengthId = statistics.AssessStrengthId,
                            EvaluationDate = evaluation.EvaluationDate,
                            EffectivenessPercentage = statistics.EffectivenessPercentage,
                            AccuracyPercentage = statistics.AccuracyPercentage,
                            EffectiveThrow = statistics.EffectiveThrow,
                            FailedThrow = statistics.FailedThrow,
                            ShortThrow = statistics.ShortThrow,
                            MediumThrow = statistics.MediumThrow,
                            LongThrow = statistics.LongThrow,
                            ShortEffectivenessPercentage = statistics.ShortEffectivenessPercentage,
                            MediumEffectivenessPercentage = statistics.MediumEffectivenessPercentage,
                            LongEffectivenessPercentage = statistics.LongEffectivenessPercentage,
                            ShortThrowAccuracy = statistics.ShortThrowAccuracy,
                            MediumThrowAccuracy = statistics.MediumThrowAccuracy,
                            LongThrowAccuracy = statistics.LongThrowAccuracy,
                            ShortAccuracyPercentage = statistics.ShortAccuracyPercentage,
                            MediumAccuracyPercentage = statistics.MediumAccuracyPercentage,
                            LongAccuracyPercentage = statistics.LongAccuracyPercentage
                        });
                    }
                    else
                    {
                        // Si no hay estadísticas, calcular manualmente desde EvaluationDetailStrength
                        var evaluationDetails = await context.EvaluationDetailStrengths
                            .Where(ed => ed.AssessStrengthId == athlete.AssessStrengthId && 
                                       ed.AthleteId == athlete.AthleteId)
                            .ToListAsync();

                        if (evaluationDetails.Any())
                        {
                            // Calcular estadísticas manualmente
                            var totalThrows = evaluationDetails.Count;
                            var effectiveThrows = evaluationDetails.Count(ed => ed.ScoreObtained >= 3);
                            var failedThrows = totalThrows - effectiveThrows;
                            var totalScore = evaluationDetails.Sum(ed => ed.ScoreObtained) ?? 0;

                            var shortThrows = evaluationDetails
                                .Where(ed => ed.TargetDistance >= 1.5m && ed.TargetDistance <= 4.0m && ed.ScoreObtained >= 3)
                                .Count();
                            var mediumThrows = evaluationDetails
                                .Where(ed => ed.TargetDistance >= 4.5m && ed.TargetDistance <= 7.0m && ed.ScoreObtained >= 3)
                                .Count();
                            var longThrows = evaluationDetails
                                .Where(ed => ed.TargetDistance >= 7.5m && ed.TargetDistance <= 10.0m && ed.ScoreObtained >= 3)
                                .Count();

                            var shortAccuracy = evaluationDetails
                                .Where(ed => ed.TargetDistance >= 1.5m && ed.TargetDistance <= 4.0m)
                                .Sum(ed => ed.ScoreObtained) ?? 0;
                            var mediumAccuracy = evaluationDetails
                                .Where(ed => ed.TargetDistance >= 4.5m && ed.TargetDistance <= 7.0m)
                                .Sum(ed => ed.ScoreObtained) ?? 0;
                            var longAccuracy = evaluationDetails
                                .Where(ed => ed.TargetDistance >= 7.5m && ed.TargetDistance <= 10.0m)
                                .Sum(ed => ed.ScoreObtained) ?? 0;

                            athleteStatistics.Add(new AthleteStatisticsDto
                            {
                                AthleteId = athlete.AthleteId,
                                AthleteName = athlete.AthleteName + " (Calculado)",
                                AssessStrengthId = athlete.AssessStrengthId,
                                EvaluationDate = evaluation.EvaluationDate,
                                EffectivenessPercentage = totalThrows > 0 ? (double)totalScore / (totalThrows * 5) : 0,
                                AccuracyPercentage = totalThrows > 0 ? (double)effectiveThrows / totalThrows : 0,
                                EffectiveThrow = effectiveThrows,
                                FailedThrow = failedThrows,
                                ShortThrow = shortThrows,
                                MediumThrow = mediumThrows,
                                LongThrow = longThrows,
                                ShortEffectivenessPercentage = shortThrows / 12.0,
                                MediumEffectivenessPercentage = mediumThrows / 12.0,
                                LongEffectivenessPercentage = longThrows / 12.0,
                                ShortThrowAccuracy = (int)shortAccuracy,
                                MediumThrowAccuracy = (int)mediumAccuracy,
                                LongThrowAccuracy = (int)longAccuracy,
                                ShortAccuracyPercentage = (double)shortAccuracy / 60,
                                MediumAccuracyPercentage = (double)mediumAccuracy / 60,
                                LongAccuracyPercentage = (double)longAccuracy / 60
                            });
                        }
                        else
                        {
                            // Si no hay detalles, mostrar evaluación vacía
                            athleteStatistics.Add(new AthleteStatisticsDto
                            {
                                AthleteId = athlete.AthleteId,
                                AthleteName = athlete.AthleteName + " (Sin datos)",
                                AssessStrengthId = athlete.AssessStrengthId,
                                EvaluationDate = evaluation.EvaluationDate,
                                EffectivenessPercentage = 0,
                                AccuracyPercentage = 0,
                                EffectiveThrow = 0,
                                FailedThrow = 0,
                                ShortThrow = 0,
                                MediumThrow = 0,
                                LongThrow = 0,
                                ShortEffectivenessPercentage = 0,
                                MediumEffectivenessPercentage = 0,
                                LongEffectivenessPercentage = 0,
                                ShortThrowAccuracy = 0,
                                MediumThrowAccuracy = 0,
                                LongThrowAccuracy = 0,
                                ShortAccuracyPercentage = 0,
                                MediumAccuracyPercentage = 0,
                                LongAccuracyPercentage = 0
                            });
                        }
                    }
                }
            }

            // Calcular promedios del equipo
            var teamAverages = new TeamAverageStatisticsDto();
            if (athleteStatistics.Any())
            {
                teamAverages.AverageEffectivenessPercentage = athleteStatistics.Average(a => a.EffectivenessPercentage);
                teamAverages.AverageAccuracyPercentage = athleteStatistics.Average(a => a.AccuracyPercentage);
                teamAverages.AverageEffectiveThrow = athleteStatistics.Average(a => a.EffectiveThrow);
                teamAverages.AverageFailedThrow = athleteStatistics.Average(a => a.FailedThrow);
                teamAverages.AverageShortThrow = athleteStatistics.Average(a => a.ShortThrow);
                teamAverages.AverageMediumThrow = athleteStatistics.Average(a => a.MediumThrow);
                teamAverages.AverageLongThrow = athleteStatistics.Average(a => a.LongThrow);
                teamAverages.AverageShortEffectivenessPercentage = athleteStatistics.Average(a => a.ShortEffectivenessPercentage);
                teamAverages.AverageMediumEffectivenessPercentage = athleteStatistics.Average(a => a.MediumEffectivenessPercentage);
                teamAverages.AverageLongEffectivenessPercentage = athleteStatistics.Average(a => a.LongEffectivenessPercentage);
                teamAverages.AverageShortAccuracyPercentage = athleteStatistics.Average(a => a.ShortAccuracyPercentage);
                teamAverages.AverageMediumAccuracyPercentage = athleteStatistics.Average(a => a.MediumAccuracyPercentage);
                teamAverages.AverageLongAccuracyPercentage = athleteStatistics.Average(a => a.LongAccuracyPercentage);
                teamAverages.TotalAthletes = athleteStatistics.GroupBy(a => a.AthleteId).Count();
            }

            var result = new TeamStrengthStatisticsDto
            {
                TeamId = team.TeamId,
                TeamName = team.NameTeam ?? "Sin nombre",
                Athletes = athleteStatistics,
                TeamAverages = teamAverages
            };

            return ResponseContract<TeamStrengthStatisticsDto>.Ok(result, $"Estadísticas individualizadas obtenidas - {athleteStatistics.Count} registros encontrados");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return ResponseContract<TeamStrengthStatisticsDto>.Fail($"Error al obtener las estadísticas individualizadas: {e.Message}");
        }
    }
}