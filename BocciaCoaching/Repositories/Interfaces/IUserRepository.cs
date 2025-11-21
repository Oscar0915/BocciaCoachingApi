using BocciaCoaching.Models.DTO.Auth;
using BocciaCoaching.Models.DTO.General;
using BocciaCoaching.Models.DTO.User;
using BocciaCoaching.Models.DTO.User.Atlhete;
using BocciaCoaching.Models.Entities;

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

        /// <summary>
        /// Método para la busqueda de los atletas por nombre
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<ResponseContract<List<User>>> GetUserForName(SearchDataAthleteDto user);
    }
}
