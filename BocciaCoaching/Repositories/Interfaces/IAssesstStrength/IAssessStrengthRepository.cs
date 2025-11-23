using BocciaCoaching.Models.DTO.AssessStrength;
using BocciaCoaching.Models.DTO.General;
using BocciaCoaching.Models.Entities;

namespace BocciaCoaching.Repositories.Interfaces.IAssesstStrength
{
    public interface IAssessStrengthRepository
    {
        Task<ResponseContract<ResponseAddAssessStrengthDto>> CrearEvaluacion(AddAssessStrengthDto addAssessStrengthDto);

        Task<ResponseContract<AthletesToEvaluated>> AgregarAtletaAEvaluacion(
            RequestAddAthleteToEvaluationDto athletesToEvaluated);

        Task<bool> AgregarDetalleDeEvaluacion(
            RequestAddDetailToEvaluationForAthlete request,
            bool isUpdate);
        Task<bool> InsertStrengthTestStats(StrengthStatistics strengthStatistics);
        Task<List<EvaluationDetailStrength>> GetAllDetailsEvaluation(RequestAddDetailToEvaluationForAthlete evaluationDetail);
    }
}
