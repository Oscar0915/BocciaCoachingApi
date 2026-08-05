using BocciaCoaching.Models.DTO.AssessDirection;
using BocciaCoaching.Models.DTO.General;
using BocciaCoaching.Models.Entities;

namespace BocciaCoaching.Services.Interfaces
{
    public interface IAssessDirectionService
    {
        Task<ResponseContract<ResponseAddAssessDirectionDto>> CreateEvaluation(AddAssessDirectionDto addAssessDirectionDto);

        Task<ResponseContract<AthletesToEvaluatedDirection>> AgregarAtletaAEvaluacion(
            RequestAddAthleteToDirectionEvaluationDto athletesToEvaluated);

        Task<bool> AgregarDetalleDeEvaluacion(RequestAddDetailToDirectionEvaluation request);

        Task<ResponseContract<ActiveDirectionEvaluationDto>> GetActiveEvaluationWithDetails(Guid teamId, Guid coachId);

        Task<object> GetEvaluationDebugInfo(Guid teamId);

        Task<ResponseContract<bool>> UpdateEvaluationState(UpdateAssessDirectionDto updateDto);

        Task<ResponseContract<List<DirectionEvaluationSummaryDto>>> GetTeamEvaluations(Guid teamId);

        Task<ResponseContract<List<DirectionAthleteStatisticsDto>>> GetEvaluationStatistics(Guid assessDirectionId);

        Task<ResponseContract<DirectionEvaluationDetailsDto>> GetEvaluationDetails(Guid assessDirectionId);

        Task<ResponseContract<bool>> CancelEvaluation(CancelAssessDirectionDto cancelDto);
        Task<ResponseContract<CoachHasDirectionEvaluationsDto>> CoachHasEvaluations(Guid coachId);
    }
}

