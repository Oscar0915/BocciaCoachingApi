using BocciaCoaching.Models.DTO.General;
using BocciaCoaching.Models.DTO.Statistic;

namespace BocciaCoaching.Repositories.Statistic.Interfce;

public interface IStatisticAssessStrength
{
    Task<ResponseContract<List<StrengthTestSummaryDto>>> GetRecentStatistics(int coachId, int teamId);
}