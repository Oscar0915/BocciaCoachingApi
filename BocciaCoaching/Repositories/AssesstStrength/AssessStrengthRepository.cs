using BocciaCoaching.Data;
using BocciaCoaching.Models.DTO.AssessStrength;
using BocciaCoaching.Models.DTO.General;
using BocciaCoaching.Models.DTO.Team;
using BocciaCoaching.Models.Entities;
using BocciaCoaching.Repositories.Interfaces;
using BocciaCoaching.Repositories.Interfaces.IAssesstStrength;
using Microsoft.EntityFrameworkCore;

namespace BocciaCoaching.Repositories.AssesstStrength
{
    public class AssessStrengthRepository: IAssessStrengthRepository
    {

        private readonly ApplicationDbContext _context;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
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

        /// <summary>
        /// Crear una nueva evaluación 
        /// </summary>
        /// <param name="addAssessStrengthDto"></param>
        /// <returns></returns>
        public async Task<ResponseContract<ResponseAddAssessStrengthDto>> CrearEvaluacion(
            AddAssessStrengthDto addAssessStrengthDto)
        {
            try
            {
                var assessStrength = new AssessStrength
                {
                    EvaluationDate = DateTime.Now,
                    Description = addAssessStrengthDto.Description,
                    State = "A",
                    TeamId = addAssessStrengthDto.TeamId
                };

                await _context.AssessStrengths.AddAsync(assessStrength);
                await _context.SaveChangesAsync();

                var resultDto = new ResponseAddAssessStrengthDto
                {
                    AssessStrengthId = assessStrength.AssessStrengthId,
                    DateEvaluation = assessStrength.EvaluationDate,
                    State = true
                };

                return ResponseContract<ResponseAddAssessStrengthDto>.Ok(
                    resultDto,
                    "Evaluación creada correctamente"
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en CrearEvaluacion: {ex.Message}");

                return ResponseContract<ResponseAddAssessStrengthDto>.Fail(
                    $"Error al crear la evaluación: {ex.Message}"
                )!;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="athletesToEvaluated"></param>
        /// <returns></returns>
        public async Task<ResponseContract<AthletesToEvaluated>> AgregarAtletaAEvaluacion(RequestAddAthleteToEvaluationDto athletesToEvaluated)
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
                return ResponseContract<AthletesToEvaluated>.Ok(athletesInfo,"Inserción exitosa");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en AddUser: {ex.Message}");
                return ResponseContract<AthletesToEvaluated>.Fail("Error al agregar al atleta")!;

            }
        }
        /// <summary>
        /// Funcion que permite agregar el detalle de la evaluacion de fuerza
        /// </summary>
        /// <param name="requestAddDetailToEvaluationForAthlete"></param>
        /// <returns></returns>
        public async Task<bool> AgregarDetalleDeEvaluacion(
            RequestAddDetailToEvaluationForAthlete request,
            bool isUpdate)
        {
            try
            {
                if (isUpdate)
                {
                    // 1. Buscar registro existente
                    var entity = await _context.EvaluationDetailStrengths
                        .FirstOrDefaultAsync(x =>
                            x.AssessStrengthId == request.AssessStrengthId &&
                            x.AthleteId == request.AthleteId);

                    if (entity == null)
                        return false;

                    // 2. Actualizar campos
                    entity.BoxNumber = request.BoxNumber;
                    entity.Observations = request.Observations;
                    entity.ScoreObtained = request.ScoreObtained;
                    entity.Status = request.Status;
                    entity.TargetDistance = request.TargetDistance;
                    entity.ThrowOrder = request.ThrowOrder;

                    _context.EvaluationDetailStrengths.Update(entity);
                }
                else
                {
                    // Crear nueva entidad
                    var newEntity = new EvaluationDetailStrength
                    {
                        AssessStrengthId = request.AssessStrengthId,
                        AthleteId = request.AthleteId,
                        BoxNumber = request.BoxNumber,
                        Observations = request.Observations,
                        ScoreObtained = request.ScoreObtained,
                        Status = request.Status,
                        TargetDistance = request.TargetDistance,
                        ThrowOrder = request.ThrowOrder
                    };

                    await _context.EvaluationDetailStrengths.AddAsync(newEntity);
                }

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }


        public async Task<bool> UpdateState(UpdateAssessStregthDto updateAssessStregthDto)
        {
            try
            {
                var existing = await _context.AssessStrengths
                    .FirstOrDefaultAsync(x => x.AssessStrengthId == updateAssessStregthDto.Id);

                if (existing == null)
                {
                    Console.WriteLine($"No se encontró el registro con ID {updateAssessStregthDto.Id}");
                    return false;
                }

                existing.State = updateAssessStregthDto.State;

                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en UpdateState: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Inserción de las estadisticas de la prueba de fuerza
        /// </summary>
        /// <param name="strengthStatistics"></param>
        /// <returns></returns>
        public async Task<bool> InsertStrengthTestStats(StrengthStatistics strengthStatistics)
        {
            try
            {
                // Crear una nueva instancia por seguridad (evitando trackeo indebido)
                var statistics = new StrengthStatistics
                {
                    EffectivenessPercentage = strengthStatistics.EffectivenessPercentage,
                    AccuracyPercentage = strengthStatistics.AccuracyPercentage,
                    EffectiveThrow = strengthStatistics.EffectiveThrow,
                    FailedThrow = strengthStatistics.FailedThrow,
                    ShortThrow = strengthStatistics.ShortThrow,
                    MediumThrow = strengthStatistics.MediumThrow,
                    LongThrow = strengthStatistics.LongThrow,
                    ShortEffectivenessPercentage = strengthStatistics.ShortEffectivenessPercentage,
                    MediumEffectivenessPercentage = strengthStatistics.MediumEffectivenessPercentage,
                    LongEffectivenessPercentage = strengthStatistics.LongEffectivenessPercentage,
                    ShortThrowAccuracy = strengthStatistics.ShortThrowAccuracy,
                    MediumThrowAccuracy = strengthStatistics.MediumThrowAccuracy,
                    LongThrowAccuracy = strengthStatistics.LongThrowAccuracy,
                    ShortAccuracyPercentage = strengthStatistics.ShortAccuracyPercentage,
                    MediumAccuracyPercentage = strengthStatistics.MediumAccuracyPercentage,
                    LongAccuracyPercentage = strengthStatistics.LongAccuracyPercentage,
                    AssessStrengthId = strengthStatistics.AssessStrengthId,
                    AthleteId = strengthStatistics.AthleteId
                };

                await _context.StrengthStatistics.AddAsync(statistics);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception e)
            {
                // Aquí puedes loguear con serilog, nlog, etc.
                Console.WriteLine($"Error inserting StrengthStatistics: {e.Message}");
                return false;
            }
        }




        public async Task<List<EvaluationDetailStrength>> GetAllDetailsEvaluation(RequestAddDetailToEvaluationForAthlete evaluationDetail)
        {
            try
            {
                  var listEvaluationDetail = await  _context.EvaluationDetailStrengths.Where(e=> e.AssessStrengthId == evaluationDetail.AssessStrengthId).ToListAsync();
                  return listEvaluationDetail;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new List<EvaluationDetailStrength>();
            }
        }
        
        
    }
}
