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

    public async Task<ResponseContract<DashboardIndicatorsDto>> GetDashboardIndicators(int? coachId, int? teamId)
    {
        try
        {
            var totalAthletesQuery = context.Users.AsQueryable();
            var activeAthletesQuery = from ae in context.AthletesToEvaluated
                                     join ast in context.AssessStrengths on ae.AssessStrengthId equals ast.AssessStrengthId
                                     select new { ae, ast };
            var completedTestsQuery = context.StrengthStatistics.AsQueryable();
            var pendingTasksQuery = context.AssessStrengths.Where(a => a.State == "En progreso" || a.State == "Pendiente");

            if (coachId.HasValue)
            {
                activeAthletesQuery = activeAthletesQuery.Where(x => x.ae.CoachId == coachId.Value);
                pendingTasksQuery = pendingTasksQuery.Join(context.Teams, a => a.TeamId, t => t.TeamId, (a, t) => new { a, t })
                    .Where(x => x.t.CoachId == coachId.Value)
                    .Select(x => x.a);
            }
            if (teamId.HasValue)
            {
                activeAthletesQuery = activeAthletesQuery.Where(x => x.ast.TeamId == teamId.Value);
                completedTestsQuery = completedTestsQuery.Join(context.AssessStrengths, ss => ss.AssessStrengthId, ast => ast.AssessStrengthId, (ss, ast) => new { ss, ast })
                    .Where(x => x.ast.TeamId == teamId.Value)
                    .Select(x => x.ss);
                pendingTasksQuery = pendingTasksQuery.Where(a => a.TeamId == teamId.Value);
                totalAthletesQuery = totalAthletesQuery.Join(context.AthletesToEvaluated, u => u.UserId, ae => ae.AthleteId, (u, ae) => new { u, ae })
                    .Join(context.AssessStrengths, x => x.ae.AssessStrengthId, ast => ast.AssessStrengthId, (x, ast) => new { x.u, ast })
                    .Where(x => x.ast.TeamId == teamId.Value)
                    .Select(x => x.u).Distinct();
            }

            var totalAthletes = await totalAthletesQuery.CountAsync();
            var recentDate = DateTime.Now.AddDays(-30);
            var activeAthletes = await activeAthletesQuery.Where(x => x.ast.CreatedAt >= recentDate).Select(x => x.ae.AthleteId).Distinct().CountAsync();
            var completedTests = await completedTestsQuery.CountAsync();
            var averageEffectiveness = await completedTestsQuery.AverageAsync(s => (double?)s.EffectivenessPercentage) ?? 0;
            var pendingTasks = await pendingTasksQuery.CountAsync();

            var nextSession = new NextSessionInfo
            {
                SessionDate = DateTime.Now.AddDays(1),
                SessionTime = "15:00",
                TeamName = "Próxima sesión programada",
                TeamId = teamId ?? 1,
                SessionType = "Entrenamiento"
            };

            var indicators = new DashboardIndicatorsDto
            {
                TotalAthletes = totalAthletes,
                ActiveAthletes = activeAthletes,
                CompletedTests = completedTests,
                GeneralAverage = averageEffectiveness,
                PendingTasks = pendingTasks,
                NextSession = nextSession
            };

            return ResponseContract<DashboardIndicatorsDto>.Ok(indicators, "Indicadores del dashboard obtenidos correctamente");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return ResponseContract<DashboardIndicatorsDto>.Fail("Error al obtener indicadores del dashboard: " + e.Message);
        }
    }

    public async Task<ResponseContract<DashboardCompleteDto>> GetDashboardComplete(int? coachId)
    {
        try
        {
            var indicators = await GetDashboardIndicators(coachId, null);
            var topAthletes = await GetTopPerformanceAthletes(coachId, null, 5);
            var recentTests = await GetRecentTests(coachId, null, 10);
            var pendingTasks = await GetPendingTasks(coachId, null);
            var monthlyEvolution = await GetMonthlyEvolution(coachId, null, 12);

            var dashboard = new DashboardCompleteDto
            {
                Indicators = indicators.Data,
                TopPerformanceAthletes = topAthletes.Data,
                RecentTests = recentTests.Data,
                PendingTasks = pendingTasks.Data,
                MonthlyEvolution = monthlyEvolution.Data
            };

            return ResponseContract<DashboardCompleteDto>.Ok(dashboard, "Dashboard completo obtenido correctamente");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return ResponseContract<DashboardCompleteDto>.Fail("Error al obtener dashboard completo: " + e.Message);
        }
    }

    public async Task<ResponseContract<List<TopPerformanceAthleteDto>>> GetTopPerformanceAthletes(int? coachId, int? teamId, int limit)
    {
        try
        {
            var query = from u in context.Users
                       join ae in context.AthletesToEvaluated on u.UserId equals ae.AthleteId
                       join ss in context.StrengthStatistics on ae.AssessStrengthId equals ss.AssessStrengthId
                       join ast in context.AssessStrengths on ae.AssessStrengthId equals ast.AssessStrengthId
                       join t in context.Teams on ast.TeamId equals t.TeamId
                       select new { u, ae, ss, ast, t };

            if (coachId.HasValue)
            {
                query = query.Where(x => x.ae.CoachId == coachId.Value);
            }

            if (teamId.HasValue)
            {
                query = query.Where(x => x.ast.TeamId == teamId.Value);
            }

            var data = await query
                .GroupBy(x => new { x.u.UserId, x.u.FirstName, x.u.LastName, x.t.NameTeam })
                .Select(g => new TopPerformanceAthleteDto
                {
                    AthleteId = g.Key.UserId,
                    AthleteName = g.Key.FirstName + " " + g.Key.LastName,
                    PerformanceScore = g.Average(x => x.ss.EffectivenessPercentage),
                    TeamName = g.Key.NameTeam ?? "Sin equipo",
                    LastEvaluationDate = g.Max(x => x.ast.CreatedAt)
                })
                .OrderByDescending(x => x.PerformanceScore)
                .Take(limit)
                .ToListAsync();

            return ResponseContract<List<TopPerformanceAthleteDto>>.Ok(data, "Top " + limit + " atletas obtenidos correctamente");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return ResponseContract<List<TopPerformanceAthleteDto>>.Fail("Error al obtener top atletas: " + e.Message);
        }
    }

    public async Task<ResponseContract<List<RecentTestDto>>> GetRecentTests(int? coachId, int? teamId, int limit)
    {
        try
        {
            var query = from u in context.Users
                       join ae in context.AthletesToEvaluated on u.UserId equals ae.AthleteId
                       join ss in context.StrengthStatistics on ae.AssessStrengthId equals ss.AssessStrengthId
                       join ast in context.AssessStrengths on ae.AssessStrengthId equals ast.AssessStrengthId
                       select new { u, ae, ss, ast };

            if (coachId.HasValue)
            {
                query = query.Where(x => x.ae.CoachId == coachId.Value);
            }

            if (teamId.HasValue)
            {
                query = query.Where(x => x.ast.TeamId == teamId.Value);
            }

            var data = await query
                .OrderByDescending(x => x.ast.CreatedAt)
                .Take(limit)
                .Select(x => new RecentTestDto
                {
                    TestId = x.ss.StrengthStatisticsId,
                    AthleteName = x.u.FirstName + " " + x.u.LastName,
                    TestType = "Evaluación de Fuerza",
                    Score = x.ss.EffectivenessPercentage,
                    TestDate = x.ast.CreatedAt,
                    Status = x.ast.State ?? "Completada"
                })
                .ToListAsync();

            return ResponseContract<List<RecentTestDto>>.Ok(data, "Últimas " + limit + " pruebas obtenidas correctamente");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return ResponseContract<List<RecentTestDto>>.Fail("Error al obtener pruebas recientes: " + e.Message);
        }
    }

    public async Task<ResponseContract<List<PendingTaskDto>>> GetPendingTasks(int? coachId, string? priority)
    {
        try
        {
            var query = from ast in context.AssessStrengths
                       join t in context.Teams on ast.TeamId equals t.TeamId
                       where ast.State == "En progreso" || ast.State == "Pendiente"
                       select new { ast, t };

            if (coachId.HasValue)
            {
                query = query.Where(x => x.t.CoachId == coachId.Value);
            }

            var data = await query
                .Select(x => new PendingTaskDto
                {
                    TaskId = x.ast.AssessStrengthId,
                    TaskDescription = "Revisar evaluación de " + (x.t.NameTeam ?? "equipo"),
                    Priority = "Media", // Valor por defecto
                    DueDate = x.ast.CreatedAt.AddDays(7), // Una semana después de creación
                    TaskType = "Evaluación",
                    AssignedTo = "Coach ID: " + x.t.CoachId.ToString()
                })
                .ToListAsync();

            if (!string.IsNullOrEmpty(priority))
            {
                data = data.Where(x => x.Priority.Equals(priority, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            return ResponseContract<List<PendingTaskDto>>.Ok(data, data.Count + " tareas pendientes encontradas");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return ResponseContract<List<PendingTaskDto>>.Fail("Error al obtener tareas pendientes: " + e.Message);
        }
    }

    public async Task<ResponseContract<List<MonthlyEvolutionDto>>> GetMonthlyEvolution(int? coachId, int? teamId, int months)
    {
        try
        {
            var startDate = DateTime.Now.AddMonths(-months);
            
            var query = from ae in context.AthletesToEvaluated
                       join ss in context.StrengthStatistics on ae.AssessStrengthId equals ss.AssessStrengthId
                       join ast in context.AssessStrengths on ae.AssessStrengthId equals ast.AssessStrengthId
                       where ast.CreatedAt >= startDate
                       select new { ae, ss, ast };

            if (coachId.HasValue)
            {
                query = query.Where(x => x.ae.CoachId == coachId.Value);
            }

            if (teamId.HasValue)
            {
                query = query.Where(x => x.ast.TeamId == teamId.Value);
            }

            var data = await query.ToListAsync();

            var monthlyData = data
                .GroupBy(x => new { x.ast.CreatedAt.Year, x.ast.CreatedAt.Month })
                .Select(g => new MonthlyEvolutionDto
                {
                    Month = g.Key.Month.ToString("00") + "/" + g.Key.Year.ToString(),
                    AveragePerformance = g.Average(x => x.ss.EffectivenessPercentage),
                    TestsCount = g.Count(),
                    PeriodStart = new DateTime(g.Key.Year, g.Key.Month, 1),
                    PeriodEnd = new DateTime(g.Key.Year, g.Key.Month, DateTime.DaysInMonth(g.Key.Year, g.Key.Month))
                })
                .OrderBy(x => x.PeriodStart)
                .ToList();

            return ResponseContract<List<MonthlyEvolutionDto>>.Ok(monthlyData, "Evolución de " + monthlyData.Count + " meses obtenida");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return ResponseContract<List<MonthlyEvolutionDto>>.Fail("Error al obtener evolución mensual: " + e.Message);
        }
    }

    public async Task<ResponseContract<NextSessionInfo>> GetNextSession(int coachId)
    {
        try
        {
            // Como no hay tabla de sesiones, creamos datos de ejemplo
            var nextTeam = await context.Teams
                .Where(t => t.CoachId == coachId)
                .FirstOrDefaultAsync();

            var nextSession = new NextSessionInfo
            {
                SessionDate = DateTime.Now.AddDays(1),
                SessionTime = "15:00",
                TeamName = nextTeam?.NameTeam ?? "Sin equipo asignado",
                TeamId = nextTeam?.TeamId ?? 0,
                SessionType = "Entrenamiento"
            };

            return ResponseContract<NextSessionInfo>.Ok(nextSession, "Próxima sesión obtenida");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return ResponseContract<NextSessionInfo>.Fail($"Error al obtener próxima sesión: {e.Message}");
        }
    }

    public async Task<ResponseContract<List<TeamOverviewDto>>> GetCoachTeamsOverview(int coachId)
    {
        try
        {
            var teams = await context.Teams
                .Where(t => t.CoachId == coachId)
                .ToListAsync();

            var teamOverviews = new List<TeamOverviewDto>();

            foreach (var team in teams)
            {
                // Obtener atletas del equipo
                var athletesCount = await context.AthletesToEvaluated
                    .Where(ae => ae.CoachId == coachId)
                    .Join(context.AssessStrengths, ae => ae.AssessStrengthId, ast => ast.AssessStrengthId, (ae, ast) => new { ae, ast })
                    .Where(x => x.ast.TeamId == team.TeamId)
                    .Select(x => x.ae.AthleteId)
                    .Distinct()
                    .CountAsync();

                // Atletas activos (con evaluaciones recientes)
                var recentDate = DateTime.Now.AddDays(-30);
                var activeAthletes = await context.AthletesToEvaluated
                    .Where(ae => ae.CoachId == coachId)
                    .Join(context.AssessStrengths, ae => ae.AssessStrengthId, ast => ast.AssessStrengthId, (ae, ast) => new { ae, ast })
                    .Where(x => x.ast.TeamId == team.TeamId && x.ast.CreatedAt >= recentDate)
                    .Select(x => x.ae.AthleteId)
                    .Distinct()
                    .CountAsync();

                // Tests completados
                var completedTests = await context.StrengthStatistics
                    .Join(context.AssessStrengths, ss => ss.AssessStrengthId, ast => ast.AssessStrengthId, (ss, ast) => new { ss, ast })
                    .Where(x => x.ast.TeamId == team.TeamId)
                    .CountAsync();

                // Promedio de rendimiento
                var averagePerformance = await context.StrengthStatistics
                    .Join(context.AssessStrengths, ss => ss.AssessStrengthId, ast => ast.AssessStrengthId, (ss, ast) => new { ss, ast })
                    .Where(x => x.ast.TeamId == team.TeamId)
                    .AverageAsync(x => (double?)x.ss.EffectivenessPercentage) ?? 0;

                // Fecha del último test
                var lastTestDate = await context.AssessStrengths
                    .Where(ast => ast.TeamId == team.TeamId)
                    .MaxAsync(ast => (DateTime?)ast.CreatedAt);

                // Evaluaciones pendientes
                var pendingEvaluations = await context.AssessStrengths
                    .Where(ast => ast.TeamId == team.TeamId && 
                                 (ast.State == "En progreso" || ast.State == "Pendiente"))
                    .CountAsync();

                teamOverviews.Add(new TeamOverviewDto
                {
                    TeamId = team.TeamId,
                    TeamName = team.NameTeam ?? "Sin nombre",
                    TotalAthletes = athletesCount,
                    ActiveAthletes = activeAthletes,
                    CompletedTests = completedTests,
                    AveragePerformance = averagePerformance,
                    LastTestDate = lastTestDate,
                    PendingEvaluations = pendingEvaluations,
                    Status = activeAthletes > 0 ? "Activo" : "Inactivo"
                });
            }

            return ResponseContract<List<TeamOverviewDto>>.Ok(teamOverviews, "Resumen de " + teamOverviews.Count + " equipos obtenido");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return ResponseContract<List<TeamOverviewDto>>.Fail("Error al obtener resumen de equipos: " + e.Message);
        }
    }
}