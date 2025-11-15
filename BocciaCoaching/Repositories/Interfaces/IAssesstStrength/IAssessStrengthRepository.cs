using BocciaCoaching.Models.DTO.AssessStrength;
using BocciaCoaching.Models.Entities;

namespace BocciaCoaching.Repositories.Interfaces
{
    public interface IAssessStrengthRepository
    {
        Task<ResponseAddAssessStrengthDto> CrearEvaluacion(AddAssessStrengthDto addAssessStrengthDto);
        Task<AthletesToEvaluated> AgregarAtletaAEvaluacion(RequestAddAthleteToEvaluationDto athletesToEvaluated);
        Task<bool> AgregarDetalleDeEvaluacion(RequestAddDetailToEvaluationForAthlete requestAddDetailToEvaluationForAthlete);
    }
}
