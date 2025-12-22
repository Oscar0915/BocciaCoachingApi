using BocciaCoaching.Models.DTO.General;
using BocciaCoaching.Models.DTO.Statistic;

namespace BocciaCoaching.Repositories.Statistic.Interfce;

public interface IStatisticAssessStrength
{
    Task<ResponseContract<List<StrengthTestSummaryDto>>> GetRecentStatistics(int coachId, int teamId);
    Task<ResponseContract<TeamStrengthStatisticsDto>> GetTeamStrengthStatistics(int teamId);
    Task<ResponseContract<object>> GetTeamEvaluationsDebug(int teamId);
    Task<ResponseContract<TeamStrengthStatisticsDto>> GetTeamStrengthStatisticsIndividualized(int teamId);
    
    // Nuevos métodos para el dashboard
    Task<ResponseContract<DashboardIndicatorsDto>> GetDashboardIndicators(int? coachId, int? teamId);
    Task<ResponseContract<DashboardCompleteDto>> GetDashboardComplete(int? coachId);
    Task<ResponseContract<List<TopPerformanceAthleteDto>>> GetTopPerformanceAthletes(int? coachId, int? teamId, int limit);
    Task<ResponseContract<List<RecentTestDto>>> GetRecentTests(int? coachId, int? teamId, int limit);
    Task<ResponseContract<List<PendingTaskDto>>> GetPendingTasks(int? coachId, string? priority);
    Task<ResponseContract<List<MonthlyEvolutionDto>>> GetMonthlyEvolution(int? coachId, int? teamId, int months);
    Task<ResponseContract<NextSessionInfo>> GetNextSession(int coachId);
    Task<ResponseContract<List<TeamOverviewDto>>> GetCoachTeamsOverview(int coachId);
}