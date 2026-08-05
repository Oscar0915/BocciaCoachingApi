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
        Task<ActiveSaremasEvaluationDto?> GetActiveEvaluationAsync(Guid teamId, Guid coachId);
        Task<ResponseContract<bool>> UpdateStateAsync(UpdateSaremasStateDto dto);
        Task<ResponseContract<bool>> CancelAsync(Guid saremasEvalId, Guid coachId, string? reason);
        Task<List<SaremasEvaluationSummaryDto>> GetTeamEvaluationsAsync(Guid teamId);
        Task<SaremasEvaluationDetailsDto?> GetEvaluationDetailsAsync(Guid saremasEvalId);
        Task<SaremasStatisticsDto?> GetEvaluationStatisticsAsync(Guid saremasEvalId);
        Task<SaremasAthleteHistoryDto?> GetAthleteHistoryAsync(Guid athleteId);
        Task<List<SaremasThrow>> GetAllThrowsForAthleteAsync(Guid saremasEvalId, Guid athleteId);
        Task<Guid?> GetCoachIdByEvaluationAsync(Guid saremasEvalId);
        Task<bool> UpdateEvaluationScoresAsync(Guid saremasEvalId, int totalScore, double averageScore);

        /// <summary>
        /// Verifica si un entrenador ya ha generado alguna evaluación SAREMAS+
        /// </summary>
        Task<CoachHasSaremasEvaluationsDto> CoachHasEvaluationsAsync(Guid coachId);
    }
}

