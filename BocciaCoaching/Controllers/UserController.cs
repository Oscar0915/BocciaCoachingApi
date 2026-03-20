using BocciaCoaching.Models.DTO.Auth;
using BocciaCoaching.Models.DTO.General;
using BocciaCoaching.Models.DTO.User;
using BocciaCoaching.Models.DTO.User.Atlhete;
using BocciaCoaching.Services.Interfaces;
using BocciaCoaching.Services;
using Microsoft.AspNetCore.Http;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BocciaCoaching.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
       private readonly IUserService _user;
       private readonly IFileStorageService _fileStorage;


        public UserController(IUserService user, IFileStorageService fileStorage)
        {
            _user = user;
            _fileStorage = fileStorage;
        }

        [HttpGet(Name = "GetInfoUser")]
        public async Task<ActionResult<ResponseContract<IEnumerable<InfoBasicUserDto>>>> Get()
        {
            var response = await _user.GetAllAsync();
            return Ok(response);
        }


        [HttpPost("AddInfoUser")]
        public async Task<ActionResult<ResponseContract<bool>>> Add(InfoUserRegisterDto user)
        {
            var response = await _user.AddUser(user);
            return Ok(response);
        }

        [HttpPost("login")]
        public async Task<ActionResult<ResponseContract<LoginResponseDto>>> Login(LoginRequestDto user)
        {
            var response = await _user.Login(user);
            return Ok(response);
        }


        [HttpPost("AddAthlete")]
        public async Task<ActionResult<ResponseContract<int>>> NewAthlete(AtlheteInfoSave user)
        {
            var response = await _user.RegistrarAtleta(user);
            return Ok(response);
        }


        [HttpPost("ValidateEmail")]
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

        [HttpPut("UpdatePassword")]
        public async Task<ActionResult<ResponseContract<bool>>> UpdatePassword(UpdatePasswordDto updatePasswordDto)
        {
            var response = await _user.UpdatePassword(updatePasswordDto);
            return Ok(response);
        }

        [HttpPut("UpdateUserInfo")]
        public async Task<ActionResult<ResponseContract<bool>>> UpdateUserInfo(UpdateUserInfoDto updateUserInfoDto)
        {
            var response = await _user.UpdateUserInfo(updateUserInfoDto);
            return Ok(response);
        }

        [HttpPut("me/avatar")]
        [Authorize]
        public async Task<IActionResult> UpdateMyAvatar([FromForm] IFormFile? file)
        {
            try
            {
                if (file == null)
                    return BadRequest((object)ResponseContract<bool>.Fail("Archivo no proporcionado"));

                // Obtener userId del claim si aplica. Aquí se asume que existe claim "UserId" o similar
                // Buscar diversos claims comunes que pueden contener el id de usuario
                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId" || c.Type == "userId" || c.Type == System.Security.Claims.ClaimTypes.NameIdentifier || c.Type == "sub");
                if (userIdClaim == null)
                    return Unauthorized(ResponseContract<bool>.Fail("Usuario no autenticado"));

                if (!int.TryParse(userIdClaim.Value, out int userId))
                    return Unauthorized(ResponseContract<bool>.Fail("Usuario no autenticado"));

                var relativePath = await _fileStorage.SaveFileAsync(file, "avatars");

                var updateResult = await _user.UpdateUserImageAsync(userId, relativePath);
                if (!updateResult.Success)
                {
                    // Intentar borrar el archivo guardado
                    await _fileStorage.DeleteFileAsync(relativePath);
                    return BadRequest((object)updateResult);
                }

                // si existía imagen previa, eliminarla del disco
                if (!string.IsNullOrWhiteSpace(updateResult.Data))
                {
                    await _fileStorage.DeleteFileAsync(updateResult.Data);
                }

                return Ok(ResponseContract<string>.Ok(relativePath, "Imagen actualizada"));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ResponseContract<bool>.Fail(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ResponseContract<bool>.Fail($"Error interno: {ex.Message}"));
            }
        }
    }
}
