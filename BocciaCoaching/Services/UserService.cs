using BocciaCoaching.Models.DTO;
using BocciaCoaching.Models.DTO.AssessStrength;
using BocciaCoaching.Models.DTO.Auth;
using BocciaCoaching.Models.DTO.User;
using BocciaCoaching.Models.Entities;
using BocciaCoaching.Repositories;
using BocciaCoaching.Services.Interfaces;

namespace BocciaCoaching.Services
{
    public class UserService: IUserService
    {
        private readonly IUserRepository _repository;

        public UserService(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> AddUser(InfoUserRegisterDto userDto)
        {
            return await _repository.AddUser(userDto);
        }

        public async Task<AthletesToEvaluated> AgregarAtletaAEvaluacion(RequestAddAthleteToEvaluationDto athletesToEvaluated)
        {
            return await _repository.AgregarAtletaAEvaluacion(athletesToEvaluated);
        }

        public async Task<bool> AgregarDetalleDeEvaluacion(RequestAddDetailToEvaluationForAthlete requestAddDetailToEvaluationForAthlete)
        {
            return await _repository.AgregarDetalleDeEvaluacion(requestAddDetailToEvaluationForAthlete);
        }

        public async Task<ResponseAddAssessStrengthDto> CrearEvaluacion(AddAssessStrengthDto addAssessStrengthDto)
        {
            return await _repository.CrearEvaluacion(addAssessStrengthDto);
        }

        public async Task<IEnumerable<InfoBasicUserDto>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public Task<InfoBasicUserDto?> GetByIdAsync(int id)
        {
            return _repository.GetByIdAsync(id);
        }

        public Task<LoginResponseDto?> IniciarSesion(LoginRequestDto loginDto)
        {
            return _repository.IniciarSesion(loginDto);
        }

        public Task<bool> RegistrarAtleta(AtlheteInfoSave atlheteInfoSave)
        {
            return _repository.RegistrarAtleta(atlheteInfoSave);
        }


        public Task<String> ValidateEmail(ValidateEmailDto email)
        {
            return _repository.ValidateEmail(email);
        }
    }
}
