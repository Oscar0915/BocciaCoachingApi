using BocciaCoaching.Models.DTO.General;
using BocciaCoaching.Models.DTO.Statistic;

namespace BocciaCoaching.Services.Interfaces
{
    public interface IStatisticsService
    {
        Task<ResponseContract<List<StrengthTestSummaryDto>>> GetRecentStatistics(Guid coachId, Guid teamId);
        Task<ResponseContract<TeamStrengthStatisticsDto>> GetTeamStrengthStatistics(Guid teamId);
        Task<ResponseContract<object>> GetTeamEvaluationsDebug(Guid teamId);
        Task<ResponseContract<TeamStrengthStatisticsDto>> GetTeamStrengthStatisticsIndividualized(Guid teamId);
        
        // Nuevos métodos para el dashboard
        Task<ResponseContract<DashboardIndicatorsDto>> GetDashboardIndicators(Guid? coachId, Guid? teamId);
        Task<ResponseContract<DashboardCompleteDto>> GetDashboardComplete(Guid? coachId);
        Task<ResponseContract<List<TopPerformanceAthleteDto>>> GetTopPerformanceAthletes(Guid? coachId, Guid? teamId, int limit);
        Task<ResponseContract<List<RecentTestDto>>> GetRecentTests(Guid? coachId, Guid? teamId, int limit);
        Task<ResponseContract<List<PendingTaskDto>>> GetPendingTasks(Guid? coachId, string? priority);
        Task<ResponseContract<List<MonthlyEvolutionDto>>> GetMonthlyEvolution(Guid? coachId, Guid? teamId, int months);
        Task<ResponseContract<NextSessionInfo>> GetNextSession(Guid coachId);
        Task<ResponseContract<List<TeamOverviewDto>>> GetCoachTeamsOverview(Guid coachId);

        // Cross-statistics: SAREMAS+ & Macrociclo
        Task<ResponseContract<SaremasTeamStatsDto>> GetSaremasTeamStats(Guid teamId);
        Task<ResponseContract<SaremasAthleteEvolutionDto>> GetSaremasAthleteStats(Guid athleteId);
        Task<ResponseContract<MacrocycleProgressDto>> GetMacrocycleProgress(Guid macrocycleId);
        Task<ResponseContract<AthleteFullDashboardDto>> GetAthleteFullDashboard(Guid athleteId);
    }
}
