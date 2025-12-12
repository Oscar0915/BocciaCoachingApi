﻿using BocciaCoaching.Data;
using BocciaCoaching.Models.DTO.AssessStrength;
using BocciaCoaching.Models.DTO.General;
using BocciaCoaching.Models.DTO.Team;
using BocciaCoaching.Models.Entities;
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
                // (La validación de evaluación activa se realiza en el service)

                var assessStrength = new AssessStrength()
                {
                    EvaluationDate = DateTime.Now,
                    Description = requestEvaluationForceDto.Description,
                    // Asegurar que la evaluación iniciada quede en estado 'A' (Activa)
                    State = "A",
                    CreatedAt = DateTime.Now
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
                // (La validación de evaluación activa se realiza en el service)

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
                );
             }
         }

        // Crear evaluación de forma atómica solo si no hay evaluación activa en el mismo team
        public async Task<ResponseContract<ResponseAddAssessStrengthDto>> CreateAssessmentIfNoneActiveAsync(AddAssessStrengthDto addAssessStrengthDto)
        {
            try
            {
                await using var transaction = await _context.Database.BeginTransactionAsync();

                // Verificar si existe evaluación activa para el mismo Team
                var existsActiveForTeam = await _context.AssessStrengths.AnyAsync(a => a.TeamId == addAssessStrengthDto.TeamId && a.State == "A");
                if (existsActiveForTeam)
                {
                    await transaction.RollbackAsync();
                    return ResponseContract<ResponseAddAssessStrengthDto>.Fail("Ya existe una evaluación activa para este equipo. Finaliza o cancela antes de crear una nueva.");
                }

                var assessStrength = new AssessStrength
                {
                    EvaluationDate = DateTime.Now,
                    Description = addAssessStrengthDto.Description,
                    State = "A",
                    TeamId = addAssessStrengthDto.TeamId,
                    CreatedAt = DateTime.Now
                };

                await _context.AssessStrengths.AddAsync(assessStrength);
                await _context.SaveChangesAsync();

                var resultDto = new ResponseAddAssessStrengthDto
                {
                    AssessStrengthId = assessStrength.AssessStrengthId,
                    DateEvaluation = assessStrength.EvaluationDate,
                    State = true
                };

                await transaction.CommitAsync();

                return ResponseContract<ResponseAddAssessStrengthDto>.Ok(resultDto, "Evaluación creada correctamente");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en CreateAssessmentIfNoneActiveAsync: {ex.Message}");
                try
                {
                    await _context.Database.RollbackTransactionAsync();
                }
                catch (Exception rollbackEx)
                {
                    Console.WriteLine($"Error al hacer rollback en CreateAssessmentIfNoneActiveAsync: {rollbackEx.Message}");
                }

                return ResponseContract<ResponseAddAssessStrengthDto>.Fail($"Error al crear la evaluación: {ex.Message}");
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
                return ResponseContract<AthletesToEvaluated>.Fail("Error al agregar al atleta");
 
            }
        }
        /// <summary>
        /// Funcion que permite agregar el detalle de la evaluacion de fuerza
        /// </summary>
        /// <param name="request">Detalle de la evaluación (lanzamiento)</param>
        /// <param name="isUpdate">Indica si es actualización o inserción</param>
        /// <returns>True si la operación fue exitosa</returns>
        public async Task<bool> AgregarDetalleDeEvaluacion(
            RequestAddDetailToEvaluationForAthlete request,
            bool isUpdate)
        {
            try
            {
                // Iniciar transacción para garantizar atomicidad
                await using var transaction = await _context.Database.BeginTransactionAsync();

                if (isUpdate)
                {
                    // 1. Buscar registro existente
                    var entity = await _context.EvaluationDetailStrengths
                        .FirstOrDefaultAsync(x =>
                            x.AssessStrengthId == request.AssessStrengthId &&
                            x.AthleteId == request.AthleteId &&
                            x.ThrowOrder == request.ThrowOrder);

                    if (entity == null)
                    {
                        await transaction.RollbackAsync();
                        return false;
                    }

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

                // Si se completó el lanzamiento 36, preparar la actualización del estado de la evaluación
                if (request.ThrowOrder == 36)
                {
                    var assess = await _context.AssessStrengths.FirstOrDefaultAsync(a => a.AssessStrengthId == request.AssessStrengthId);
                    if (assess != null && assess.State != "T")
                    {
                        assess.State = "T";
                        assess.UpdatedAt = DateTime.Now;
                        _context.AssessStrengths.Update(assess);
                    }
                }

                // Guardar todos los cambios juntos
                await _context.SaveChangesAsync();

                // Commit de la transacción
                await transaction.CommitAsync();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                try
                {
                    await _context.Database.RollbackTransactionAsync();
                }
                catch (Exception rollbackEx)
                {
                    Console.WriteLine($"Error al hacer rollback de la transacción: {rollbackEx.Message}");
                }
                return false;
            }
        }


        public async Task<ResponseContract<bool>> UpdateState(UpdateAssessStregthDto updateAssessStregthDto)
        {
            try
            {
                var existing = await _context.AssessStrengths
                    .FirstOrDefaultAsync(x => x.AssessStrengthId == updateAssessStregthDto.Id);

                if (existing == null)
                {
                    var message = $"No se encontró el registro con ID {updateAssessStregthDto.Id}";
                    Console.WriteLine(message);
                    return ResponseContract<bool>.Fail(message);
                }

                existing.State = updateAssessStregthDto.State;

                await _context.SaveChangesAsync();

                return ResponseContract<bool>.Ok(true, "Estado actualizado correctamente");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en UpdateState: {ex.Message}");
                return ResponseContract<bool>.Fail($"Error al actualizar el estado: {ex.Message}");
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
                var listEvaluationDetail = await _context.EvaluationDetailStrengths
                    .Where(e => e.AssessStrengthId == evaluationDetail.AssessStrengthId)
                    .ToListAsync();
                return listEvaluationDetail;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new List<EvaluationDetailStrength>();
            }
        }

        // Nuevo: implementación para consultar si existe evaluación activa
        public async Task<bool> HasActiveAssessmentAsync()
        {
            return await _context.AssessStrengths.AnyAsync(a => a.State == "A");
        }

        /// <summary>
        /// Obtener el CoachId de una evaluación
        /// </summary>
        /// <param name="assessStrengthId"></param>
        /// <returns></returns>
        public async Task<int?> GetCoachIdByAssessmentAsync(int assessStrengthId)
        {
            try
            {
                var assessment = await _context.AssessStrengths
                    .Include(a => a.Team)
                    .FirstOrDefaultAsync(a => a.AssessStrengthId == assessStrengthId);

                return assessment?.Team?.CoachId;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error obteniendo CoachId: {e.Message}");
                return null;
            }
        }
    }
}
