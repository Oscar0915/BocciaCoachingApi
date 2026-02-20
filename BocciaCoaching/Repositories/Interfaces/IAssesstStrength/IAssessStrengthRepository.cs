using BocciaCoaching.Models.DTO.AssessStrength;
using BocciaCoaching.Models.DTO.General;
using BocciaCoaching.Models.DTO.Statistic;
using BocciaCoaching.Models.Entities;

namespace BocciaCoaching.Repositories.Interfaces.IAssesstStrength
{
    public interface IAssessStrengthRepository
    {
        // Nuevo: consulta para saber si existe una evaluación activa
        Task<bool> HasActiveAssessmentAsync();

        Task<ResponseContract<ResponseAddAssessStrengthDto>> CrearEvaluacion(AddAssessStrengthDto addAssessStrengthDto);
        // Nueva: crear evaluación de forma atómica solo si no hay evaluación activa en el mismo team
        Task<ResponseContract<ResponseAddAssessStrengthDto>> CreateAssessmentIfNoneActiveAsync(AddAssessStrengthDto addAssessStrengthDto);

        Task<ResponseContract<AthletesToEvaluated>> AgregarAtletaAEvaluacion(
            RequestAddAthleteToEvaluationDto athletesToEvaluated);

        Task<bool> AgregarDetalleDeEvaluacion(
            RequestAddDetailToEvaluationForAthlete request,
            bool isUpdate);
        Task<bool> InsertStrengthTestStats(StrengthStatistics strengthStatistics);
        Task<List<EvaluationDetailStrength>> GetAllDetailsEvaluation(RequestAddDetailToEvaluationForAthlete evaluationDetail);
        Task<ResponseContract<bool>> UpdateState(UpdateAssessStregthDto updateAssessStregthDto);
        Task<int?> GetCoachIdByAssessmentAsync(int assessStrengthId);
        Task<ActiveEvaluationDto?> GetActiveEvaluationWithDetailsAsync(int teamId);
        Task<object> GetEvaluationDebugInfoAsync(int teamId);
        Task<List<EvaluationSummaryDto>> GetTeamEvaluationsAsync(int teamId);
        Task<List<AthleteStatisticsDto>> GetEvaluationStatisticsAsync(int assessStrengthId);
        Task<EvaluationDetailsDto?> GetEvaluationDetailsAsync(int assessStrengthId);
    }
}
