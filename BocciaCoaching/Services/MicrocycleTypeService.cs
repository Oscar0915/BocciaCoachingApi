using BocciaCoaching.Models.DTO.General;
using BocciaCoaching.Models.DTO.MicrocycleType;
using BocciaCoaching.Models.Entities;
using BocciaCoaching.Repositories.Interfaces.IMicrocycleType;
using BocciaCoaching.Services.Interfaces;
using MicrocycleTypeEntity = BocciaCoaching.Models.Entities.MicrocycleType;

namespace BocciaCoaching.Services
{
    public class MicrocycleTypeService : IMicrocycleTypeService
    {
        private readonly IMicrocycleTypeRepository _repository;

        public MicrocycleTypeService(IMicrocycleTypeRepository repository)
        {
            _repository = repository;
        }

        public async Task<ResponseContract<MicrocycleTypeResponseDto>> Create(CreateMicrocycleTypeDto dto)
        {
            try
            {
                var entity = new MicrocycleTypeEntity
                {
                    MicrocycleTypeId = Guid.NewGuid().ToString(),
                    Name = dto.Name,
                    ShortCode = dto.ShortCode,
                    Description = dto.Description,
                    CreatedAt = DateTime.Now
                };

                foreach (var day in dto.Days)
                {
                    entity.DayConfigs.Add(new MicrocycleTypeDayDefault
                    {
                        MicrocycleTypeDayDefaultId = Guid.NewGuid().ToString(),
                        MicrocycleTypeId = entity.MicrocycleTypeId,
                        CoachId = null,   // null = default global del sistema
                        DayOfWeek = day.DayOfWeek,
                        ThrowPercentage = day.ThrowPercentage,
                        CreatedAt = DateTime.Now
                    });
                }

                var created = await _repository.CreateAsync(entity);
                return ResponseContract<MicrocycleTypeResponseDto>.Ok(MapToResponse(created));
            }
            catch (Exception ex)
            {
                return ResponseContract<MicrocycleTypeResponseDto>.Fail($"Error al crear tipo de microciclo: {ex.Message}");
            }
        }

        public async Task<ResponseContract<List<MicrocycleTypeResponseDto>>> GetAll()
        {
            try
            {
                var types = await _repository.GetAllAsync();
                return ResponseContract<List<MicrocycleTypeResponseDto>>.Ok(
                    types.Select(t => MapToResponse(t)).ToList());
            }
            catch (Exception ex)
            {
                return ResponseContract<List<MicrocycleTypeResponseDto>>.Fail($"Error al obtener tipos de microciclo: {ex.Message}");
            }
        }

        public async Task<ResponseContract<MicrocycleTypeResponseDto>> GetById(string id)
        {
            try
            {
                var entity = await _repository.GetByIdAsync(id);
                if (entity == null)
                    return ResponseContract<MicrocycleTypeResponseDto>.Fail("Tipo de microciclo no encontrado");
                return ResponseContract<MicrocycleTypeResponseDto>.Ok(MapToResponse(entity));
            }
            catch (Exception ex)
            {
                return ResponseContract<MicrocycleTypeResponseDto>.Fail($"Error al obtener tipo de microciclo: {ex.Message}");
            }
        }

        public async Task<ResponseContract<MicrocycleTypeResponseDto>> GetByIdForCoach(string id, int coachId)
        {
            try
            {
                var entity = await _repository.GetByIdAsync(id);
                if (entity == null)
                    return ResponseContract<MicrocycleTypeResponseDto>.Fail("Tipo de microciclo no encontrado");

                var coachDays = await _repository.GetCoachDaysAsync(coachId, id);
                return ResponseContract<MicrocycleTypeResponseDto>.Ok(MapToResponseWithCoach(entity, coachDays));
            }
            catch (Exception ex)
            {
                return ResponseContract<MicrocycleTypeResponseDto>.Fail($"Error al obtener tipo de microciclo para coach: {ex.Message}");
            }
        }

        public async Task<ResponseContract<List<MicrocycleTypeResponseDto>>> GetAllForCoach(int coachId)
        {
            try
            {
                var types = await _repository.GetAllAsync();
                var result = new List<MicrocycleTypeResponseDto>();

                foreach (var type in types)
                {
                    var coachDays = await _repository.GetCoachDaysAsync(coachId, type.MicrocycleTypeId);
                    result.Add(MapToResponseWithCoach(type, coachDays));
                }

                return ResponseContract<List<MicrocycleTypeResponseDto>>.Ok(result);
            }
            catch (Exception ex)
            {
                return ResponseContract<List<MicrocycleTypeResponseDto>>.Fail($"Error al obtener tipos de microciclo para coach: {ex.Message}");
            }
        }

        public async Task<ResponseContract<bool>> UpdateCoachPercentages(UpdateCoachPercentagesDto dto)
        {
            try
            {
                var entity = await _repository.GetByIdAsync(dto.MicrocycleTypeId);
                if (entity == null)
                    return ResponseContract<bool>.Fail("Tipo de microciclo no encontrado");

                // Crear registros con CoachId != null (override del coach)
                var days = dto.Days.Select(d => new MicrocycleTypeDayDefault
                {
                    MicrocycleTypeDayDefaultId = Guid.NewGuid().ToString(),
                    CoachId = dto.CoachId,
                    MicrocycleTypeId = dto.MicrocycleTypeId,
                    DayOfWeek = d.DayOfWeek,
                    ThrowPercentage = d.ThrowPercentage,
                    CreatedAt = DateTime.Now
                }).ToList();

                await _repository.SaveCoachDaysAsync(dto.CoachId, dto.MicrocycleTypeId, days);
                return ResponseContract<bool>.Ok(true, "Porcentajes actualizados correctamente");
            }
            catch (Exception ex)
            {
                return ResponseContract<bool>.Fail($"Error al actualizar porcentajes: {ex.Message}");
            }
        }

        public async Task<ResponseContract<bool>> ResetCoachPercentages(int coachId, string microcycleTypeId)
        {
            try
            {
                var result = await _repository.ResetCoachDaysAsync(coachId, microcycleTypeId);
                return result
                    ? ResponseContract<bool>.Ok(true, "Porcentajes restablecidos a valores por defecto")
                    : ResponseContract<bool>.Fail("No se encontraron porcentajes personalizados para restablecer");
            }
            catch (Exception ex)
            {
                return ResponseContract<bool>.Fail($"Error al restablecer porcentajes: {ex.Message}");
            }
        }

        public async Task<ResponseContract<MicrocycleTypesOverviewDto>> GetOverview()
        {
            try
            {
                // 1. Tipos configurados en el catálogo
                var configuredTypes = await _repository.GetAllAsync();

                // 2. Resumen de microciclos construidos en macrociclos (agrupados por tipo)
                var builtSummary = await _repository.GetBuiltTypeSummaryAsync();

                var overview = new MicrocycleTypesOverviewDto
                {
                    ConfiguredTypes = configuredTypes.Select(t => new MicrocycleTypeWithCountDto
                    {
                        MicrocycleTypeId = t.MicrocycleTypeId,
                        Name = t.Name,
                        Description = t.Description,
                        Status = t.Status,
                        // Solo mostrar los defaults globales (CoachId == null) en el overview
                        Days = t.DayConfigs
                            .Where(d => d.CoachId == null)
                            .Select(d => new MicrocycleTypeDayDto
                            {
                                DayOfWeek = d.DayOfWeek,
                                ThrowPercentage = d.ThrowPercentage,
                                IsCustom = false
                            }).ToList(),
                        // Busca coincidencia por nombre (case-insensitive) con los tipos construidos
                        TotalBuilt = builtSummary
                            .Where(b => b.Key.Equals(t.Name, StringComparison.OrdinalIgnoreCase))
                            .Select(b => b.Value)
                            .FirstOrDefault()
                    }).ToList(),

                    BuiltTypes = builtSummary
                        .OrderByDescending(b => b.Value)
                        .Select(b => new BuiltMicrocycleTypeSummaryDto
                        {
                            TypeName = b.Key,
                            Count = b.Value
                        }).ToList()
                };

                return ResponseContract<MicrocycleTypesOverviewDto>.Ok(overview,
                    $"Se encontraron {overview.ConfiguredTypes.Count} tipos configurados y {overview.BuiltTypes.Count} tipos construidos en la aplicación");
            }
            catch (Exception ex)
            {
                return ResponseContract<MicrocycleTypesOverviewDto>.Fail($"Error al obtener el resumen de tipos de microciclo: {ex.Message}");
            }
        }

        public async Task<ResponseContract<MicrocycleTypeDayDefaultResponseDto>> CreateDayDefault(CreateMicrocycleTypeDayDefaultDto dto)
        {
            try
            {
                // Verificar que el MicrocycleType exista
                var microcycleType = await _repository.GetByIdAsync(dto.MicrocycleTypeId);
                if (microcycleType == null)
                    return ResponseContract<MicrocycleTypeDayDefaultResponseDto>.Fail("Tipo de microciclo no encontrado");

                var entity = new MicrocycleTypeDayDefault
                {
                    MicrocycleTypeDayDefaultId = Guid.NewGuid().ToString(),
                    MicrocycleTypeId = dto.MicrocycleTypeId,
                    CoachId = null,  // null = global default
                    DayOfWeek = dto.DayOfWeek,
                    ThrowPercentage = dto.ThrowPercentage,
                    CreatedAt = DateTime.Now
                };

                var created = await _repository.CreateDayDefaultAsync(entity);

                var response = new MicrocycleTypeDayDefaultResponseDto
                {
                    MicrocycleTypeDayDefaultId = created.MicrocycleTypeDayDefaultId,
                    MicrocycleTypeId = created.MicrocycleTypeId,
                    DayOfWeek = created.DayOfWeek,
                    ThrowPercentage = created.ThrowPercentage
                };

                return ResponseContract<MicrocycleTypeDayDefaultResponseDto>.Ok(response, "Día por defecto creado correctamente");
            }
            catch (Exception ex)
            {
                return ResponseContract<MicrocycleTypeDayDefaultResponseDto>.Fail($"Error al crear día por defecto: {ex.Message}");
            }
        }

        // --- Mappers ---

        private MicrocycleTypeResponseDto MapToResponse(MicrocycleTypeEntity entity)
        {
            return new MicrocycleTypeResponseDto
            {
                MicrocycleTypeId = entity.MicrocycleTypeId,
                Name = entity.Name,
                ShortCode = entity.ShortCode,
                Description = entity.Description,
                Status = entity.Status,
                // Solo los defaults globales (sin coach override)
                Days = entity.DayConfigs
                    .Where(d => d.CoachId == null)
                    .Select(d => new MicrocycleTypeDayDto
                    {
                        DayOfWeek = d.DayOfWeek,
                        ThrowPercentage = d.ThrowPercentage,
                        IsCustom = false
                    }).ToList()
            };
        }

        private MicrocycleTypeResponseDto MapToResponseWithCoach(
            MicrocycleTypeEntity entity,
            List<MicrocycleTypeDayDefault> coachDays)
        {
            var dto = new MicrocycleTypeResponseDto
            {
                MicrocycleTypeId = entity.MicrocycleTypeId,
                Name = entity.Name,
                ShortCode = entity.ShortCode,
                Description = entity.Description,
                Status = entity.Status
            };

            // Los defaults globales son los que tienen CoachId == null
            var globalDefaults = entity.DayConfigs.Where(d => d.CoachId == null).ToList();

            foreach (var defaultDay in globalDefaults)
            {
                var coachDay = coachDays.FirstOrDefault(c =>
                    c.DayOfWeek.Equals(defaultDay.DayOfWeek, StringComparison.OrdinalIgnoreCase));

                dto.Days.Add(new MicrocycleTypeDayDto
                {
                    DayOfWeek = defaultDay.DayOfWeek,
                    ThrowPercentage = coachDay?.ThrowPercentage ?? defaultDay.ThrowPercentage,
                    IsCustom = coachDay != null
                });
            }

            return dto;
        }

        // ─── CoachMicrocycleTypeDistribution ────────────────────────────────────

        public async Task<ResponseContract<CoachMicrocycleTypeDistributionDto>> UpsertCoachDistribution(UpsertCoachMicrocycleTypeDistributionDto dto)
        {
            try
            {
                var type = await _repository.GetByIdAsync(dto.MicrocycleTypeId);
                if (type == null)
                    return ResponseContract<CoachMicrocycleTypeDistributionDto>.Fail("Tipo de microciclo no encontrado");

                var sum = dto.FisicaGeneral + dto.FisicaEspecial + dto.Tecnica + dto.Tactica + dto.Teorica + dto.Psicologica;
                if (Math.Abs(sum - 1.0) > 0.01)
                    return ResponseContract<CoachMicrocycleTypeDistributionDto>.Fail("La suma de los porcentajes debe ser 1.0 (±0.01)");

                var entity = new CoachMicrocycleTypeDistribution
                {
                    CoachMicrocycleTypeDistributionId = Guid.NewGuid().ToString(),
                    CoachId = dto.CoachId,
                    MicrocycleTypeId = dto.MicrocycleTypeId,
                    FisicaGeneral = dto.FisicaGeneral,
                    FisicaEspecial = dto.FisicaEspecial,
                    Tecnica = dto.Tecnica,
                    Tactica = dto.Tactica,
                    Teorica = dto.Teorica,
                    Psicologica = dto.Psicologica,
                    CreatedAt = DateTime.Now
                };

                await _repository.UpsertCoachDistributionAsync(entity);

                var saved = await _repository.GetCoachDistributionAsync(dto.CoachId, dto.MicrocycleTypeId);
                return ResponseContract<CoachMicrocycleTypeDistributionDto>.Ok(
                    MapDistributionToDto(saved!, type),
                    "Distribución guardada correctamente");
            }
            catch (Exception ex)
            {
                return ResponseContract<CoachMicrocycleTypeDistributionDto>.Fail($"Error al guardar distribución: {ex.Message}");
            }
        }

        public async Task<ResponseContract<CoachMicrocycleTypeDistributionDto?>> GetCoachDistribution(int coachId, string microcycleTypeId)
        {
            try
            {
                var type = await _repository.GetByIdAsync(microcycleTypeId);
                if (type == null)
                    return ResponseContract<CoachMicrocycleTypeDistributionDto?>.Fail("Tipo de microciclo no encontrado");

                var distribution = await _repository.GetCoachDistributionAsync(coachId, microcycleTypeId);
                if (distribution == null)
                    return ResponseContract<CoachMicrocycleTypeDistributionDto?>.Ok(null, "El coach no tiene distribución personalizada para este tipo");

                return ResponseContract<CoachMicrocycleTypeDistributionDto?>.Ok(MapDistributionToDto(distribution, type));
            }
            catch (Exception ex)
            {
                return ResponseContract<CoachMicrocycleTypeDistributionDto?>.Fail($"Error al obtener distribución: {ex.Message}");
            }
        }

        public async Task<ResponseContract<List<CoachMicrocycleTypeDistributionDto>>> GetAllCoachDistributions(int coachId)
        {
            try
            {
                var distributions = await _repository.GetAllCoachDistributionsAsync(coachId);
                var result = distributions.Select(d => MapDistributionToDto(d, d.MicrocycleType)).ToList();
                return ResponseContract<List<CoachMicrocycleTypeDistributionDto>>.Ok(result,
                    $"Se encontraron {result.Count} distribuciones personalizadas");
            }
            catch (Exception ex)
            {
                return ResponseContract<List<CoachMicrocycleTypeDistributionDto>>.Fail($"Error al obtener distribuciones: {ex.Message}");
            }
        }

        public async Task<ResponseContract<bool>> DeleteCoachDistribution(int coachId, string microcycleTypeId)
        {
            try
            {
                var result = await _repository.DeleteCoachDistributionAsync(coachId, microcycleTypeId);
                return result
                    ? ResponseContract<bool>.Ok(true, "Distribución personalizada eliminada. Se usarán los valores por defecto")
                    : ResponseContract<bool>.Fail("No se encontró distribución personalizada para ese coach y tipo");
            }
            catch (Exception ex)
            {
                return ResponseContract<bool>.Fail($"Error al eliminar distribución: {ex.Message}");
            }
        }

        private static CoachMicrocycleTypeDistributionDto MapDistributionToDto(
            CoachMicrocycleTypeDistribution d,
            MicrocycleTypeEntity? type)
        {
            return new CoachMicrocycleTypeDistributionDto
            {
                CoachMicrocycleTypeDistributionId = d.CoachMicrocycleTypeDistributionId,
                CoachId = d.CoachId,
                MicrocycleTypeId = d.MicrocycleTypeId,
                MicrocycleTypeName = type?.Name ?? string.Empty,
                MicrocycleTypeShortCode = type?.ShortCode,
                FisicaGeneral = d.FisicaGeneral,
                FisicaEspecial = d.FisicaEspecial,
                Tecnica = d.Tecnica,
                Tactica = d.Tactica,
                Teorica = d.Teorica,
                Psicologica = d.Psicologica,
                CreatedAt = d.CreatedAt,
                UpdatedAt = d.UpdatedAt
            };
        }
    }
}
