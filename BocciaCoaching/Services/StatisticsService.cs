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
    }
}
