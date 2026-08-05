using BocciaCoaching.Models.DTO.General;
using BocciaCoaching.Models.DTO.Rol;

namespace BocciaCoaching.Services.Interfaces
{
    public interface IRolService
    {
        Task<ResponseContract<RolResponseDto>> Create(CreateRolDto dto);
        Task<ResponseContract<List<RolResponseDto>>> GetAll();
        Task<ResponseContract<RolResponseDto>> GetById(Guid id);
        Task<ResponseContract<RolResponseDto>> Update(UpdateRolDto dto);
        Task<ResponseContract<bool>> Delete(Guid id);
    }
}

