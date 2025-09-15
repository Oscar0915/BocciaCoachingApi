using BocciaCoaching.Models.DTO.AssessStrength;
using BocciaCoaching.Models.DTO.Auth;
using BocciaCoaching.Models.DTO.User;
using BocciaCoaching.Models.Entities;
using BocciaCoaching.Services;
using BocciaCoaching.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BocciaCoaching.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
       private readonly IUserService _user;


        public UserController(IUserService user)
        {
            _user = user;
        }

        [HttpGet(Name = "GetInfoUser")]
        public async Task<ActionResult<IEnumerable<InfoBasicUserDto>>> Get()
        {
            var users = await _user.GetAllAsync();
            return Ok(users);
        }


        [HttpPost("AddInfoUser")]
        public async Task<ActionResult<IEnumerable<bool>>> Add(InfoUserRegisterDto user)
        {
            var users = await _user.AddUser(user);
            return Ok(users);
        }

        [HttpPost("login")]
        public async Task<ActionResult<IEnumerable<LoginResponseDto>>> Login(LoginRequestDto user)
        {
            var users = await _user.IniciarSesion(user);

            var result = await _user.IniciarSesion(user);
            if (result == null)
                return Unauthorized(new { message = "Credenciales inválidas" });

            return Ok(result);
        }


        [HttpPost("AddAthlete")]
        public async Task<ActionResult<IEnumerable<bool>>> NewAthlete(AtlheteInfoSave user)
        {
            var users = await _user.RegistrarAtleta(user);
            return Ok(users);
        }


        [HttpPost("AddEvaluation")]
        public async Task<ActionResult<IEnumerable<bool>>> NewEvaluation(AddAssessStrengthDto user)
        {
            var users = await _user.CrearEvaluacion(user);
            return Ok(users);
        }


        [HttpPost("AthletesToEvaluated")]
        public async Task<ActionResult<IEnumerable<bool>>> AthletesToEvaluated(RequestAddAthleteToEvaluationDto user)
        {
            var users = await _user.AgregarAtletaAEvaluacion(user);
            return Ok(users);
        }


        [HttpPost("AddDeatilsToEvaluation")]
        public async Task<ActionResult<IEnumerable<bool>>> AddDeatilsToEvaluation(RequestAddDetailToEvaluationForAthlete user)
        {
            var users = await _user.AgregarDetalleDeEvaluacion(user);
            return Ok(users);
        }


        [HttpPost("ValidateEmail")]
        public async Task<ActionResult<IEnumerable<ValidateEmailDto>>> ValidateEmail(ValidateEmailDto email)
        {
            ValidateEmailDto Response = await _user.ValidateEmail(email);
            return Ok(Response);
        }
    }
}
