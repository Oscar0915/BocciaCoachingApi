using BocciaCoaching.Infrastructure.Auth;
using BocciaCoaching.Models.DTO.Auth;
using BocciaCoaching.Models.DTO.General;
using BocciaCoaching.Models.DTO.User;
using BocciaCoaching.Models.DTO.User.Atlhete;
using BocciaCoaching.Services.Auth;
using BocciaCoaching.Services.Interfaces;
using BocciaCoaching.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace BocciaCoaching.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
       private readonly IUserService _user;
       private readonly IFileStorageService _fileStorage;
       private readonly IAuthService _auth;


        public UserController(IUserService user, IFileStorageService fileStorage, IAuthService auth)
        {
            _user = user;
            _fileStorage = fileStorage;
            _auth = auth;
        }

        private string? ClientIp => HttpContext.Connection.RemoteIpAddress?.ToString();

        [HttpGet(Name = "GetInfoUser")]
        public async Task<ActionResult<ResponseContract<IEnumerable<InfoBasicUserDto>>>> Get()
        {
            var response = await _user.GetAllAsync();
            return Ok(response);
        }


        /// <summary>Registro público. Solo admite los roles entrenador o atleta.</summary>
        [HttpPost("AddInfoUser")]
        [AllowAnonymous]
        [EnableRateLimiting(AuthenticationSetup.AuthRateLimitPolicy)]
        public async Task<ActionResult<ResponseContract<bool>>> Add(InfoUserRegisterDto user)
        {
            var response = await _user.AddUser(user);
            return Ok(response);
        }

        /// <summary>Login. La respuesta incluye <c>data.tokens</c> (access y refresh token).</summary>
        [HttpPost("login")]
        [AllowAnonymous]
        [EnableRateLimiting(AuthenticationSetup.AuthRateLimitPolicy)]
        public async Task<ActionResult<ResponseContract<LoginResponseDto>>> Login(LoginRequestDto user)
        {
            var response = await _auth.LoginAsync(user, ClientIp, HttpContext.RequestAborted);
            return Ok(response);
        }

        /// <summary>Login con Google. La respuesta incluye <c>data.tokens</c>.</summary>
        [HttpPost("login/google")]
        [AllowAnonymous]
        [EnableRateLimiting(AuthenticationSetup.AuthRateLimitPolicy)]
        public async Task<ActionResult<ResponseContract<LoginResponseDto>>> LoginWithGoogle(FirebaseLoginRequestDto login)
        {
            var response = await _auth.LoginWithGoogleAsync(login, ClientIp, HttpContext.RequestAborted);
            return Ok(response);
        }

        [HttpPost("verify-email/google")]
        [AllowAnonymous]
        [EnableRateLimiting(AuthenticationSetup.AuthRateLimitPolicy)]
        public async Task<ActionResult<ResponseContract<GoogleEmailVerificationDto>>> VerifyGoogleEmail(FirebaseLoginRequestDto request)
        {
            var response = await _user.VerifyGoogleEmail(request);
            return Ok(response);
        }

        [HttpPost("register/google")]
        [AllowAnonymous]
        [EnableRateLimiting(AuthenticationSetup.AuthRateLimitPolicy)]
        public async Task<ActionResult<ResponseContract<bool>>> RegisterWithGoogle(GoogleRegisterRequestDto request)
        {
            var response = await _user.RegisterWithGoogle(request);
            return Ok(response);
        }


        [HttpPost("AddAthlete")]
        public async Task<ActionResult<ResponseContract<int>>> NewAthlete(AtlheteInfoSave user)
        {
            var response = await _user.RegistrarAtleta(user);
            return Ok(response);
        }


        /// <summary>Indica si un email está disponible para registrarse.</summary>
        [HttpPost("ValidateEmail")]
        [AllowAnonymous]
        [EnableRateLimiting(AuthenticationSetup.AuthRateLimitPolicy)]
        public async Task<ActionResult<ResponseContract<ValidateEmailDto>>> ValidateEmail(ValidateEmailDto email)
        {
            var response = await _user.ValidateEmail(email);
            return Ok(response);
        }


        [HttpPost("SearchAthletesForNameAndTeams")]
        public async Task<ActionResult<ResponseContract<List<AtlheteInfo>>>> SearchAthletesForNameAndTeams(SearchDataAthleteDto email)
        {
            var response = await _user.GetAthleteForName(email);
            return Ok(response);
        }

        /// <summary>
        /// Cambia la contraseña del usuario autenticado. El usuario sale del token (se ignoran
        /// UserId/Email del cuerpo) y se exige la contraseña actual. Cierra todas las sesiones.
        /// </summary>
        [HttpPut("UpdatePassword")]
        [EnableRateLimiting(AuthenticationSetup.AuthRateLimitPolicy)]
        public async Task<ActionResult<ResponseContract<bool>>> UpdatePassword(UpdatePasswordDto updatePasswordDto)
        {
            var response = await _auth.ChangePasswordLegacyAsync(User.GetUserId(), updatePasswordDto, HttpContext.RequestAborted);
            return Ok(response);
        }

        /// <summary>
        /// Restablece la contraseña. Exige el <c>resetToken</c> que devuelve <c>Email/ValidateCode</c>
        /// al validar el código enviado al correo. Cierra todas las sesiones del usuario.
        /// </summary>
        [HttpPost("ResetPassword")]
        [AllowAnonymous]
        [EnableRateLimiting(AuthenticationSetup.AuthRateLimitPolicy)]
        public async Task<ActionResult<ResponseContract<bool>>> ResetPassword(ResetPasswordDto resetPasswordDto)
        {
            var response = await _auth.ResetPasswordAsync(resetPasswordDto, HttpContext.RequestAborted);
            return Ok(response);
        }

        /// <summary>Actualiza el perfil. Solo el propio usuario (o un Admin) puede modificarlo.</summary>
        [HttpPut("UpdateUserInfo")]
        public async Task<ActionResult<ResponseContract<bool>>> UpdateUserInfo(UpdateUserInfoDto updateUserInfoDto)
        {
            if (updateUserInfoDto.UserId != User.GetUserId() && !User.IsAdmin())
                return StatusCode(StatusCodes.Status403Forbidden,
                    ResponseContract<bool>.Forbidden("Solo puedes modificar tu propio perfil."));

            var response = await _user.UpdateUserInfo(updateUserInfoDto);
            return Ok(response);
        }


    }
}
