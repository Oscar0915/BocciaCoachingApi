using BocciaCoaching.Data;
using BocciaCoaching.Models.DTO.AssessDirection;
using BocciaCoaching.Models.DTO.General;
using BocciaCoaching.Models.Entities;
using BocciaCoaching.Repositories.Interfaces.IAssessDirection;
using Microsoft.EntityFrameworkCore;

namespace BocciaCoaching.Repositories.AssessDirection
{
    public class AssessDirectionRepository : IAssessDirectionRepository
    {
        private readonly ApplicationDbContext _context;

        public AssessDirectionRepository(ApplicationDbContext context) { _context = context; }

        // Nuevo: consulta para saber si existe una evaluación activa
        public async Task<bool> HasActiveAssessmentAsync()
        {
            return await _context.AssessDirections.AnyAsync(a => a.State == "A");
        }

        // Crear evaluación de forma atómica solo si no hay evaluación activa en el mismo team
        public async Task<ResponseContract<ResponseAddAssessDirectionDto>> CreateAssessmentIfNoneActiveAsync(AddAssessDirectionDto addAssessDirectionDto)
        {
            try
            {
                await using var transaction = await _context.Database.BeginTransactionAsync();

                var existsActiveForTeam = await _context.AssessDirections
                    .AnyAsync(a => a.TeamId == addAssessDirectionDto.TeamId && a.State == "A");
                if (existsActiveForTeam)
                {
                    await transaction.RollbackAsync();
                    return ResponseContract<ResponseAddAssessDirectionDto>.Fail(
                        "Ya existe una evaluación de dirección activa para este equipo. Finaliza o cancela antes de crear una nueva.");
                }

                var assessDirection = new Models.Entities.AssessDirection
                {
                    EvaluationDate = DateTime.Now,
                    Description = addAssessDirectionDto.Description,
                    State = "A",
                    TeamId = addAssessDirectionDto.TeamId,
                    CoachId = addAssessDirectionDto.CoachId,
                    CreatedAt = DateTime.Now
                };

                await _context.AssessDirections.AddAsync(assessDirection);
                await _context.SaveChangesAsync();

                var resultDto = new ResponseAddAssessDirectionDto
                {
                    AssessDirectionId = assessDirection.AssessDirectionId,
                    DateEvaluation = assessDirection.EvaluationDate,
                    State = true
                };

                await transaction.CommitAsync();

                return ResponseContract<ResponseAddAssessDirectionDto>.Ok(resultDto, "Evaluación de dirección creada correctamente");
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
                    Console.WriteLine($"Error al hacer rollback: {rollbackEx.Message}");
                }

                return ResponseContract<ResponseAddAssessDirectionDto>.Fail($"Error al crear la evaluación: {ex.Message}");
            }
        }

        public async Task<ResponseContract<AthletesToEvaluatedDirection>> AgregarAtletaAEvaluacion(
            RequestAddAthleteToDirectionEvaluationDto athletesToEvaluated)
        {
            try
            {
                var athletesInfo = new AthletesToEvaluatedDirection
                {
                    AthleteId = athletesToEvaluated.AthleteId,
                    CoachId = athletesToEvaluated.CoachId,
                    AssessDirectionId = athletesToEvaluated.AssessDirectionId,
                };
                await _context.AthletesToEvaluatedDirection.AddAsync(athletesInfo);
                await _context.SaveChangesAsync();
                return ResponseContract<AthletesToEvaluatedDirection>.Ok(athletesInfo, "Inserción exitosa");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en AgregarAtletaAEvaluacion (Direction): {ex.Message}");
                return ResponseContract<AthletesToEvaluatedDirection>.Fail("Error al agregar al atleta");
            }
        }

        /// <summary>
        /// Función que permite agregar el detalle de la evaluación de dirección
        /// </summary>
        public async Task<bool> AgregarDetalleDeEvaluacion(
            RequestAddDetailToDirectionEvaluation request,
            bool isUpdate)
        {
            try
            {
                await using var transaction = await _context.Database.BeginTransactionAsync();

                if (isUpdate)
                {
                    var entity = await _context.EvaluationDetailDirections
                        .FirstOrDefaultAsync(x =>
                            x.AssessDirectionId == request.AssessDirectionId &&
                            x.AthleteId == request.AthleteId &&
                            x.ThrowOrder == request.ThrowOrder);

                    if (entity == null)
                    {
                        await transaction.RollbackAsync();
                        return false;
                    }

                    entity.BoxNumber = request.BoxNumber;
                    entity.Observations = request.Observations;
                    entity.ScoreObtained = request.ScoreObtained;
                    entity.Status = request.Status;
                    entity.TargetDistance = request.TargetDistance;
                    entity.ThrowOrder = request.ThrowOrder;
                    entity.CoordinateX = request.CoordinateX;
                    entity.CoordinateY = request.CoordinateY;
                    entity.DeviatedRight = request.DeviatedRight;
                    entity.DeviatedLeft = request.DeviatedLeft;

                    _context.EvaluationDetailDirections.Update(entity);
                }
                else
                {
                    var newEntity = new EvaluationDetailDirection
                    {
                        AssessDirectionId = request.AssessDirectionId,
                        AthleteId = request.AthleteId,
                        BoxNumber = request.BoxNumber,
                        Observations = request.Observations,
                        ScoreObtained = request.ScoreObtained,
                        Status = request.Status,
                        TargetDistance = request.TargetDistance,
                        ThrowOrder = request.ThrowOrder,
                        CoordinateX = request.CoordinateX,
                        CoordinateY = request.CoordinateY,
                        DeviatedRight = request.DeviatedRight,
                        DeviatedLeft = request.DeviatedLeft
                    };

                    await _context.EvaluationDetailDirections.AddAsync(newEntity);
                }

                // Si se completó el lanzamiento 24, marcar la evaluación como terminada
                if (request.ThrowOrder == 24)
                {
                    var assess = await _context.AssessDirections
                        .FirstOrDefaultAsync(a => a.AssessDirectionId == request.AssessDirectionId);
                    if (assess != null && assess.State != "T")
                    {
                        assess.State = "T";
                        assess.UpdatedAt = DateTime.Now;
                        _context.AssessDirections.Update(assess);
                    }
                }

                await _context.SaveChangesAsync();
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

        public async Task<ResponseContract<bool>> UpdateState(UpdateAssessDirectionDto updateAssessDirectionDto)
        {
            try
            {
                var existing = await _context.AssessDirections
                    .FirstOrDefaultAsync(x => x.AssessDirectionId == updateAssessDirectionDto.Id);

                if (existing == null)
                {
                    var message = $"No se encontró el registro con ID {updateAssessDirectionDto.Id}";
                    Console.WriteLine(message);
                    return ResponseContract<bool>.Fail(message);
                }

                existing.State = updateAssessDirectionDto.State;
                existing.UpdatedAt = DateTime.Now;

                await _context.SaveChangesAsync();

                return ResponseContract<bool>.Ok(true, "Estado actualizado correctamente");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en UpdateState: {ex.Message}");
                return ResponseContract<bool>.Fail($"Error al actualizar el estado: {ex.Message}");
            }
        }

        public async Task<bool> InsertDirectionTestStats(DirectionStatistics directionStatistics)
        {
            try
            {
                var statistics = new DirectionStatistics
                {
                    EffectivenessPercentage = directionStatistics.EffectivenessPercentage,
                    AccuracyPercentage = directionStatistics.AccuracyPercentage,
                    EffectiveThrow = directionStatistics.EffectiveThrow,
                    FailedThrow = directionStatistics.FailedThrow,
                    ShortThrow = directionStatistics.ShortThrow,
                    MediumThrow = directionStatistics.MediumThrow,
                    LongThrow = directionStatistics.LongThrow,
                    ShortEffectivenessPercentage = directionStatistics.ShortEffectivenessPercentage,
                    MediumEffectivenessPercentage = directionStatistics.MediumEffectivenessPercentage,
                    LongEffectivenessPercentage = directionStatistics.LongEffectivenessPercentage,
                    ShortThrowAccuracy = directionStatistics.ShortThrowAccuracy,
                    MediumThrowAccuracy = directionStatistics.MediumThrowAccuracy,
                    LongThrowAccuracy = directionStatistics.LongThrowAccuracy,
                    ShortAccuracyPercentage = directionStatistics.ShortAccuracyPercentage,
                    MediumAccuracyPercentage = directionStatistics.MediumAccuracyPercentage,
                    LongAccuracyPercentage = directionStatistics.LongAccuracyPercentage,
                    TotalDeviatedRight = directionStatistics.TotalDeviatedRight,
                    TotalDeviatedLeft = directionStatistics.TotalDeviatedLeft,
                    DeviatedRightPercentage = directionStatistics.DeviatedRightPercentage,
                    DeviatedLeftPercentage = directionStatistics.DeviatedLeftPercentage,
                    ShortDeviatedRight = directionStatistics.ShortDeviatedRight,
                    ShortDeviatedLeft = directionStatistics.ShortDeviatedLeft,
                    MediumDeviatedRight = directionStatistics.MediumDeviatedRight,
                    MediumDeviatedLeft = directionStatistics.MediumDeviatedLeft,
                    LongDeviatedRight = directionStatistics.LongDeviatedRight,
                    LongDeviatedLeft = directionStatistics.LongDeviatedLeft,
                    AssessDirectionId = directionStatistics.AssessDirectionId,
                    AthleteId = directionStatistics.AthleteId
                };

                await _context.DirectionStatistics.AddAsync(statistics);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error inserting DirectionStatistics: {e.Message}");
                return false;
            }
        }

        public async Task<List<EvaluationDetailDirection>> GetAllDetailsEvaluation(
            RequestAddDetailToDirectionEvaluation evaluationDetail)
        {
            try
            {
                var listEvaluationDetail = await _context.EvaluationDetailDirections
                    .Where(e => e.AssessDirectionId == evaluationDetail.AssessDirectionId
                                && e.AthleteId == evaluationDetail.AthleteId)
                    .ToListAsync();
                return listEvaluationDetail;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new List<EvaluationDetailDirection>();
            }
        }

        public async Task<int?> GetCoachIdByAssessmentAsync(int assessDirectionId)
        {
            try
            {
                var assessment = await _context.AssessDirections
                    .FirstOrDefaultAsync(a => a.AssessDirectionId == assessDirectionId);
                return assessment?.CoachId;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error obteniendo CoachId: {e.Message}");
                return null;
            }
        }

        public async Task<ActiveDirectionEvaluationDto?> GetActiveEvaluationWithDetailsAsync(int teamId, int coachId)
        {
            try
            {
                Console.WriteLine($"🔍 Buscando evaluación de dirección activa para el equipo: {teamId} y entrenador: {coachId}");

                var activeAssessment = await _context.AssessDirections
                    .Include(a => a.Team)
                    .Include(a => a.Coach)
                    .FirstOrDefaultAsync(a => a.TeamId == teamId && a.CoachId == coachId && a.State == "A");

                if (activeAssessment == null)
                {
                    Console.WriteLine($"❌ No se encontró evaluación de dirección activa para el equipo {teamId}");
                    return null;
                }

                Console.WriteLine($"✅ Evaluación de dirección activa encontrada - ID: {activeAssessment.AssessDirectionId}");

                // Obtener atletas participantes
                var athletesQuery = await _context.AthletesToEvaluatedDirection
                    .Where(ate => ate.AssessDirectionId == activeAssessment.AssessDirectionId)
                    .ToListAsync();

                var athletes = new List<AthleteInDirectionEvaluationDto>();
                foreach (var athlete in athletesQuery)
                {
                    var athleteUser = await _context.Users.FindAsync(athlete.AthleteId);
                    var coachUser = await _context.Users.FindAsync(athlete.CoachId);

                    if (athleteUser != null && coachUser != null)
                    {
                        athletes.Add(new AthleteInDirectionEvaluationDto
                        {
                            AthleteId = athlete.AthleteId,
                            AthleteName = $"{athleteUser.FirstName} {athleteUser.LastName}",
                            AthleteEmail = athleteUser.Email,
                            CoachId = athlete.CoachId,
                            CoachName = $"{coachUser.FirstName} {coachUser.LastName}"
                        });
                    }
                }

                // Obtener todos los lanzamientos de la evaluación
                var throwsQuery = await _context.EvaluationDetailDirections
                    .Where(eds => eds.AssessDirectionId == activeAssessment.AssessDirectionId)
                    .OrderBy(eds => eds.AthleteId)
                    .ThenBy(eds => eds.ThrowOrder)
                    .ToListAsync();

                var throws = new List<DirectionEvaluationThrowDto>();
                foreach (var throwDetail in throwsQuery)
                {
                    var athleteUser = await _context.Users.FindAsync(throwDetail.AthleteId);

                    throws.Add(new DirectionEvaluationThrowDto
                    {
                        EvaluationDetailDirectionId = throwDetail.EvaluationDetailDirectionId,
                        BoxNumber = throwDetail.BoxNumber,
                        ThrowOrder = throwDetail.ThrowOrder,
                        TargetDistance = throwDetail.TargetDistance,
                        ScoreObtained = throwDetail.ScoreObtained,
                        Observations = throwDetail.Observations,
                        Status = throwDetail.Status,
                        AthleteId = throwDetail.AthleteId,
                        AthleteName = athleteUser != null ? $"{athleteUser.FirstName} {athleteUser.LastName}" : "Atleta desconocido",
                        DeviatedRight = throwDetail.DeviatedRight,
                        DeviatedLeft = throwDetail.DeviatedLeft,
                        CreatedAt = throwDetail.CreatedAt,
                        UpdatedAt = throwDetail.UpdatedAt
                    });
                }

                var result = new ActiveDirectionEvaluationDto
                {
                    AssessDirectionId = activeAssessment.AssessDirectionId,
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

                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine($"❌ Error obteniendo evaluación de dirección activa: {e.Message}");
                Console.WriteLine($"❌ StackTrace: {e.StackTrace}");
                return null;
            }
        }

        public async Task<object> GetEvaluationDebugInfoAsync(int teamId)
        {
            try
            {
                var evaluations = await _context.AssessDirections
                    .Where(a => a.TeamId == teamId)
                    .Select(a => new
                    {
                        a.AssessDirectionId,
                        a.TeamId,
                        a.State,
                        a.EvaluationDate,
                        a.Description
                    })
                    .ToListAsync();

                var debugInfo = new List<object>();

                foreach (var eval in evaluations)
                {
                    var athletes = await _context.AthletesToEvaluatedDirection
                        .Where(ate => ate.AssessDirectionId == eval.AssessDirectionId)
                        .Select(ate => new
                        {
                            ate.AthleteId,
                            ate.CoachId,
                            AthleteName = ate.Athlete != null ? $"{ate.Athlete.FirstName} {ate.Athlete.LastName}" : "No cargado",
                            CoachName = ate.Coach != null ? $"{ate.Coach.FirstName} {ate.Coach.LastName}" : "No cargado"
                        })
                        .ToListAsync();

                    var throws = await _context.EvaluationDetailDirections
                        .Where(eds => eds.AssessDirectionId == eval.AssessDirectionId)
                        .Select(eds => new
                        {
                            eds.EvaluationDetailDirectionId,
                            eds.AthleteId,
                            eds.ThrowOrder,
                            eds.BoxNumber,
                            eds.TargetDistance,
                            eds.ScoreObtained,
                            eds.DeviatedRight,
                            eds.DeviatedLeft
                        })
                        .ToListAsync();

                    debugInfo.Add(new
                    {
                        Evaluation = eval,
                        AthletesCount = athletes.Count,
                        Athletes = athletes,
                        ThrowsCount = throws.Count,
                        Throws = throws.Take(5).ToList()
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
                return new { Err = e.Message, Stack = e.StackTrace };
            }
        }

        public async Task<List<DirectionEvaluationSummaryDto>> GetTeamEvaluationsAsync(int teamId)
        {
            try
            {
                var evaluations = await _context.AssessDirections
                    .Include(a => a.Team)
                    .Include(a => a.Coach)
                    .Where(a => a.TeamId == teamId)
                    .OrderByDescending(a => a.EvaluationDate)
                    .ToListAsync();

                var summaries = new List<DirectionEvaluationSummaryDto>();

                foreach (var eval in evaluations)
                {
                    var athletesCount = await _context.AthletesToEvaluatedDirection
                        .CountAsync(ate => ate.AssessDirectionId == eval.AssessDirectionId);

                    var throwsCount = await _context.EvaluationDetailDirections
                        .CountAsync(eds => eds.AssessDirectionId == eval.AssessDirectionId);

                    summaries.Add(new DirectionEvaluationSummaryDto
                    {
                        AssessDirectionId = eval.AssessDirectionId,
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
                Console.WriteLine($"Error obteniendo evaluaciones de dirección del equipo: {e.Message}");
                return new List<DirectionEvaluationSummaryDto>();
            }
        }

        public async Task<List<DirectionAthleteStatisticsDto>> GetEvaluationStatisticsAsync(int assessDirectionId)
        {
            try
            {
                var statistics = await _context.DirectionStatistics
                    .Include(s => s.Athlete)
                    .Where(s => s.AssessDirectionId == assessDirectionId)
                    .ToListAsync();

                return statistics.Select(s => new DirectionAthleteStatisticsDto
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
                    LongAccuracyPercentage = s.LongAccuracyPercentage,
                    TotalDeviatedRight = s.TotalDeviatedRight,
                    TotalDeviatedLeft = s.TotalDeviatedLeft,
                    DeviatedRightPercentage = s.DeviatedRightPercentage,
                    DeviatedLeftPercentage = s.DeviatedLeftPercentage,
                    ShortDeviatedRight = s.ShortDeviatedRight,
                    ShortDeviatedLeft = s.ShortDeviatedLeft,
                    MediumDeviatedRight = s.MediumDeviatedRight,
                    MediumDeviatedLeft = s.MediumDeviatedLeft,
                    LongDeviatedRight = s.LongDeviatedRight,
                    LongDeviatedLeft = s.LongDeviatedLeft
                }).ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error obteniendo estadísticas de evaluación de dirección: {e.Message}");
                return new List<DirectionAthleteStatisticsDto>();
            }
        }

        public async Task<DirectionEvaluationDetailsDto?> GetEvaluationDetailsAsync(int assessDirectionId)
        {
            try
            {
                var assessment = await _context.AssessDirections
                    .Include(a => a.Team)
                    .Include(a => a.Coach)
                    .FirstOrDefaultAsync(a => a.AssessDirectionId == assessDirectionId);

                if (assessment == null)
                {
                    return null;
                }

                // Obtener atletas participantes
                var athletesQuery = await _context.AthletesToEvaluatedDirection
                    .Where(ate => ate.AssessDirectionId == assessDirectionId)
                    .ToListAsync();

                var athletes = new List<AthleteInDirectionEvaluationDto>();
                foreach (var athlete in athletesQuery)
                {
                    var athleteUser = await _context.Users.FindAsync(athlete.AthleteId);
                    var coachUser = await _context.Users.FindAsync(athlete.CoachId);

                    if (athleteUser != null && coachUser != null)
                    {
                        athletes.Add(new AthleteInDirectionEvaluationDto
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
                var throwsQuery = await _context.EvaluationDetailDirections
                    .Where(eds => eds.AssessDirectionId == assessDirectionId)
                    .OrderBy(eds => eds.AthleteId)
                    .ThenBy(eds => eds.ThrowOrder)
                    .ToListAsync();

                var throws = new List<DirectionEvaluationThrowDto>();
                foreach (var throwDetail in throwsQuery)
                {
                    var athleteUser = await _context.Users.FindAsync(throwDetail.AthleteId);

                    throws.Add(new DirectionEvaluationThrowDto
                    {
                        EvaluationDetailDirectionId = throwDetail.EvaluationDetailDirectionId,
                        BoxNumber = throwDetail.BoxNumber,
                        ThrowOrder = throwDetail.ThrowOrder,
                        TargetDistance = throwDetail.TargetDistance,
                        ScoreObtained = throwDetail.ScoreObtained,
                        Observations = throwDetail.Observations,
                        Status = throwDetail.Status,
                        AthleteId = throwDetail.AthleteId,
                        AthleteName = athleteUser != null ? $"{athleteUser.FirstName} {athleteUser.LastName}" : "Atleta desconocido",
                        DeviatedRight = throwDetail.DeviatedRight,
                        DeviatedLeft = throwDetail.DeviatedLeft,
                        CreatedAt = throwDetail.CreatedAt,
                        UpdatedAt = throwDetail.UpdatedAt
                    });
                }

                // Obtener estadísticas si la evaluación está terminada
                List<DirectionAthleteStatisticsDto>? statistics = null;
                if (assessment.State == "T")
                {
                    statistics = await GetEvaluationStatisticsAsync(assessDirectionId);
                }

                return new DirectionEvaluationDetailsDto
                {
                    AssessDirectionId = assessment.AssessDirectionId,
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
                Console.WriteLine($"Error obteniendo detalles de evaluación de dirección: {e.Message}");
                return null;
            }
        }

        public async Task<ResponseContract<bool>> CancelAssessmentAsync(int assessDirectionId, int coachId, string? reason)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var assess = await _context.AssessDirections
                    .FirstOrDefaultAsync(a => a.AssessDirectionId == assessDirectionId);
                if (assess == null)
                {
                    await transaction.RollbackAsync();
                    return ResponseContract<bool>.Fail($"No se encontró la evaluación con ID {assessDirectionId}");
                }

                if (assess.State != "A")
                {
                    await transaction.RollbackAsync();
                    return ResponseContract<bool>.Fail("Sólo se puede cancelar una evaluación que esté en estado Activa (A)");
                }

                if (assess.CoachId != coachId)
                {
                    await transaction.RollbackAsync();
                    return ResponseContract<bool>.Fail("No autorizado: el coach no coincide con el creador de la evaluación");
                }

                // Eliminar detalles de lanzamientos asociados
                var details = _context.EvaluationDetailDirections.Where(d => d.AssessDirectionId == assessDirectionId);
                _context.EvaluationDetailDirections.RemoveRange(details);

                // Eliminar registros de atletas asignados a la evaluación
                var athletesToEval = _context.AthletesToEvaluatedDirection.Where(a => a.AssessDirectionId == assessDirectionId);
                _context.AthletesToEvaluatedDirection.RemoveRange(athletesToEval);

                // Eliminar estadísticas asociadas
                var stats = _context.DirectionStatistics.Where(s => s.AssessDirectionId == assessDirectionId);
                _context.DirectionStatistics.RemoveRange(stats);

                // Actualizar estado de la evaluación
                assess.State = "C";
                assess.UpdatedAt = DateTime.Now;
                _context.AssessDirections.Update(assess);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return ResponseContract<bool>.Ok(true, "Evaluación de dirección cancelada y datos asociados eliminados correctamente");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en CancelAssessmentAsync: {ex.Message}");
                try { await _context.Database.RollbackTransactionAsync(); } catch { /* ignore */ }
                return ResponseContract<bool>.Fail($"Error al cancelar la evaluación: {ex.Message}");
            }
        }
    }
}


