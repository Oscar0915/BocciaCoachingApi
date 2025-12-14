using BocciaCoaching.Models.DTO.General;
using BocciaCoaching.Models.DTO.Statistic;

namespace BocciaCoaching.Services.Interfaces
{
    public interface IStatisticsService
    {
        Task<ResponseContract<List<StrengthTestSummaryDto>>> GetRecentStatistics(int coachId, int teamId);
        Task<ResponseContract<TeamStrengthStatisticsDto>> GetTeamStrengthStatistics(int teamId);
        Task<ResponseContract<object>> GetTeamEvaluationsDebug(int teamId);
        Task<ResponseContract<TeamStrengthStatisticsDto>> GetTeamStrengthStatisticsIndividualized(int teamId);
    }
}
