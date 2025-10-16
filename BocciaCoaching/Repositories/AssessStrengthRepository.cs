using BocciaCoaching.Data;
using BocciaCoaching.Models.DTO.AssessStrength;
using BocciaCoaching.Models.DTO.General;
using BocciaCoaching.Models.DTO.Team;
using BocciaCoaching.Models.Entities;
using BocciaCoaching.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BocciaCoaching.Repositories
{
    public class AssessStrengthRepository: IAssessStrengthRepository
    {

        private readonly ApplicationDbContext _context;

        public AssessStrengthRepository(ApplicationDbContext context) { _context = context; }

        /// <summary>
        /// Método para agregar una nueva evaluación
        /// </summary>
        /// <param name="requestEvaluationForceDto"></param>
        /// <returns></returns>
        public async Task<ResponseNewRecordDto> AddNewEvaluationForce(RequestEvaluationForceDto requestEvaluationForceDto)
        {
            try
            {
                var assessStrength = new AssessStrength()
                {
                    EvaluationDate = DateTime.Now,
                    Description = requestEvaluationForceDto.Description,
                    State = requestEvaluationForceDto.State
                };

                await _context.AssessStrengths.AddAsync(assessStrength);
                await _context.SaveChangesAsync();
                return new ResponseNewRecordDto()
                {
                    Success = true
                };
            }
            catch (Exception ex)
            {
                return new ResponseNewRecordDto()
                {
                    Success = true,
                    Message = ex.Message

                };
            }
        }
        public async Task<ResponseAddAssessStrengthDto> CrearEvaluacion(AddAssessStrengthDto addAssessStrengthDto)
        {
            try
            {
                var assessStrength = new AssessStrength
                {
                    EvaluationDate = DateTime.Now,
                    Description = addAssessStrengthDto.Description, 
                    State = addAssessStrengthDto.State
                };

                await _context.AssessStrengths.AddAsync(assessStrength);
                await _context.SaveChangesAsync();

                var response = new ResponseAddAssessStrengthDto
                {
                    AssessStrengthId = assessStrength.AssessStrengthId,
                    DateEvaluation = assessStrength.EvaluationDate,
                    State = true
                };



                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en AddUser: {ex.Message}");
                return new ResponseAddAssessStrengthDto
                {
                    AssessStrengthId = 0,
                    DateEvaluation = DateTime.Now,
                    State = false
                };
            }
        }
        public async Task<AthletesToEvaluated> AgregarAtletaAEvaluacion(RequestAddAthleteToEvaluationDto athletesToEvaluated)
        {
            try
            {
                var athletesInfo = new AthletesToEvaluated
                {
                    AthleteId = athletesToEvaluated.AthleteId,
                    CoachId = athletesToEvaluated.CoachId,
                    AssessStrengthId = athletesToEvaluated.AssessStrengthId,
                };
                await _context.AthletesToEvaluated.AddAsync(athletesInfo);
                await _context.SaveChangesAsync();
                return athletesInfo;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en AddUser: {ex.Message}");
                return new AthletesToEvaluated
                {
                    AssessStrengthId = 0,
                    AthleteId = 0,
                    CoachId = 0,
                };
            }
        }
        public async Task<bool> AgregarDetalleDeEvaluacion(RequestAddDetailToEvaluationForAthlete requestAddDetailToEvaluationForAthlete)
        {
            try
            {
                var athletesInfo = new EvaluationDetailStrength
                {
                    AssessStrengthId = requestAddDetailToEvaluationForAthlete.AssessStrengthId,
                    AthleteId = requestAddDetailToEvaluationForAthlete.AthleteId,
                    BoxNumber = requestAddDetailToEvaluationForAthlete.BoxNumber,
                    Observations = requestAddDetailToEvaluationForAthlete.Observations,
                    ScoreObtained = requestAddDetailToEvaluationForAthlete.ScoreObtained,
                    Status = requestAddDetailToEvaluationForAthlete.Status,
                    TargetDistance = requestAddDetailToEvaluationForAthlete.TargetDistance,
                    ThrowOrder = requestAddDetailToEvaluationForAthlete.ThrowOrder

                };
                await _context.EvaluationDetailStrengths.AddAsync(athletesInfo);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en AddUser: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> UpdateState(AddAssessStrengthDto addAssessStrengthDto)
        {
            try
            {
                var existing = await _context.AssessStrengths
                    .FirstOrDefaultAsync(x => x.AssessStrengthId == addAssessStrengthDto.Id);

                if (existing == null)
                {
                    Console.WriteLine($"No se encontró el registro con ID {addAssessStrengthDto.Id}");
                    return false;
                }

                existing.State = addAssessStrengthDto.State;

                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en UpdateState: {ex.Message}");
                return false;
            }
        }

    }
}
