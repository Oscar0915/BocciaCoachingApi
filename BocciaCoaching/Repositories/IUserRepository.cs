using BocciaCoaching.Models.DTO;
using BocciaCoaching.Models.DTO.AssessStrength;
using BocciaCoaching.Models.DTO.Auth;
using BocciaCoaching.Models.DTO.User;
using BocciaCoaching.Models.Entities;
using System.Threading.Tasks;

namespace BocciaCoaching.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<InfoBasicUserDto>> GetAllAsync();
        Task<InfoBasicUserDto?> GetByIdAsync(int id);
        Task<bool> AddUser(InfoUserRegisterDto userDto);
        Task<LoginResponseDto?> IniciarSesion(LoginRequestDto loginDto);
         Task<bool> RegistrarAtleta(AtlheteInfoSave atlheteInfoSave);
        Task<ResponseAddAssessStrengthDto> CrearEvaluacion(AddAssessStrengthDto addAssessStrengthDto);
        Task<AthletesToEvaluated> AgregarAtletaAEvaluacion(RequestAddAthleteToEvaluationDto athletesToEvaluated);
        Task<bool> AgregarDetalleDeEvaluacion(RequestAddDetailToEvaluationForAthlete requestAddDetailToEvaluationForAthlete);

    }
}
