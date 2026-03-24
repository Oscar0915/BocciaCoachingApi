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

        Task<ResponseContract<ActiveDirectionEvaluationDto>> GetActiveEvaluationWithDetails(int teamId, int coachId);

        Task<object> GetEvaluationDebugInfo(int teamId);

        Task<ResponseContract<bool>> UpdateEvaluationState(UpdateAssessDirectionDto updateDto);

        Task<ResponseContract<List<DirectionEvaluationSummaryDto>>> GetTeamEvaluations(int teamId);

        Task<ResponseContract<List<DirectionAthleteStatisticsDto>>> GetEvaluationStatistics(int assessDirectionId);

        Task<ResponseContract<DirectionEvaluationDetailsDto>> GetEvaluationDetails(int assessDirectionId);

        Task<ResponseContract<bool>> CancelEvaluation(CancelAssessDirectionDto cancelDto);
    }
}

