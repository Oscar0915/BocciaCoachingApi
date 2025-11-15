using BocciaCoaching.Models.DTO.AssessStrength;
using BocciaCoaching.Models.Entities;

namespace BocciaCoaching.Services.Interfaces
{
    public interface IAssessStrengthService
    {
        Task<ResponseAddAssessStrengthDto> CreateEvaluation(AddAssessStrengthDto addAssessStrengthDto);
        Task<AthletesToEvaluated> AgregarAtletaAEvaluacion(RequestAddAthleteToEvaluationDto athletesToEvaluated);
        Task<bool> AgregarDetalleDeEvaluacion(RequestAddDetailToEvaluationForAthlete requestAddDetailToEvaluationForAthlete);
    }
}
