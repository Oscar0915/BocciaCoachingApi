﻿using BocciaCoaching.Models.DTO.AssessStrength;
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
        Task<Guid?> GetCoachIdByAssessmentAsync(Guid assessStrengthId);
        Task<ActiveEvaluationDto?> GetActiveEvaluationWithDetailsAsync(Guid teamId, Guid coachId);
        Task<object> GetEvaluationDebugInfoAsync(Guid teamId);
        Task<List<EvaluationSummaryDto>> GetTeamEvaluationsAsync(Guid teamId);
        Task<List<AthleteStatisticsDto>> GetEvaluationStatisticsAsync(Guid assessStrengthId);
        Task<EvaluationDetailsDto?> GetEvaluationDetailsAsync(Guid assessStrengthId);
        Task<ResponseContract<bool>> CancelAssessmentAsync(Guid assessStrengthId, Guid coachId, string? reason);

        /// <summary>
        /// Verifica si un entrenador ya ha generado alguna evaluación de fuerza
        /// </summary>
        Task<CoachHasEvaluationsDto> CoachHasEvaluationsAsync(Guid coachId);
    }
}
