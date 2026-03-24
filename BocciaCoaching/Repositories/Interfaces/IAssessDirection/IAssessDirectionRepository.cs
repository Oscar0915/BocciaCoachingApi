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

        Task<int?> GetCoachIdByAssessmentAsync(int assessDirectionId);

        Task<ActiveDirectionEvaluationDto?> GetActiveEvaluationWithDetailsAsync(int teamId, int coachId);

        Task<object> GetEvaluationDebugInfoAsync(int teamId);

        Task<List<DirectionEvaluationSummaryDto>> GetTeamEvaluationsAsync(int teamId);

        Task<List<DirectionAthleteStatisticsDto>> GetEvaluationStatisticsAsync(int assessDirectionId);

        Task<DirectionEvaluationDetailsDto?> GetEvaluationDetailsAsync(int assessDirectionId);

        Task<ResponseContract<bool>> CancelAssessmentAsync(int assessDirectionId, int coachId, string? reason);
    }
}

