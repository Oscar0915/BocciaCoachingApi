using BocciaCoaching.Models.DTO.AssessDirection;
using BocciaCoaching.Models.DTO.General;
using BocciaCoaching.Models.Entities;

namespace BocciaCoaching.Repositories.Interfaces.IAssessDirection
{
    public interface IAssessDirectionRepository
    {
        Task<bool> HasActiveAssessmentAsync();

        Task<ResponseContract<ResponseAddAssessDirectionDto>> CreateAssessmentIfNoneActiveAsync(AddAssessDirectionDto addAssessDirectionDto);

        Task<ResponseContract<AthletesToEvaluatedDirection>> AgregarAtletaAEvaluacion(
            RequestAddAthleteToDirectionEvaluationDto athletesToEvaluated);

        Task<bool> AgregarDetalleDeEvaluacion(
            RequestAddDetailToDirectionEvaluation request,
            bool isUpdate);

        Task<bool> InsertDirectionTestStats(DirectionStatistics directionStatistics);

        Task<List<EvaluationDetailDirection>> GetAllDetailsEvaluation(RequestAddDetailToDirectionEvaluation evaluationDetail);

        Task<ResponseContract<bool>> UpdateState(UpdateAssessDirectionDto updateAssessDirectionDto);

        Task<Guid?> GetCoachIdByAssessmentAsync(Guid assessDirectionId);

        Task<ActiveDirectionEvaluationDto?> GetActiveEvaluationWithDetailsAsync(Guid teamId, Guid coachId);

        Task<object> GetEvaluationDebugInfoAsync(Guid teamId);

        Task<List<DirectionEvaluationSummaryDto>> GetTeamEvaluationsAsync(Guid teamId);

        Task<List<DirectionAthleteStatisticsDto>> GetEvaluationStatisticsAsync(Guid assessDirectionId);

        Task<DirectionEvaluationDetailsDto?> GetEvaluationDetailsAsync(Guid assessDirectionId);

        Task<ResponseContract<bool>> CancelAssessmentAsync(Guid assessDirectionId, Guid coachId, string? reason);

        /// <summary>
        /// Verifica si un entrenador ya ha generado alguna evaluación de dirección
        /// </summary>
        Task<CoachHasDirectionEvaluationsDto> CoachHasEvaluationsAsync(Guid coachId);
    }
}

