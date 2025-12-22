using BocciaCoaching.Models.DTO.General;
using BocciaCoaching.Models.DTO.Statistic;
using BocciaCoaching.Repositories.Statistic.Interfce;
using BocciaCoaching.Services.Interfaces;

namespace BocciaCoaching.Services
{
    public class StatisticsService : IStatisticsService
    {
        private readonly IStatisticAssessStrength _statisticRepository;

        public StatisticsService(IStatisticAssessStrength statisticRepository)
        {
            _statisticRepository = statisticRepository;
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
    }
}
