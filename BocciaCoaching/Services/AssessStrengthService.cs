using BocciaCoaching.Models.DTO.AssessStrength;
using BocciaCoaching.Models.Entities;
using BocciaCoaching.Repositories.Interfaces;
using BocciaCoaching.Services.Interfaces;

namespace BocciaCoaching.Services
{
    public class AssessStrengthService: IAssessStrengthService
    {
        private readonly IAssessStrengthRepository _assessStrengthRepository;

        public AssessStrengthService(IAssessStrengthRepository assessStrengthRepository)
        {
            _assessStrengthRepository = assessStrengthRepository;
        }
        public async Task<AthletesToEvaluated> AgregarAtletaAEvaluacion(RequestAddAthleteToEvaluationDto athletesToEvaluated)
        {
            return await _assessStrengthRepository.AgregarAtletaAEvaluacion(athletesToEvaluated);
        }

        public async Task<bool> AgregarDetalleDeEvaluacion(RequestAddDetailToEvaluationForAthlete requestAddDetailToEvaluationForAthlete)
        {
            return await _assessStrengthRepository.AgregarDetalleDeEvaluacion(requestAddDetailToEvaluationForAthlete);
        }

        public async Task<ResponseAddAssessStrengthDto> CrearEvaluacion(AddAssessStrengthDto addAssessStrengthDto)
        {
            return await _assessStrengthRepository.CrearEvaluacion(addAssessStrengthDto);
        }

    }
}
