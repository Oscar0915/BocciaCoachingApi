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
                    Description = dto.Description,
                    CreatedAt = DateTime.Now
                };

                foreach (var day in dto.Days)
                {
                    entity.DefaultDays.Add(new MicrocycleTypeDayDefault
                    {
                        MicrocycleTypeDayDefaultId = Guid.NewGuid().ToString(),
                        MicrocycleTypeId = entity.MicrocycleTypeId,
                        DayOfWeek = day.DayOfWeek,
                        ThrowPercentage = day.ThrowPercentage
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

                var days = dto.Days.Select(d => new CoachMicrocycleTypeDay
                {
                    CoachMicrocycleTypeDayId = Guid.NewGuid().ToString(),
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

        // --- Mappers ---

        private MicrocycleTypeResponseDto MapToResponse(MicrocycleTypeEntity entity)
        {
            return new MicrocycleTypeResponseDto
            {
                MicrocycleTypeId = entity.MicrocycleTypeId,
                Name = entity.Name,
                Description = entity.Description,
                Status = entity.Status,
                Days = entity.DefaultDays.Select(d => new MicrocycleTypeDayDto
                {
                    DayOfWeek = d.DayOfWeek,
                    ThrowPercentage = d.ThrowPercentage,
                    IsCustom = false
                }).ToList()
            };
        }

        private MicrocycleTypeResponseDto MapToResponseWithCoach(
            MicrocycleTypeEntity entity,
            List<CoachMicrocycleTypeDay> coachDays)
        {
            var dto = new MicrocycleTypeResponseDto
            {
                MicrocycleTypeId = entity.MicrocycleTypeId,
                Name = entity.Name,
                Description = entity.Description,
                Status = entity.Status
            };

            // Para cada día default, verificar si el coach tiene personalización
            foreach (var defaultDay in entity.DefaultDays)
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
    }
}
