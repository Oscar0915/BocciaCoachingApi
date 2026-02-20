﻿﻿using BocciaCoaching.Data;
using BocciaCoaching.Models.DTO.AssessStrength;
using BocciaCoaching.Models.DTO.General;
using BocciaCoaching.Models.DTO.Statistic;
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
                    TeamId = addAssessStrengthDto.TeamId,
                    CoachId = addAssessStrengthDto.CoachId,
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
                    CoachId = addAssessStrengthDto.CoachId,
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
                    .FirstOrDefaultAsync(a => a.AssessStrengthId == assessStrengthId);

                return assessment?.CoachId;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error obteniendo CoachId: {e.Message}");
                return null;
            }
        }

        /// <summary>
        /// Obtiene la evaluación de fuerza activa para un equipo con todos sus detalles
        /// </summary>
        /// <param name="teamId">ID del equipo</param>
        /// <returns>Información completa de la evaluación activa o null si no hay ninguna</returns>
        public async Task<ActiveEvaluationDto?> GetActiveEvaluationWithDetailsAsync(int teamId)
        {
            try
            {
                Console.WriteLine($"🔍 Buscando evaluación activa para el equipo: {teamId}");
                
                // Buscar evaluación activa para el equipo
                var activeAssessment = await _context.AssessStrengths
                    .Include(a => a.Team)
                    .Include(a => a.Coach)
                    .FirstOrDefaultAsync(a => a.TeamId == teamId && a.State == "A");

                if (activeAssessment == null)
                {
                    Console.WriteLine($"❌ No se encontró evaluación activa para el equipo {teamId}");
                    
                    // Verificar si hay evaluaciones para este equipo en otros estados
                    var allEvaluations = await _context.AssessStrengths
                        .Where(a => a.TeamId == teamId)
                        .Select(a => new { a.AssessStrengthId, a.State, a.EvaluationDate })
                        .ToListAsync();
                    
                    Console.WriteLine($"📋 Evaluaciones encontradas para el equipo {teamId}: {allEvaluations.Count}");
                    foreach (var eval in allEvaluations)
                    {
                        Console.WriteLine($"  - ID: {eval.AssessStrengthId}, Estado: {eval.State}, Fecha: {eval.EvaluationDate}");
                    }
                    
                    return null;
                }

                Console.WriteLine($"✅ Evaluación activa encontrada - ID: {activeAssessment.AssessStrengthId}");

                // Obtener atletas participantes
                Console.WriteLine($"🏃‍♂️ Buscando atletas para la evaluación {activeAssessment.AssessStrengthId}");
                
                var athletesQuery = await _context.AthletesToEvaluated
                    .Where(ate => ate.AssessStrengthId == activeAssessment.AssessStrengthId)
                    .ToListAsync();

                Console.WriteLine($"📊 Atletas encontrados en AthletesToEvaluated: {athletesQuery.Count}");

                var athletes = new List<AthleteInEvaluationDto>();
                foreach (var athlete in athletesQuery)
                {
                    var athleteUser = await _context.Users.FindAsync(athlete.AthleteId);
                    var coachUser = await _context.Users.FindAsync(athlete.CoachId);
                    
                    if (athleteUser != null && coachUser != null)
                    {
                        athletes.Add(new AthleteInEvaluationDto
                        {
                            AthleteId = athlete.AthleteId,
                            AthleteName = $"{athleteUser.FirstName} {athleteUser.LastName}",
                            AthleteEmail = athleteUser.Email,
                            CoachId = athlete.CoachId,
                            CoachName = $"{coachUser.FirstName} {coachUser.LastName}"
                        });
                    }
                }

                Console.WriteLine($"👥 Atletas procesados correctamente: {athletes.Count}");

                // Obtener todos los lanzamientos de la evaluación
                Console.WriteLine($"🎯 Buscando lanzamientos para la evaluación {activeAssessment.AssessStrengthId}");
                
                var throwsQuery = await _context.EvaluationDetailStrengths
                    .Where(eds => eds.AssessStrengthId == activeAssessment.AssessStrengthId)
                    .OrderBy(eds => eds.AthleteId)
                    .ThenBy(eds => eds.ThrowOrder)
                    .ToListAsync();

                Console.WriteLine($"🎯 Lanzamientos encontrados: {throwsQuery.Count}");

                var throws = new List<EvaluationThrowDto>();
                foreach (var throwDetail in throwsQuery)
                {
                    var athleteUser = await _context.Users.FindAsync(throwDetail.AthleteId);
                    
                    throws.Add(new EvaluationThrowDto
                    {
                        EvaluationDetailStrengthId = throwDetail.EvaluationDetailStrengthId,
                        BoxNumber = throwDetail.BoxNumber,
                        ThrowOrder = throwDetail.ThrowOrder,
                        TargetDistance = throwDetail.TargetDistance,
                        ScoreObtained = throwDetail.ScoreObtained,
                        Observations = throwDetail.Observations,
                        Status = throwDetail.Status,
                        AthleteId = throwDetail.AthleteId,
                        AthleteName = athleteUser != null ? $"{athleteUser.FirstName} {athleteUser.LastName}" : "Atleta desconocido",
                        CreatedAt = throwDetail.CreatedAt,
                        UpdatedAt = throwDetail.UpdatedAt
                    });
                }

                Console.WriteLine($"🎯 Lanzamientos procesados correctamente: {throws.Count}");

                // Construir el DTO de respuesta
                var result = new ActiveEvaluationDto
                {
                    AssessStrengthId = activeAssessment.AssessStrengthId,
                    EvaluationDate = activeAssessment.EvaluationDate,
                    Description = activeAssessment.Description,
                    State = activeAssessment.State,
                    TeamId = activeAssessment.TeamId,
                    TeamName = activeAssessment.Team?.NameTeam,
                    CreatedByCoachId = activeAssessment.CoachId,
                    CreatedByCoachName = activeAssessment.Coach != null 
                        ? $"{activeAssessment.Coach.FirstName} {activeAssessment.Coach.LastName}" 
                        : "Entrenador desconocido",
                    CreatedByCoachEmail = activeAssessment.Coach?.Email,
                    CreatedAt = activeAssessment.CreatedAt,
                    UpdatedAt = activeAssessment.UpdatedAt,
                    Athletes = athletes,
                    Throws = throws
                };

                Console.WriteLine($"✅ Resultado final - Atletas: {result.Athletes.Count}, Lanzamientos: {result.Throws.Count}");
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine($"❌ Error obteniendo evaluación activa: {e.Message}");
                Console.WriteLine($"❌ StackTrace: {e.StackTrace}");
                return null;
            }
        }

        /// <summary>
        /// Método de debugging para verificar datos relacionados con evaluaciones
        /// </summary>
        /// <param name="teamId">ID del equipo</param>
        /// <returns>Información de debugging</returns>
        public async Task<object> GetEvaluationDebugInfoAsync(int teamId)
        {
            try
            {
                // Obtener todas las evaluaciones del equipo
                var evaluations = await _context.AssessStrengths
                    .Where(a => a.TeamId == teamId)
                    .Select(a => new 
                    {
                        a.AssessStrengthId,
                        a.TeamId,
                        a.State,
                        a.EvaluationDate,
                        a.Description
                    })
                    .ToListAsync();

                // Para cada evaluación, obtener atletas y lanzamientos
                var debugInfo = new List<object>();
                
                foreach (var eval in evaluations)
                {
                    var athletes = await _context.AthletesToEvaluated
                        .Where(ate => ate.AssessStrengthId == eval.AssessStrengthId)
                        .Select(ate => new
                        {
                            ate.AthleteId,
                            ate.CoachId,
                            AthleteName = ate.Athlete != null ? $"{ate.Athlete.FirstName} {ate.Athlete.LastName}" : "No cargado",
                            CoachName = ate.Coach != null ? $"{ate.Coach.FirstName} {ate.Coach.LastName}" : "No cargado"
                        })
                        .ToListAsync();

                    var throws = await _context.EvaluationDetailStrengths
                        .Where(eds => eds.AssessStrengthId == eval.AssessStrengthId)
                        .Select(eds => new
                        {
                            eds.EvaluationDetailStrengthId,
                            eds.AthleteId,
                            eds.ThrowOrder,
                            eds.BoxNumber,
                            eds.TargetDistance,
                            eds.ScoreObtained
                        })
                        .ToListAsync();

                    debugInfo.Add(new
                    {
                        Evaluation = eval,
                        AthletesCount = athletes.Count,
                        Athletes = athletes,
                        ThrowsCount = throws.Count,
                        Throws = throws.Take(5).ToList() // Solo los primeros 5 para no sobrecargar
                    });
                }

                return new
                {
                    TeamId = teamId,
                    TotalEvaluations = evaluations.Count,
                    ActiveEvaluations = evaluations.Where(e => e.State == "A").Count(),
                    Evaluations = debugInfo
                };
            }
            catch (Exception e)
            {
                return new { Error = e.Message, StackTrace = e.StackTrace };
            }
        }

        /// <summary>
        /// Obtiene todas las evaluaciones de un equipo con información resumida
        /// </summary>
        /// <param name="teamId">ID del equipo</param>
        /// <returns>Lista de resúmenes de evaluaciones</returns>
        public async Task<List<EvaluationSummaryDto>> GetTeamEvaluationsAsync(int teamId)
        {
            try
            {
                var evaluations = await _context.AssessStrengths
                    .Include(a => a.Team)
                    .Include(a => a.Coach)
                    .Where(a => a.TeamId == teamId)
                    .OrderByDescending(a => a.EvaluationDate)
                    .ToListAsync();

                var summaries = new List<EvaluationSummaryDto>();

                foreach (var eval in evaluations)
                {
                    var athletesCount = await _context.AthletesToEvaluated
                        .CountAsync(ate => ate.AssessStrengthId == eval.AssessStrengthId);

                    var throwsCount = await _context.EvaluationDetailStrengths
                        .CountAsync(eds => eds.AssessStrengthId == eval.AssessStrengthId);

                    summaries.Add(new EvaluationSummaryDto
                    {
                        AssessStrengthId = eval.AssessStrengthId,
                        EvaluationDate = eval.EvaluationDate,
                        Description = eval.Description,
                        State = eval.State,
                        StateName = eval.State switch
                        {
                            "A" => "Activa",
                            "T" => "Terminada",
                            "C" => "Cancelada",
                            _ => "Desconocido"
                        },
                        TeamId = eval.TeamId,
                        TeamName = eval.Team?.NameTeam,
                        CoachId = eval.CoachId,
                        CoachName = eval.Coach != null ? $"{eval.Coach.FirstName} {eval.Coach.LastName}" : null,
                        AthletesCount = athletesCount,
                        ThrowsCount = throwsCount,
                        CreatedAt = eval.CreatedAt,
                        UpdatedAt = eval.UpdatedAt
                    });
                }

                return summaries;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error obteniendo evaluaciones del equipo: {e.Message}");
                return new List<EvaluationSummaryDto>();
            }
        }

        /// <summary>
        /// Obtiene las estadísticas de todos los atletas en una evaluación
        /// </summary>
        /// <param name="assessStrengthId">ID de la evaluación</param>
        /// <returns>Lista de estadísticas por atleta</returns>
        public async Task<List<AthleteStatisticsDto>> GetEvaluationStatisticsAsync(int assessStrengthId)
        {
            try
            {
                var statistics = await _context.StrengthStatistics
                    .Include(s => s.Athlete)
                    .Where(s => s.AssessStrengthId == assessStrengthId)
                    .ToListAsync();

                return statistics.Select(s => new AthleteStatisticsDto
                {
                    AthleteId = s.AthleteId,
                    AthleteName = s.Athlete != null ? $"{s.Athlete.FirstName} {s.Athlete.LastName}" : string.Empty,
                    EffectivenessPercentage = s.EffectivenessPercentage,
                    AccuracyPercentage = s.AccuracyPercentage,
                    EffectiveThrow = s.EffectiveThrow,
                    FailedThrow = s.FailedThrow,
                    ShortThrow = s.ShortThrow,
                    MediumThrow = s.MediumThrow,
                    LongThrow = s.LongThrow,
                    ShortEffectivenessPercentage = s.ShortEffectivenessPercentage,
                    MediumEffectivenessPercentage = s.MediumEffectivenessPercentage,
                    LongEffectivenessPercentage = s.LongEffectivenessPercentage,
                    ShortThrowAccuracy = s.ShortThrowAccuracy,
                    MediumThrowAccuracy = s.MediumThrowAccuracy,
                    LongThrowAccuracy = s.LongThrowAccuracy,
                    ShortAccuracyPercentage = s.ShortAccuracyPercentage,
                    MediumAccuracyPercentage = s.MediumAccuracyPercentage,
                    LongAccuracyPercentage = s.LongAccuracyPercentage
                }).ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error obteniendo estadísticas de evaluación: {e.Message}");
                return new List<AthleteStatisticsDto>();
            }
        }

        /// <summary>
        /// Obtiene los detalles completos de una evaluación específica
        /// </summary>
        /// <param name="assessStrengthId">ID de la evaluación</param>
        /// <returns>Detalles completos de la evaluación o null si no existe</returns>
        public async Task<EvaluationDetailsDto?> GetEvaluationDetailsAsync(int assessStrengthId)
        {
            try
            {
                var assessment = await _context.AssessStrengths
                    .Include(a => a.Team)
                    .Include(a => a.Coach)
                    .FirstOrDefaultAsync(a => a.AssessStrengthId == assessStrengthId);

                if (assessment == null)
                {
                    return null;
                }

                // Obtener atletas participantes
                var athletesQuery = await _context.AthletesToEvaluated
                    .Where(ate => ate.AssessStrengthId == assessStrengthId)
                    .ToListAsync();

                var athletes = new List<AthleteInEvaluationDto>();
                foreach (var athlete in athletesQuery)
                {
                    var athleteUser = await _context.Users.FindAsync(athlete.AthleteId);
                    var coachUser = await _context.Users.FindAsync(athlete.CoachId);
                    
                    if (athleteUser != null && coachUser != null)
                    {
                        athletes.Add(new AthleteInEvaluationDto
                        {
                            AthleteId = athlete.AthleteId,
                            AthleteName = $"{athleteUser.FirstName} {athleteUser.LastName}",
                            AthleteEmail = athleteUser.Email,
                            CoachId = athlete.CoachId,
                            CoachName = $"{coachUser.FirstName} {coachUser.LastName}"
                        });
                    }
                }

                // Obtener lanzamientos
                var throwsQuery = await _context.EvaluationDetailStrengths
                    .Where(eds => eds.AssessStrengthId == assessStrengthId)
                    .OrderBy(eds => eds.AthleteId)
                    .ThenBy(eds => eds.ThrowOrder)
                    .ToListAsync();

                var throws = new List<EvaluationThrowDto>();
                foreach (var throwDetail in throwsQuery)
                {
                    var athleteUser = await _context.Users.FindAsync(throwDetail.AthleteId);
                    
                    throws.Add(new EvaluationThrowDto
                    {
                        EvaluationDetailStrengthId = throwDetail.EvaluationDetailStrengthId,
                        BoxNumber = throwDetail.BoxNumber,
                        ThrowOrder = throwDetail.ThrowOrder,
                        TargetDistance = throwDetail.TargetDistance,
                        ScoreObtained = throwDetail.ScoreObtained,
                        Observations = throwDetail.Observations,
                        Status = throwDetail.Status,
                        AthleteId = throwDetail.AthleteId,
                        AthleteName = athleteUser != null ? $"{athleteUser.FirstName} {athleteUser.LastName}" : "Atleta desconocido",
                        CreatedAt = throwDetail.CreatedAt,
                        UpdatedAt = throwDetail.UpdatedAt
                    });
                }

                // Obtener estadísticas si la evaluación está terminada
                List<AthleteStatisticsDto>? statistics = null;
                if (assessment.State == "T")
                {
                    statistics = await GetEvaluationStatisticsAsync(assessStrengthId);
                }

                return new EvaluationDetailsDto
                {
                    AssessStrengthId = assessment.AssessStrengthId,
                    EvaluationDate = assessment.EvaluationDate,
                    Description = assessment.Description,
                    State = assessment.State,
                    StateName = assessment.State switch
                    {
                        "A" => "Activa",
                        "T" => "Terminada",
                        "C" => "Cancelada",
                        _ => "Desconocido"
                    },
                    TeamId = assessment.TeamId,
                    TeamName = assessment.Team?.NameTeam,
                    CoachId = assessment.CoachId,
                    CoachName = assessment.Coach != null ? $"{assessment.Coach.FirstName} {assessment.Coach.LastName}" : null,
                    CoachEmail = assessment.Coach?.Email,
                    CreatedAt = assessment.CreatedAt,
                    UpdatedAt = assessment.UpdatedAt,
                    Athletes = athletes,
                    Throws = throws,
                    Statistics = statistics
                };
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error obteniendo detalles de evaluación: {e.Message}");
                return null;
            }
        }
    }
}
