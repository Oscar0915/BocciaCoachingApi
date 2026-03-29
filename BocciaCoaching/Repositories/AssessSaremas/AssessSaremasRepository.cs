using BocciaCoaching.Data;
using BocciaCoaching.Models.DTO.AssessSaremas;
using BocciaCoaching.Models.DTO.General;
using BocciaCoaching.Models.Entities;
using BocciaCoaching.Repositories.Interfaces.IAssessSaremas;
using Microsoft.EntityFrameworkCore;

namespace BocciaCoaching.Repositories.AssessSaremas
{
    public class AssessSaremasRepository : IAssessSaremasRepository
    {
        private readonly ApplicationDbContext _context;

        public AssessSaremasRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseContract<ResponseAddSaremasDto>> CreateEvaluationIfNoneActiveAsync(AddSaremasEvaluationDto dto)
        {
            try
            {
                await using var transaction = await _context.Database.BeginTransactionAsync();

                var existsActive = await _context.SaremasEvaluations
                    .AnyAsync(e => e.TeamId == dto.TeamId && e.CoachId == dto.CoachId && e.State == "Active");

                if (existsActive)
                {
                    await transaction.RollbackAsync();
                    return ResponseContract<ResponseAddSaremasDto>.Fail(
                        "Ya existe una evaluación SAREMAS+ activa para este equipo y coach. Finaliza o cancela antes de crear una nueva.");
                }

                var evaluation = new SaremasEvaluation
                {
                    Description = dto.Description,
                    TeamId = dto.TeamId,
                    CoachId = dto.CoachId,
                    EvaluationDate = DateTime.Now,
                    State = "Active",
                    CreatedAt = DateTime.Now
                };

                await _context.SaremasEvaluations.AddAsync(evaluation);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                var result = new ResponseAddSaremasDto
                {
                    SaremasEvaluationId = evaluation.SaremasEvaluationId,
                    Description = evaluation.Description,
                    TeamId = evaluation.TeamId,
                    CoachId = evaluation.CoachId,
                    EvaluationDate = evaluation.EvaluationDate,
                    State = evaluation.State
                };

                return ResponseContract<ResponseAddSaremasDto>.Ok(result, "Evaluación SAREMAS+ creada correctamente");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en CreateEvaluationIfNoneActiveAsync: {ex.Message}");
                return ResponseContract<ResponseAddSaremasDto>.Fail($"Error al crear la evaluación: {ex.Message}");
            }
        }

        public async Task<ResponseContract<SaremasAthleteEvaluation>> AddAthleteToEvaluationAsync(RequestAddAthleteToSaremasDto dto)
        {
            try
            {
                var athlete = await _context.Users.FindAsync(dto.AthleteId);
                var athleteName = athlete != null ? $"{athlete.FirstName} {athlete.LastName}" : "Desconocido";

                var entry = new SaremasAthleteEvaluation
                {
                    SaremasEvalId = dto.SaremasEvalId,
                    AthleteId = dto.AthleteId,
                    AthleteName = athleteName
                };

                await _context.SaremasAthleteEvaluations.AddAsync(entry);
                await _context.SaveChangesAsync();

                return ResponseContract<SaremasAthleteEvaluation>.Ok(entry, "Atleta agregado a la evaluación SAREMAS+");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en AddAthleteToEvaluationAsync: {ex.Message}");
                return ResponseContract<SaremasAthleteEvaluation>.Fail("Error al agregar atleta a la evaluación");
            }
        }

        public async Task<bool> AddThrowDetailAsync(RequestAddSaremasDetailDto dto, bool isUpdate)
        {
            try
            {
                await using var transaction = await _context.Database.BeginTransactionAsync();

                if (isUpdate)
                {
                    var existing = await _context.SaremasThrows
                        .FirstOrDefaultAsync(t =>
                            t.SaremasEvalId == dto.SaremasEvalId &&
                            t.AthleteId == dto.AthleteId &&
                            t.ThrowNumber == dto.ThrowNumber);

                    if (existing == null)
                    {
                        await transaction.RollbackAsync();
                        return false;
                    }

                    existing.Diagonal = dto.Diagonal;
                    existing.TechnicalComponent = dto.TechnicalComponent;
                    existing.ScoreObtained = dto.ScoreObtained;
                    existing.Observations = dto.Observations;
                    existing.FailureTags = dto.FailureTags;
                    existing.Status = dto.Status;
                    existing.WhiteBallX = dto.WhiteBallX;
                    existing.WhiteBallY = dto.WhiteBallY;
                    existing.ColorBallX = dto.ColorBallX;
                    existing.ColorBallY = dto.ColorBallY;
                    existing.EstimatedDistance = dto.EstimatedDistance;
                    existing.LaunchPointX = dto.LaunchPointX;
                    existing.LaunchPointY = dto.LaunchPointY;
                    existing.DistanceToLaunchPoint = dto.DistanceToLaunchPoint;
                    existing.Timestamp = DateTime.Now;

                    _context.SaremasThrows.Update(existing);
                }
                else
                {
                    var newThrow = new SaremasThrow
                    {
                        SaremasEvalId = dto.SaremasEvalId,
                        AthleteId = dto.AthleteId,
                        ThrowNumber = dto.ThrowNumber,
                        Diagonal = dto.Diagonal,
                        TechnicalComponent = dto.TechnicalComponent,
                        ScoreObtained = dto.ScoreObtained,
                        Observations = dto.Observations,
                        FailureTags = dto.FailureTags,
                        Status = dto.Status,
                        WhiteBallX = dto.WhiteBallX,
                        WhiteBallY = dto.WhiteBallY,
                        ColorBallX = dto.ColorBallX,
                        ColorBallY = dto.ColorBallY,
                        EstimatedDistance = dto.EstimatedDistance,
                        LaunchPointX = dto.LaunchPointX,
                        LaunchPointY = dto.LaunchPointY,
                        DistanceToLaunchPoint = dto.DistanceToLaunchPoint,
                        Timestamp = DateTime.Now
                    };

                    await _context.SaremasThrows.AddAsync(newThrow);
                }

                // Si es el tiro 28, marcar como completada automáticamente
                if (dto.ThrowNumber == 28)
                {
                    var evaluation = await _context.SaremasEvaluations
                        .FirstOrDefaultAsync(e => e.SaremasEvaluationId == dto.SaremasEvalId);
                    if (evaluation != null && evaluation.State == "Active")
                    {
                        evaluation.State = "Completed";
                        evaluation.UpdatedAt = DateTime.Now;
                        _context.SaremasEvaluations.Update(evaluation);
                    }
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en AddThrowDetailAsync: {ex.Message}");
                return false;
            }
        }

        public async Task<ActiveSaremasEvaluationDto?> GetActiveEvaluationAsync(int teamId, int coachId)
        {
            try
            {
                var evaluation = await _context.SaremasEvaluations
                    .Include(e => e.Team)
                    .Include(e => e.Coach)
                    .FirstOrDefaultAsync(e => e.TeamId == teamId && e.CoachId == coachId && e.State == "Active");

                if (evaluation == null) return null;

                var athletes = await _context.SaremasAthleteEvaluations
                    .Where(a => a.SaremasEvalId == evaluation.SaremasEvaluationId)
                    .ToListAsync();

                var athleteDtos = new List<SaremasAthleteInEvaluationDto>();
                foreach (var a in athletes)
                {
                    var user = await _context.Users.FindAsync(a.AthleteId);
                    athleteDtos.Add(new SaremasAthleteInEvaluationDto
                    {
                        AthleteId = a.AthleteId,
                        AthleteName = a.AthleteName ?? (user != null ? $"{user.FirstName} {user.LastName}" : ""),
                        AthleteEmail = user?.Email,
                        CoachId = evaluation.CoachId,
                        CoachName = evaluation.Coach != null ? $"{evaluation.Coach.FirstName} {evaluation.Coach.LastName}" : ""
                    });
                }

                var throws = await _context.SaremasThrows
                    .Where(t => t.SaremasEvalId == evaluation.SaremasEvaluationId)
                    .OrderBy(t => t.AthleteId)
                    .ThenBy(t => t.ThrowNumber)
                    .ToListAsync();

                var throwDtos = new List<SaremasThrowDto>();
                foreach (var t in throws)
                {
                    var user = await _context.Users.FindAsync(t.AthleteId);
                    throwDtos.Add(new SaremasThrowDto
                    {
                        SaremasThrowId = t.SaremasThrowId,
                        ThrowNumber = t.ThrowNumber,
                        Diagonal = t.Diagonal,
                        TechnicalComponent = t.TechnicalComponent,
                        ScoreObtained = t.ScoreObtained,
                        Observations = t.Observations,
                        FailureTags = t.FailureTags,
                        Status = t.Status,
                        AthleteId = t.AthleteId,
                        AthleteName = user != null ? $"{user.FirstName} {user.LastName}" : "",
                        WhiteBallX = t.WhiteBallX,
                        WhiteBallY = t.WhiteBallY,
                        ColorBallX = t.ColorBallX,
                        ColorBallY = t.ColorBallY,
                        EstimatedDistance = t.EstimatedDistance,
                        LaunchPointX = t.LaunchPointX,
                        LaunchPointY = t.LaunchPointY,
                        DistanceToLaunchPoint = t.DistanceToLaunchPoint,
                        Timestamp = t.Timestamp
                    });
                }

                return new ActiveSaremasEvaluationDto
                {
                    SaremasEvaluationId = evaluation.SaremasEvaluationId,
                    EvaluationDate = evaluation.EvaluationDate,
                    Description = evaluation.Description,
                    State = evaluation.State,
                    TeamId = evaluation.TeamId,
                    TeamName = evaluation.Team?.NameTeam,
                    CoachId = evaluation.CoachId,
                    CoachName = evaluation.Coach != null ? $"{evaluation.Coach.FirstName} {evaluation.Coach.LastName}" : "",
                    CreatedAt = evaluation.CreatedAt,
                    UpdatedAt = evaluation.UpdatedAt,
                    Athletes = athleteDtos,
                    Throws = throwDtos
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en GetActiveEvaluationAsync: {ex.Message}");
                return null;
            }
        }

        public async Task<ResponseContract<bool>> UpdateStateAsync(UpdateSaremasStateDto dto)
        {
            try
            {
                var existing = await _context.SaremasEvaluations
                    .FirstOrDefaultAsync(e => e.SaremasEvaluationId == dto.Id);

                if (existing == null)
                    return ResponseContract<bool>.Fail($"No se encontró la evaluación con ID {dto.Id}");

                existing.State = dto.State;
                existing.UpdatedAt = DateTime.Now;
                await _context.SaveChangesAsync();

                return ResponseContract<bool>.Ok(true, "Estado actualizado correctamente");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en UpdateStateAsync: {ex.Message}");
                return ResponseContract<bool>.Fail($"Error al actualizar el estado: {ex.Message}");
            }
        }

        public async Task<ResponseContract<bool>> CancelAsync(int saremasEvalId, int coachId, string? reason)
        {
            try
            {
                var evaluation = await _context.SaremasEvaluations
                    .FirstOrDefaultAsync(e => e.SaremasEvaluationId == saremasEvalId);

                if (evaluation == null)
                    return ResponseContract<bool>.Fail("Evaluación no encontrada");

                if (evaluation.CoachId != coachId)
                    return ResponseContract<bool>.Fail("Solo el coach creador puede cancelar la evaluación");

                if (evaluation.State == "Cancelled")
                    return ResponseContract<bool>.Fail("La evaluación ya está cancelada");

                evaluation.State = "Cancelled";
                evaluation.Description = string.IsNullOrEmpty(reason)
                    ? evaluation.Description
                    : $"{evaluation.Description} - Cancelada: {reason}";
                evaluation.UpdatedAt = DateTime.Now;

                await _context.SaveChangesAsync();
                return ResponseContract<bool>.Ok(true, "Evaluación cancelada correctamente");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en CancelAsync: {ex.Message}");
                return ResponseContract<bool>.Fail($"Error al cancelar: {ex.Message}");
            }
        }

        public async Task<List<SaremasEvaluationSummaryDto>> GetTeamEvaluationsAsync(int teamId)
        {
            try
            {
                var evaluations = await _context.SaremasEvaluations
                    .Include(e => e.Team)
                    .Include(e => e.Coach)
                    .Where(e => e.TeamId == teamId)
                    .OrderByDescending(e => e.EvaluationDate)
                    .ToListAsync();

                var result = new List<SaremasEvaluationSummaryDto>();
                foreach (var e in evaluations)
                {
                    var athleteCount = await _context.SaremasAthleteEvaluations
                        .CountAsync(a => a.SaremasEvalId == e.SaremasEvaluationId);
                    var throwCount = await _context.SaremasThrows
                        .CountAsync(t => t.SaremasEvalId == e.SaremasEvaluationId);

                    result.Add(new SaremasEvaluationSummaryDto
                    {
                        SaremasEvaluationId = e.SaremasEvaluationId,
                        EvaluationDate = e.EvaluationDate,
                        Description = e.Description,
                        State = e.State,
                        TeamId = e.TeamId,
                        TeamName = e.Team?.NameTeam,
                        CoachId = e.CoachId,
                        CoachName = e.Coach != null ? $"{e.Coach.FirstName} {e.Coach.LastName}" : "",
                        AthletesCount = athleteCount,
                        ThrowsCount = throwCount,
                        TotalScore = e.TotalScore,
                        AverageScore = e.AverageScore,
                        CreatedAt = e.CreatedAt,
                        UpdatedAt = e.UpdatedAt
                    });
                }

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en GetTeamEvaluationsAsync: {ex.Message}");
                return new List<SaremasEvaluationSummaryDto>();
            }
        }

        public async Task<SaremasEvaluationDetailsDto?> GetEvaluationDetailsAsync(int saremasEvalId)
        {
            try
            {
                var evaluation = await _context.SaremasEvaluations
                    .Include(e => e.Team)
                    .Include(e => e.Coach)
                    .FirstOrDefaultAsync(e => e.SaremasEvaluationId == saremasEvalId);

                if (evaluation == null) return null;

                var athletes = await _context.SaremasAthleteEvaluations
                    .Where(a => a.SaremasEvalId == saremasEvalId)
                    .ToListAsync();

                var athleteDtos = new List<SaremasAthleteInEvaluationDto>();
                foreach (var a in athletes)
                {
                    var user = await _context.Users.FindAsync(a.AthleteId);
                    athleteDtos.Add(new SaremasAthleteInEvaluationDto
                    {
                        AthleteId = a.AthleteId,
                        AthleteName = a.AthleteName,
                        AthleteEmail = user?.Email,
                        CoachId = evaluation.CoachId,
                        CoachName = evaluation.Coach != null ? $"{evaluation.Coach.FirstName} {evaluation.Coach.LastName}" : ""
                    });
                }

                var throws = await _context.SaremasThrows
                    .Where(t => t.SaremasEvalId == saremasEvalId)
                    .OrderBy(t => t.AthleteId).ThenBy(t => t.ThrowNumber)
                    .ToListAsync();

                var throwDtos = new List<SaremasThrowDto>();
                foreach (var t in throws)
                {
                    var user = await _context.Users.FindAsync(t.AthleteId);
                    throwDtos.Add(new SaremasThrowDto
                    {
                        SaremasThrowId = t.SaremasThrowId,
                        ThrowNumber = t.ThrowNumber,
                        Diagonal = t.Diagonal,
                        TechnicalComponent = t.TechnicalComponent,
                        ScoreObtained = t.ScoreObtained,
                        Observations = t.Observations,
                        FailureTags = t.FailureTags,
                        Status = t.Status,
                        AthleteId = t.AthleteId,
                        AthleteName = user != null ? $"{user.FirstName} {user.LastName}" : "",
                        WhiteBallX = t.WhiteBallX,
                        WhiteBallY = t.WhiteBallY,
                        ColorBallX = t.ColorBallX,
                        ColorBallY = t.ColorBallY,
                        EstimatedDistance = t.EstimatedDistance,
                        LaunchPointX = t.LaunchPointX,
                        LaunchPointY = t.LaunchPointY,
                        DistanceToLaunchPoint = t.DistanceToLaunchPoint,
                        Timestamp = t.Timestamp
                    });
                }

                return new SaremasEvaluationDetailsDto
                {
                    SaremasEvaluationId = evaluation.SaremasEvaluationId,
                    EvaluationDate = evaluation.EvaluationDate,
                    Description = evaluation.Description,
                    State = evaluation.State,
                    TeamId = evaluation.TeamId,
                    TeamName = evaluation.Team?.NameTeam,
                    CoachId = evaluation.CoachId,
                    CoachName = evaluation.Coach != null ? $"{evaluation.Coach.FirstName} {evaluation.Coach.LastName}" : "",
                    TotalScore = evaluation.TotalScore,
                    AverageScore = evaluation.AverageScore,
                    CreatedAt = evaluation.CreatedAt,
                    UpdatedAt = evaluation.UpdatedAt,
                    Athletes = athleteDtos,
                    Throws = throwDtos
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en GetEvaluationDetailsAsync: {ex.Message}");
                return null;
            }
        }

        public async Task<SaremasStatisticsDto?> GetEvaluationStatisticsAsync(int saremasEvalId)
        {
            try
            {
                var throws = await _context.SaremasThrows
                    .Where(t => t.SaremasEvalId == saremasEvalId && t.Status)
                    .ToListAsync();

                if (!throws.Any()) return null;

                var totalScore = throws.Sum(t => t.ScoreObtained);
                var averageScore = throws.Average(t => (double)t.ScoreObtained);

                // Score by diagonal
                var scoreByDiagonal = throws
                    .GroupBy(t => t.Diagonal)
                    .ToDictionary(
                        g => g.Key,
                        g => new DiagonalStatsDto
                        {
                            Total = g.Sum(t => t.ScoreObtained),
                            Average = g.Average(t => (double)t.ScoreObtained),
                            Count = g.Count()
                        });

                // Score by component
                var scoreByComponent = throws
                    .GroupBy(t => t.TechnicalComponent)
                    .ToDictionary(
                        g => g.Key,
                        g => new ComponentStatsDto
                        {
                            Total = g.Sum(t => t.ScoreObtained),
                            Average = g.Average(t => (double)t.ScoreObtained),
                            Count = g.Count()
                        });

                // Score by block (1-7=block1, 8-14=block2, 15-21=block3, 22-28=block4)
                var scoreByBlock = new Dictionary<string, BlockStatsDto>();
                for (int block = 1; block <= 4; block++)
                {
                    int startThrow = (block - 1) * 7 + 1;
                    int endThrow = block * 7;
                    var blockThrows = throws.Where(t => t.ThrowNumber >= startThrow && t.ThrowNumber <= endThrow).ToList();
                    if (blockThrows.Any())
                    {
                        scoreByBlock[block.ToString()] = new BlockStatsDto
                        {
                            Total = blockThrows.Sum(t => t.ScoreObtained),
                            Average = blockThrows.Average(t => (double)t.ScoreObtained)
                        };
                    }
                }

                // Failure tag frequency
                var failureTagFrequency = new Dictionary<string, int>
                {
                    { "Fuerza", 0 }, { "Dirección", 0 }, { "Cadencia", 0 }, { "Trayectoria", 0 }
                };
                foreach (var t in throws.Where(t => !string.IsNullOrEmpty(t.FailureTags)))
                {
                    var tags = t.FailureTags!.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
                    foreach (var tag in tags)
                    {
                        if (failureTagFrequency.ContainsKey(tag))
                            failureTagFrequency[tag]++;
                        else
                            failureTagFrequency[tag] = 1;
                    }
                }

                // Salida metrics
                var salidaThrows = throws
                    .Where(t => t.TechnicalComponent == "Salida" && t.EstimatedDistance.HasValue)
                    .ToList();

                SalidaMetricsDto? salidaMetrics = null;
                if (salidaThrows.Any())
                {
                    salidaMetrics = new SalidaMetricsDto
                    {
                        AverageDistance = salidaThrows.Average(t => t.EstimatedDistance!.Value),
                        AverageLaunchDistance = salidaThrows
                            .Where(t => t.DistanceToLaunchPoint.HasValue)
                            .DefaultIfEmpty()
                            .Average(t => t?.DistanceToLaunchPoint ?? 0),
                        ThrowsWithCourtData = salidaThrows.Count
                    };
                }

                return new SaremasStatisticsDto
                {
                    EvaluationId = saremasEvalId,
                    TotalScore = totalScore,
                    MaxPossibleScore = 140,
                    AverageScore = Math.Round(averageScore, 2),
                    ThrowsCompleted = throws.Count,
                    ScoreByDiagonal = scoreByDiagonal,
                    ScoreByComponent = scoreByComponent,
                    ScoreByBlock = scoreByBlock,
                    FailureTagFrequency = failureTagFrequency,
                    SalidaMetrics = salidaMetrics
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en GetEvaluationStatisticsAsync: {ex.Message}");
                return null;
            }
        }

        public async Task<SaremasAthleteHistoryDto?> GetAthleteHistoryAsync(int athleteId)
        {
            try
            {
                var athlete = await _context.Users.FindAsync(athleteId);
                if (athlete == null) return null;

                var evalIds = await _context.SaremasAthleteEvaluations
                    .Where(a => a.AthleteId == athleteId)
                    .Select(a => a.SaremasEvalId)
                    .ToListAsync();

                var evaluations = await _context.SaremasEvaluations
                    .Include(e => e.Team)
                    .Include(e => e.Coach)
                    .Where(e => evalIds.Contains(e.SaremasEvaluationId))
                    .OrderByDescending(e => e.EvaluationDate)
                    .ToListAsync();

                var items = new List<SaremasHistoryItemDto>();
                foreach (var e in evaluations)
                {
                    var throwCount = await _context.SaremasThrows
                        .CountAsync(t => t.SaremasEvalId == e.SaremasEvaluationId && t.AthleteId == athleteId);

                    items.Add(new SaremasHistoryItemDto
                    {
                        SaremasEvaluationId = e.SaremasEvaluationId,
                        EvaluationDate = e.EvaluationDate,
                        Description = e.Description,
                        State = e.State,
                        TotalScore = e.TotalScore,
                        AverageScore = e.AverageScore,
                        ThrowsCompleted = throwCount,
                        TeamName = e.Team?.NameTeam,
                        CoachName = e.Coach != null ? $"{e.Coach.FirstName} {e.Coach.LastName}" : ""
                    });
                }

                return new SaremasAthleteHistoryDto
                {
                    AthleteId = athleteId,
                    AthleteName = $"{athlete.FirstName} {athlete.LastName}",
                    Evaluations = items
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en GetAthleteHistoryAsync: {ex.Message}");
                return null;
            }
        }

        public async Task<List<SaremasThrow>> GetAllThrowsForAthleteAsync(int saremasEvalId, int athleteId)
        {
            return await _context.SaremasThrows
                .Where(t => t.SaremasEvalId == saremasEvalId && t.AthleteId == athleteId)
                .OrderBy(t => t.ThrowNumber)
                .ToListAsync();
        }

        public async Task<int?> GetCoachIdByEvaluationAsync(int saremasEvalId)
        {
            var evaluation = await _context.SaremasEvaluations
                .FirstOrDefaultAsync(e => e.SaremasEvaluationId == saremasEvalId);
            return evaluation?.CoachId;
        }

        public async Task<bool> UpdateEvaluationScoresAsync(int saremasEvalId, int totalScore, double averageScore)
        {
            try
            {
                var evaluation = await _context.SaremasEvaluations
                    .FirstOrDefaultAsync(e => e.SaremasEvaluationId == saremasEvalId);
                if (evaluation == null) return false;

                evaluation.TotalScore = totalScore;
                evaluation.AverageScore = averageScore;
                evaluation.UpdatedAt = DateTime.Now;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en UpdateEvaluationScoresAsync: {ex.Message}");
                return false;
            }
        }
    }
}

