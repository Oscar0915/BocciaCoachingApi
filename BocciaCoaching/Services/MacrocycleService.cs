using System.Text.Json;
using BocciaCoaching.Models.DTO.General;
using BocciaCoaching.Models.DTO.Macrocycle;
using BocciaCoaching.Models.Entities;
using BocciaCoaching.Repositories.Interfaces.IMacrocycle;
using BocciaCoaching.Repositories.Interfaces.ITeams;
using BocciaCoaching.Services.Interfaces;

namespace BocciaCoaching.Services
{
    public class MacrocycleService : IMacrocycleService
    {
        private readonly IMacrocycleRepository _repository;
        private readonly ITeamValidationRepository _teamValidation;

        public MacrocycleService(IMacrocycleRepository repository, ITeamValidationRepository teamValidation)
        {
            _repository = repository;
            _teamValidation = teamValidation;
        }

        public async Task<ResponseContract<MacrocycleResponseDto>> CreateMacrocycle(CreateMacrocycleDto dto)
        {
            try
            {
                var isValidTeam = await _teamValidation.ValidateTeam(new Team { TeamId = dto.TeamId });
                if (!isValidTeam)
                    return ResponseContract<MacrocycleResponseDto>.Fail("El equipo no está activo");

                // Validar fechas
                if (dto.EndDate <= dto.StartDate)
                    return ResponseContract<MacrocycleResponseDto>.Fail("La fecha de fin debe ser posterior a la de inicio");

                // Validar no solapamiento
                var noOverlap = await _repository.ValidateNoOverlapAsync(dto.AthleteId, dto.StartDate, dto.EndDate);
                if (!noOverlap)
                    return ResponseContract<MacrocycleResponseDto>.Fail("Ya existe un macrociclo que se solapa con las fechas indicadas para este atleta");

                // Normalizar startDate al lunes más reciente
                var normalizedStart = NormalizeToMonday(dto.StartDate);

                var macrocycle = new Macrocycle
                {
                    MacrocycleId = Guid.NewGuid().ToString(),
                    AthleteId = dto.AthleteId,
                    AthleteName = dto.AthleteName,
                    Name = dto.Name,
                    StartDate = normalizedStart,
                    EndDate = dto.EndDate,
                    Notes = dto.Notes,
                    CoachId = dto.CoachId,
                    TeamId = dto.TeamId,
                    CreatedAt = DateTime.Now
                };

                // Crear eventos
                foreach (var evtDto in dto.Events)
                {
                    macrocycle.Events.Add(new MacrocycleEvent
                    {
                        MacrocycleEventId = Guid.NewGuid().ToString(),
                        MacrocycleId = macrocycle.MacrocycleId,
                        Name = evtDto.Name,
                        Type = evtDto.Type,
                        StartDate = evtDto.StartDate,
                        EndDate = evtDto.EndDate,
                        Location = evtDto.Location,
                        Notes = evtDto.Notes
                    });
                }

                // Calcular estructura
                var microcycles = BuildMicrocycles(macrocycle.MacrocycleId, normalizedStart, dto.EndDate, macrocycle.Events.ToList());
                var periods = BuildPeriods(macrocycle.MacrocycleId, normalizedStart, dto.EndDate, macrocycle.Events.ToList(), microcycles);
                var mesocycles = BuildMesocycles(macrocycle.MacrocycleId, microcycles, periods);

                // Asignar período y mesociclo a cada microciclo
                foreach (var micro in microcycles)
                {
                    var period = periods.FirstOrDefault(p => micro.StartDate >= p.StartDate && micro.StartDate < p.EndDate);
                    if (period != null) micro.PeriodName = period.Name;

                    var meso = mesocycles.FirstOrDefault(m => micro.StartDate >= m.StartDate && micro.StartDate < m.EndDate);
                    if (meso != null) micro.MesocycleName = meso.Name;
                }

                // Persistir
                var created = await _repository.CreateAsync(macrocycle);
                await _repository.AddPeriodsAsync(periods);
                await _repository.AddMesocyclesAsync(mesocycles);
                await _repository.AddMicrocyclesAsync(microcycles);

                // Recargar completo
                var full = await _repository.GetByIdAsync(created.MacrocycleId);
                return ResponseContract<MacrocycleResponseDto>.Ok(MapToResponse(full!), "Macrociclo creado correctamente");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return ResponseContract<MacrocycleResponseDto>.Fail($"Error al crear el macrociclo: {e.Message}");
            }
        }

        public async Task<ResponseContract<List<MacrocycleSummaryDto>>> GetByAthlete(int athleteId)
        {
            try
            {
                var result = await _repository.GetByAthleteAsync(athleteId);
                return ResponseContract<List<MacrocycleSummaryDto>>.Ok(result, $"Se encontraron {result.Count} macrociclos");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return ResponseContract<List<MacrocycleSummaryDto>>.Fail($"Error: {e.Message}");
            }
        }

        public async Task<ResponseContract<List<MacrocycleSummaryDto>>> GetByTeam(int teamId)
        {
            try
            {
                var result = await _repository.GetByTeamAsync(teamId);
                return ResponseContract<List<MacrocycleSummaryDto>>.Ok(result, $"Se encontraron {result.Count} macrociclos");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return ResponseContract<List<MacrocycleSummaryDto>>.Fail($"Error: {e.Message}");
            }
        }

        public async Task<ResponseContract<MacrocycleResponseDto>> GetById(string macrocycleId)
        {
            try
            {
                var macrocycle = await _repository.GetByIdAsync(macrocycleId);
                if (macrocycle == null)
                    return ResponseContract<MacrocycleResponseDto>.Fail("Macrociclo no encontrado");

                return ResponseContract<MacrocycleResponseDto>.Ok(MapToResponse(macrocycle), "Macrociclo encontrado");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return ResponseContract<MacrocycleResponseDto>.Fail($"Error: {e.Message}");
            }
        }

        public async Task<ResponseContract<MacrocycleResponseDto>> UpdateMacrocycle(UpdateMacrocycleDto dto)
        {
            try
            {
                var existing = await _repository.GetByIdAsync(dto.MacrocycleId);
                if (existing == null)
                    return ResponseContract<MacrocycleResponseDto>.Fail("Macrociclo no encontrado");

                if (dto.EndDate <= dto.StartDate)
                    return ResponseContract<MacrocycleResponseDto>.Fail("La fecha de fin debe ser posterior a la de inicio");

                var noOverlap = await _repository.ValidateNoOverlapAsync(dto.AthleteId, dto.StartDate, dto.EndDate, dto.MacrocycleId);
                if (!noOverlap)
                    return ResponseContract<MacrocycleResponseDto>.Fail("Ya existe un macrociclo que se solapa con las fechas indicadas");

                var normalizedStart = NormalizeToMonday(dto.StartDate);

                existing.AthleteId = dto.AthleteId;
                existing.AthleteName = dto.AthleteName;
                existing.Name = dto.Name;
                existing.StartDate = normalizedStart;
                existing.EndDate = dto.EndDate;
                existing.Notes = dto.Notes;
                existing.CoachId = dto.CoachId;
                existing.TeamId = dto.TeamId;
                existing.UpdatedAt = DateTime.Now;

                await _repository.UpdateAsync(existing);

                // Recalcular estructura
                await RecalculateStructure(existing.MacrocycleId);

                var full = await _repository.GetByIdAsync(existing.MacrocycleId);
                return ResponseContract<MacrocycleResponseDto>.Ok(MapToResponse(full!), "Macrociclo actualizado correctamente");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return ResponseContract<MacrocycleResponseDto>.Fail($"Error al actualizar: {e.Message}");
            }
        }

        public async Task<ResponseContract<bool>> DeleteMacrocycle(string macrocycleId)
        {
            try
            {
                var result = await _repository.DeleteAsync(macrocycleId);
                if (!result)
                    return ResponseContract<bool>.Fail("Macrociclo no encontrado");

                return ResponseContract<bool>.Ok(true, "Macrociclo eliminado correctamente");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return ResponseContract<bool>.Fail($"Error al eliminar: {e.Message}");
            }
        }

        public async Task<ResponseContract<MacrocycleResponseDto>> AddEvent(AddMacrocycleEventDto dto)
        {
            try
            {
                var macrocycle = await _repository.GetByIdAsync(dto.MacrocycleId);
                if (macrocycle == null)
                    return ResponseContract<MacrocycleResponseDto>.Fail("Macrociclo no encontrado");

                var evt = new MacrocycleEvent
                {
                    MacrocycleEventId = Guid.NewGuid().ToString(),
                    MacrocycleId = dto.MacrocycleId,
                    Name = dto.Name,
                    Type = dto.Type,
                    StartDate = dto.StartDate,
                    EndDate = dto.EndDate,
                    Location = dto.Location,
                    Notes = dto.Notes
                };

                await _repository.AddEventAsync(evt);
                await RecalculateStructure(dto.MacrocycleId);

                var full = await _repository.GetByIdAsync(dto.MacrocycleId);
                return ResponseContract<MacrocycleResponseDto>.Ok(MapToResponse(full!), "Evento agregado y estructura recalculada");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return ResponseContract<MacrocycleResponseDto>.Fail($"Error al agregar evento: {e.Message}");
            }
        }

        public async Task<ResponseContract<MacrocycleResponseDto>> UpdateEvent(UpdateMacrocycleEventDto dto)
        {
            try
            {
                var evt = await _repository.GetEventByIdAsync(dto.MacrocycleEventId);
                if (evt == null)
                    return ResponseContract<MacrocycleResponseDto>.Fail("Evento no encontrado");

                evt.Name = dto.Name;
                evt.Type = dto.Type;
                evt.StartDate = dto.StartDate;
                evt.EndDate = dto.EndDate;
                evt.Location = dto.Location;
                evt.Notes = dto.Notes;

                await _repository.UpdateEventAsync(evt);
                await RecalculateStructure(evt.MacrocycleId);

                var full = await _repository.GetByIdAsync(evt.MacrocycleId);
                return ResponseContract<MacrocycleResponseDto>.Ok(MapToResponse(full!), "Evento actualizado y estructura recalculada");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return ResponseContract<MacrocycleResponseDto>.Fail($"Error al actualizar evento: {e.Message}");
            }
        }

        public async Task<ResponseContract<MacrocycleResponseDto>> DeleteEvent(string eventId)
        {
            try
            {
                var evt = await _repository.GetEventByIdAsync(eventId);
                if (evt == null)
                    return ResponseContract<MacrocycleResponseDto>.Fail("Evento no encontrado");

                var macrocycleId = evt.MacrocycleId;
                await _repository.DeleteEventAsync(eventId);
                await RecalculateStructure(macrocycleId);

                var full = await _repository.GetByIdAsync(macrocycleId);
                return ResponseContract<MacrocycleResponseDto>.Ok(MapToResponse(full!), "Evento eliminado y estructura recalculada");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return ResponseContract<MacrocycleResponseDto>.Fail($"Error al eliminar evento: {e.Message}");
            }
        }

        public async Task<ResponseContract<bool>> UpdateMicrocycle(UpdateMicrocycleDto dto)
        {
            try
            {
                var micro = await _repository.GetMicrocycleByIdAsync(dto.MicrocycleId);
                if (micro == null)
                    return ResponseContract<bool>.Fail("Microciclo no encontrado");

                if (dto.Type != null) micro.Type = dto.Type;
                if (dto.HasPeakPerformance.HasValue) micro.HasPeakPerformance = dto.HasPeakPerformance.Value;
                if (dto.TrainingDistribution != null)
                {
                    // Validar suma ≈ 1.0
                    var sum = dto.TrainingDistribution.FisicaGeneral + dto.TrainingDistribution.FisicaEspecial +
                              dto.TrainingDistribution.Tecnica + dto.TrainingDistribution.Tactica +
                              dto.TrainingDistribution.Teorica + dto.TrainingDistribution.Psicologica;
                    if (Math.Abs(sum - 1.0) > 0.01)
                        return ResponseContract<bool>.Fail("La suma de la distribución de entrenamiento debe ser 1.0 (±0.01)");

                    micro.TrainingDistribution = JsonSerializer.Serialize(dto.TrainingDistribution);
                }

                await _repository.UpdateMicrocycleAsync(micro);
                return ResponseContract<bool>.Ok(true, "Microciclo actualizado correctamente");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return ResponseContract<bool>.Fail($"Error al actualizar microciclo: {e.Message}");
            }
        }

        public async Task<ResponseContract<List<MacrocycleSummaryDto>>> GetCoachMacrocycles(int coachId)
        {
            try
            {
                var result = await _repository.GetCoachMacrocyclesAsync(coachId);
                return ResponseContract<List<MacrocycleSummaryDto>>.Ok(result, $"Se encontraron {result.Count} macrociclos");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return ResponseContract<List<MacrocycleSummaryDto>>.Fail($"Error: {e.Message}");
            }
        }

        public async Task<ResponseContract<MacrocycleResponseDto>> DuplicateMacrocycle(string macrocycleId, DuplicateMacrocycleDto dto)
        {
            try
            {
                var source = await _repository.GetByIdAsync(macrocycleId);
                if (source == null)
                    return ResponseContract<MacrocycleResponseDto>.Fail("Macrociclo origen no encontrado");

                var daysDiff = dto.NewStartDate.HasValue
                    ? (dto.NewStartDate.Value - source.StartDate).Days
                    : 0;

                var newStart = NormalizeToMonday(dto.NewStartDate ?? source.StartDate);
                var newEnd = source.EndDate.AddDays(daysDiff);

                var noOverlap = await _repository.ValidateNoOverlapAsync(dto.NewAthleteId, newStart, newEnd);
                if (!noOverlap)
                    return ResponseContract<MacrocycleResponseDto>.Fail("Ya existe un macrociclo que se solapa con las fechas para este atleta");

                var createDto = new CreateMacrocycleDto
                {
                    AthleteId = dto.NewAthleteId,
                    AthleteName = dto.NewAthleteName,
                    Name = $"{source.Name} (copia)",
                    StartDate = newStart,
                    EndDate = newEnd,
                    Notes = source.Notes,
                    CoachId = source.CoachId,
                    TeamId = source.TeamId,
                    Events = source.Events.Select(e => new CreateMacrocycleEventDto
                    {
                        Name = e.Name,
                        Type = e.Type,
                        StartDate = e.StartDate.AddDays(daysDiff),
                        EndDate = e.EndDate.AddDays(daysDiff),
                        Location = e.Location,
                        Notes = e.Notes
                    }).ToList()
                };

                return await CreateMacrocycle(createDto);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return ResponseContract<MacrocycleResponseDto>.Fail($"Error al duplicar: {e.Message}");
            }
        }

        // ===================== CALCULATION LOGIC =====================

        private async Task RecalculateStructure(string macrocycleId)
        {
            var macrocycle = await _repository.GetByIdAsync(macrocycleId);
            if (macrocycle == null) return;

            // Delete old calculated data
            await _repository.DeletePeriodsAsync(macrocycleId);
            await _repository.DeleteMesocyclesAsync(macrocycleId);
            await _repository.DeleteMicrocyclesAsync(macrocycleId);

            var events = macrocycle.Events.ToList();
            var microcycles = BuildMicrocycles(macrocycleId, macrocycle.StartDate, macrocycle.EndDate, events);
            var periods = BuildPeriods(macrocycleId, macrocycle.StartDate, macrocycle.EndDate, events, microcycles);
            var mesocycles = BuildMesocycles(macrocycleId, microcycles, periods);

            foreach (var micro in microcycles)
            {
                var period = periods.FirstOrDefault(p => micro.StartDate >= p.StartDate && micro.StartDate < p.EndDate);
                if (period != null) micro.PeriodName = period.Name;

                var meso = mesocycles.FirstOrDefault(m => micro.StartDate >= m.StartDate && micro.StartDate < m.EndDate);
                if (meso != null) micro.MesocycleName = meso.Name;
            }

            await _repository.AddPeriodsAsync(periods);
            await _repository.AddMesocyclesAsync(mesocycles);
            await _repository.AddMicrocyclesAsync(microcycles);
        }

        private static DateTime NormalizeToMonday(DateTime date)
        {
            var daysToMonday = ((int)date.DayOfWeek - 1 + 7) % 7;
            return date.AddDays(-daysToMonday).Date;
        }

        private static List<Microcycle> BuildMicrocycles(string macrocycleId, DateTime start, DateTime end, List<MacrocycleEvent> events)
        {
            var microcycles = new List<Microcycle>();
            var normalizedStart = NormalizeToMonday(start);
            var current = normalizedStart;
            int number = 1;

            while (current < end)
            {
                var weekEnd = current.AddDays(6); // Sunday
                if (weekEnd > end) weekEnd = end;

                var type = DetermineMicrocycleType(current, weekEnd, events);
                var hasPeak = events.Any(e => e.Type == "competencia" && e.StartDate <= weekEnd && e.EndDate >= current);
                var distribution = GetDefaultDistribution(type);

                var calendar = System.Globalization.CultureInfo.InvariantCulture.Calendar;
                var weekNumber = calendar.GetWeekOfYear(current, System.Globalization.CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

                microcycles.Add(new Microcycle
                {
                    MacrocycleId = macrocycleId,
                    Number = number,
                    WeekNumber = weekNumber,
                    StartDate = current,
                    EndDate = weekEnd,
                    Type = type,
                    HasPeakPerformance = hasPeak,
                    TrainingDistribution = JsonSerializer.Serialize(distribution)
                });

                current = current.AddDays(7);
                number++;
            }

            return microcycles;
        }

        private static string DetermineMicrocycleType(DateTime weekStart, DateTime weekEnd, List<MacrocycleEvent> events)
        {
            var weekEvents = events.Where(e => e.StartDate <= weekEnd && e.EndDate >= weekStart).ToList();

            if (weekEvents.Any(e => e.Type == "competencia")) return "competitivo";
            if (weekEvents.Any(e => e.Type == "evaluacion")) return "evaluacion";
            if (weekEvents.Any(e => e.Type == "descanso")) return "recuperacion";
            if (weekEvents.Any(e => e.Type == "campus")) return "choque";
            if (weekEvents.Any(e => e.Type == "concentracion")) return "activacion";

            // Check if it's 1-2 weeks before a competition
            var nextCompetition = events
                .Where(e => e.Type == "competencia" && e.StartDate > weekEnd)
                .OrderBy(e => e.StartDate)
                .FirstOrDefault();

            if (nextCompetition != null)
            {
                var daysToComp = (nextCompetition.StartDate - weekStart).Days;
                if (daysToComp <= 14 && daysToComp > 7) return "activacion";
                if (daysToComp <= 7) return "activacion";
            }

            return "ordinario";
        }

        private static List<MacrocyclePeriod> BuildPeriods(string macrocycleId, DateTime start, DateTime end, List<MacrocycleEvent> events, List<Microcycle> microcycles)
        {
            var periods = new List<MacrocyclePeriod>();
            var competitions = events.Where(e => e.Type == "competencia").OrderBy(e => e.StartDate).ToList();

            if (!competitions.Any())
            {
                // No competitions: everything is preparatory general
                var weeks = (int)Math.Ceiling((end - start).TotalDays / 7.0);
                periods.Add(new MacrocyclePeriod
                {
                    MacrocycleId = macrocycleId,
                    Name = "Preparatorio General",
                    Type = "preparatorioGeneral",
                    StartDate = start,
                    EndDate = end,
                    Weeks = weeks
                });
                return periods;
            }

            var firstComp = competitions.First();
            var lastComp = competitions.Last();
            var twoWeeksBefore = firstComp.StartDate.AddDays(-14);

            // Preparatorio General: start → 2 weeks before first competition
            if (twoWeeksBefore > start)
            {
                var weeks = (int)Math.Ceiling((twoWeeksBefore - start).TotalDays / 7.0);
                periods.Add(new MacrocyclePeriod
                {
                    MacrocycleId = macrocycleId,
                    Name = "Preparatorio General",
                    Type = "preparatorioGeneral",
                    StartDate = start,
                    EndDate = twoWeeksBefore.AddDays(-1),
                    Weeks = Math.Max(1, weeks)
                });
            }

            // Preparatorio Especial: 2 weeks before first competition
            if (twoWeeksBefore > start)
            {
                periods.Add(new MacrocyclePeriod
                {
                    MacrocycleId = macrocycleId,
                    Name = "Preparatorio Especial",
                    Type = "preparatorioEspecial",
                    StartDate = twoWeeksBefore,
                    EndDate = firstComp.StartDate.AddDays(-1),
                    Weeks = 2
                });
            }

            // Competitivo: first competition start → last competition end
            var compWeeks = (int)Math.Ceiling((lastComp.EndDate - firstComp.StartDate).TotalDays / 7.0);
            periods.Add(new MacrocyclePeriod
            {
                MacrocycleId = macrocycleId,
                Name = "Competitivo",
                Type = "competitivo",
                StartDate = firstComp.StartDate,
                EndDate = lastComp.EndDate,
                Weeks = Math.Max(1, compWeeks)
            });

            // Transición: after last competition → end
            if (lastComp.EndDate < end)
            {
                var transStart = lastComp.EndDate.AddDays(1);
                var weeks = (int)Math.Ceiling((end - transStart).TotalDays / 7.0);
                periods.Add(new MacrocyclePeriod
                {
                    MacrocycleId = macrocycleId,
                    Name = "Transición",
                    Type = "transicion",
                    StartDate = transStart,
                    EndDate = end,
                    Weeks = Math.Max(1, weeks)
                });
            }

            return periods;
        }

        private static List<Mesocycle> BuildMesocycles(string macrocycleId, List<Microcycle> microcycles, List<MacrocyclePeriod> periods)
        {
            var mesocycles = new List<Mesocycle>();
            if (!microcycles.Any()) return mesocycles;

            int mesoNumber = 1;
            int blockSize = 4;

            foreach (var period in periods)
            {
                var periodMicros = microcycles
                    .Where(m => m.StartDate >= period.StartDate && m.StartDate <= period.EndDate)
                    .OrderBy(m => m.Number)
                    .ToList();

                if (!periodMicros.Any()) continue;

                // Split into blocks of ~4 weeks
                for (int i = 0; i < periodMicros.Count; i += blockSize)
                {
                    var block = periodMicros.Skip(i).Take(blockSize).ToList();
                    var mesoType = GetMesocycleType(period.Type, mesoNumber, i == 0);

                    mesocycles.Add(new Mesocycle
                    {
                        MacrocycleId = macrocycleId,
                        Number = mesoNumber,
                        Name = $"Meso {mesoNumber} – {CapitalizeFirst(mesoType)}",
                        Type = mesoType,
                        StartDate = block.First().StartDate,
                        EndDate = block.Last().EndDate,
                        Weeks = block.Count,
                        Objective = GetMesocycleObjective(mesoType)
                    });

                    mesoNumber++;
                }
            }

            return mesocycles;
        }

        private static string GetMesocycleType(string periodType, int mesoNumber, bool isFirst)
        {
            return periodType switch
            {
                "preparatorioGeneral" => isFirst ? "introductorio" : "desarrollador",
                "preparatorioEspecial" => "precompetitivo",
                "competitivo" => "competitivo",
                "transicion" => "recuperacion",
                _ => "desarrollador"
            };
        }

        private static string? GetMesocycleObjective(string mesoType)
        {
            return mesoType switch
            {
                "introductorio" => "Adaptación y acondicionamiento base",
                "desarrollador" => "Desarrollo de capacidades físicas y técnicas",
                "precompetitivo" => "Preparación específica para competencia",
                "competitivo" => "Rendimiento máximo en competencia",
                "recuperacion" => "Recuperación y regeneración",
                "estabilizador" => "Estabilización y consolidación de logros",
                _ => null
            };
        }

        private static TrainingDistributionDto GetDefaultDistribution(string microcycleType)
        {
            return microcycleType switch
            {
                "ordinario" => new TrainingDistributionDto { FisicaGeneral = 0.15, FisicaEspecial = 0.15, Tecnica = 0.25, Tactica = 0.20, Teorica = 0.15, Psicologica = 0.10 },
                "choque" => new TrainingDistributionDto { FisicaGeneral = 0.20, FisicaEspecial = 0.20, Tecnica = 0.20, Tactica = 0.15, Teorica = 0.15, Psicologica = 0.10 },
                "activacion" => new TrainingDistributionDto { FisicaGeneral = 0.10, FisicaEspecial = 0.15, Tecnica = 0.20, Tactica = 0.25, Teorica = 0.15, Psicologica = 0.15 },
                "competitivo" => new TrainingDistributionDto { FisicaGeneral = 0.05, FisicaEspecial = 0.10, Tecnica = 0.20, Tactica = 0.30, Teorica = 0.15, Psicologica = 0.20 },
                "recuperacion" => new TrainingDistributionDto { FisicaGeneral = 0.20, FisicaEspecial = 0.10, Tecnica = 0.15, Tactica = 0.10, Teorica = 0.25, Psicologica = 0.20 },
                "descarga" => new TrainingDistributionDto { FisicaGeneral = 0.25, FisicaEspecial = 0.10, Tecnica = 0.15, Tactica = 0.10, Teorica = 0.20, Psicologica = 0.20 },
                "evaluacion" => new TrainingDistributionDto { FisicaGeneral = 0.10, FisicaEspecial = 0.10, Tecnica = 0.25, Tactica = 0.25, Teorica = 0.15, Psicologica = 0.15 },
                _ => new TrainingDistributionDto { FisicaGeneral = 0.15, FisicaEspecial = 0.15, Tecnica = 0.20, Tactica = 0.20, Teorica = 0.20, Psicologica = 0.10 }
            };
        }

        private static string CapitalizeFirst(string s) =>
            string.IsNullOrEmpty(s) ? s : char.ToUpper(s[0]) + s[1..];

        // ===================== MAPPING =====================

        private static MacrocycleResponseDto MapToResponse(Macrocycle m)
        {
            return new MacrocycleResponseDto
            {
                MacrocycleId = m.MacrocycleId,
                AthleteId = m.AthleteId,
                AthleteName = m.AthleteName,
                Name = m.Name,
                StartDate = m.StartDate,
                EndDate = m.EndDate,
                Notes = m.Notes,
                CoachId = m.CoachId,
                TeamId = m.TeamId,
                CreatedAt = m.CreatedAt,
                UpdatedAt = m.UpdatedAt,
                Events = m.Events.Select(e => new MacrocycleEventResponseDto
                {
                    MacrocycleEventId = e.MacrocycleEventId,
                    Name = e.Name,
                    Type = e.Type,
                    StartDate = e.StartDate,
                    EndDate = e.EndDate,
                    Location = e.Location,
                    Notes = e.Notes
                }).ToList(),
                Periods = m.Periods.OrderBy(p => p.StartDate).Select(p => new MacrocyclePeriodResponseDto
                {
                    MacrocyclePeriodId = p.MacrocyclePeriodId,
                    Name = p.Name,
                    Type = p.Type,
                    StartDate = p.StartDate,
                    EndDate = p.EndDate,
                    Weeks = p.Weeks
                }).ToList(),
                Mesocycles = m.Mesocycles.OrderBy(ms => ms.Number).Select(ms => new MesocycleResponseDto
                {
                    MesocycleId = ms.MesocycleId,
                    Number = ms.Number,
                    Name = ms.Name,
                    Type = ms.Type,
                    StartDate = ms.StartDate,
                    EndDate = ms.EndDate,
                    Weeks = ms.Weeks,
                    Objective = ms.Objective
                }).ToList(),
                Microcycles = m.Microcycles.OrderBy(mi => mi.Number).Select(mi => new MicrocycleResponseDto
                {
                    MicrocycleId = mi.MicrocycleId,
                    Number = mi.Number,
                    WeekNumber = mi.WeekNumber,
                    StartDate = mi.StartDate,
                    EndDate = mi.EndDate,
                    Type = mi.Type,
                    PeriodName = mi.PeriodName,
                    MesocycleName = mi.MesocycleName,
                    HasPeakPerformance = mi.HasPeakPerformance,
                    TrainingDistribution = string.IsNullOrEmpty(mi.TrainingDistribution)
                        ? null
                        : JsonSerializer.Deserialize<TrainingDistributionDto>(mi.TrainingDistribution)
                }).ToList()
            };
        }
    }
}

