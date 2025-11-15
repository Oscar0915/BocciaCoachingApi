using BocciaCoaching.Models.DTO.AssessStrength;
using BocciaCoaching.Models.Entities;
using BocciaCoaching.Repositories.Interfaces;
using BocciaCoaching.Repositories.Interfaces.ITeams;
using BocciaCoaching.Services.Interfaces;

namespace BocciaCoaching.Services
{
    public class AssessStrengthService: IAssessStrengthService
    {
        private readonly IAssessStrengthRepository _assessStrengthRepository;
        private readonly ITeamValidationRepository   _teamValidationRepository;


        /// <summary>
        ///  Método constructor
        /// </summary>
        /// <param name="assessStrengthRepository"></param>
        /// <param name="teamValidationRepository"></param>
        public AssessStrengthService(IAssessStrengthRepository assessStrengthRepository, ITeamValidationRepository teamValidationRepository)
        {
            _assessStrengthRepository = assessStrengthRepository;
            _teamValidationRepository = teamValidationRepository;
        }
        public async Task<AthletesToEvaluated> AgregarAtletaAEvaluacion(RequestAddAthleteToEvaluationDto athletesToEvaluated)
        {
            return await _assessStrengthRepository.AgregarAtletaAEvaluacion(athletesToEvaluated);
        }

        public async Task<bool> AgregarDetalleDeEvaluacion(RequestAddDetailToEvaluationForAthlete requestAddDetailToEvaluationForAthlete)
        {
            return await _assessStrengthRepository.AgregarDetalleDeEvaluacion(requestAddDetailToEvaluationForAthlete);
        }

        /// <summary>
        /// Crear una nueva evaluación 
        /// </summary>
        /// <param name="addAssessStrengthDto"></param>
        /// <returns></returns>
        public async Task<ResponseAddAssessStrengthDto> CreateEvaluation(AddAssessStrengthDto addAssessStrengthDto)
        {
            try
            {
                var isValidTeam= await _teamValidationRepository.ValidateTeam(new Team
                {
                    TeamId = addAssessStrengthDto.TeamId
                }); 
                if ( isValidTeam == false)
                     throw new Exception("El equipo no esta activo");
                
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
               
            }
            return await _assessStrengthRepository.CrearEvaluacion(addAssessStrengthDto);
        }
    }
}
