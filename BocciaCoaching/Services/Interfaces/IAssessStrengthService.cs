﻿using BocciaCoaching.Models.DTO.AssessStrength;
using BocciaCoaching.Models.DTO.General;
using BocciaCoaching.Models.DTO.Statistic;
using BocciaCoaching.Models.Entities;

namespace BocciaCoaching.Services.Interfaces
{
    public interface IAssessStrengthService
    {
        Task<ResponseContract<ResponseAddAssessStrengthDto>> CreateEvaluation(AddAssessStrengthDto addAssessStrengthDto);

        Task<ResponseContract<AthletesToEvaluated>> AgregarAtletaAEvaluacion(
            RequestAddAthleteToEvaluationDto athletesToEvaluated);
        Task<bool> AgregarDetalleDeEvaluacion(RequestAddDetailToEvaluationForAthlete requestAddDetailToEvaluationForAthlete);
        Task<ResponseContract<ActiveEvaluationDto>> GetActiveEvaluationWithDetails(Guid teamId, Guid coachId);
        Task<object> GetEvaluationDebugInfo(Guid teamId);
        Task<ResponseContract<bool>> UpdateEvaluationState(UpdateAssessStregthDto updateDto);
        Task<ResponseContract<List<EvaluationSummaryDto>>> GetTeamEvaluations(Guid teamId);
        Task<ResponseContract<List<AthleteStatisticsDto>>> GetEvaluationStatistics(Guid assessStrengthId);
        Task<ResponseContract<EvaluationDetailsDto>> GetEvaluationDetails(Guid assessStrengthId);
        Task<ResponseContract<bool>> CancelEvaluation(CancelAssessStrengthDto cancelDto);
        Task<ResponseContract<CoachHasEvaluationsDto>> CoachHasEvaluations(Guid coachId);
    }
}
