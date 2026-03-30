using BocciaCoaching.Models.DTO.General;
using BocciaCoaching.Models.DTO.Session;
using BocciaCoaching.Models.Entities;
using BocciaCoaching.Repositories.Interfaces.ITrainingSession;
using BocciaCoaching.Services.Interfaces;
using Microsoft.AspNetCore.Http;

namespace BocciaCoaching.Services
{
    public class TrainingSessionService : ITrainingSessionService
    {
        private readonly ITrainingSessionRepository _repository;
        private readonly IFileStorageService _fileStorage;

        private static readonly string[] ValidDays = { "lunes", "martes", "miercoles", "jueves", "viernes", "sabado", "domingo" };
        private static readonly string[] ValidStatuses = { "programada", "en_proceso", "terminada", "finalizada", "cancelada" };
        private static readonly string[] DefaultPartNames = { "Propulsion", "Saremas", "2x1", "Escenarios de juego" };

        public TrainingSessionService(ITrainingSessionRepository repository, IFileStorageService fileStorage)
        {
            _repository = repository;
            _fileStorage = fileStorage;
        }

        public async Task<ResponseContract<TrainingSessionResponseDto>> CreateSession(CreateTrainingSessionDto dto)
        {
            try
            {
                // Validate microcycle exists
                var microcycleExists = await _repository.MicrocycleExistsAsync(dto.MicrocycleId);
                if (!microcycleExists)
                    return ResponseContract<TrainingSessionResponseDto>.Fail("El microciclo no existe");

                // Validate day of week
                var dayLower = dto.DayOfWeek.ToLower().Trim();
                if (!ValidDays.Contains(dayLower))
                    return ResponseContract<TrainingSessionResponseDto>.Fail($"Día de la semana inválido. Valores permitidos: {string.Join(", ", ValidDays)}");

                // Validate throw percentage
                if (dto.ThrowPercentage < 0 || dto.ThrowPercentage > 100)
                    return ResponseContract<TrainingSessionResponseDto>.Fail("El porcentaje de lanzamientos debe estar entre 0 y 100");

                var session = new Models.Entities.TrainingSession
                {
                    MicrocycleId = dto.MicrocycleId,
                    DayOfWeek = dayLower,
                    Duration = dto.Duration,
                    Status = "programada",
                    StartTime = dto.StartTime,
                    EndTime = dto.EndTime,
                    ThrowPercentage = dto.ThrowPercentage,
                    TotalThrowsBase = dto.TotalThrowsBase > 0 ? dto.TotalThrowsBase : 1000,
                    CreatedAt = DateTime.Now
                };

                // Create the 4 default parts or use provided parts
                if (dto.Parts != null && dto.Parts.Any())
                {
                    foreach (var partDto in dto.Parts)
                    {
                        var part = new SessionPart
                        {
                            Name = partDto.Name,
                            Order = partDto.Order,
                            CreatedAt = DateTime.Now
                        };

                        if (partDto.Sections != null)
                        {
                            foreach (var secDto in partDto.Sections)
                            {
                                part.Sections.Add(new SessionSection
                                {
                                    Name = secDto.Name,
                                    NumberOfThrows = secDto.NumberOfThrows,
                                    Status = "pendiente",
                                    IsOwnDiagonal = secDto.IsOwnDiagonal,
                                    StartTime = secDto.StartTime,
                                    EndTime = secDto.EndTime,
                                    Observation = secDto.Observation,
                                    CreatedAt = DateTime.Now
                                });
                            }
                        }

                        session.Parts.Add(part);
                    }
                }
                else
                {
                    // Auto-create the 4 default parts
                    for (int i = 0; i < DefaultPartNames.Length; i++)
                    {
                        session.Parts.Add(new SessionPart
                        {
                            Name = DefaultPartNames[i],
                            Order = i + 1,
                            CreatedAt = DateTime.Now
                        });
                    }
                }

                var created = await _repository.CreateAsync(session);
                var response = MapToResponseDto(created);

                return ResponseContract<TrainingSessionResponseDto>.Ok(response, "Sesión de entrenamiento creada exitosamente");
            }
            catch (Exception ex)
            {
                return ResponseContract<TrainingSessionResponseDto>.Fail($"Error al crear la sesión: {ex.Message}");
            }
        }

        public async Task<ResponseContract<TrainingSessionResponseDto>> GetById(int sessionId)
        {
            try
            {
                var session = await _repository.GetByIdAsync(sessionId);
                if (session == null)
                    return ResponseContract<TrainingSessionResponseDto>.Fail("Sesión no encontrada");

                return ResponseContract<TrainingSessionResponseDto>.Ok(MapToResponseDto(session));
            }
            catch (Exception ex)
            {
                return ResponseContract<TrainingSessionResponseDto>.Fail($"Error al obtener la sesión: {ex.Message}");
            }
        }

        public async Task<ResponseContract<List<TrainingSessionSummaryDto>>> GetByMicrocycle(int microcycleId)
        {
            try
            {
                var microcycleExists = await _repository.MicrocycleExistsAsync(microcycleId);
                if (!microcycleExists)
                    return ResponseContract<List<TrainingSessionSummaryDto>>.Fail("El microciclo no existe");

                var sessions = await _repository.GetByMicrocycleAsync(microcycleId);
                var summaries = sessions.Select(s => new TrainingSessionSummaryDto
                {
                    TrainingSessionId = s.TrainingSessionId,
                    MicrocycleId = s.MicrocycleId,
                    DayOfWeek = s.DayOfWeek,
                    Duration = s.Duration,
                    Status = s.Status,
                    ThrowPercentage = s.ThrowPercentage,
                    MaxThrows = s.MaxThrows,
                    TotalParts = s.Parts.Count,
                    TotalSections = s.Parts.Sum(p => p.Sections.Count),
                    CreatedAt = s.CreatedAt
                }).ToList();

                return ResponseContract<List<TrainingSessionSummaryDto>>.Ok(summaries);
            }
            catch (Exception ex)
            {
                return ResponseContract<List<TrainingSessionSummaryDto>>.Fail($"Error al obtener sesiones: {ex.Message}");
            }
        }

        public async Task<ResponseContract<TrainingSessionResponseDto>> UpdateSession(UpdateTrainingSessionDto dto)
        {
            try
            {
                var session = await _repository.GetByIdAsync(dto.TrainingSessionId);
                if (session == null)
                    return ResponseContract<TrainingSessionResponseDto>.Fail("Sesión no encontrada");

                // Update fields if provided
                if (!string.IsNullOrEmpty(dto.Status))
                {
                    var statusLower = dto.Status.ToLower().Trim();
                    if (!ValidStatuses.Contains(statusLower))
                        return ResponseContract<TrainingSessionResponseDto>.Fail($"Estado inválido. Valores permitidos: {string.Join(", ", ValidStatuses)}");
                    session.Status = statusLower;
                }

                if (!string.IsNullOrEmpty(dto.DayOfWeek))
                {
                    var dayLower = dto.DayOfWeek.ToLower().Trim();
                    if (!ValidDays.Contains(dayLower))
                        return ResponseContract<TrainingSessionResponseDto>.Fail($"Día inválido. Valores permitidos: {string.Join(", ", ValidDays)}");
                    session.DayOfWeek = dayLower;
                }

                if (dto.Duration.HasValue) session.Duration = dto.Duration.Value;
                if (dto.StartTime.HasValue) session.StartTime = dto.StartTime.Value;
                if (dto.EndTime.HasValue) session.EndTime = dto.EndTime.Value;
                if (dto.ThrowPercentage.HasValue)
                {
                    if (dto.ThrowPercentage.Value < 0 || dto.ThrowPercentage.Value > 100)
                        return ResponseContract<TrainingSessionResponseDto>.Fail("El porcentaje de lanzamientos debe estar entre 0 y 100");
                    session.ThrowPercentage = dto.ThrowPercentage.Value;
                }
                if (dto.TotalThrowsBase.HasValue && dto.TotalThrowsBase.Value > 0)
                    session.TotalThrowsBase = dto.TotalThrowsBase.Value;

                var updated = await _repository.UpdateAsync(session);
                if (!updated)
                    return ResponseContract<TrainingSessionResponseDto>.Fail("Error al actualizar la sesión");

                // Re-fetch to get updated data
                var refreshed = await _repository.GetByIdAsync(dto.TrainingSessionId);
                return ResponseContract<TrainingSessionResponseDto>.Ok(MapToResponseDto(refreshed!), "Sesión actualizada exitosamente");
            }
            catch (Exception ex)
            {
                return ResponseContract<TrainingSessionResponseDto>.Fail($"Error al actualizar la sesión: {ex.Message}");
            }
        }

        public async Task<ResponseContract<bool>> DeleteSession(int sessionId)
        {
            try
            {
                var session = await _repository.GetByIdAsync(sessionId);
                if (session == null)
                    return ResponseContract<bool>.Fail("Sesión no encontrada");

                // Delete associated photos
                if (!string.IsNullOrEmpty(session.PhotoEvidence1))
                    await _fileStorage.DeleteFileAsync(session.PhotoEvidence1);
                if (!string.IsNullOrEmpty(session.PhotoEvidence2))
                    await _fileStorage.DeleteFileAsync(session.PhotoEvidence2);
                if (!string.IsNullOrEmpty(session.PhotoEvidence3))
                    await _fileStorage.DeleteFileAsync(session.PhotoEvidence3);
                if (!string.IsNullOrEmpty(session.PhotoEvidence4))
                    await _fileStorage.DeleteFileAsync(session.PhotoEvidence4);

                var deleted = await _repository.DeleteAsync(sessionId);
                if (!deleted)
                    return ResponseContract<bool>.Fail("Error al eliminar la sesión");

                return ResponseContract<bool>.Ok(true, "Sesión eliminada exitosamente");
            }
            catch (Exception ex)
            {
                return ResponseContract<bool>.Fail($"Error al eliminar la sesión: {ex.Message}");
            }
        }

        public async Task<ResponseContract<TrainingSessionResponseDto>> UploadPhoto(int sessionId, int photoNumber, IFormFile file)
        {
            try
            {
                if (photoNumber < 1 || photoNumber > 4)
                    return ResponseContract<TrainingSessionResponseDto>.Fail("El número de foto debe estar entre 1 y 4");

                var session = await _repository.GetByIdAsync(sessionId);
                if (session == null)
                    return ResponseContract<TrainingSessionResponseDto>.Fail("Sesión no encontrada");

                // Delete old photo if exists
                string? oldPath = photoNumber switch
                {
                    1 => session.PhotoEvidence1,
                    2 => session.PhotoEvidence2,
                    3 => session.PhotoEvidence3,
                    4 => session.PhotoEvidence4,
                    _ => null
                };

                if (!string.IsNullOrEmpty(oldPath))
                    await _fileStorage.DeleteFileAsync(oldPath);

                // Save new photo
                var relativePath = await _fileStorage.SaveFileAsync(file, $"sessions/{sessionId}");

                switch (photoNumber)
                {
                    case 1: session.PhotoEvidence1 = relativePath; break;
                    case 2: session.PhotoEvidence2 = relativePath; break;
                    case 3: session.PhotoEvidence3 = relativePath; break;
                    case 4: session.PhotoEvidence4 = relativePath; break;
                }

                var updated = await _repository.UpdateAsync(session);
                if (!updated)
                    return ResponseContract<TrainingSessionResponseDto>.Fail("Error al guardar la foto");

                var refreshed = await _repository.GetByIdAsync(sessionId);
                return ResponseContract<TrainingSessionResponseDto>.Ok(MapToResponseDto(refreshed!), $"Foto {photoNumber} subida exitosamente");
            }
            catch (Exception ex)
            {
                return ResponseContract<TrainingSessionResponseDto>.Fail($"Error al subir la foto: {ex.Message}");
            }
        }

        public async Task<ResponseContract<SessionSectionResponseDto>> AddSection(AddSessionSectionDto dto)
        {
            try
            {
                var part = await _repository.GetPartByIdAsync(dto.SessionPartId);
                if (part == null)
                    return ResponseContract<SessionSectionResponseDto>.Fail("Parte de sesión no encontrada");

                var section = new SessionSection
                {
                    SessionPartId = dto.SessionPartId,
                    Name = dto.Name,
                    NumberOfThrows = dto.NumberOfThrows,
                    Status = "pendiente",
                    IsOwnDiagonal = dto.IsOwnDiagonal,
                    StartTime = dto.StartTime,
                    EndTime = dto.EndTime,
                    Observation = dto.Observation,
                    CreatedAt = DateTime.Now
                };

                var created = await _repository.AddSectionAsync(section);
                return ResponseContract<SessionSectionResponseDto>.Ok(MapSectionToDto(created), "Sección agregada exitosamente");
            }
            catch (Exception ex)
            {
                return ResponseContract<SessionSectionResponseDto>.Fail($"Error al agregar sección: {ex.Message}");
            }
        }

        public async Task<ResponseContract<SessionSectionResponseDto>> UpdateSection(UpdateSessionSectionDto dto)
        {
            try
            {
                var section = await _repository.GetSectionByIdAsync(dto.SessionSectionId);
                if (section == null)
                    return ResponseContract<SessionSectionResponseDto>.Fail("Sección no encontrada");

                if (!string.IsNullOrEmpty(dto.Name)) section.Name = dto.Name;
                if (dto.NumberOfThrows.HasValue) section.NumberOfThrows = dto.NumberOfThrows.Value;
                if (!string.IsNullOrEmpty(dto.Status)) section.Status = dto.Status;
                if (dto.IsOwnDiagonal.HasValue) section.IsOwnDiagonal = dto.IsOwnDiagonal.Value;
                if (dto.StartTime.HasValue) section.StartTime = dto.StartTime.Value;
                if (dto.EndTime.HasValue) section.EndTime = dto.EndTime.Value;
                if (dto.Observation != null) section.Observation = dto.Observation;

                var updated = await _repository.UpdateSectionAsync(section);
                if (!updated)
                    return ResponseContract<SessionSectionResponseDto>.Fail("Error al actualizar la sección");

                var refreshed = await _repository.GetSectionByIdAsync(dto.SessionSectionId);
                return ResponseContract<SessionSectionResponseDto>.Ok(MapSectionToDto(refreshed!), "Sección actualizada exitosamente");
            }
            catch (Exception ex)
            {
                return ResponseContract<SessionSectionResponseDto>.Fail($"Error al actualizar la sección: {ex.Message}");
            }
        }

        public async Task<ResponseContract<bool>> DeleteSection(int sectionId)
        {
            try
            {
                var section = await _repository.GetSectionByIdAsync(sectionId);
                if (section == null)
                    return ResponseContract<bool>.Fail("Sección no encontrada");

                var deleted = await _repository.DeleteSectionAsync(sectionId);
                if (!deleted)
                    return ResponseContract<bool>.Fail("Error al eliminar la sección");

                return ResponseContract<bool>.Ok(true, "Sección eliminada exitosamente");
            }
            catch (Exception ex)
            {
                return ResponseContract<bool>.Fail($"Error al eliminar la sección: {ex.Message}");
            }
        }

        #region Private Helpers

        private static TrainingSessionResponseDto MapToResponseDto(Models.Entities.TrainingSession session)
        {
            return new TrainingSessionResponseDto
            {
                TrainingSessionId = session.TrainingSessionId,
                MicrocycleId = session.MicrocycleId,
                DayOfWeek = session.DayOfWeek,
                Duration = session.Duration,
                Status = session.Status,
                StartTime = session.StartTime,
                EndTime = session.EndTime,
                PhotoEvidence1 = session.PhotoEvidence1,
                PhotoEvidence2 = session.PhotoEvidence2,
                PhotoEvidence3 = session.PhotoEvidence3,
                PhotoEvidence4 = session.PhotoEvidence4,
                ThrowPercentage = session.ThrowPercentage,
                TotalThrowsBase = session.TotalThrowsBase,
                MaxThrows = session.MaxThrows,
                CreatedAt = session.CreatedAt,
                UpdatedAt = session.UpdatedAt,
                Parts = session.Parts.OrderBy(p => p.Order).Select(p => new SessionPartResponseDto
                {
                    SessionPartId = p.SessionPartId,
                    Name = p.Name,
                    Order = p.Order,
                    CreatedAt = p.CreatedAt,
                    Sections = p.Sections.Select(s => MapSectionToDto(s)).ToList()
                }).ToList()
            };
        }

        private static SessionSectionResponseDto MapSectionToDto(SessionSection section)
        {
            return new SessionSectionResponseDto
            {
                SessionSectionId = section.SessionSectionId,
                SessionPartId = section.SessionPartId,
                Name = section.Name,
                NumberOfThrows = section.NumberOfThrows,
                Status = section.Status,
                IsOwnDiagonal = section.IsOwnDiagonal,
                StartTime = section.StartTime,
                EndTime = section.EndTime,
                Observation = section.Observation,
                CreatedAt = section.CreatedAt,
                UpdatedAt = section.UpdatedAt
            };
        }

        #endregion

        #region Athlete Operations

        public async Task<ResponseContract<List<AthleteSessionSummaryDto>>> GetSessionsByAthleteInDateRange(GetAthleteSessionsDto dto)
        {
            try
            {
                if (dto.StartDate > dto.EndDate)
                    return ResponseContract<List<AthleteSessionSummaryDto>>.Fail("La fecha de inicio no puede ser mayor a la fecha de fin");

                var sessions = await _repository.GetByAthleteAndDateRangeAsync(dto.AthleteId, dto.StartDate, dto.EndDate);

                var summaries = sessions.Select(s => new AthleteSessionSummaryDto
                {
                    TrainingSessionId = s.TrainingSessionId,
                    MicrocycleId = s.MicrocycleId,
                    MacrocycleName = s.Microcycle?.Macrocycle?.Name ?? string.Empty,
                    MicrocycleNumber = s.Microcycle?.Number ?? 0,
                    MicrocycleStartDate = s.Microcycle?.StartDate ?? DateTime.MinValue,
                    MicrocycleEndDate = s.Microcycle?.EndDate ?? DateTime.MinValue,
                    MicrocycleType = s.Microcycle?.Type ?? string.Empty,
                    DayOfWeek = s.DayOfWeek,
                    Duration = s.Duration,
                    Status = s.Status,
                    StartTime = s.StartTime,
                    EndTime = s.EndTime,
                    ThrowPercentage = s.ThrowPercentage,
                    MaxThrows = s.MaxThrows,
                    TotalParts = s.Parts.Count,
                    TotalSections = s.Parts.Sum(p => p.Sections.Count),
                    CreatedAt = s.CreatedAt
                }).ToList();

                return ResponseContract<List<AthleteSessionSummaryDto>>.Ok(summaries, $"Se encontraron {summaries.Count} sesiones");
            }
            catch (Exception ex)
            {
                return ResponseContract<List<AthleteSessionSummaryDto>>.Fail($"Error al obtener sesiones del atleta: {ex.Message}");
            }
        }

        public async Task<ResponseContract<TrainingSessionResponseDto>> GetSessionDetailForAthlete(int sessionId, int athleteId)
        {
            try
            {
                // Validate that the session belongs to the athlete
                var belongsToAthlete = await _repository.SessionBelongsToAthleteAsync(sessionId, athleteId);
                if (!belongsToAthlete)
                    return ResponseContract<TrainingSessionResponseDto>.Fail("La sesión no existe o no pertenece al atleta");

                var session = await _repository.GetByIdAsync(sessionId);
                if (session == null)
                    return ResponseContract<TrainingSessionResponseDto>.Fail("Sesión no encontrada");

                return ResponseContract<TrainingSessionResponseDto>.Ok(MapToResponseDto(session));
            }
            catch (Exception ex)
            {
                return ResponseContract<TrainingSessionResponseDto>.Fail($"Error al obtener el detalle de la sesión: {ex.Message}");
            }
        }

        public async Task<ResponseContract<TrainingSessionResponseDto>> StartSession(AthleteUpdateSessionStatusDto dto)
        {
            try
            {
                // Validate that the session belongs to the athlete
                var belongsToAthlete = await _repository.SessionBelongsToAthleteAsync(dto.TrainingSessionId, dto.AthleteId);
                if (!belongsToAthlete)
                    return ResponseContract<TrainingSessionResponseDto>.Fail("La sesión no existe o no pertenece al atleta");

                var session = await _repository.GetByIdAsync(dto.TrainingSessionId);
                if (session == null)
                    return ResponseContract<TrainingSessionResponseDto>.Fail("Sesión no encontrada");

                // Validate current status - can only start a "programada" session
                if (session.Status != "programada")
                    return ResponseContract<TrainingSessionResponseDto>.Fail($"No se puede iniciar la sesión. Estado actual: '{session.Status}'. Solo se pueden iniciar sesiones con estado 'programada'");

                session.Status = "en_proceso";
                session.StartTime = DateTime.Now;

                var updated = await _repository.UpdateAsync(session);
                if (!updated)
                    return ResponseContract<TrainingSessionResponseDto>.Fail("Error al iniciar la sesión");

                var refreshed = await _repository.GetByIdAsync(dto.TrainingSessionId);
                return ResponseContract<TrainingSessionResponseDto>.Ok(MapToResponseDto(refreshed!), "Sesión iniciada exitosamente");
            }
            catch (Exception ex)
            {
                return ResponseContract<TrainingSessionResponseDto>.Fail($"Error al iniciar la sesión: {ex.Message}");
            }
        }

        public async Task<ResponseContract<TrainingSessionResponseDto>> FinishSession(AthleteUpdateSessionStatusDto dto)
        {
            try
            {
                // Validate that the session belongs to the athlete
                var belongsToAthlete = await _repository.SessionBelongsToAthleteAsync(dto.TrainingSessionId, dto.AthleteId);
                if (!belongsToAthlete)
                    return ResponseContract<TrainingSessionResponseDto>.Fail("La sesión no existe o no pertenece al atleta");

                var session = await _repository.GetByIdAsync(dto.TrainingSessionId);
                if (session == null)
                    return ResponseContract<TrainingSessionResponseDto>.Fail("Sesión no encontrada");

                // Validate current status - can only finish an "en_proceso" session
                if (session.Status != "en_proceso")
                    return ResponseContract<TrainingSessionResponseDto>.Fail($"No se puede finalizar la sesión. Estado actual: '{session.Status}'. Solo se pueden finalizar sesiones con estado 'en_proceso'");

                session.Status = "finalizada";
                session.EndTime = DateTime.Now;

                var updated = await _repository.UpdateAsync(session);
                if (!updated)
                    return ResponseContract<TrainingSessionResponseDto>.Fail("Error al finalizar la sesión");

                var refreshed = await _repository.GetByIdAsync(dto.TrainingSessionId);
                return ResponseContract<TrainingSessionResponseDto>.Ok(MapToResponseDto(refreshed!), "Sesión finalizada exitosamente");
            }
            catch (Exception ex)
            {
                return ResponseContract<TrainingSessionResponseDto>.Fail($"Error al finalizar la sesión: {ex.Message}");
            }
        }

        #endregion
    }
}

