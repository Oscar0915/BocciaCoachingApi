using BocciaCoaching.Models.DTO.AssessStrength;
using BocciaCoaching.Models.DTO.Auth;
using BocciaCoaching.Models.DTO.General;
using BocciaCoaching.Models.DTO.User;
using BocciaCoaching.Models.DTO.User.Atlhete;
using BocciaCoaching.Models.Entities;

namespace BocciaCoaching.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<InfoBasicUserDto>> GetAllAsync();
        Task<InfoBasicUserDto?> GetByIdAsync(int id);
        Task<bool> AddUser(InfoUserRegisterDto userDto);
        Task<LoginResponseDto?> Login(LoginRequestDto loginDto);
        Task<bool> RegistrarAtleta(AtlheteInfoSave atlheteInfoSave);
      
        Task<ValidateEmailDto> ValidateEmail(ValidateEmailDto email);

        /// <summary>
        /// Método para buscar atletas por nombre pertenecientes a un club o equipo
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<ResponseContract<List<AtlheteInfo>>> GetAthleteForName(SearchDataAthleteDto user);

    }
}
