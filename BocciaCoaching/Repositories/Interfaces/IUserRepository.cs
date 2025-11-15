using BocciaCoaching.Models.DTO.Auth;
using BocciaCoaching.Models.DTO.User;

namespace BocciaCoaching.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<InfoBasicUserDto>> GetAllAsync();
        Task<InfoBasicUserDto?> GetByIdAsync(int id);
        Task<bool> AddUser(InfoUserRegisterDto userDto);
        Task<LoginResponseDto?> Login(LoginRequestDto loginDto);
        Task<bool> RegistrarAtleta(AtlheteInfoSave atlheteInfoSave);
        Task<ValidateEmailDto> ValidateEmail(ValidateEmailDto email);
    }
}
