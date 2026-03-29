using BocciaCoaching.Models.DTO.AssessSaremas;
using BocciaCoaching.Models.DTO.General;
using BocciaCoaching.Models.Entities;

namespace BocciaCoaching.Repositories.Interfaces.IAssessSaremas
{
    public interface IAssessSaremasRepository
    {
        Task<ResponseContract<ResponseAddSaremasDto>> CreateEvaluationIfNoneActiveAsync(AddSaremasEvaluationDto dto);
        Task<ResponseContract<SaremasAthleteEvaluation>> AddAthleteToEvaluationAsync(RequestAddAthleteToSaremasDto dto);
        Task<bool> AddThrowDetailAsync(RequestAddSaremasDetailDto dto, bool isUpdate);
        Task<ActiveSaremasEvaluationDto?> GetActiveEvaluationAsync(int teamId, int coachId);
        Task<ResponseContract<bool>> UpdateStateAsync(UpdateSaremasStateDto dto);
        Task<ResponseContract<bool>> CancelAsync(int saremasEvalId, int coachId, string? reason);
        Task<List<SaremasEvaluationSummaryDto>> GetTeamEvaluationsAsync(int teamId);
        Task<SaremasEvaluationDetailsDto?> GetEvaluationDetailsAsync(int saremasEvalId);
        Task<SaremasStatisticsDto?> GetEvaluationStatisticsAsync(int saremasEvalId);
        Task<SaremasAthleteHistoryDto?> GetAthleteHistoryAsync(int athleteId);
        Task<List<SaremasThrow>> GetAllThrowsForAthleteAsync(int saremasEvalId, int athleteId);
        Task<int?> GetCoachIdByEvaluationAsync(int saremasEvalId);
        Task<bool> UpdateEvaluationScoresAsync(int saremasEvalId, int totalScore, double averageScore);
    }
}

