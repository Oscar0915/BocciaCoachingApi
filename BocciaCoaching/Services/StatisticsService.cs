using BocciaCoaching.Data;
using BocciaCoaching.Models.DTO.General;
using BocciaCoaching.Models.DTO.Statistic;
using BocciaCoaching.Repositories.Statistic.Interfce;
using BocciaCoaching.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BocciaCoaching.Services
{
    public class StatisticsService : IStatisticsService
    {
        private readonly IStatisticAssessStrength _statisticRepository;
        private readonly ApplicationDbContext _context;

        public StatisticsService(IStatisticAssessStrength statisticRepository, ApplicationDbContext context)
        {
            _statisticRepository = statisticRepository;
            _context = context;
        }

        public async Task<ResponseContract<List<StrengthTestSummaryDto>>> GetRecentStatistics(int coachId, int teamId)
        {
            try
            {
                return await _statisticRepository.GetRecentStatistics(coachId, teamId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return ResponseContract<List<StrengthTestSummaryDto>>.Fail($"Error al obtener estadísticas recientes: {e.Message}");
            }
        }

        public async Task<ResponseContract<TeamStrengthStatisticsDto>> GetTeamStrengthStatistics(int teamId)
        {
            try
            {
                if (teamId <= 0)
                {
                    return ResponseContract<TeamStrengthStatisticsDto>.Fail("ID de equipo inválido");
                }

                return await _statisticRepository.GetTeamStrengthStatistics(teamId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return ResponseContract<TeamStrengthStatisticsDto>.Fail($"Error al obtener estadísticas del equipo: {e.Message}");
            }
        }

        public async Task<ResponseContract<object>> GetTeamEvaluationsDebug(int teamId)
        {
            try
            {
                if (teamId <= 0)
                {
                    return ResponseContract<object>.Fail("ID de equipo inválido");
                }

                return await _statisticRepository.GetTeamEvaluationsDebug(teamId);
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
                if (teamId <= 0)
                {
                    return ResponseContract<TeamStrengthStatisticsDto>.Fail("ID de equipo inválido");
                }

                return await _statisticRepository.GetTeamStrengthStatisticsIndividualized(teamId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return ResponseContract<TeamStrengthStatisticsDto>.Fail($"Error al obtener estadísticas individualizadas: {e.Message}");
            }
        }

        public async Task<ResponseContract<DashboardIndicatorsDto>> GetDashboardIndicators(int? coachId, int? teamId)
        {
            try
            {
                return await _statisticRepository.GetDashboardIndicators(coachId, teamId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return ResponseContract<DashboardIndicatorsDto>.Fail($"Error al obtener indicadores del dashboard: {e.Message}");
            }
        }

        public async Task<ResponseContract<DashboardCompleteDto>> GetDashboardComplete(int? coachId)
        {
            try
            {
                return await _statisticRepository.GetDashboardComplete(coachId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return ResponseContract<DashboardCompleteDto>.Fail($"Error al obtener dashboard completo: {e.Message}");
            }
        }

        public async Task<ResponseContract<List<TopPerformanceAthleteDto>>> GetTopPerformanceAthletes(int? coachId, int? teamId, int limit)
        {
            try
            {
                return await _statisticRepository.GetTopPerformanceAthletes(coachId, teamId, limit);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return ResponseContract<List<TopPerformanceAthleteDto>>.Fail($"Error al obtener atletas top: {e.Message}");
            }
        }

        public async Task<ResponseContract<List<RecentTestDto>>> GetRecentTests(int? coachId, int? teamId, int limit)
        {
            try
            {
                return await _statisticRepository.GetRecentTests(coachId, teamId, limit);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return ResponseContract<List<RecentTestDto>>.Fail($"Error al obtener pruebas recientes: {e.Message}");
            }
        }

        public async Task<ResponseContract<List<PendingTaskDto>>> GetPendingTasks(int? coachId, string? priority)
        {
            try
            {
                return await _statisticRepository.GetPendingTasks(coachId, priority);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return ResponseContract<List<PendingTaskDto>>.Fail($"Error al obtener tareas pendientes: {e.Message}");
            }
        }

        public async Task<ResponseContract<List<MonthlyEvolutionDto>>> GetMonthlyEvolution(int? coachId, int? teamId, int months)
        {
            try
            {
                return await _statisticRepository.GetMonthlyEvolution(coachId, teamId, months);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return ResponseContract<List<MonthlyEvolutionDto>>.Fail($"Error al obtener evolución mensual: {e.Message}");
            }
        }

        public async Task<ResponseContract<NextSessionInfo>> GetNextSession(int coachId)
        {
            try
            {
                return await _statisticRepository.GetNextSession(coachId);
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
                return await _statisticRepository.GetCoachTeamsOverview(coachId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return ResponseContract<List<TeamOverviewDto>>.Fail($"Error al obtener resumen de equipos: {e.Message}");
            }
        }

        // ==================== Cross-Statistics: SAREMAS+ & Macrociclo ====================

        public async Task<ResponseContract<SaremasTeamStatsDto>> GetSaremasTeamStats(int teamId)
        {
            try
            {
                var team = await _context.Teams.FindAsync(teamId);
                if (team == null)
                    return ResponseContract<SaremasTeamStatsDto>.Fail("Equipo no encontrado");

                var evaluations = await _context.SaremasEvaluations
                    .Where(e => e.TeamId == teamId)
                    .ToListAsync();

                var athleteEvals = await _context.SaremasAthleteEvaluations
                    .Where(a => evaluations.Select(e => e.SaremasEvaluationId).Contains(a.SaremasEvalId))
                    .ToListAsync();

                var athleteIds = athleteEvals.Select(a => a.AthleteId).Distinct().ToList();
                var athleteStats = new List<SaremasAthleteStatsItemDto>();

                foreach (var athleteId in athleteIds)
                {
                    var user = await _context.Users.FindAsync(athleteId);
                    var athleteEvalIds = athleteEvals.Where(a => a.AthleteId == athleteId).Select(a => a.SaremasEvalId).ToList();
                    var completedEvals = evaluations.Where(e => athleteEvalIds.Contains(e.SaremasEvaluationId) && e.State == "Completed").ToList();

                    athleteStats.Add(new SaremasAthleteStatsItemDto
                    {
                        AthleteId = athleteId,
                        AthleteName = user != null ? $"{user.FirstName} {user.LastName}" : "",
                        EvaluationsCompleted = completedEvals.Count,
                        AverageScore = completedEvals.Any(e => e.AverageScore.HasValue) ? completedEvals.Where(e => e.AverageScore.HasValue).Average(e => e.AverageScore!.Value) : 0,
                        BestScore = completedEvals.Any(e => e.TotalScore.HasValue) ? completedEvals.Max(e => e.TotalScore) : null,
                        LastEvaluationDate = completedEvals.Any() ? completedEvals.Max(e => e.EvaluationDate) : null
                    });
                }

                var completed = evaluations.Where(e => e.State == "Completed").ToList();
                var result = new SaremasTeamStatsDto
                {
                    TeamId = teamId,
                    TeamName = team.NameTeam,
                    TotalEvaluations = evaluations.Count,
                    CompletedEvaluations = completed.Count,
                    AverageTeamScore = completed.Any(e => e.AverageScore.HasValue) ? Math.Round(completed.Where(e => e.AverageScore.HasValue).Average(e => e.AverageScore!.Value), 2) : 0,
                    AthleteStats = athleteStats
                };

                return ResponseContract<SaremasTeamStatsDto>.Ok(result, "Estadísticas SAREMAS+ del equipo obtenidas");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return ResponseContract<SaremasTeamStatsDto>.Fail($"Error: {e.Message}");
            }
        }

        public async Task<ResponseContract<SaremasAthleteEvolutionDto>> GetSaremasAthleteStats(int athleteId)
        {
            try
            {
                var user = await _context.Users.FindAsync(athleteId);
                if (user == null)
                    return ResponseContract<SaremasAthleteEvolutionDto>.Fail("Atleta no encontrado");

                var evalIds = await _context.SaremasAthleteEvaluations
                    .Where(a => a.AthleteId == athleteId)
                    .Select(a => a.SaremasEvalId)
                    .ToListAsync();

                var completedEvals = await _context.SaremasEvaluations
                    .Where(e => evalIds.Contains(e.SaremasEvaluationId) && e.State == "Completed" && e.TotalScore.HasValue)
                    .OrderBy(e => e.EvaluationDate)
                    .ToListAsync();

                var points = completedEvals.Select(e => new SaremasEvolutionPointDto
                {
                    EvaluationId = e.SaremasEvaluationId,
                    EvaluationDate = e.EvaluationDate,
                    TotalScore = e.TotalScore ?? 0,
                    AverageScore = e.AverageScore ?? 0
                }).ToList();

                double trend = 0;
                if (points.Count >= 2)
                {
                    var firstHalf = points.Take(points.Count / 2).Average(p => p.AverageScore);
                    var secondHalf = points.Skip(points.Count / 2).Average(p => p.AverageScore);
                    trend = Math.Round(secondHalf - firstHalf, 2);
                }

                var result = new SaremasAthleteEvolutionDto
                {
                    AthleteId = athleteId,
                    AthleteName = $"{user.FirstName} {user.LastName}",
                    EvolutionPoints = points,
                    OverallAverage = points.Any() ? Math.Round(points.Average(p => p.AverageScore), 2) : 0,
                    Trend = trend
                };

                return ResponseContract<SaremasAthleteEvolutionDto>.Ok(result, "Evolución SAREMAS+ del atleta obtenida");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return ResponseContract<SaremasAthleteEvolutionDto>.Fail($"Error: {e.Message}");
            }
        }

        public async Task<ResponseContract<MacrocycleProgressDto>> GetMacrocycleProgress(string macrocycleId)
        {
            try
            {
                var macrocycle = await _context.Macrocycles
                    .Include(m => m.Events)
                    .Include(m => m.Microcycles)
                    .Include(m => m.Periods)
                    .Include(m => m.Mesocycles)
                    .FirstOrDefaultAsync(m => m.MacrocycleId == macrocycleId);

                if (macrocycle == null)
                    return ResponseContract<MacrocycleProgressDto>.Fail("Macrociclo no encontrado");

                var now = DateTime.Now;
                var totalWeeks = macrocycle.Microcycles.Count;
                var completedWeeks = macrocycle.Microcycles.Count(m => m.EndDate < now);
                var currentMicro = macrocycle.Microcycles.FirstOrDefault(m => m.StartDate <= now && m.EndDate >= now);

                var completedEvents = macrocycle.Events.Count(e => e.EndDate < now);
                var upcomingEvents = macrocycle.Events.Count(e => e.StartDate > now);

                var result = new MacrocycleProgressDto
                {
                    MacrocycleId = macrocycle.MacrocycleId,
                    Name = macrocycle.Name,
                    AthleteName = macrocycle.AthleteName,
                    StartDate = macrocycle.StartDate,
                    EndDate = macrocycle.EndDate,
                    TotalWeeks = totalWeeks,
                    CompletedWeeks = completedWeeks,
                    ProgressPercentage = totalWeeks > 0 ? Math.Round((double)completedWeeks / totalWeeks * 100, 1) : 0,
                    CurrentPeriod = currentMicro?.PeriodName,
                    CurrentMesocycle = currentMicro?.MesocycleName,
                    CurrentWeekNumber = currentMicro?.Number ?? 0,
                    TotalEvents = macrocycle.Events.Count,
                    CompletedEvents = completedEvents,
                    UpcomingEvents = upcomingEvents
                };

                return ResponseContract<MacrocycleProgressDto>.Ok(result, "Progreso del macrociclo obtenido");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return ResponseContract<MacrocycleProgressDto>.Fail($"Error: {e.Message}");
            }
        }

        public async Task<ResponseContract<AthleteFullDashboardDto>> GetAthleteFullDashboard(int athleteId)
        {
            try
            {
                var user = await _context.Users.FindAsync(athleteId);
                if (user == null)
                    return ResponseContract<AthleteFullDashboardDto>.Fail("Atleta no encontrado");

                // Strength
                var strengthStats = await _context.StrengthStatistics
                    .Where(s => s.AthleteId == athleteId)
                    .OrderByDescending(s => s.StrengthStatisticsId)
                    .ToListAsync();

                // Direction
                var directionStats = await _context.DirectionStatistics
                    .Where(d => d.AthleteId == athleteId)
                    .OrderByDescending(d => d.DirectionStatisticsId)
                    .ToListAsync();

                // SAREMAS+
                var saremasAthleteEvals = await _context.SaremasAthleteEvaluations
                    .Where(a => a.AthleteId == athleteId)
                    .Select(a => a.SaremasEvalId)
                    .ToListAsync();

                var saremasCompleted = await _context.SaremasEvaluations
                    .Where(e => saremasAthleteEvals.Contains(e.SaremasEvaluationId) && e.State == "Completed")
                    .OrderByDescending(e => e.EvaluationDate)
                    .ToListAsync();

                // Macrocycle
                var activeMacrocycle = await _context.Macrocycles
                    .Include(m => m.Microcycles)
                    .Where(m => m.AthleteId == athleteId && m.StartDate <= DateTime.Now && m.EndDate >= DateTime.Now)
                    .FirstOrDefaultAsync();

                double? macroProgress = null;
                string? currentPeriod = null;
                if (activeMacrocycle != null)
                {
                    var totalWeeks = activeMacrocycle.Microcycles.Count;
                    var completedWeeks = activeMacrocycle.Microcycles.Count(m => m.EndDate < DateTime.Now);
                    macroProgress = totalWeeks > 0 ? Math.Round((double)completedWeeks / totalWeeks * 100, 1) : 0;
                    currentPeriod = activeMacrocycle.Microcycles
                        .FirstOrDefault(m => m.StartDate <= DateTime.Now && m.EndDate >= DateTime.Now)?.PeriodName;
                }

                var result = new AthleteFullDashboardDto
                {
                    AthleteId = athleteId,
                    AthleteName = $"{user.FirstName} {user.LastName}",
                    StrengthEvaluationsCompleted = strengthStats.Count,
                    StrengthLastScore = strengthStats.FirstOrDefault()?.EffectivenessPercentage,
                    StrengthAverageEffectiveness = strengthStats.Any() ? Math.Round(strengthStats.Average(s => s.EffectivenessPercentage), 2) : null,
                    DirectionEvaluationsCompleted = directionStats.Count,
                    DirectionLastScore = directionStats.FirstOrDefault()?.AccuracyPercentage,
                    SaremasEvaluationsCompleted = saremasCompleted.Count,
                    SaremasLastTotalScore = saremasCompleted.FirstOrDefault()?.TotalScore,
                    SaremasAverageScore = saremasCompleted.Any(e => e.AverageScore.HasValue) ? Math.Round(saremasCompleted.Where(e => e.AverageScore.HasValue).Average(e => e.AverageScore!.Value), 2) : null,
                    ActiveMacrocycleName = activeMacrocycle?.Name,
                    MacrocycleProgressPercentage = macroProgress,
                    CurrentPeriod = currentPeriod
                };

                return ResponseContract<AthleteFullDashboardDto>.Ok(result, "Dashboard completo del atleta obtenido");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return ResponseContract<AthleteFullDashboardDto>.Fail($"Error: {e.Message}");
            }
        }
    }
}
