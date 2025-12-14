﻿using BocciaCoaching.Models.DTO.AssessStrength;
using BocciaCoaching.Models.DTO.General;
using BocciaCoaching.Models.Entities;

namespace BocciaCoaching.Services.Interfaces
{
    public interface IAssessStrengthService
    {
        Task<ResponseContract<ResponseAddAssessStrengthDto>> CreateEvaluation(AddAssessStrengthDto addAssessStrengthDto);

        Task<ResponseContract<AthletesToEvaluated>> AgregarAtletaAEvaluacion(
            RequestAddAthleteToEvaluationDto athletesToEvaluated);
        Task<bool> AgregarDetalleDeEvaluacion(RequestAddDetailToEvaluationForAthlete requestAddDetailToEvaluationForAthlete);
        Task<ResponseContract<ActiveEvaluationDto>> GetActiveEvaluationWithDetails(int teamId);
        Task<object> GetEvaluationDebugInfo(int teamId);
    }
}
