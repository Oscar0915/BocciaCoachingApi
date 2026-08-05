using BocciaCoaching.Models.DTO.General;
using BocciaCoaching.Models.DTO.Rol;
using BocciaCoaching.Repositories.Interfaces.IRol;
using BocciaCoaching.Services.Interfaces;
using RolEntity = BocciaCoaching.Models.Entities.Rol;

namespace BocciaCoaching.Services
{
    public class RolService : IRolService
    {
        private readonly IRolRepository _repository;

        public RolService(IRolRepository repository)
        {
            _repository = repository;
        }

        public async Task<ResponseContract<RolResponseDto>> Create(CreateRolDto dto)
        {
            try
            {
                if (await _repository.ExistsByDescriptionAsync(dto.Description))
                    return ResponseContract<RolResponseDto>.Fail("Ya existe un rol con esa descripción.");

                var entity = new RolEntity
                {
                    RolId = Guid.NewGuid(),
                    Description = dto.Description.Trim()
                };

                var created = await _repository.CreateAsync(entity);
                return ResponseContract<RolResponseDto>.Ok(MapToResponse(created), "Rol creado correctamente.");
            }
            catch (Exception ex)
            {
                return ResponseContract<RolResponseDto>.Fail($"Error al crear el rol: {ex.Message}");
            }
        }

        public async Task<ResponseContract<List<RolResponseDto>>> GetAll()
        {
            try
            {
                var roles = await _repository.GetAllAsync();
                return ResponseContract<List<RolResponseDto>>.Ok(
                    roles.Select(MapToResponse).ToList());
            }
            catch (Exception ex)
            {
                return ResponseContract<List<RolResponseDto>>.Fail($"Error al obtener los roles: {ex.Message}");
            }
        }

        public async Task<ResponseContract<RolResponseDto>> GetById(Guid id)
        {
            try
            {
                var rol = await _repository.GetByIdAsync(id);
                if (rol == null)
                    return ResponseContract<RolResponseDto>.Fail("Rol no encontrado.");

                return ResponseContract<RolResponseDto>.Ok(MapToResponse(rol));
            }
            catch (Exception ex)
            {
                return ResponseContract<RolResponseDto>.Fail($"Error al obtener el rol: {ex.Message}");
            }
        }

        public async Task<ResponseContract<RolResponseDto>> Update(UpdateRolDto dto)
        {
            try
            {
                var rol = await _repository.GetByIdAsync(dto.RolId);
                if (rol == null)
                    return ResponseContract<RolResponseDto>.Fail("Rol no encontrado.");

                if (await _repository.ExistsByDescriptionAsync(dto.Description, dto.RolId))
                    return ResponseContract<RolResponseDto>.Fail("Ya existe otro rol con esa descripción.");

                rol.Description = dto.Description.Trim();

                var updated = await _repository.UpdateAsync(rol);
                if (!updated)
                    return ResponseContract<RolResponseDto>.Fail("No se pudo actualizar el rol.");

                return ResponseContract<RolResponseDto>.Ok(MapToResponse(rol), "Rol actualizado correctamente.");
            }
            catch (Exception ex)
            {
                return ResponseContract<RolResponseDto>.Fail($"Error al actualizar el rol: {ex.Message}");
            }
        }

        public async Task<ResponseContract<bool>> Delete(Guid id)
        {
            try
            {
                var rol = await _repository.GetByIdAsync(id);
                if (rol == null)
                    return ResponseContract<bool>.Fail("Rol no encontrado.");

                if (await _repository.IsAssignedToUsersAsync(id))
                    return ResponseContract<bool>.Fail("No se puede eliminar el rol porque está asignado a uno o más usuarios.");

                var deleted = await _repository.DeleteAsync(id);
                if (!deleted)
                    return ResponseContract<bool>.Fail("No se pudo eliminar el rol.");

                return ResponseContract<bool>.Ok(true, "Rol eliminado correctamente.");
            }
            catch (Exception ex)
            {
                return ResponseContract<bool>.Fail($"Error al eliminar el rol: {ex.Message}");
            }
        }

        private static RolResponseDto MapToResponse(RolEntity rol) => new()
        {
            RolId = rol.RolId,
            Description = rol.Description
        };
    }
}

